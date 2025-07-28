'  Copyright Tony Matous
'
'  Licensed under the Apache License, Version 2.0 (the "License");
'  you may not use this file except in compliance with the License.
'  You may obtain a copy of the License at
'
'  http://www.apache.org/licenses/LICENSE-2.0
'
'  Unless required by applicable law or agreed to in writing, software
'  distributed under the License is distributed on an "AS IS" BASIS,
'  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
'  See the License for the specific language governing permissions and
'  limitations under the License. 



Imports System.Data.Common

Public Class frmEditDb
    Implements IFormGroupKey

    Public Property FormGroupKey As String Implements IFormGroupKey.FormGroupKey
    Public Property IsFormGroupParent As Boolean Implements IFormGroupKey.IsFormGroupParent

    Private _connInfo As Rational.DB.DbConnectionInfo
    Private _query As String = ""

    Private _conn As DbConnection
    Private _selectCmd As DbCommand
    Private _da As DbDataAdapter
    Private _cb As DbCommandBuilder
    Private _ds As DataSet

    Public ReadOnly Property Modified() As Boolean
        Get
            If (_ds Is Nothing) Then Return False
            Return _ds.HasChanges
        End Get
    End Property

    Public Sub New(pConnInfo As Rational.DB.DbConnectionInfo, pQuery As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _connInfo = pConnInfo
        _query = pQuery
    End Sub

    Private Sub frmEditDb_Load(sender As Object, e As EventArgs) Handles Me.Load
        If (Not String.IsNullOrWhiteSpace(_query)) Then
            Reload()
        Else
            btnQuery_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub frmEditDb_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If (ModifiedConfirm()) Then
            Clear()
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub btnLoad_Click(sender As System.Object, e As System.EventArgs) Handles btnLoad.Click
        If (Not ModifiedConfirm()) Then Return
        Reload()
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        Save()
    End Sub

    Private Sub btnQuery_Click(sender As System.Object, e As System.EventArgs) Handles btnQuery.Click
        Using frm = New frmEditSql
            frm.SqlText = _query
            If (frm.ShowDialog(Me) = Windows.Forms.DialogResult.Cancel) Then Return
            If (Not ModifiedConfirm()) Then Return
            _query = frm.SqlText
            Reload()
        End Using
    End Sub




    Private Sub Save()
        If (_ds Is Nothing) Then Throw New Exception("Not loaded")
        Try
            _da.Update(_ds)
            Reload()
        Catch ex As Exception
            Util.ShowError(String.Format("Error saving data: {0}", ex.Message))
        End Try
    End Sub

    Private Function ModifiedConfirm() As Boolean
        If (Modified) Then
            Me.BringToFront()
            Dim res = MessageBox.Show("Data has changed. Do you want to save your changes?", "Confirm", MessageBoxButtons.YesNoCancel)
            If (res = Windows.Forms.DialogResult.Yes) Then
                Save()
                Return True
            ElseIf (res = Windows.Forms.DialogResult.Cancel) Then
                Return False
            Else
                Return True
            End If
        End If
        Return True
    End Function




    Private Sub Reload()
        If (_ds IsNot Nothing) Then Clear()
        Dim fctry = _connInfo.GetFactory()
        _conn = fctry.CreateConnection()
        _conn.ConnectionString = _connInfo.ConnectionString
        _conn.Open()
        _selectCmd = _conn.CreateCommand()
        _selectCmd.CommandText = _query
        _selectCmd.CommandTimeout = Util.GetDefaultTimeout(_connInfo)

        _da = fctry.CreateDataAdapter()
        _da.SelectCommand = _selectCmd
        _cb = fctry.CreateCommandBuilder()
        _cb.DataAdapter = _da
        Try
            Dim updCmd = _cb.GetUpdateCommand()
            Dim insCmd = _cb.GetInsertCommand()
            Dim delcmd = _cb.GetDeleteCommand()
        Catch ex As Exception
            Util.ShowError(String.Format("The specified query does not support updates. ({0})", ex.Message))
            Clear()
            Return
        End Try

        _ds = New DataSet
        _da.Fill(_ds)
        Me.grdEdit.DataSource = _ds.Tables(0)
        FixImageColumns()
        SmartSizeColumns()
        DisableSort()
    End Sub

    Public Sub Clear()
        _fixedImageColumns.Clear()
        _contextMenuCell = Nothing
        _contextMenuRow = Nothing
        _copiedRow = Nothing
        grdEdit.DataSource = Nothing
        If (_conn IsNot Nothing) Then
            _conn.Close()
            _conn.Dispose()
            _conn = Nothing
        End If
        If (_selectCmd IsNot Nothing) Then
            _selectCmd.Dispose()
            _selectCmd = Nothing
        End If
        If (_da IsNot Nothing) Then
            _da.Dispose()
            _da = Nothing
        End If
        If (_cb IsNot Nothing) Then
            _cb.Dispose()
            _cb = Nothing
        End If
        If (_ds IsNot Nothing) Then
            _ds.Dispose()
            _ds = Nothing
        End If
    End Sub

    Private Sub grdEdit_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles grdEdit.CellPainting
        If ((e.ColumnIndex = -1) OrElse (e.RowIndex = -1)) Then Return
        If ((e.Value Is Nothing) OrElse (e.Value Is DBNull.Value)) Then
            'set different background for null values
            e.CellStyle.BackColor = Color.FromArgb(230, 230, 230)
        End If
    End Sub


    Private Sub grdEdit_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles grdEdit.DataError
        'ignore the message from the Image class trying to load a binary field
        If ((e.Exception.Message = "Parameter is not valid.") AndAlso (e.Exception.TargetSite.DeclaringType.Name = "Image")) Then Return
        Util.ShowError(String.Format("Error in field: {0}", e.Exception.Message))
    End Sub



#Region "Menus"

    Private _contextMenuCell As DataGridViewCell = Nothing
    Private _contextMenuRow As DataGridViewRow = Nothing
    Private _copiedRow As DataGridViewRow = Nothing

    Private Sub grdEdit_CellContextMenuStripNeeded(sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs) Handles grdEdit.CellContextMenuStripNeeded
        If ((e.ColumnIndex = -1) AndAlso (e.RowIndex > -1)) Then
            'menu for entire row
            _contextMenuRow = grdEdit.Rows(e.RowIndex)
            grdEdit.ClearSelection()
            grdEdit.EndEdit()
            _contextMenuRow.Selected = True
            e.ContextMenuStrip = mnuContextRow

            If (e.RowIndex = grdEdit.NewRowIndex) Then
                mnuRowCopy.Enabled = False
                mnuRowUndoChanges.Enabled = False
            Else
                mnuRowCopy.Enabled = True
                Dim dvRow = DirectCast(_contextMenuRow.DataBoundItem, DataRowView).Row
                If (dvRow.RowState = DataRowState.Unchanged) Then
                    mnuRowUndoChanges.Enabled = False
                Else
                    mnuRowUndoChanges.Enabled = True
                End If
            End If
            If (_copiedRow IsNot Nothing) Then
                mnuRowPaste.Enabled = True
            Else
                mnuRowPaste.Enabled = False
            End If

            'save changes on a single row is not working, disable the menu
            mnuRowSaveChanges.Visible = False
        ElseIf ((e.ColumnIndex > -1) AndAlso (e.RowIndex > -1) AndAlso (e.RowIndex < grdEdit.NewRowIndex)) Then
            'menu for single cell
            _contextMenuCell = grdEdit.Rows(e.RowIndex).Cells(e.ColumnIndex)
            grdEdit.ClearSelection()
            _contextMenuCell.Selected = True
            e.ContextMenuStrip = mnuContextCell
        End If
    End Sub

    Private Sub mnuCellSetNull_Click(sender As Object, e As EventArgs) Handles mnuCellSetNull.Click
        If (_contextMenuCell Is Nothing) Then Return
        Dim dvRow = DirectCast(_contextMenuCell.OwningRow.DataBoundItem, DataRowView).Row
        dvRow.Item(_contextMenuCell.OwningColumn.DataPropertyName) = DBNull.Value
        _contextMenuCell.Selected = False
    End Sub

    Private Sub mnuRowCopy_Click(sender As Object, e As EventArgs) Handles mnuRowCopy.Click
        If (_contextMenuRow Is Nothing) Then Return
        _copiedRow = _contextMenuRow
    End Sub

    Private Sub mnuRowPaste_Click(sender As Object, e As EventArgs) Handles mnuRowPaste.Click
        If (_contextMenuRow Is Nothing) Then Return
        If (_copiedRow Is Nothing) Then Return
        If (_contextMenuRow.Index = grdEdit.NewRowIndex) Then
            Dim row = _ds.Tables(0).NewRow()
            For idx = 0 To _copiedRow.Cells.Count - 1
                row(idx) = _copiedRow.Cells(idx).Value
            Next
            _ds.Tables(0).Rows.Add(row)
        Else
            For idx = 0 To _copiedRow.Cells.Count - 1
                _contextMenuRow.Cells(idx).Value = _copiedRow.Cells(idx).Value
            Next
        End If
    End Sub

    Private Sub mnuRowUndoChanges_Click(sender As Object, e As EventArgs) Handles mnuRowUndoChanges.Click
        If (_contextMenuRow Is Nothing) Then Return
        Dim dvRow = DirectCast(_contextMenuRow.DataBoundItem, DataRowView).Row
        dvRow.RejectChanges()
        dvRow.ClearErrors()
    End Sub

    Private Sub mnuRowSaveChanges_Click(sender As Object, e As EventArgs) Handles mnuRowSaveChanges.Click
        'BUG: This is not working for some reason, it just doesn't save
        If (_contextMenuRow Is Nothing) Then Return
        Dim dvRow = DirectCast(_contextMenuRow.DataBoundItem, DataRowView).Row
        Try
            dvRow.AcceptChanges()
            Dim changes = _da.Update({dvRow}.ToArray())
            Beep()
        Catch ex As Exception
            Util.ShowError(String.Format("Error saving data: {0}", ex.Message))
        End Try
    End Sub

#End Region


    Private Sub SmartSizeColumns()
        Dim maxColWidth = Convert.ToInt32(Me.Width * 0.9)
        grdEdit.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
        For Each col As DataGridViewColumn In grdEdit.Columns
            If (col.Width > maxColWidth) Then col.Width = maxColWidth
        Next
    End Sub

    Private Sub DisableSort()
        For Each col As DataGridViewColumn In grdEdit.Columns
            col.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub


#Region "binary field fix"

    'DataGridView automatically creates Image columns for all binary data types, generating errors for invalid image data
    'This converts those columns to text columns
    Private _fixedImageColumns As New List(Of DataGridViewColumn)
    Private Sub FixImageColumns()
        _fixedImageColumns.Clear()
        For colIdx = grdEdit.Columns.Count - 1 To 0 Step -1
            Dim col = grdEdit.Columns.Item(colIdx)
            If (col.GetType() = GetType(DataGridViewImageColumn)) Then
                col.Visible = False
                Dim newCol = New DataGridViewTextBoxColumn()
                newCol.Name = col.Name
                newCol.DataPropertyName = col.DataPropertyName
                newCol.HeaderText = col.HeaderText + "_new"
                grdEdit.Columns.Remove(col)
                grdEdit.Columns.Insert(col.Index, newCol)
                col.DataPropertyName = Nothing
                col.Dispose()
                _fixedImageColumns.Add(newCol)
            End If
        Next
    End Sub

    Private Sub grdEdit_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles grdEdit.CellFormatting
        If ((e.Value Is Nothing) OrElse (e.Value Is DBNull.Value)) Then Return
        Dim col = grdEdit.Columns.Item(e.ColumnIndex)
        If (_fixedImageColumns.Contains(col)) Then
            e.Value = Util.ByteArrayToHexString(DirectCast(e.Value, Byte()))
            e.FormattingApplied = True
        End If
    End Sub

    Private Sub grdEdit_CellParsing(sender As Object, e As DataGridViewCellParsingEventArgs) Handles grdEdit.CellParsing
        If ((e.Value Is Nothing) OrElse (e.Value Is DBNull.Value) OrElse (e.Value Is String.Empty)) Then Return
        Dim col = grdEdit.Columns.Item(e.ColumnIndex)
        If (_fixedImageColumns.Contains(col)) Then
            Try
                e.Value = Util.HexStringToByteArray(e.Value.ToString)
                e.ParsingApplied = True
            Catch ex As Exception
            End Try
        End If
    End Sub

#End Region

End Class

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



Public Class ctlQueryResult

    Private _data As DataTable
    Private _status As String = ""
    Private _complete As Boolean = False

    Private Sub ctlQueryResult_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblStatus.Text = _status
        grdData.VirtualMode = True
        grdData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        grdData.AllowUserToResizeColumns = True
        grdData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
        grdData.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        ctlProgress.Visible = Not _complete
    End Sub

    Public Sub SetData(pData As DataTable)
        _data = pData
        grdData.Rows.Clear()
        grdData.Columns.Clear()
        For Each col As DataColumn In _data.Columns
            grdData.Columns.Add(col.ColumnName, col.ColumnName)
        Next

        'disable sort
        For Each col As DataGridViewTextBoxColumn In grdData.Columns
            col.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Public Sub SetStatus(pStatus As String)
        _status = pStatus
    End Sub

    Public Sub SetComplete()
        _complete = True
        SmartSizeColumns()
        ctlProgress.Visible = False
    End Sub

    Public Sub UpdateDisplay()
        lblStatus.Text = _status
        If (_data.Rows.Count > grdData.Rows.Count) Then
            grdData.Rows.Add(_data.Rows.Count - grdData.Rows.Count)
        End If
        SmartSizeColumns()
    End Sub

    Public Sub SmartSizeColumns()
        Dim maxColWidth = Convert.ToInt32(Me.Width * 0.9)
        grdData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)
        For Each col As DataGridViewColumn In grdData.Columns
            If (col.Width > maxColWidth) Then col.Width = maxColWidth
        Next
    End Sub

    Private Sub grdData_CellContextMenuStripNeeded(sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs) Handles grdData.CellContextMenuStripNeeded
        If (grdData.SelectedCells.Count > 0) Then
            e.ContextMenuStrip = mnuRightClickCells
        End If
    End Sub

    Private Sub mnuCopy_Click(sender As Object, e As EventArgs) Handles mnuCopy.Click
        Dim copyData = grdData.GetClipboardContent()
        Util.CopyToClipboard(copyData)
    End Sub

    Private Sub mnuCopyWithHeaders_Click(sender As Object, e As EventArgs) Handles mnuCopyWithHeaders.Click
        grdData.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        Dim copyData = grdData.GetClipboardContent()
        Util.CopyToClipboard(copyData)
        grdData.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText
    End Sub


    Private Sub mnuCopyAsList_Click(sender As Object, e As EventArgs) Handles mnuCopyAsList.Click
        Dim data = New System.Text.StringBuilder()
        Dim delim = ""
        Dim cells = (From c In grdData.SelectedCells.Cast(Of DataGridViewCell)() Order By c.RowIndex, c.ColumnIndex).ToList
        For Each cell In cells
            data.Append(delim)
            Util.AppendSqlListFieldData(data, cell.Value, _data.Columns(cell.ColumnIndex).DataType)
            delim = ", "
        Next
        Util.CopyToClipboard(data.ToString)
    End Sub

    Private Sub grdData_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles grdData.CellPainting
        'sets a dark gray background on null cells
        If ((e.ColumnIndex = -1) OrElse (e.RowIndex = -1)) Then Return
        If ((e.Value Is Nothing) OrElse (e.Value Is DBNull.Value)) Then
            e.CellStyle.BackColor = Color.FromArgb(230, 230, 230)
        End If
    End Sub

    Private Sub grdData_CellValueNeeded(sender As Object, e As DataGridViewCellValueEventArgs) Handles grdData.CellValueNeeded
        'this implements virtual mode for the grid, only displays what is visible. better performance
        If (_data Is Nothing) Then Return
        If ((e.RowIndex > _data.Rows.Count) OrElse (e.ColumnIndex > _data.Columns.Count)) Then Return
        e.Value = _data.Rows.Item(e.RowIndex).Item(e.ColumnIndex)
    End Sub



    Private Sub grdData_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles grdData.ColumnHeaderMouseClick
        If (grdData.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect) Then
            grdData.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect
            grdData.Columns.Item(e.ColumnIndex).Selected = True
        End If
    End Sub



    Private Sub grdData_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles grdData.RowHeaderMouseClick
        If (grdData.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect) Then
            grdData.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            grdData.Rows.Item(e.RowIndex).Selected = True
        End If
    End Sub

    Private Sub grdData_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles grdData.RowPostPaint
        'this adds the row number to the row header
        Dim grid As DataGridView = CType(sender, DataGridView)
        Dim rowIdx As String = (e.RowIndex + 1).ToString()
        Dim rowFont = grid.RowHeadersDefaultCellStyle.Font

        Dim centerFormat = New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}

        Dim textSize = TextRenderer.MeasureText(rowIdx, rowFont)
        If (grid.RowHeadersWidth < textSize.Width + 10) Then grid.RowHeadersWidth = textSize.Width + 10

        Dim headerBounds As Rectangle = New Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height)
        e.Graphics.DrawString(rowIdx, rowFont, SystemBrushes.ControlText, headerBounds, centerFormat)
    End Sub

End Class

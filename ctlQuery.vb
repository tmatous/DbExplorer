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

Public Class ctlQuery

    Private _db As Rational.DB.Database
    Private _connInfo As Rational.DB.DbConnectionInfo
    Public Property ConnInfo() As Rational.DB.DbConnectionInfo
        Get
            Return _connInfo
        End Get
        Set(ByVal value As Rational.DB.DbConnectionInfo)
            If (Not Object.Equals(value, ConnInfo)) Then
                _connInfo = value
                If (_connInfo IsNot Nothing) Then
                    _db = New Rational.DB.Database(_connInfo)
                End If
            End If
        End Set
    End Property


    Public Property ParentQueryForm As frmQuery

    Public Function GetActiveQuery() As String
        If (txtQuery.SelectionLength > 0) Then
            Return txtQuery.SelectedText
        Else
            Return txtQuery.SqlText
        End If
    End Function

    Public Function GetFullQuery() As String
        Return txtQuery.SqlText
    End Function

    Public Sub SetQuery(pQuery As String)
        txtQuery.SqlText = pQuery
        Modified = False
    End Sub

    Public Property QuickView As Boolean

    Private _viewResultsAsList As Boolean = True
    Public Property ViewResultsAsList() As Boolean
        Get
            Return _viewResultsAsList
        End Get
        Set(ByVal value As Boolean)
            _viewResultsAsList = value
        End Set
    End Property

    Private _filename As String
    Public ReadOnly Property Filename() As String
        Get
            Return _filename
        End Get
    End Property

    Private _filePath As String
    Public ReadOnly Property FilePath() As String
        Get
            Return _filePath
        End Get
    End Property


    Public Property Modified() As Boolean
        Get
            Return txtQuery.Modified
        End Get
        Private Set(value As Boolean)
            txtQuery.Modified = value
        End Set
    End Property


    Private _resultUpdateInterval As TimeSpan = TimeSpan.FromSeconds(0.9)

    Private _runThread As ThreadRunData
    Private _runTimer As Stopwatch = Nothing
    Private _runQueryInfo As DoQueryInfo
    Private _runExportInfo As DoExportInfo

    Public Function IsRunning() As Boolean
        Return (_runThread IsNot Nothing)
    End Function

    Public Function RunTimeString() As String
        If (_runTimer IsNot Nothing) Then
            Return String.Format("{0:hh\:mm\:ss}", _runTimer.Elapsed)
        Else
            Return ""
        End If
    End Function

#Region "Events"

    Private Sub ctlQuery_Load(sender As Object, e As EventArgs) Handles Me.Load
        tmrCheckThread.Interval = 100
        If (QuickView) Then DoQuery()
    End Sub


    Private Sub tmrCheckThread_Tick(sender As Object, e As EventArgs) Handles tmrCheckThread.Tick
        If (_runThread IsNot Nothing) Then
            If (_runThread.RunType = ThreadRunData.eRunType.Query) Then
                CheckQueryThreadStatus()
            ElseIf (_runThread.RunType = ThreadRunData.eRunType.Export) Then
                CheckExportThreadStatus()
            End If
        End If
    End Sub


    Private Sub txtQuery_SqlTextChanged(sender As Object, e As EventArgs) Handles txtQuery.SqlTextChanged
        'once text has changed, disable QuickView
        QuickView = False
    End Sub

#End Region




#Region "Query"


    Public Sub DoQuery(Optional pAsBatches As Boolean = False)
        If (IsRunning()) Then Return
        If (String.IsNullOrWhiteSpace(GetActiveQuery)) Then Return



        CleanupCurrentResult()
        _runQueryInfo = New DoQueryInfo()
        _runQueryInfo.Results = New List(Of DataTableResult)()
        _runQueryInfo.Query = GetActiveQuery()

        _runThread = New ThreadRunData With {
            .RunType = ThreadRunData.eRunType.Query,
            .Thread = New Threading.Thread(AddressOf DoQueryThread),
            .LastUiUpdate = DateTime.Now.AddSeconds(-_resultUpdateInterval.TotalSeconds + 1)
        }

        ParentQueryForm.UpdateRunningTabs()
        txtQuery.Cursor = Cursors.WaitCursor

        _runThread.Thread.Start()
        _runTimer = Stopwatch.StartNew()
        tmrCheckThread.Start()
    End Sub

    Private Sub DoQueryThread()
        Try
            Dim batches = Util.SplitSqlBatches(_runQueryInfo.Query)
            For bnum = 1 To batches.Count
                Dim sql = batches(bnum - 1)
                If (batches.Count > 1) Then AppendMessage(String.Format("Running batch {0} ...", bnum))
                _runThread.CurrentBatchStartTime = DateTime.Now
                _runThread.SelectHelper = New Util.SelectHelper
                _runThread.SelectHelper.SelectIntoAction(ConnInfo, sql, AddressOf LoadResultSet)
                If (batches.Count > 1) Then AppendMessage(String.Format("Batch {0} complete.{1}", bnum, Environment.NewLine))
            Next
        Catch ex As Exception
            _runThread.RunError = ex
        End Try
    End Sub

    Private Sub CheckQueryThreadStatus()
        Dim thrd = _runThread
        If (thrd.Thread.IsAlive) Then
            If ((DateTime.Now - thrd.LastUiUpdate > _resultUpdateInterval) OrElse thrd.TriggerUiUpdate) Then
                'update the UI on a longer interval than the check interval
                DisplayResults()
                thrd.LastUiUpdate = DateTime.Now
                thrd.TriggerUiUpdate = False
            End If
        Else
            'done, clean up and display results
            _runThread = Nothing
            _runTimer.Stop()
            ParentQueryForm.UpdateRunningTabs()
            txtQuery.Cursor = Cursors.Default
            tmrCheckThread.Stop()

            thrd.Thread.Abort()

            If (thrd.RequestCancel) Then
                AppendMessage("Query cancelled")
            ElseIf (thrd.RunError IsNot Nothing) Then
                If (TypeOf thrd.RunError Is DbException) Then
                    AppendMessage(String.Format("Error: {0}", thrd.RunError.Message))
                Else
                    Util.ShowError(thrd.RunError)
                End If
            End If

            For Each res In _runQueryInfo.Results
                If (Not res.Complete) Then
                    res.IsCancelled = True
                    res.Complete = True
                End If
            Next

            DisplayResults()
            If (_runQueryInfo.Results.Count = 0) Then
                'focus messages tab
                ctlTabs.SelectTab(ctlTabs.TabCount - 1)
            End If

            'if quickview, focus results for easy scrolling, selection
            If (Not QuickView) Then txtQuery.Focus()
        End If
    End Sub

    Private Sub DisplayResults()
        SyncLock _runQueryInfo
            For Each curResult In _runQueryInfo.Results
                If (curResult.DisplayControl Is Nothing) Then
                    'create new tab
                    If (ViewResultsAsList) Then
                        Dim tabIdx = ctlResultsSplitter.TabCount
                        Dim resultsTab = ctlResultsSplitter.InsertTab(tabIdx)
                        Dim newCtl = New ctlQueryResult()
                        newCtl.Dock = DockStyle.Fill
                        newCtl.Refresh()
                        newCtl.SetData(curResult.Table)
                        curResult.DisplayControl = newCtl
                        resultsTab.Controls.Add(newCtl)
                        ctlResultsSplitter.AutosizeTabs()
                        If (ctlResultsSplitter.TabCount = 1) Then
                            'first data tab, focus it
                            If (QuickView) Then newCtl.Focus()
                        End If
                    Else
                        Dim tabIdx = ctlTabs.TabCount - 1
                        ctlTabs.TabPages.Insert(tabIdx, String.Format("Result {0}", tabIdx + 1))
                        Dim tab = ctlTabs.TabPages.Item(tabIdx)
                        Dim newCtl = New ctlQueryResult()
                        newCtl.Dock = DockStyle.Fill
                        newCtl.Refresh()
                        newCtl.SetData(curResult.Table)
                        curResult.DisplayControl = newCtl
                        tab.Controls.Add(newCtl)
                        If (ctlTabs.TabCount = 2) Then
                            'first data tab, switch to it
                            ctlTabs.SelectTab(0)
                            If (QuickView) Then newCtl.Focus()
                        End If
                    End If
                End If
                If (curResult.Complete) Then curResult.DisplayControl.SetComplete()
                Dim extraMsg = ""
                If (curResult.IsCancelled) Then
                    extraMsg = " (cancelled)"
                ElseIf (Not curResult.Complete) Then
                    extraMsg = " (loading)"
                End If
                curResult.DisplayControl.SetStatus(String.Format("{0} result{1}{2}", curResult.Table.Rows.Count, If(curResult.Table.Rows.Count = 1, "", "s"), extraMsg))
                curResult.DisplayControl.UpdateDisplay()
            Next
        End SyncLock
    End Sub

    Private Sub LoadResultSet(pDbRes As IDataReader)
        Dim res As DataTableResult
        Do
            If (pDbRes.RecordsAffected <> -1) Then
                AppendMessage(String.Format("{0} record{1} affected", pDbRes.RecordsAffected, If((pDbRes.RecordsAffected <> 1), "s", "")))
            End If
            Dim fieldCount = pDbRes.FieldCount
            If (fieldCount = 0) Then
                'no fields, not a select
                Continue Do
            End If
            SyncLock _runQueryInfo
                res = New DataTableResult()
                _runQueryInfo.Results.Add(res)
                Dim fieldNames = New List(Of String)
                For curCol = 0 To fieldCount - 1
                    fieldNames.Add(pDbRes.GetName(curCol))
                Next
                fieldNames = Util.CleanupFieldList(fieldNames)
                For curCol = 0 To fieldCount - 1
                    Dim typ = pDbRes.GetFieldType(curCol)
                    If (typ.IsArray AndAlso (typ.GetElementType = GetType(Byte))) Then typ = GetType(String)
                    res.Table.Columns.Add(fieldNames(curCol), typ)
                Next
            End SyncLock

            _runThread.TriggerUiUpdate = True
            Dim vals(fieldCount) As Object
            While pDbRes.Read()
                SyncLock _runQueryInfo
                    Dim row = res.Table.NewRow()
                    pDbRes.GetValues(vals)
                    For curCol = 0 To fieldCount - 1
                        Dim curVal = vals(curCol)
                        If (curVal IsNot Nothing) Then
                            Dim typ = curVal.GetType()
                            If (typ.IsArray AndAlso (typ.GetElementType = GetType(Byte))) Then
                                curVal = Util.ByteArrayToHexString(DirectCast(curVal, Byte()))
                            End If
                        End If
                        row.Item(curCol) = curVal
                    Next
                    res.Table.Rows.Add(row)
                End SyncLock
            End While
            res.Complete = True

        Loop While pDbRes.NextResult()
    End Sub


#End Region



#Region "Export"


    Public Sub DoExport(pMode As eExportMode)
        If (IsRunning()) Then Return
        If (String.IsNullOrWhiteSpace(GetActiveQuery)) Then Return

        txtMessages.Text = ""
        _runExportInfo = New DoExportInfo()
        _runExportInfo.Query = GetActiveQuery()
        If ((_runQueryInfo IsNot Nothing) AndAlso (_runQueryInfo.Query = _runExportInfo.Query) AndAlso ({eExportMode.Csv, eExportMode.Excel}.Contains(_runExportInfo.ExportMode))) Then
            Dim res = MessageBox.Show("This query is currently displayed. Export existing results?", "Prompt", MessageBoxButtons.YesNoCancel)
            If (res = DialogResult.Cancel) Then Return
            If (res = DialogResult.Yes) Then _runExportInfo.SavedResults = (From i In _runQueryInfo.Results Select i.Table).ToList()
        End If
        _runExportInfo.ExportMode = pMode
        If (_runExportInfo.ExportMode = eExportMode.Csv) Then
            _runExportInfo.CsvDelimiter = Util.PromptDelimiter()
            If (_runExportInfo.CsvDelimiter Is Nothing) Then Return
            _runExportInfo.Filename = Util.PromptSaveFile("csv")
            If (String.IsNullOrEmpty(_runExportInfo.Filename)) Then Return
        ElseIf (_runExportInfo.ExportMode = eExportMode.Excel) Then
            _runExportInfo.Filename = Util.PromptSaveFile("xlsx")
            If (String.IsNullOrEmpty(_runExportInfo.Filename)) Then Return
        Else
            'CodeGen
            _runExportInfo.CodeGenTemplateFilename = Util.PromptRazorTemplate()
            If (String.IsNullOrEmpty(_runExportInfo.CodeGenTemplateFilename)) Then Return

            If (_runExportInfo.ExportMode = eExportMode.CodeGen) Then
                Using frm As New SaveFileDialog()
                    frm.Filter = "All Files|*.*"
                    frm.RestoreDirectory = True
                    frm.InitialDirectory = Util.GetCurrentOutputFolder()
                    If (frm.ShowDialog() <> DialogResult.OK) Then Return
                    _runExportInfo.Filename = frm.FileName
                    Util.SetCurrentOutputFolder(IO.Path.GetDirectoryName(frm.FileName))
                End Using
            Else
                'CodeGen by row
                Using frm As New FolderBrowserDialog()
                    frm.SelectedPath = Util.GetCurrentOutputFolder()
                    If (frm.ShowDialog() <> DialogResult.OK) Then Return
                    _runExportInfo.CodeGenByRowOutputPath = frm.SelectedPath
                    Util.SetCurrentOutputFolder(frm.SelectedPath)
                End Using

                Dim fnMsg = New System.Text.StringBuilder
                fnMsg.AppendLine("By default, a file named 'result_x.out' will be created for each row of the result. Alternately, you can add a column to your query to specify the output filename for each row.")
                fnMsg.AppendLine()
                fnMsg.AppendLine("Enter the column name which contains filenames, if applicable. Leave blank for none.")
                If (PromptForm.PromptForSingleLineString(_runExportInfo.CodeGenByRowFilenameField, fnMsg.ToString) <> DialogResult.OK) Then Return
            End If
        End If

        If (_runExportInfo.ExportMode = eExportMode.CodeGenByRow) Then
            AppendMessage(String.Format("Exporting to '{0}' ...", _runExportInfo.CodeGenByRowOutputPath))
        Else
            AppendMessage(String.Format("Exporting to '{0}' ...", _runExportInfo.Filename))
        End If

        _runThread = New ThreadRunData() With {
            .RunType = ThreadRunData.eRunType.Export,
            .Thread = New Threading.Thread(AddressOf DoExportThread),
            .LastUiUpdate = DateTime.Now
        }

        ParentQueryForm.UpdateRunningTabs()
        txtQuery.Cursor = Cursors.WaitCursor

        _runThread.Thread.Start()
        tmrCheckThread.Start()
    End Sub

    Private Sub DoExportThread()
        Try
            Dim sql = _runExportInfo.Query
            If (_runExportInfo.ExportMode = eExportMode.Csv) Then
                If (_runExportInfo.SavedResults IsNot Nothing) Then
                    ExportDataTablesToCsv(_runExportInfo.SavedResults)
                Else
                    _runThread.SelectHelper = New Util.SelectHelper()
                    _runThread.SelectHelper.SelectIntoAction(ConnInfo, sql, AddressOf ExportResultSetToCsv)
                End If
            ElseIf (_runExportInfo.ExportMode = eExportMode.Excel) Then
                If (_runExportInfo.SavedResults IsNot Nothing) Then
                    ExportDataTablesToExcel(_runExportInfo.SavedResults)
                Else
                    _runThread.SelectHelper = New Util.SelectHelper()
                    _runThread.SelectHelper.SelectIntoAction(ConnInfo, sql, AddressOf ExportResultSetToExcel)
                End If
            ElseIf (_runExportInfo.ExportMode = eExportMode.CodeGen) Then
                _runThread.SelectHelper = New Util.SelectHelper()
                _runThread.SelectHelper.SelectIntoAction(ConnInfo, sql, AddressOf ExportResultSetToCodeGen)
            ElseIf (_runExportInfo.ExportMode = eExportMode.CodeGenByRow) Then
                _runThread.SelectHelper = New Util.SelectHelper()
                _runThread.SelectHelper.SelectIntoAction(ConnInfo, sql, AddressOf ExportResultSetToCodeGenByRow)
            End If
        Catch ex As Exception
            _runThread.RunError = ex
        End Try
    End Sub

    Private Sub ExportResultSetToCsv(pDbRes As IDataReader)
        Dim resCount = 0
        Dim fileName = _runExportInfo.Filename
        Do
            resCount += 1
            If (resCount > 1) Then
                fileName = String.Format("{0}\{1}_{2}.{3}", IO.Path.GetDirectoryName(_runExportInfo.Filename), IO.Path.GetFileNameWithoutExtension(_runExportInfo.Filename), resCount, IO.Path.GetExtension(_runExportInfo.Filename))
                If (IO.File.Exists(fileName)) Then Throw New Exception(String.Format("File already exists: {0}", fileName))
            End If
            Dim rowCount = Util.DataReaderToCsvFile(pDbRes, includeHeader:=True, fileName:=fileName, delimiter:=_runExportInfo.CsvDelimiter)
            AppendMessage(String.Format("{0} records exported to {1}.", rowCount, fileName))
        Loop Until Not pDbRes.NextResult()
    End Sub

    Private Sub ExportDataTablesToCsv(pTables As IList(Of DataTable))
        Dim resCount = 0
        Dim fileName = _runExportInfo.Filename

        For Each tbl In pTables
            resCount += 1
            If (resCount > 1) Then
                fileName = String.Format("{0}\{1}_{2}.{3}", IO.Path.GetDirectoryName(_runExportInfo.Filename), IO.Path.GetFileNameWithoutExtension(_runExportInfo.Filename), resCount, IO.Path.GetExtension(_runExportInfo.Filename))
                If (IO.File.Exists(fileName)) Then Throw New Exception(String.Format("File already exists: {0}", fileName))
            End If
            Dim rowCount = Util.DataReaderToCsvFile(tbl.CreateDataReader(), includeHeader:=True, fileName:=fileName, delimiter:=_runExportInfo.CsvDelimiter)
            AppendMessage(String.Format("{0} records exported to {1}.", rowCount, fileName))
        Next
    End Sub

    Private Sub ExportResultSetToExcel(pDbRes As IDataReader)
        If (IO.File.Exists(_runExportInfo.Filename)) Then IO.File.Delete(_runExportInfo.Filename)
        Dim resCount = 0
        Do
            resCount += 1
            Dim sheetName = String.Format("Sheet{0}", resCount)
            Dim rowCount = Util.DataReaderToExcelFile(pDbRes, includeHeader:=True, fileName:=_runExportInfo.Filename, sheetName:=sheetName)
            AppendMessage(String.Format("{0} records exported to {1}, {2}.", rowCount, _runExportInfo.Filename, sheetName))
        Loop Until Not pDbRes.NextResult()
    End Sub

    Private Sub ExportDataTablesToExcel(pTables As IList(Of DataTable))
        If (IO.File.Exists(_runExportInfo.Filename)) Then IO.File.Delete(_runExportInfo.Filename)
        Dim resCount = 0

        For Each tbl In pTables
            resCount += 1
            Dim sheetName = String.Format("Sheet{0}", resCount)
            Dim rowCount = Util.DataReaderToExcelFile(tbl.CreateDataReader(), includeHeader:=True, fileName:=_runExportInfo.Filename, sheetName:=sheetName)
            AppendMessage(String.Format("{0} records exported to {1}, {2}.", rowCount, _runExportInfo.Filename, sheetName))
        Next
    End Sub

    Private Sub ExportResultSetToCodeGen(pDbRes As IDataReader)
        Dim resCount = 0
        Dim fileName = _runExportInfo.Filename
        Do
            resCount += 1
            If (resCount > 1) Then
                fileName = String.Format("{0}\{1}_{2}.{3}", IO.Path.GetDirectoryName(_runExportInfo.Filename), IO.Path.GetFileNameWithoutExtension(_runExportInfo.Filename), resCount, IO.Path.GetExtension(_runExportInfo.Filename))
                If (IO.File.Exists(fileName)) Then Throw New Exception(String.Format("File already exists: {0}", fileName))
            End If
            Util.DataReaderToCodeGen(pDbRes, _runExportInfo.CodeGenTemplateFilename, fileName)
            AppendMessage(String.Format("Exported to {0}.", fileName))
        Loop Until Not pDbRes.NextResult()
    End Sub

    Private Sub ExportResultSetToCodeGenByRow(pDbRes As IDataReader)
        'Do
        Util.DataReaderToCodeGenByRow(pDbRes, _runExportInfo.CodeGenTemplateFilename, _runExportInfo.CodeGenByRowOutputPath, _runExportInfo.CodeGenByRowFilenameField)
        'Loop Until Not pDbRes.NextResult()
    End Sub

    Private Sub CheckExportThreadStatus()
        Dim thrd = _runThread
        If (thrd.Thread.IsAlive) Then
            If ((DateTime.Now - thrd.LastUiUpdate > _resultUpdateInterval) OrElse thrd.TriggerUiUpdate) Then
                'update the UI on a longer interval than the check interval
                'TODO: display something, status/row count?
                thrd.LastUiUpdate = DateTime.Now
                thrd.TriggerUiUpdate = False
            End If
        Else
            'done, clean up and display results
            _runThread = Nothing
            ParentQueryForm.UpdateRunningTabs()
            txtQuery.Cursor = Cursors.Default
            tmrCheckThread.Stop()

            thrd.Thread.Abort()

            If (thrd.RequestCancel) Then
                AppendMessage("Export cancelled")
            ElseIf (thrd.RunError IsNot Nothing) Then
                If (TypeOf thrd.RunError Is DbException) Then
                    AppendMessage(String.Format("Error: {0}", thrd.RunError.Message))
                Else
                    AppendMessage(String.Format("Error: {0}", thrd.RunError.Message))
                    'Util.ShowError(thrd.RunError)
                End If
            Else
                AppendMessage("Export completed")
            End If

            _runExportInfo = Nothing

            txtQuery.Focus()
        End If
    End Sub


#End Region



    Public Sub DoStop()
        If (_runThread IsNot Nothing) Then
            If (_runThread.SelectHelper IsNot Nothing) Then
                If (Not _runThread.RequestCancel) Then
                    AppendMessage("Attempting to cancel...")
                    _runThread.RequestCancel = True
                    _runThread.SelectHelper.Cancel()
                Else
                    AppendMessage("Attempting to terminate...")
                    _runThread.Thread.Abort()
                End If
            Else
                AppendMessage("Attempting to terminate...")
                _runThread.RequestCancel = True
                _runThread.Thread.Abort()
            End If
        End If
    End Sub

    Public Sub DoLoad(pFilename As String)
        Dim fi = New IO.FileInfo(pFilename)
        _filename = fi.Name
        _filePath = fi.FullName
        SetQuery(IO.File.ReadAllText(pFilename))
        Modified = False
    End Sub

    Public Sub DoSave(pFileName As String)
        Dim fi = New IO.FileInfo(pFileName)
        _filename = fi.Name
        _filePath = fi.FullName
        IO.File.WriteAllText(_filePath, txtQuery.SqlText)
        Modified = False
    End Sub

    Public Sub CleanupCurrentResult()
        _runQueryInfo = Nothing
        _runExportInfo = Nothing
        _runTimer = Nothing
        txtMessages.Text = ""
        ctlTabs.TabPages.Remove(tabResults)
        ctlTabs.TabPages.Remove(tabMessages)
        While ctlTabs.TabPages.Count > 0
            Dim curTab = ctlTabs.TabPages.Item(0)
            ctlTabs.TabPages.Remove(curTab)
            While curTab.Controls.Count > 0
                Dim curCtl = curTab.Controls.Item(0)
                curTab.Controls.Remove(curCtl)
                curCtl.Dispose()
            End While
            curTab.Dispose()
        End While
        While ctlResultsSplitter.TabPages.Count > 0
            Dim curTab = ctlResultsSplitter.TabPages.Item(0)
            ctlResultsSplitter.RemoveTab(curTab)
            While curTab.Controls.Count > 0
                Dim curCtl = curTab.Controls.Item(0)
                curTab.Controls.Remove(curCtl)
                curCtl.Dispose()
            End While
            curTab.Dispose()
        End While
        GC.Collect()

        If (ViewResultsAsList) Then ctlTabs.TabPages.Add(tabResults)
        ctlTabs.TabPages.Add(tabMessages)
    End Sub

    Private Sub AppendMessage(pMessage As String)
        If (Me.InvokeRequired) Then Me.Invoke(Sub() AppendMessage(pMessage)) : Return
        txtMessages.Text += pMessage + Environment.NewLine
        ctlTabs.SelectTab(ctlTabs.TabCount - 1)
    End Sub


    Private Class ThreadRunData
        Public Enum eRunType
            Query
            Export
        End Enum
        Public Property RunType As eRunType
        Public Property Thread As Threading.Thread
        Public Property RequestCancel As Boolean
        Public Property SelectHelper As Util.SelectHelper
        Public Property RunError As Exception
        Public Property CurrentBatchStartTime As DateTime
        Public Property LastUiUpdate As DateTime
        Public Property TriggerUiUpdate As Boolean
    End Class

    Private Class DataTableResult
        Public Property Table As New DataTable
        Public Property IsCancelled As Boolean
        Public Property DisplayControl As ctlQueryResult
        Public Property Complete As Boolean
    End Class

    Private Class DoQueryInfo
        Public Property Query As String
        Public Property Results As List(Of DataTableResult)
    End Class

    Public Enum eExportMode
        Csv
        Excel
        CodeGen
        CodeGenByRow
    End Enum

    Private Class DoExportInfo
        Public Property Query As String
        Public Property CsvDelimiter As String
        Public Property Filename As String

        Public Property ExportMode As eExportMode
        Public Property SavedResults As List(Of DataTable)
        Public Property CodeGenTemplateFilename As String
        Public Property CodeGenByRowOutputPath As String
        Public Property CodeGenByRowFilenameField As String
    End Class


End Class
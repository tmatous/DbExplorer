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



Public Class frmMain
    Implements IFormGroupKey

    Public Property FormGroupKey As String Implements IFormGroupKey.FormGroupKey
    Public Property IsFormGroupParent As Boolean Implements IFormGroupKey.IsFormGroupParent

    Private _embedDll As New EmbedDll("EmbedDllData")

    Private Const SQL_INDENT = "    "

    Private _attachedConn As Settings.Connection = Nothing
    Private _attachedConnInfo As Rational.DB.DbConnectionInfo
    Private _dbDef As DbSchemaTools.DbInfo
    Private _tableDefs As List(Of DbSchemaTools.TableInfo)
    Private _sqlGen As GenSql = Nothing
    Private _showQueryFormsTabbed As Boolean = True


    Public Shared Sub Launch(pConn As Settings.Connection, pLoadSchema As Boolean)
        Dim frm As New frmMain
        frm.Show()
        If (pConn IsNot Nothing) Then
            frm.DoAttach(pConn, pLoadSchema)
        Else
            frm.mnuDatabaseConnect_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub frmMain_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        ExitAppIfNoWindows()
    End Sub

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If (Not CleanupChildForms()) Then
            e.Cancel = True
            Return
        End If
        PromptSaveSettingsIfLastWindow()
    End Sub


    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.FormGroupKey = Guid.NewGuid.ToString
        Me.IsFormGroupParent = True
        EnableMenus(eMenuMode.NotAttached)
        If (Not System.Environment.Is64BitProcess) Then
            Me.Text += "32"
        End If
    End Sub

    Private Function CleanupChildForms() As Boolean
        Dim childForms = GetOpenFormGroupChildForms(Me.FormGroupKey)
        While (childForms.Count > 0)
            Dim frm = childForms(0)
            If (Not frm.IsDisposed) Then frm.Close()
            If (Not frm.IsDisposed) Then Return False
            childForms.Remove(frm)
        End While
        Return True
    End Function

    Private Sub DoAttach(pConn As Settings.Connection, pLoadSchema As Boolean)
        _attachedConn = Nothing
        _attachedConnInfo = Nothing
        EnableMenus(eMenuMode.NotAttached)
        Dim ci As Rational.DB.DbConnectionInfo
        Using New WaitCursorHandler(Me)
            Try
                ci = Util.CreateConnInfo(pConn)
                If (ci Is Nothing) Then Return

                If (pLoadSchema) Then
                    lblDatabase.Text = "Loading schema..."
                    Dim loadEx As Exception = Nothing
                    Dim bgLoad =
                    Sub()
                        Try
                            _dbDef = DbSchemaTools.SchemaLoader.LoadDbSchema(ci, AddressOf SchemaLoaderOnError)
                        Catch ex As Exception
                            loadEx = ex
                        End Try
                    End Sub
                    Dim asRes = bgLoad.BeginInvoke(Nothing, Nothing)
                    While (Not asRes.IsCompleted)
                        Threading.Thread.Sleep(100)
                        Application.DoEvents()
                    End While

                    If (loadEx IsNot Nothing) Then Throw loadEx
                Else
                    _dbDef = New DbSchemaTools.DbInfo()
                End If

                _tableDefs = New List(Of DbSchemaTools.TableInfo)()
                For Each tbl In _dbDef.Tables
                    _tableDefs.Add(tbl)
                Next
                For Each vw In _dbDef.Views
                    _tableDefs.Add(vw)
                Next
                For Each tvf In _dbDef.TableFunctions
                    _tableDefs.Add(tvf)
                Next

                PopulateTables(_tableDefs)
                ShowColumns()

                If (pLoadSchema) Then
                    EnableMenus(eMenuMode.Full)
                    lblDatabase.Text = pConn.Name
                Else
                    EnableMenus(eMenuMode.NoSchema)
                    lblDatabase.Text = String.Format("{0}  (schema not loaded)", pConn.Name)
                End If
                Me.Text = String.Format("DbExplorer{0} - {1}", If((Not System.Environment.Is64BitProcess), "32", ""), pConn.Name)
                txtTableFilter.Focus()

                _sqlGen = New GenSql(ci)

                _attachedConn = pConn
                _attachedConnInfo = ci
            Catch ex As Exception
                EnableMenus(eMenuMode.NotAttached)
                lblDatabase.Text = ""
                Util.ShowError(ex.Message)
            End Try
        End Using
    End Sub


    Private _ignoreSchemaLoaderErrors As Boolean = False
    Private Sub SchemaLoaderOnError(sender As Object, args As DbSchemaTools.SchemaLoaderErrorEventArgs)
        If (Me.InvokeRequired) Then Me.Invoke(Sub() SchemaLoaderOnError(sender, args))
        If (args.ErrorType = DbSchemaTools.SchemaLoaderErrorEventArgs.eErrorType.UnsupportedType) Then
            If (Not _ignoreSchemaLoaderErrors) Then
                Dim conf = MessageBox.Show(args.Message + vbCrLf + vbCrLf + "Do you want to ignore errors of this type?", "Prompt", MessageBoxButtons.YesNoCancel)
                If (conf = Windows.Forms.DialogResult.Cancel) Then
                    Throw New NotImplementedException(args.Message)
                ElseIf (conf = Windows.Forms.DialogResult.Yes) Then
                    _ignoreSchemaLoaderErrors = True
                End If
            End If
        End If
    End Sub


    Private Enum eMenuMode
        NotAttached
        NoSchema
        Full
    End Enum

    Private Sub EnableMenus(pMode As eMenuMode)
        mnuDbActions.Enabled = True
        mnuGenerate.Enabled = (pMode = eMenuMode.Full)
        mnuDatabaseViewSummary.Enabled = (pMode = eMenuMode.Full)
        mnuDatabaseSearchNames.Enabled = (pMode = eMenuMode.Full)
        mnuDatabaseSearchData.Enabled = (pMode = eMenuMode.Full)
        mnuDatabaseQuery.Enabled = ((pMode = eMenuMode.Full) OrElse (pMode = eMenuMode.NoSchema))
        mnuDatabaseCodeGen.Enabled = (pMode = eMenuMode.Full)
        mnuTableActions.Enabled = (pMode = eMenuMode.Full)
    End Sub

    Private Function CreateConnection() As System.Data.Common.DbConnection
        Dim conn = _attachedConnInfo.GetFactory().CreateConnection()
        conn.ConnectionString = _attachedConnInfo.ConnectionString
        Return conn
    End Function

#Region "Menus"

    Private Sub mnuDatabaseConnect_Click(sender As Object, e As EventArgs) Handles mnuDatabaseConnect.Click
        Dim res = LaunchConnectionsDialog(Me)
        If (res.DialogResult <> Windows.Forms.DialogResult.OK) Then Return
        If (_attachedConn Is Nothing) Then
            DoAttach(res.Connection, res.LoadSchema)
        Else
            frmMain.Launch(res.Connection, res.LoadSchema)
        End If
    End Sub

    Private Sub mnuExit_Click(sender As Object, e As EventArgs) Handles mnuExit.Click
        Me.Close()
    End Sub

    Private Sub mnuDatabaseViewSummary_Click(sender As Object, e As EventArgs) Handles mnuDatabaseViewSummary.Click
        Try
            Dim codeFn = IO.Path.Combine(Util.GetDefaultCodeGenFolder(), "DatabaseSummary.HTM.csrzr")
            If (Not IO.File.Exists(codeFn)) Then Throw New Exception("Unable to find DatabaseSummary script. Please check your installation.")

            Dim sourceCode = IO.File.ReadAllText(codeFn)
            Dim outputFilename = Util.GetTempFilename("htm")

            Dim output = DbCodeGen.CodeGenerator.RenderTemplateForSchema(sourceCode, _dbDef, _dbDef.Tables)
            IO.File.WriteAllText(outputFilename, output)
            System.Diagnostics.Process.Start(outputFilename)
        Catch ex As Exception
            Util.ShowError(ex.Message)
        End Try
    End Sub

    Private Sub mnuDatabaseSearchNames_Click(sender As Object, e As EventArgs) Handles mnuDatabaseSearchNames.Click
        Dim str = InputBox("Enter substring to search for", "Input")
        If (String.IsNullOrWhiteSpace(str)) Then Return
        DoSearchFieldNames(str)
    End Sub

    Private Sub mnuDatabaseSearchData_Click(sender As Object, e As EventArgs) Handles mnuDatabaseSearchData.Click
        DoSearchData()
    End Sub

    Private Sub mnuDatabaseQuery_Click(sender As Object, e As EventArgs) Handles mnuDatabaseQuery.Click
        LaunchQueryForm("", pQuickView:=False, pReuseForm:=False)
    End Sub

    Private Sub mnuTableQuery_Click(sender As Object, e As EventArgs) Handles mnuTableQuery.Click
        Dim sql = ""
        Dim qv = False
        Dim selTables = GetSelectedTables()
        If (selTables.Count = 1) Then
            sql = GetSelectQuery(selTables(0), 100).GetSql()
            qv = True
        End If
        LaunchQueryForm(sql, pQuickView:=qv, pReuseForm:=True)
    End Sub

    Private Sub mnuTableEdit_Click(sender As Object, e As EventArgs) Handles mnuTableEdit.Click
        Dim sql = ""
        Dim selTables = GetSelectedTables()
        If (selTables.Count <> 1) Then Return
        Dim selTable = selTables(0)
        sql = GetSelectQuery(selTable, 20).GetSql()

        Dim frm As New frmEditDb(_attachedConnInfo, sql)
        frm.FormGroupKey = Me.FormGroupKey
        frm.Text = String.Format("Edit Data - {0}", _attachedConn.Name)
        frm.Show()
    End Sub

    Private Sub mnuTableViewSummary_Click(sender As Object, e As EventArgs) Handles mnuTableViewSummary.Click
        Dim selTables = GetSelectedTables()
        If (selTables.Count = 0) Then Return

        Try
            Dim codeFn = IO.Path.Combine(Util.GetDefaultCodeGenFolder(), "TableSummary.HTM.csrzr")
            If (Not IO.File.Exists(codeFn)) Then Throw New Exception("Unable to find TableSummary script. Please check your installation.")

            Dim sourceCode = IO.File.ReadAllText(codeFn)
            Dim outputFilename = Util.GetTempFilename("htm")

            Dim output = DbCodeGen.CodeGenerator.RenderTemplateForSchema(sourceCode, _dbDef, selTables)
            IO.File.WriteAllText(outputFilename, output)
            System.Diagnostics.Process.Start(outputFilename)
        Catch ex As Exception
            Util.ShowError(ex.Message)
        End Try
    End Sub

    Private Function GetSelectQuery(pSelectedTable As DbSchemaTools.TableInfo, Optional pMaxRows As Int32? = Nothing) As Rational.DB.DbSelectStatement
        Dim db = New Rational.DB.Database(_attachedConnInfo)
        Dim stmt = db.CreateSelectStatement()
        Dim schemaPrefix = ""
        If (Not String.IsNullOrWhiteSpace(pSelectedTable.SchemaName)) Then
            schemaPrefix = String.Format("{0}.", stmt.QuoteIdentifier(pSelectedTable.SchemaName))
        End If
        Dim args = ""
        If (pSelectedTable.TableType = DbSchemaTools.TableInfo.eTableType.TableFunction) Then
            args = "()"
        End If

        stmt.From.Append(String.Format("{0}{1}{2}", schemaPrefix, stmt.QuoteIdentifier(pSelectedTable.Name), args))
        stmt.Fields.Add((From i In pSelectedTable.Columns Select i.Name))
        stmt.ResultMax = pMaxRows

        Return stmt
    End Function

    Private Sub mnuTableExportCsv_Click(sender As Object, e As EventArgs) Handles mnuTableExportCsv.Click
        Dim selTables = GetSelectedTables()
        If (selTables.Count = 0) Then Return

        Dim delim = Util.PromptDelimiter()
        If (delim Is Nothing) Then Return
        If (selTables.Count > 1) Then
            Dim savePath = ""
            Using frm As New Windows.Forms.FolderBrowserDialog()
                If (frm.ShowDialog() <> Windows.Forms.DialogResult.OK) Then Return
                savePath = frm.SelectedPath
            End Using
            Using New WaitCursorHandler(Me)
                ExportTablesAsCsv(selTables, savePath, delim)
            End Using
            System.Diagnostics.Process.Start(savePath)
        Else
            Dim fname = Util.PromptSaveFile("csv")
            If (String.IsNullOrEmpty(fname)) Then Return
            Using New WaitCursorHandler(Me)
                ExportTableAsCsv(selTables(0), fname, delim)
            End Using
        End If
    End Sub

    Private Sub mnuTableExportInserts_Click(sender As Object, e As EventArgs) Handles mnuTableExportInserts.Click
        Dim selTables = GetSelectedTables()
        If (selTables.Count <> 1) Then Return

        Dim selTable = selTables(0)

        Dim sql = GetSelectQuery(selTable).GetSql()
        Dim destTable As String = selTable.Name
        If (PromptForm.PromptForSingleLineString(destTable, "Enter destination table") <> Windows.Forms.DialogResult.OK) Then Return

        Dim fname As String = Nothing
        Dim saveTo = CustomMessageBox.Show("Export results to:", {"File", "Output Window"})
        If (saveTo = "File") Then
            fname = Util.PromptSaveFile("sql")
            If (String.IsNullOrEmpty(fname)) Then Return
        End If
        Using New WaitCursorHandler(Me)
            txtResult.Text = ExportDataAsInserts(sql, destTable, fname, Nothing)
        End Using
    End Sub

    Private Sub mnuTableImportCsv_Click(sender As Object, e As EventArgs) Handles mnuTableImportCsv.Click
        Dim selTables = GetSelectedTables()
        If (selTables.Count <> 1) Then Return

        Dim selTable = selTables(0)

        Dim fname As String
        Using frm As New OpenFileDialog()
            frm.Filter = "CSV Files|*.csv|All Files|*.*"
            frm.RestoreDirectory = True
            frm.InitialDirectory = Util.GetCurrentOutputFolder()
            If (frm.ShowDialog() <> DialogResult.OK) Then Return
            fname = frm.FileName
            Util.SetCurrentOutputFolder(IO.Path.GetDirectoryName(frm.FileName))
        End Using
        Using New WaitCursorHandler(Me)
            ImportDataAsCsv(selTable, fname)
        End Using
        MessageBox.Show("Done", "Message", MessageBoxButtons.OK)

    End Sub

    Private Sub mnuGenerateSelect_Click(sender As Object, e As EventArgs) Handles mnuGenerateSelect.Click
        Dim selTables = GetSelectedTables()
        If (selTables.Count <> 1) Then Return

        Dim selTable = selTables(0)
        Dim als = ""
        als = InputBox("Enter an alias for the table, if desired.", "Prompt", als)
        txtResult.Text = _sqlGen.GenerateSelectSql(selTable, als, pIncludeWhere:=True)
    End Sub

    Private Sub mnuGenerateInsertWithParams_Click(sender As Object, e As EventArgs) Handles mnuGenerateInsertWithParams.Click
        Dim selTables = GetSelectedTables()
        If (selTables.Count <> 1) Then Return

        Dim selTable = selTables(0)
        txtResult.Text = _sqlGen.GenerateInsertWithParams(selTable, pIncludeIdentity:=False)
    End Sub

    Private Sub mnuGenerateInsertWithValues_Click(sender As Object, e As EventArgs) Handles mnuGenerateInsertWithValues.Click
        Dim selTables = GetSelectedTables()
        If (selTables.Count <> 1) Then Return

        Dim selTable = selTables(0)
        txtResult.Text = _sqlGen.GenerateInsertWithValues(selTable, pIncludeIdentity:=False)
    End Sub

    Private Sub mnuGenerateUpdateWithParams_Click(sender As Object, e As EventArgs) Handles mnuGenerateUpdateWithParams.Click
        Dim selTables = GetSelectedTables()
        If (selTables.Count <> 1) Then Return

        Dim selTable = selTables(0)
        txtResult.Text = _sqlGen.GenerateUpdateWithParams(selTable, pUpdateKeyFields:=False, pIncludeWhere:=True)
    End Sub

    Private Sub mnuDatabaseCodeGen_Click(sender As Object, e As EventArgs) Handles mnuDatabaseCodeGen.Click
        Dim codeFn = Util.PromptRazorTemplate()
        If (String.IsNullOrEmpty(codeFn)) Then Return

        Dim sourceCode = IO.File.ReadAllText(codeFn)
        Dim fileType = DbCodeGen.CodeGenerator.GetTemplateOutputFormat(sourceCode).ToString().ToLower()

        Dim resultAction = CustomMessageBox.Show("What do you want to do with the results?", {"Save File", "Open File", "Output Window"})
        Dim outputFilename = ""
        If (resultAction = "Save File") Then
            outputFilename = Util.PromptSaveFile(fileType)
            If (String.IsNullOrEmpty(outputFilename)) Then Return
        Else
            outputFilename = Util.GetTempFilename(fileType)
        End If

        Try
            Dim output = DbCodeGen.CodeGenerator.RenderTemplateForSchema(sourceCode, _dbDef, _dbDef.Tables)

            If (resultAction = "Save File") Then
                IO.File.WriteAllText(outputFilename, output)
                txtResult.Text = String.Format("Saved to '{0}'.", outputFilename)
            ElseIf (resultAction = "Open File") Then
                IO.File.WriteAllText(outputFilename, output)
                txtResult.Text = String.Format("Saved to '{0}'.", outputFilename)
                System.Diagnostics.Process.Start(outputFilename)
            Else
                txtResult.Text = output
            End If
        Catch ex As Exception
            Util.ShowError(ex.Message)
        End Try
    End Sub


    Private Sub mnuTableCodeGen_Click(sender As Object, e As EventArgs) Handles mnuTableCodeGen.Click
        Dim selTables = GetSelectedTables()
        If (selTables.Count = 0) Then Return

        txtResult.Text = ""
        Dim codeFn = Util.PromptRazorTemplate()
        If (String.IsNullOrEmpty(codeFn)) Then Return

        Dim sourceCode = IO.File.ReadAllText(codeFn)
        Dim fileType = DbCodeGen.CodeGenerator.GetTemplateOutputFormat(sourceCode).ToString().ToLower()

        Dim generateAs = "Single File"
        If (selTables.Count > 1) Then
            generateAs = CustomMessageBox.Show("Multiple tables were selected. What do you want to generate?", {"Single File", "Multiple Files"})
        End If

        If (generateAs = "Single File") Then
            Dim resultAction = CustomMessageBox.Show("What do you want to do with the results?", {"Save File", "Open File", "Output Window"})
            Dim outputFilename = ""
            If (resultAction = "Save File") Then
                outputFilename = Util.PromptSaveFile(fileType)
                If (String.IsNullOrEmpty(outputFilename)) Then Return
            Else
                outputFilename = Util.GetTempFilename(fileType)
            End If

            Try
                Dim output = DbCodeGen.CodeGenerator.RenderTemplateForSchema(sourceCode, _dbDef, selTables)
                If (resultAction = "Save File") Then
                    IO.File.WriteAllText(outputFilename, output)
                    txtResult.Text = String.Format("Saved to '{0}'.", outputFilename)
                ElseIf (resultAction = "Open File") Then
                    IO.File.WriteAllText(outputFilename, output)
                    txtResult.Text = String.Format("Saved to '{0}'.", outputFilename)
                    System.Diagnostics.Process.Start(outputFilename)
                Else
                    txtResult.Text = output
                End If
            Catch ex As Exception
                Util.ShowError(ex.Message)
            End Try
        Else
            Dim savePath = ""
            Using frm As New Windows.Forms.FolderBrowserDialog()
                If (frm.ShowDialog() <> Windows.Forms.DialogResult.OK) Then Return
                savePath = frm.SelectedPath
            End Using
            Dim filenamePattern = "{TableName}." + fileType
            filenamePattern = InputBox("Enter the pattern for the files to generate.", "Prompt", filenamePattern)
            If (String.IsNullOrWhiteSpace(filenamePattern)) Then Return

            Try
                For Each tbl In selTables
                    Dim output = DbCodeGen.CodeGenerator.RenderTemplateForSchema(sourceCode, _dbDef, {tbl})
                    Dim outputFilename = IO.Path.Combine(savePath, Util.CleanFileName(filenamePattern.Replace("{TableName}", tbl.Name)))
                    If (IO.File.Exists(outputFilename)) Then Throw New Exception(String.Format("File exists: {0}", outputFilename))
                    IO.File.WriteAllText(outputFilename, output)
                    txtResult.Text += String.Format("Saved '{0}'.{1}", outputFilename, Environment.NewLine)
                Next
                System.Diagnostics.Process.Start(savePath)
            Catch ex As Exception
                Util.ShowError(ex.Message)
            End Try
        End If
    End Sub

    Private Sub mnuHelpAbout_Click(sender As Object, e As EventArgs) Handles mnuHelpAbout.Click
        Using frm = New frmAbout
            frm.ShowDialog()
        End Using
    End Sub

    Private Sub txtResult_KeyDown(sender As Object, e As KeyEventArgs) Handles txtResult.KeyDown
        If ((e.KeyCode = Keys.A) AndAlso e.Control) Then
            txtResult.SelectAll()
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

#End Region




#Region "Table/column lists"

    Private _lstTables_SelectionChangingProgrammatically As Boolean = False

    Private Function GetSelectedTables() As List(Of DbSchemaTools.TableInfo)
        Dim res = New List(Of DbSchemaTools.TableInfo)
        For Each rw As DataGridViewRow In lstTables.SelectedRows
            Dim tbl = (From i In _tableDefs Where i.SchemaName = rw.Cells("Schema").Value.ToString AndAlso i.Name = rw.Cells("Name").Value.ToString Select i).Single
            res.Add(tbl)
        Next
        Return res
    End Function

    Private Sub PopulateTables(pTables As IEnumerable(Of DbSchemaTools.TableInfo))
        _lstTables_SelectionChangingProgrammatically = True
        Dim dtTables = New DataTable()
        dtTables.Columns.Add("Name")
        dtTables.Columns.Add("Schema")
        dtTables.Columns.Add("Type")
        For Each tbl In pTables
            dtTables.Rows.Add({tbl.Name, tbl.SchemaName, tbl.TableType.ToString})
        Next

        lstTables.DataSource = dtTables
        lstTables.ClearSelection()
        lstTables.Columns("Schema").MinimumWidth = 50
        lstTables.Columns("Type").MinimumWidth = 50
        _lstTables_SelectionChangingProgrammatically = False
    End Sub

    Private Sub txtTableFilter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTableFilter.TextChanged
        If (_attachedConn Is Nothing) Then Return

        Dim selTables = GetSelectedTables()

        Dim findStr = txtTableFilter.Text.Trim.ToUpper
        Dim matchTables As New List(Of DbSchemaTools.TableInfo)
        For Each tbl In _tableDefs
            If (selTables.Contains(tbl) OrElse tbl.Name.ToUpper.Contains(findStr)) Then
                matchTables.Add(tbl)
            End If
        Next

        PopulateTables(matchTables)

        're-select rows
        _lstTables_SelectionChangingProgrammatically = True
        Dim selTableNames = (From t In selTables Select t.Name).ToList()
        For Each rw As DataGridViewRow In lstTables.Rows
            If (selTableNames.Contains(rw.Cells("Name").Value.ToString)) Then
                rw.Selected = True
            End If
        Next
        _lstTables_SelectionChangingProgrammatically = False
        ShowColumns()
    End Sub

    Private Sub txtColumnFilter_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtColumnFilter.TextChanged
        ShowColumns()
    End Sub

    Private Sub lstTables_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles lstTables.DataBindingComplete
        Dim grd = lstTables
        grd.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
        If (grd.Columns.GetColumnsWidth(DataGridViewElementStates.None) < grd.Width) Then
            'fill up remaining space
            grd.Columns("Name").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub

    Private Sub lstTables_SelectionChanged(sender As Object, e As EventArgs) Handles lstTables.SelectionChanged
        If (_attachedConn Is Nothing) Then Return
        If (_lstTables_SelectionChangingProgrammatically) Then Return

        ShowColumns()
    End Sub

    Private Sub lstTables_DoubleClick(sender As Object, e As EventArgs) Handles lstTables.DoubleClick
        mnuTableQuery_Click(Nothing, Nothing)
    End Sub

    Private Sub lstTables_KeyPress(sender As Object, e As KeyPressEventArgs) Handles lstTables.KeyPress
    End Sub

    Private Sub lstTables_KeyDown(sender As Object, e As KeyEventArgs) Handles lstTables.KeyDown
        If (e.KeyCode = Keys.Enter) Then
            e.Handled = True
            mnuTableQuery_Click(Nothing, Nothing)
        ElseIf (e.KeyCode = Keys.C) AndAlso (e.Modifiers = Keys.Control) Then
            mnuCopyTableNames_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub lstColumns_KeyDown(sender As Object, e As KeyEventArgs) Handles lstColumns.KeyDown
        If (e.KeyCode = Keys.C) AndAlso (e.Modifiers = Keys.Control) Then
            mnuCopyColumnNames_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub ShowColumns()
        lstColumns.DataSource = Nothing
        If (_attachedConn Is Nothing) Then Return
        Dim selTables = GetSelectedTables()
        If (selTables.Count <> 1) Then Return

        Dim selTable = selTables(0)
        Dim dtColumns = New DataTable()
        dtColumns.Columns.Add("Name")
        dtColumns.Columns.Add("Key")
        dtColumns.Columns.Add("DataType")
        dtColumns.Columns.Add("IsNotNull")

        Dim findStr = txtColumnFilter.Text.Trim.ToUpper
        Dim filterMsg = If(String.IsNullOrWhiteSpace(findStr), "", " (filtered)")
        dtColumns.Rows.Add(String.Format("-- {0}{1} --", selTable.Name, filterMsg))

        For Each col In selTable.Columns
            If (String.IsNullOrWhiteSpace(findStr) OrElse col.Name.ToUpper.Contains(findStr)) Then
                Dim iskey = ""
                If (col.InPrimaryKey) Then iskey += "PK "
                If (col.InForeignKey) Then iskey += "FK"
                Dim isNN = If(col.Nullable, "", "NN")
                dtColumns.Rows.Add({col.Name, iskey, col.DdlDataType, isNN})
            End If
        Next

        lstColumns.DataSource = dtColumns
        lstColumns.ClearSelection()
        lstColumns.Columns("Key").MinimumWidth = 30
        lstColumns.Columns("DataType").MinimumWidth = 120
        lstColumns.Columns("DataType").HeaderText = "Data Type"
        lstColumns.Columns("IsNotNull").MinimumWidth = 30
        lstColumns.Columns("IsNotNull").HeaderText = "NN"
    End Sub

    Private Sub lstColumns_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles lstColumns.DataBindingComplete
        Dim grd = lstColumns
        grd.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
        If (grd.Columns.GetColumnsWidth(DataGridViewElementStates.None) < grd.Width) Then
            'fill up remaining space
            grd.Columns("Name").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub

#End Region




    Private Sub btnReload_Click(sender As Object, e As EventArgs) Handles btnReload.Click
        If (_attachedConn Is Nothing) Then Return
        DoAttach(_attachedConn, pLoadSchema:=True)
    End Sub








    Private Function ExportDataAsInserts(pSourceSql As String, pDestTable As String, pFilename As String, pBatchSize As Int32?) As String
        Dim db As New Rational.DB.Database(_attachedConnInfo)
        Dim stmt = db.CreateStatement(Of Rational.DB.DbStatement)()
        stmt.Statement.AppendRaw(pSourceSql)
        Dim dt = db.SelectDataTable(stmt)

        Dim flds = ""
        Dim delim = ""
        For Each col As DataColumn In dt.Columns
            flds += String.Format("{0}{1}", delim, _sqlGen.QuoteIdentifier(col.ColumnName))
            delim = ", "
        Next

        Dim outFile As IO.TextWriter
        If (String.IsNullOrEmpty(pFilename)) Then
            outFile = New IO.StringWriter()
        Else
            outFile = New IO.StreamWriter(pFilename)
        End If

        Try
            Dim vals As String
            Dim batchLineNum As Int32 = 1
            For Each dr As DataRow In dt.Rows

                vals = ""
                delim = ""
                For Each col As DataColumn In dt.Columns
                    Dim curVal = ""
                    If (dr.Item(col.ColumnName) Is DBNull.Value) Then
                        curVal = "NULL"
                    Else
                        Select Case col.DataType
                            Case GetType(String)
                                curVal = String.Format("'{0}'", dr.Item(col.ColumnName).ToString.Replace("'", "''"))
                            Case GetType(Int32)
                                curVal = dr.Item(col.ColumnName).ToString
                            Case GetType(Boolean)
                                curVal = If((DirectCast(dr.Item(col.ColumnName), Boolean) = True), "1", "0")
                            Case GetType(DateTime)
                                curVal = String.Format("'{0}'", dr.Item(col.ColumnName).ToString)
                            Case GetType(Decimal)
                                curVal = dr.Item(col.ColumnName).ToString
                            Case Else
                                curVal = String.Format("'{0}'", dr.Item(col.ColumnName).ToString.Replace("'", "''"))
                        End Select
                    End If
                    vals += String.Format("{0}{1}", delim, curVal)
                    delim = ", "
                Next
                outFile.Write(String.Format("INSERT INTO {0} ( {1} ) ", _sqlGen.QuoteIdentifier(pDestTable), flds))
                outFile.WriteLine()
                outFile.Write(SQL_INDENT)
                outFile.Write(String.Format("VALUES ( {0} )", vals))
                outFile.WriteLine()

                'batch output
                batchLineNum += 1
                If (pBatchSize.HasValue AndAlso (batchLineNum > pBatchSize.Value)) Then
                    outFile.WriteLine("GO")
                    outFile.WriteLine()
                    batchLineNum = 1
                End If
            Next
            If (String.IsNullOrEmpty(pFilename)) Then
                Return outFile.ToString
            Else
                Return "Saved to " + pFilename
            End If
        Finally
            outFile.Close()
        End Try
    End Function

    Private Sub ExportTableAsCsv(pTable As DbSchemaTools.TableInfo, pDestFile As String, pDelim As String)
        ExportTablesAsCsv({pTable}, {pDestFile}, pDelim)
    End Sub

    Private Sub ExportTablesAsCsv(pTables As IEnumerable(Of DbSchemaTools.TableInfo), pDestFolder As String, pDelim As String)
        Dim files = New List(Of String)()
        For Each tbl In pTables
            files.Add(IO.Path.Combine(pDestFolder, Util.CleanFileName(tbl.Name + ".csv")))
        Next
        ExportTablesAsCsv(pTables.ToList, files, pDelim)
    End Sub

    Private Sub ExportTablesAsCsv(pTables As IList(Of DbSchemaTools.TableInfo), pDestFiles As IList(Of String), pDelim As String)
        Using waitform = New WaitForm(Me, "Exporting data...", pShowProgress:=True)
            Using conn = CreateConnection()
                conn.ConnectionString = _attachedConnInfo.ConnectionString
                conn.Open()

                Dim cur = 1
                Dim max = pTables.Count
                For tblIdx = 0 To pTables.Count - 1
                    Dim tblBase = pTables.Item(tblIdx)
                    If (Not TypeOf tblBase Is DbSchemaTools.TableInfo) Then Continue For
                    Dim tbl = DirectCast(tblBase, DbSchemaTools.TableInfo)
                    waitform.UpdateProgress(CInt((cur / max) * 100))
                    waitform.UpdateMessage(String.Format("Exporting '{0}'...", tbl.Name))
                    Try
                        Using cmd = conn.CreateCommand()
                            cmd.CommandText = GetSelectQuery(tbl).GetSql()
                            cmd.CommandTimeout = Util.GetMaxTimeout(_attachedConnInfo)
                            Using rdr = cmd.ExecuteReader()
                                Util.DataReaderToCsvFile(rdr, includeHeader:=True, fileName:=pDestFiles.Item(tblIdx), delimiter:=pDelim)
                            End Using
                        End Using
                    Catch ex As Exception
                        Util.ShowError(ex.Message)
                    End Try
                    cur += 1
                Next
            End Using
        End Using
    End Sub

    Private Sub ImportDataAsCsv(pTable As DbSchemaTools.TableInfo, pFilename As String)
        If (Not TypeOf pTable Is DbSchemaTools.TableInfo) Then Util.ShowError("Not supported on this table type") : Return

        Using conn = CreateConnection()
            conn.Open()

            'don't include any type of computed column
            Dim settableColumns = (From col In pTable.Columns Where (Not col.IsIdentity) AndAlso (Not col.IsRowVersion) AndAlso (Not col.IsComputed)).ToList
            Dim cmd = conn.CreateCommand()
            cmd.CommandText = _sqlGen.GenerateInsertWithParams(pTable, pIncludeIdentity:=False)
            cmd.CommandTimeout = Util.GetDefaultTimeout(_attachedConnInfo)
            Dim params As New Dictionary(Of String, System.Data.Common.DbParameter)
            For Each col In settableColumns
                Dim prm = cmd.CreateParameter()
                prm.ParameterName = String.Format("@{0}", col.Name)
                cmd.Parameters.Add(prm)
                params.Add(col.Name, prm)
            Next

            Using rdr = New IO.StreamReader(pFilename)
                Using csvFile As New LumenWorks.Framework.IO.Csv.CsvReader(rdr, hasHeaders:=True)
                    While csvFile.ReadNextRecord
                        For Each col In settableColumns
                            Dim val As Object = csvFile.Item(col.Name).ToString
                            If ((val.ToString = "") AndAlso (col.DataClass <> DbSchemaTools.eDataClass.StringType)) Then
                                'empty column stores empty string for strings, null for other types
                                val = DBNull.Value
                            End If
                            params.Item(col.Name).Value = val
                        Next
                        cmd.ExecuteNonQuery()
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub DoSearchData()
        If (MessageBox.Show("A data search can be very time consuming and resource intensive on large databases. Are you sure?", "Confirm", MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.Cancel) Then Return

        Dim dataType As DbSchemaTools.eDataClass = DbSchemaTools.eDataClass.StringType
        Dim statementPattern = ""
        Dim dataTypeInt As Int32 = CInt(dataType)
        If (PromptForm.PromptForValueFromList(dataTypeInt, Util.EnumToDict(GetType(DbSchemaTools.eDataClass)), "Select data type to search") = DialogResult.Cancel) Then Return
        dataType = DirectCast(dataTypeInt, DbSchemaTools.eDataClass)

        Dim db = New Rational.DB.Database(_attachedConnInfo)
        statementPattern = String.Format("SELECT * FROM {0}{{0}}{1} WHERE {0}{{1}}{1} ", db.QuotePrefix, db.QuoteSuffix)
        Dim searchDesc = ""
        If (dataType = DbSchemaTools.eDataClass.StringType) Then
            Dim searchSimple As String = ""
            If (PromptForm.PromptForSingleLineString(searchSimple, "Enter value to search for") = DialogResult.Cancel) Then Return
            statementPattern += String.Format("LIKE '%{0}%'", searchSimple.Replace("'", "''"))
            searchDesc = searchSimple
        ElseIf (dataType = DbSchemaTools.eDataClass.IntegerType) Then
            Dim searchSimple As Int32 = 0
            If (PromptForm.PromptForNumber(searchSimple, "Enter value to search for") = DialogResult.Cancel) Then Return
            statementPattern += String.Format("= {0}", searchSimple)
            searchDesc = searchSimple.ToString
        ElseIf (dataType = DbSchemaTools.eDataClass.FloatingPointType) Then
            Dim searchSimple As String = "0"
            If (PromptForm.PromptForSingleLineString(searchSimple, "Enter value to search for") = DialogResult.Cancel) Then Return
            Dim searchSimpleNum As Double = 0
            If (Not Double.TryParse(searchSimple, searchSimpleNum)) Then Return
            statementPattern += String.Format("= {0}", searchSimpleNum)
            searchDesc = searchSimple
        ElseIf (dataType = DbSchemaTools.eDataClass.GuidType) Then
            Dim searchSimple As String = Guid.Empty.ToString
            If (PromptForm.PromptForSingleLineString(searchSimple, "Enter value to search for") = DialogResult.Cancel) Then Return
            Dim searchSimpleGuid As Guid = Guid.Empty
            If (Not Guid.TryParse(searchSimple, searchSimpleGuid)) Then Return
            statementPattern += String.Format("= '{0}'", searchSimpleGuid)
            searchDesc = searchSimple
        Else
            Throw New NotImplementedException
        End If
        If (PromptForm.PromptForMultilineString(statementPattern, "Enter search statement pattern") = DialogResult.Cancel) Then Return

        txtResult.Text = String.Format("--Searching for ""{0}""...", searchDesc) + vbNewLine
        Dim foundStmts As New List(Of String)
        Using New WaitCursorHandler(Me)
            For Each tbl In _tableDefs
                If (Not TypeOf tbl Is DbSchemaTools.TableInfo) Then Continue For
                For Each col In tbl.Columns
                    If (col.DataClass = dataType) Then
                        Dim sql = String.Format(statementPattern, tbl.Name, col.Name)
                        Dim stmt = New Rational.DB.DbStatement(String.Format("SELECT TOP 1 * FROM ( {0} ) AS MyCountQuery", sql))
                        Dim dt = db.SelectDataTable(stmt)
                        If (dt.Rows.Count > 0) Then
                            foundStmts.Add(sql)
                            txtResult.Text += sql + vbNewLine
                        End If
                    End If
                    Application.DoEvents()
                Next
            Next
        End Using
        txtResult.Text += "--Done." + vbNewLine

        'txtResult.Text = Join(foundStmts.ToArray, vbCrLf)
    End Sub

    Private Sub DoSearchFieldNames(pSubstring As String)
        Dim res As New System.Text.StringBuilder
        Dim toSearch = pSubstring.ToUpper
        For Each tbl In _tableDefs
            If (tbl.Name.ToUpper().Contains(toSearch)) Then
                res.exAppendFormatLine("Table: {0}", tbl.Name)
            End If
        Next
        If (res.Length > 0) Then res.AppendLine()
        For Each tbl In _tableDefs
            For Each col In tbl.Columns
                If (col.Name.ToUpper().Contains(toSearch)) Then
                    res.AppendLine(_sqlGen.GetSchemaText(col, pIncludeTableName:=True, pIncludeNullSetting:=False))
                End If
            Next
        Next
        If (res.Length = 0) Then res.Append("Not found")
        txtResult.Text = res.ToString
    End Sub



    Private _queryFormsParent As frmQuery = Nothing
    Private Sub LaunchQueryForm(pSql As String, pQuickView As Boolean, pReuseForm As Boolean)
        Dim frm As frmQuery
        If (_showQueryFormsTabbed AndAlso pReuseForm) Then
            If ((_queryFormsParent Is Nothing) OrElse (_queryFormsParent.IsDisposed)) Then
                _queryFormsParent = New frmQuery()
            End If
            frm = _queryFormsParent
        Else
            frm = New frmQuery()
        End If

        If (frm.WindowState = FormWindowState.Minimized) Then frm.WindowState = FormWindowState.Normal
        frm.FormGroupKey = Me.FormGroupKey
        frm.ConnInfo = _attachedConnInfo
        frm.Text = String.Format("Query - {0}", _attachedConn.Name)
        frm.AddQueryTab(pSql, pQuickView)

        If (Not frm.Visible) Then
            Util.ShowFormRelativeTo(frm, Me.Bounds)
        End If
    End Sub





#Region "Retired"

    Private Function Retired_GenerateSyncData() As String
        Dim selTables = GetSelectedTables()
        If (selTables.Count <> 1) Then Return ""

        Dim tbl = selTables(0)

        If (tbl.TableType <> DbSchemaTools.TableInfo.eTableType.Table) Then Return "Not supported for this table type"
        If (tbl.PkColumns.Count <> 1) Then Throw New Exception("Only implemented for tables with one primary key column")
        Dim onlyKeyCol = tbl.PkColumns(0).Name

        Dim syncTable = String.Format("#_SYNC_{0}", tbl.Name)
        Dim sb As New System.Text.StringBuilder

        Dim colList = ""
        Dim colListDelim = ""
        For Each col In tbl.Columns
            colList += String.Format("{0}{1}", colListDelim, _sqlGen.QuoteIdentifier(col.Name))
            colListDelim = ", "
        Next

        sb.exAppendFormatLine("--========== BEGIN synchronize {0} table", tbl.Name)
        sb.AppendLine()
        sb.exAppendFormatLine("PRINT 'Synchronizing {0}...'", tbl.Name)
        sb.AppendLine()
        sb.AppendLine("--create temp table for required data")

        sb.exAppendFormatLine("CREATE TABLE {0}", syncTable)
        sb.AppendLine("(")

        Dim colDefDelim = ""
        Dim pkDelim = ""
        Dim pkList = ""
        For Each col In tbl.Columns
            sb.Append(colDefDelim)
            sb.Append("  " + _sqlGen.GetSchemaText(col, pIncludeTableName:=False, pIncludeNullSetting:=True))
            colDefDelim = "," + vbNewLine
            If (col.InPrimaryKey) Then
                pkList += String.Format("{0}{1}", pkDelim, _sqlGen.QuoteIdentifier(col.Name))
            End If
        Next
        sb.AppendLine()

        If (pkList <> "") Then
            sb.AppendLine()
            sb.exAppendFormatLine("CONSTRAINT PK__SYNC_{0} PRIMARY KEY ( {1} )", tbl.Name, pkList)
        End If
        sb.AppendLine(")")

        sb.AppendLine()
        sb.AppendLine("--insert all required records into temp table")
        sb.AppendLine("SET NOCOUNT ON")
        sb.AppendLine()

        If (MessageBox.Show("Include data?", "Prompt", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes) Then
            Dim sql As String = _sqlGen.GenerateSelectSql(tbl, "", pIncludeWhere:=False)
            sb.Append(ExportDataAsInserts(sql, syncTable, Nothing, Nothing))
        Else
            sb.AppendLine("--TODO: add insert statements")
            sb.exAppendFormatLine("--INSERT {0} ( {1} ) VALUES ( )", _sqlGen.QuoteIdentifier(syncTable), colList)
            sb.AppendLine("RAISERROR('Add insert statements', 13, 1)")
            sb.AppendLine("RETURN")
        End If

        sb.AppendLine()
        sb.AppendLine("SET NOCOUNT OFF")
        sb.AppendLine()

        sb.AppendLine("DECLARE @toAdd int")
        sb.exAppendFormatLine("SET @toAdd = (SELECT COUNT(*) FROM {0} WHERE {1} NOT IN (SELECT {1} FROM {2}))", syncTable, _sqlGen.QuoteIdentifier(onlyKeyCol), _sqlGen.QuoteIdentifier(tbl.Name))
        sb.exAppendFormatLine("PRINT 'Adding ' + CONVERT(varchar(10), @toAdd) + ' records to {0}'", tbl.Name)
        sb.AppendLine()
        sb.exAppendFormatLine("INSERT INTO {0} ( {1} )", _sqlGen.QuoteIdentifier(tbl.Name), colList)
        sb.exAppendFormatLine("SELECT {0}", colList)
        sb.exAppendFormatLine("FROM {0}", syncTable)
        sb.exAppendFormatLine("WHERE {0} NOT IN (SELECT {0} FROM {1})", _sqlGen.QuoteIdentifier(onlyKeyCol), _sqlGen.QuoteIdentifier(tbl.Name))
        sb.AppendLine()

        sb.AppendLine("DECLARE @toDelete int")
        sb.exAppendFormatLine("SET @toDelete = (SELECT COUNT(*) FROM {0} WHERE {1} NOT IN (SELECT {1} FROM {2}))", _sqlGen.QuoteIdentifier(tbl.Name), _sqlGen.QuoteIdentifier(onlyKeyCol), syncTable)
        sb.exAppendFormatLine("PRINT 'Deleting ' + CONVERT(varchar(10), @toDelete) + ' records from {0}'", tbl.Name)
        sb.AppendLine()
        sb.exAppendFormatLine("DELETE FROM {0}", _sqlGen.QuoteIdentifier(tbl.Name))
        sb.exAppendFormatLine("WHERE {0} NOT IN (SELECT {0} FROM {1})", _sqlGen.QuoteIdentifier(onlyKeyCol), syncTable)
        sb.AppendLine()

        sb.exAppendFormatLine("PRINT 'Updating all records in {0}'", tbl.Name)
        sb.exAppendFormatLine("UPDATE {0} SET", _sqlGen.QuoteIdentifier(tbl.Name))

        Dim colAssignDelim = ""
        For Each col In tbl.Columns
            sb.Append(colAssignDelim)
            sb.AppendFormat("  {0}{1} = SyncTable.{1}", If(col.InPrimaryKey, "--", ""), _sqlGen.QuoteIdentifier(col.Name))
            colAssignDelim = "," + vbNewLine
        Next
        sb.AppendLine()

        sb.exAppendFormatLine("FROM {0} RealTable", _sqlGen.QuoteIdentifier(tbl.Name))
        sb.exAppendFormatLine("	INNER JOIN {0} SyncTable ON RealTable.{1} = SyncTable.{1}", syncTable, _sqlGen.QuoteIdentifier(onlyKeyCol))
        sb.AppendLine()
        sb.exAppendFormatLine("DROP TABLE {0}", syncTable)
        sb.AppendLine()
        sb.exAppendFormatLine("------------ END synchronize {0} table", tbl.Name)

        Return sb.ToString
    End Function


    Private Sub Retired_DoGenerateTVF()
        Dim sql = ""
        If (PromptForm.PromptForMultilineString(sql, "Enter source SQL") = DialogResult.Cancel) Then Return

        Dim dtSchema As New DataTable
        Dim result As New System.Text.StringBuilder
        Using New WaitCursorHandler(Me)
            Using conn = CreateConnection()
                conn.Open()
                Dim cmd = conn.CreateCommand()
                cmd.CommandText = sql
                cmd.CommandTimeout = Util.GetDefaultTimeout(_attachedConnInfo)
                Using da = _attachedConnInfo.GetFactory().CreateDataAdapter()
                    Using cb = _attachedConnInfo.GetFactory().CreateCommandBuilder()
                        da.SelectCommand = cmd
                        cb.DataAdapter = da
                        da.FillSchema(dtSchema, SchemaType.Source)
                    End Using
                End Using
                conn.Close()
            End Using

            result.AppendLine("CREATE FUNCTION dbo.NEWTVF")
            result.AppendLine("(")
            result.AppendLine(")")
            result.AppendLine("RETURNS @returntable TABLE")
            result.AppendLine("(")
            Dim delim = ""
            For Each col As DataColumn In dtSchema.Columns
                result.AppendFormat("{0}    {1} ", delim, _sqlGen.QuoteIdentifier(col.ColumnName))
                delim = "," + vbNewLine
                Select Case col.DataType
                    Case GetType(String)
                        'result.AppendFormat("varchar({0})", If(col.MaxLength = -1, "max", col.MaxLength.ToString))
                        result.Append("varchar(max)")
                    Case GetType(Int32)
                        result.Append("int")
                    Case GetType(Boolean)
                        result.Append("bit")
                    Case GetType(DateTime)
                        result.Append("datetime")
                    Case GetType(Decimal)
                        result.Append("decimal(10,2)")
                    Case Else
                        result.AppendFormat("notsure ({0})", col.DataType.Name)
                End Select
            Next
            result.AppendLine()
            result.AppendLine(")")
            result.AppendLine("AS")
            result.AppendLine("BEGIN")
            result.AppendLine()
            result.AppendLine("INSERT INTO @returntable")

            result.Append("    (")
            delim = ""
            For Each col As DataColumn In dtSchema.Columns
                result.AppendFormat("{0}{1}", delim, _sqlGen.QuoteIdentifier(col.ColumnName))
                delim = ", "
            Next
            result.AppendLine(")")
            result.AppendLine("SELECT")
            result.AppendLine("    ...")
            result.AppendLine()
            result.AppendLine("    RETURN")
            result.AppendLine()
            result.AppendLine("END")
        End Using

        txtResult.Text = result.ToString
    End Sub

    Private Sub lstColumns_CellContextMenuStripNeeded(sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs) Handles lstColumns.CellContextMenuStripNeeded
        If (lstColumns.SelectedCells.Count > 0) Then
            e.ContextMenuStrip = mnuRightClickColumns
        End If
    End Sub

    Private Sub lstTables_CellContextMenuStripNeeded(sender As Object, e As DataGridViewCellContextMenuStripNeededEventArgs) Handles lstTables.CellContextMenuStripNeeded
        If (lstTables.SelectedCells.Count > 0) Then
            e.ContextMenuStrip = mnuRightClickTables
        End If
    End Sub

    Private Sub CopyGridSelectedItems(grid As DataGridView, delimiter As String, skipFirstRow As Boolean, sqlEscape As Boolean)
        Dim data = New System.Text.StringBuilder()
        Dim delim = ""
        Dim rows = (From r In grid.SelectedRows.Cast(Of DataGridViewRow)() Order By r.Index).ToList
        For Each row In rows
            If (skipFirstRow AndAlso (row.Index = 0)) Then Continue For
            data.Append(delim)
            If (sqlEscape) Then
                data.Append(_sqlGen.QuoteIdentifier(row.Cells.Item(0).Value.ToString()))
            Else
                data.Append(row.Cells.Item(0).Value)
            End If
            delim = delimiter
        Next
        Util.CopyToClipboard(data.ToString)
    End Sub

    Private Sub mnuCopyTableNames_Click(sender As Object, e As EventArgs) Handles mnuCopyTableNames.Click
        CopyGridSelectedItems(lstTables, Environment.NewLine, skipFirstRow:=False, sqlEscape:=False)
    End Sub

    Private Sub mnuCopyColumnNames_Click(sender As Object, e As EventArgs) Handles mnuCopyColumnNames.Click
        CopyGridSelectedItems(lstColumns, Environment.NewLine, skipFirstRow:=True, sqlEscape:=False)
    End Sub

    Private Sub mnuCopyColumnsAsList_Click(sender As Object, e As EventArgs) Handles mnuCopyColumnsAsList.Click
        CopyGridSelectedItems(lstColumns, ", ", skipFirstRow:=True, sqlEscape:=True)
    End Sub

#End Region


End Class

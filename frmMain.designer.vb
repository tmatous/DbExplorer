<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtResult = New System.Windows.Forms.TextBox()
        Me.mnuMain = New System.Windows.Forms.MenuStrip()
        Me.mnuDbActions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDatabaseConnect = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuDatabaseViewSummary = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDatabaseSearchNames = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDatabaseSearchData = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDatabaseQuery = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDatabaseCodeGen = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTableActions = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTableQuery = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTableEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTableViewSummary = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTableExportCsv = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTableExportInserts = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTableImportCsv = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGenerate = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGenerateSelect = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGenerateInsertWithParams = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGenerateInsertWithValues = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuGenerateUpdateWithParams = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTableCodeGen = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelpAbout = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblDatabase = New System.Windows.Forms.Label()
        Me.ctlTooltip = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnReload = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.lstTables = New System.Windows.Forms.DataGridView()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtTableFilter = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lstColumns = New System.Windows.Forms.DataGridView()
        Me.txtColumnFilter = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.mnuRightClickColumns = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCopyColumnNames = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCopyColumnsAsList = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRightClickTables = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCopyTableNames = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMain.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.lstTables, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lstColumns, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuRightClickColumns.SuspendLayout()
        Me.mnuRightClickTables.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(25, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "DB:"
        '
        'txtResult
        '
        Me.txtResult.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtResult.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtResult.Location = New System.Drawing.Point(13, 395)
        Me.txtResult.Multiline = True
        Me.txtResult.Name = "txtResult"
        Me.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtResult.Size = New System.Drawing.Size(937, 193)
        Me.txtResult.TabIndex = 6
        Me.txtResult.WordWrap = False
        '
        'mnuMain
        '
        Me.mnuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDbActions, Me.mnuTableActions, Me.mnuHelp})
        Me.mnuMain.Location = New System.Drawing.Point(0, 0)
        Me.mnuMain.Name = "mnuMain"
        Me.mnuMain.Size = New System.Drawing.Size(962, 24)
        Me.mnuMain.TabIndex = 31
        Me.mnuMain.Text = "MenuStrip1"
        '
        'mnuDbActions
        '
        Me.mnuDbActions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuDatabaseConnect, Me.ToolStripMenuItem1, Me.mnuDatabaseViewSummary, Me.mnuDatabaseSearchNames, Me.mnuDatabaseSearchData, Me.mnuDatabaseQuery, Me.mnuDatabaseCodeGen, Me.ToolStripMenuItem2, Me.mnuExit})
        Me.mnuDbActions.Name = "mnuDbActions"
        Me.mnuDbActions.Size = New System.Drawing.Size(67, 20)
        Me.mnuDbActions.Text = "Database"
        '
        'mnuDatabaseConnect
        '
        Me.mnuDatabaseConnect.Name = "mnuDatabaseConnect"
        Me.mnuDatabaseConnect.Size = New System.Drawing.Size(177, 22)
        Me.mnuDatabaseConnect.Text = "Connect..."
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(174, 6)
        '
        'mnuDatabaseViewSummary
        '
        Me.mnuDatabaseViewSummary.Name = "mnuDatabaseViewSummary"
        Me.mnuDatabaseViewSummary.Size = New System.Drawing.Size(177, 22)
        Me.mnuDatabaseViewSummary.Text = "View Summary"
        '
        'mnuDatabaseSearchNames
        '
        Me.mnuDatabaseSearchNames.Name = "mnuDatabaseSearchNames"
        Me.mnuDatabaseSearchNames.Size = New System.Drawing.Size(177, 22)
        Me.mnuDatabaseSearchNames.Text = "Search Field Names"
        '
        'mnuDatabaseSearchData
        '
        Me.mnuDatabaseSearchData.Name = "mnuDatabaseSearchData"
        Me.mnuDatabaseSearchData.Size = New System.Drawing.Size(177, 22)
        Me.mnuDatabaseSearchData.Text = "Search Data"
        '
        'mnuDatabaseQuery
        '
        Me.mnuDatabaseQuery.Name = "mnuDatabaseQuery"
        Me.mnuDatabaseQuery.Size = New System.Drawing.Size(177, 22)
        Me.mnuDatabaseQuery.Text = "New Query"
        '
        'mnuDatabaseCodeGen
        '
        Me.mnuDatabaseCodeGen.Name = "mnuDatabaseCodeGen"
        Me.mnuDatabaseCodeGen.Size = New System.Drawing.Size(177, 22)
        Me.mnuDatabaseCodeGen.Text = "CodeGen"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(174, 6)
        '
        'mnuExit
        '
        Me.mnuExit.Name = "mnuExit"
        Me.mnuExit.Size = New System.Drawing.Size(177, 22)
        Me.mnuExit.Text = "Exit"
        '
        'mnuTableActions
        '
        Me.mnuTableActions.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuTableQuery, Me.mnuTableEdit, Me.mnuTableViewSummary, Me.mnuTableExportCsv, Me.mnuTableExportInserts, Me.mnuTableImportCsv, Me.mnuGenerate, Me.mnuTableCodeGen})
        Me.mnuTableActions.Name = "mnuTableActions"
        Me.mnuTableActions.Size = New System.Drawing.Size(46, 20)
        Me.mnuTableActions.Text = "Table"
        '
        'mnuTableQuery
        '
        Me.mnuTableQuery.Name = "mnuTableQuery"
        Me.mnuTableQuery.Size = New System.Drawing.Size(186, 22)
        Me.mnuTableQuery.Text = "Query"
        '
        'mnuTableEdit
        '
        Me.mnuTableEdit.Name = "mnuTableEdit"
        Me.mnuTableEdit.Size = New System.Drawing.Size(186, 22)
        Me.mnuTableEdit.Text = "Edit Table Data"
        '
        'mnuTableViewSummary
        '
        Me.mnuTableViewSummary.Name = "mnuTableViewSummary"
        Me.mnuTableViewSummary.Size = New System.Drawing.Size(186, 22)
        Me.mnuTableViewSummary.Text = "View Summary"
        '
        'mnuTableExportCsv
        '
        Me.mnuTableExportCsv.Name = "mnuTableExportCsv"
        Me.mnuTableExportCsv.Size = New System.Drawing.Size(186, 22)
        Me.mnuTableExportCsv.Text = "Export Data as CSV"
        '
        'mnuTableExportInserts
        '
        Me.mnuTableExportInserts.Name = "mnuTableExportInserts"
        Me.mnuTableExportInserts.Size = New System.Drawing.Size(186, 22)
        Me.mnuTableExportInserts.Text = "Export Data as Inserts"
        '
        'mnuTableImportCsv
        '
        Me.mnuTableImportCsv.Name = "mnuTableImportCsv"
        Me.mnuTableImportCsv.Size = New System.Drawing.Size(186, 22)
        Me.mnuTableImportCsv.Text = "Import CSV Data"
        '
        'mnuGenerate
        '
        Me.mnuGenerate.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuGenerateSelect, Me.mnuGenerateInsertWithParams, Me.mnuGenerateInsertWithValues, Me.mnuGenerateUpdateWithParams})
        Me.mnuGenerate.Name = "mnuGenerate"
        Me.mnuGenerate.Size = New System.Drawing.Size(186, 22)
        Me.mnuGenerate.Text = "Generate SQL"
        '
        'mnuGenerateSelect
        '
        Me.mnuGenerateSelect.Name = "mnuGenerateSelect"
        Me.mnuGenerateSelect.Size = New System.Drawing.Size(182, 22)
        Me.mnuGenerateSelect.Text = "Select"
        '
        'mnuGenerateInsertWithParams
        '
        Me.mnuGenerateInsertWithParams.Name = "mnuGenerateInsertWithParams"
        Me.mnuGenerateInsertWithParams.Size = New System.Drawing.Size(182, 22)
        Me.mnuGenerateInsertWithParams.Text = "Insert With Params"
        '
        'mnuGenerateInsertWithValues
        '
        Me.mnuGenerateInsertWithValues.Name = "mnuGenerateInsertWithValues"
        Me.mnuGenerateInsertWithValues.Size = New System.Drawing.Size(182, 22)
        Me.mnuGenerateInsertWithValues.Text = "Insert With Values"
        '
        'mnuGenerateUpdateWithParams
        '
        Me.mnuGenerateUpdateWithParams.Name = "mnuGenerateUpdateWithParams"
        Me.mnuGenerateUpdateWithParams.Size = New System.Drawing.Size(182, 22)
        Me.mnuGenerateUpdateWithParams.Text = "Update With Params"
        '
        'mnuTableCodeGen
        '
        Me.mnuTableCodeGen.Name = "mnuTableCodeGen"
        Me.mnuTableCodeGen.Size = New System.Drawing.Size(186, 22)
        Me.mnuTableCodeGen.Text = "CodeGen"
        '
        'mnuHelp
        '
        Me.mnuHelp.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHelpAbout})
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "Help"
        '
        'mnuHelpAbout
        '
        Me.mnuHelpAbout.Name = "mnuHelpAbout"
        Me.mnuHelpAbout.Size = New System.Drawing.Size(107, 22)
        Me.mnuHelpAbout.Text = "About"
        '
        'lblDatabase
        '
        Me.lblDatabase.AutoSize = True
        Me.lblDatabase.Location = New System.Drawing.Point(53, 36)
        Me.lblDatabase.Name = "lblDatabase"
        Me.lblDatabase.Size = New System.Drawing.Size(59, 13)
        Me.lblDatabase.TabIndex = 32
        Me.lblDatabase.Text = "[Database]"
        '
        'btnReload
        '
        Me.btnReload.FlatAppearance.BorderSize = 0
        Me.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReload.Image = Global.DbExplorer.My.Resources.Resources.reload
        Me.btnReload.Location = New System.Drawing.Point(4, 31)
        Me.btnReload.Name = "btnReload"
        Me.btnReload.Size = New System.Drawing.Size(18, 18)
        Me.btnReload.TabIndex = 33
        Me.btnReload.TabStop = False
        Me.ctlTooltip.SetToolTip(Me.btnReload, "Refresh Schema")
        Me.btnReload.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 55)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstTables)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtTableFilter)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lstColumns)
        Me.SplitContainer1.Panel2.Controls.Add(Me.txtColumnFilter)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label6)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label5)
        Me.SplitContainer1.Size = New System.Drawing.Size(958, 334)
        Me.SplitContainer1.SplitterDistance = 444
        Me.SplitContainer1.TabIndex = 36
        '
        'lstTables
        '
        Me.lstTables.AllowUserToAddRows = False
        Me.lstTables.AllowUserToDeleteRows = False
        Me.lstTables.AllowUserToResizeColumns = False
        Me.lstTables.AllowUserToResizeRows = False
        Me.lstTables.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstTables.BackgroundColor = System.Drawing.SystemColors.Window
        Me.lstTables.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lstTables.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.lstTables.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable
        Me.lstTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.lstTables.GridColor = System.Drawing.SystemColors.ControlLight
        Me.lstTables.Location = New System.Drawing.Point(53, 30)
        Me.lstTables.Name = "lstTables"
        Me.lstTables.ReadOnly = True
        Me.lstTables.RowHeadersVisible = False
        Me.lstTables.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstTables.RowTemplate.Height = 18
        Me.lstTables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.lstTables.ShowCellToolTips = False
        Me.lstTables.Size = New System.Drawing.Size(388, 297)
        Me.lstTables.TabIndex = 38
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(32, 13)
        Me.Label4.TabIndex = 37
        Me.Label4.Text = "Filter:"
        '
        'txtTableFilter
        '
        Me.txtTableFilter.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTableFilter.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTableFilter.Location = New System.Drawing.Point(53, 4)
        Me.txtTableFilter.Name = "txtTableFilter"
        Me.txtTableFilter.Size = New System.Drawing.Size(388, 20)
        Me.txtTableFilter.TabIndex = 36
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 13)
        Me.Label2.TabIndex = 35
        Me.Label2.Text = "Tables:"
        '
        'lstColumns
        '
        Me.lstColumns.AllowUserToAddRows = False
        Me.lstColumns.AllowUserToDeleteRows = False
        Me.lstColumns.AllowUserToResizeColumns = False
        Me.lstColumns.AllowUserToResizeRows = False
        Me.lstColumns.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstColumns.BackgroundColor = System.Drawing.SystemColors.Window
        Me.lstColumns.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lstColumns.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.lstColumns.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable
        Me.lstColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.lstColumns.GridColor = System.Drawing.SystemColors.ControlLight
        Me.lstColumns.Location = New System.Drawing.Point(58, 31)
        Me.lstColumns.Name = "lstColumns"
        Me.lstColumns.ReadOnly = True
        Me.lstColumns.RowHeadersVisible = False
        Me.lstColumns.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstColumns.RowTemplate.Height = 18
        Me.lstColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.lstColumns.ShowCellToolTips = False
        Me.lstColumns.Size = New System.Drawing.Size(441, 297)
        Me.lstColumns.TabIndex = 39
        '
        'txtColumnFilter
        '
        Me.txtColumnFilter.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtColumnFilter.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtColumnFilter.Location = New System.Drawing.Point(57, 4)
        Me.txtColumnFilter.Name = "txtColumnFilter"
        Me.txtColumnFilter.Size = New System.Drawing.Size(442, 20)
        Me.txtColumnFilter.TabIndex = 38
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(20, 7)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(32, 13)
        Me.Label6.TabIndex = 37
        Me.Label6.Text = "Filter:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(2, 27)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(50, 13)
        Me.Label5.TabIndex = 36
        Me.Label5.Text = "Columns:"
        '
        'mnuRightClickColumns
        '
        Me.mnuRightClickColumns.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCopyColumnNames, Me.mnuCopyColumnsAsList})
        Me.mnuRightClickColumns.Name = "ContextMenuStrip1"
        Me.mnuRightClickColumns.ShowImageMargin = False
        Me.mnuRightClickColumns.Size = New System.Drawing.Size(164, 48)
        '
        'mnuCopyColumnNames
        '
        Me.mnuCopyColumnNames.Name = "mnuCopyColumnNames"
        Me.mnuCopyColumnNames.Size = New System.Drawing.Size(163, 22)
        Me.mnuCopyColumnNames.Text = "Copy Column Names"
        '
        'mnuCopyColumnsAsList
        '
        Me.mnuCopyColumnsAsList.Name = "mnuCopyColumnsAsList"
        Me.mnuCopyColumnsAsList.Size = New System.Drawing.Size(163, 22)
        Me.mnuCopyColumnsAsList.Text = "Copy as SQL List"
        '
        'mnuRightClickTables
        '
        Me.mnuRightClickTables.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCopyTableNames})
        Me.mnuRightClickTables.Name = "ContextMenuStrip1"
        Me.mnuRightClickTables.ShowImageMargin = False
        Me.mnuRightClickTables.Size = New System.Drawing.Size(148, 26)
        '
        'mnuCopyTableNames
        '
        Me.mnuCopyTableNames.Name = "mnuCopyTableNames"
        Me.mnuCopyTableNames.Size = New System.Drawing.Size(147, 22)
        Me.mnuCopyTableNames.Text = "Copy Table Names"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(962, 600)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.btnReload)
        Me.Controls.Add(Me.lblDatabase)
        Me.Controls.Add(Me.mnuMain)
        Me.Controls.Add(Me.txtResult)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.mnuMain
        Me.Name = "frmMain"
        Me.Text = "DbExplorer"
        Me.mnuMain.ResumeLayout(False)
        Me.mnuMain.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.lstTables, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lstColumns, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuRightClickColumns.ResumeLayout(False)
        Me.mnuRightClickTables.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtResult As System.Windows.Forms.TextBox
    Friend WithEvents mnuMain As System.Windows.Forms.MenuStrip
    Friend WithEvents mnuDbActions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDatabaseViewSummary As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDatabaseSearchNames As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDatabaseSearchData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTableActions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTableEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTableExportCsv As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTableExportInserts As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTableImportCsv As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGenerate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGenerateSelect As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGenerateInsertWithParams As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGenerateInsertWithValues As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuGenerateUpdateWithParams As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDatabaseConnect As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblDatabase As System.Windows.Forms.Label
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuDatabaseCodeGen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTableCodeGen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnReload As System.Windows.Forms.Button
    Friend WithEvents mnuTableQuery As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctlTooltip As System.Windows.Forms.ToolTip
    Friend WithEvents mnuDatabaseQuery As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuHelpAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lstTables As System.Windows.Forms.DataGridView
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtTableFilter As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lstColumns As System.Windows.Forms.DataGridView
    Friend WithEvents txtColumnFilter As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents mnuTableViewSummary As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRightClickColumns As ContextMenuStrip
    Friend WithEvents mnuCopyColumnNames As ToolStripMenuItem
    Friend WithEvents mnuCopyColumnsAsList As ToolStripMenuItem
    Friend WithEvents mnuRightClickTables As ContextMenuStrip
    Friend WithEvents mnuCopyTableNames As ToolStripMenuItem
End Class

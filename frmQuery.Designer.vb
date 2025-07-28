<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmQuery
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmQuery))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripDropDownButton()
        Me.mnuNewWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuNewTab = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDuplicateTab = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuLoad = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSaveAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuClose = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCloseAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuView = New System.Windows.Forms.ToolStripDropDownButton()
        Me.mnuViewResultsList = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuViewResultsTabs = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExport = New System.Windows.Forms.ToolStripDropDownButton()
        Me.mnuExportCsv = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExportExcel = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExportCodeGen = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuExportCodeGenByRow = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnRun = New System.Windows.Forms.ToolStripButton()
        Me.btnStop = New System.Windows.Forms.ToolStripButton()
        Me.lblElapsedTime = New System.Windows.Forms.ToolStripLabel()
        Me.ctlProgress = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
        Me.ctlTabStrip = New System.Windows.Forms.ToolStrip()
        Me.btnCloseTab = New System.Windows.Forms.ToolStripButton()
        Me.tmrUpdateTabs = New System.Windows.Forms.Timer(Me.components)
        Me.ToolStrip1.SuspendLayout()
        Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.ctlTabStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuView, Me.mnuExport, Me.ToolStripSeparator1, Me.btnRun, Me.btnStop, Me.lblElapsedTime, Me.ctlProgress})
        Me.ToolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(944, 25)
        Me.ToolStrip1.Stretch = True
        Me.ToolStrip1.TabIndex = 4
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'mnuFile
        '
        Me.mnuFile.AutoToolTip = False
        Me.mnuFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNewWindow, Me.mnuNewTab, Me.mnuDuplicateTab, Me.ToolStripSeparator2, Me.mnuLoad, Me.mnuSave, Me.mnuSaveAs, Me.mnuSaveAll, Me.ToolStripSeparator3, Me.mnuClose, Me.mnuCloseAll})
        Me.mnuFile.Image = CType(resources.GetObject("mnuFile.Image"), System.Drawing.Image)
        Me.mnuFile.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuFile.Margin = New System.Windows.Forms.Padding(10, 1, 15, 2)
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.ShowDropDownArrow = False
        Me.mnuFile.Size = New System.Drawing.Size(27, 22)
        Me.mnuFile.Text = "&File"
        '
        'mnuNewWindow
        '
        Me.mnuNewWindow.Name = "mnuNewWindow"
        Me.mnuNewWindow.Size = New System.Drawing.Size(177, 22)
        Me.mnuNewWindow.Text = "New Window"
        '
        'mnuNewTab
        '
        Me.mnuNewTab.Name = "mnuNewTab"
        Me.mnuNewTab.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mnuNewTab.Size = New System.Drawing.Size(177, 22)
        Me.mnuNewTab.Text = "&New Tab"
        '
        'mnuDuplicateTab
        '
        Me.mnuDuplicateTab.Name = "mnuDuplicateTab"
        Me.mnuDuplicateTab.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.T), System.Windows.Forms.Keys)
        Me.mnuDuplicateTab.Size = New System.Drawing.Size(177, 22)
        Me.mnuDuplicateTab.Text = "Duplicate &Tab"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(174, 6)
        '
        'mnuLoad
        '
        Me.mnuLoad.Name = "mnuLoad"
        Me.mnuLoad.Size = New System.Drawing.Size(177, 22)
        Me.mnuLoad.Text = "Load..."
        '
        'mnuSave
        '
        Me.mnuSave.Name = "mnuSave"
        Me.mnuSave.Size = New System.Drawing.Size(177, 22)
        Me.mnuSave.Text = "Save"
        '
        'mnuSaveAs
        '
        Me.mnuSaveAs.Name = "mnuSaveAs"
        Me.mnuSaveAs.Size = New System.Drawing.Size(177, 22)
        Me.mnuSaveAs.Text = "Save As..."
        '
        'mnuSaveAll
        '
        Me.mnuSaveAll.Name = "mnuSaveAll"
        Me.mnuSaveAll.Size = New System.Drawing.Size(177, 22)
        Me.mnuSaveAll.Text = "Save All"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(174, 6)
        '
        'mnuClose
        '
        Me.mnuClose.Name = "mnuClose"
        Me.mnuClose.Size = New System.Drawing.Size(177, 22)
        Me.mnuClose.Text = "Close"
        '
        'mnuCloseAll
        '
        Me.mnuCloseAll.Name = "mnuCloseAll"
        Me.mnuCloseAll.Size = New System.Drawing.Size(177, 22)
        Me.mnuCloseAll.Text = "Close All"
        '
        'mnuView
        '
        Me.mnuView.AutoToolTip = False
        Me.mnuView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.mnuView.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewResultsList, Me.mnuViewResultsTabs})
        Me.mnuView.Image = CType(resources.GetObject("mnuView.Image"), System.Drawing.Image)
        Me.mnuView.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuView.Margin = New System.Windows.Forms.Padding(0, 1, 15, 2)
        Me.mnuView.Name = "mnuView"
        Me.mnuView.ShowDropDownArrow = False
        Me.mnuView.Size = New System.Drawing.Size(33, 22)
        Me.mnuView.Text = "&View"
        '
        'mnuViewResultsList
        '
        Me.mnuViewResultsList.Name = "mnuViewResultsList"
        Me.mnuViewResultsList.Size = New System.Drawing.Size(147, 22)
        Me.mnuViewResultsList.Text = "Results as list"
        '
        'mnuViewResultsTabs
        '
        Me.mnuViewResultsTabs.Name = "mnuViewResultsTabs"
        Me.mnuViewResultsTabs.Size = New System.Drawing.Size(147, 22)
        Me.mnuViewResultsTabs.Text = "Results as tabs"
        '
        'mnuExport
        '
        Me.mnuExport.AutoToolTip = False
        Me.mnuExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.mnuExport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExportCsv, Me.mnuExportExcel, Me.mnuExportCodeGen, Me.mnuExportCodeGenByRow})
        Me.mnuExport.Image = CType(resources.GetObject("mnuExport.Image"), System.Drawing.Image)
        Me.mnuExport.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.mnuExport.Margin = New System.Windows.Forms.Padding(0, 1, 15, 2)
        Me.mnuExport.Name = "mnuExport"
        Me.mnuExport.ShowDropDownArrow = False
        Me.mnuExport.Size = New System.Drawing.Size(43, 22)
        Me.mnuExport.Text = "E&xport"
        '
        'mnuExportCsv
        '
        Me.mnuExportCsv.Name = "mnuExportCsv"
        Me.mnuExportCsv.Size = New System.Drawing.Size(205, 22)
        Me.mnuExportCsv.Text = "Export to CSV"
        '
        'mnuExportExcel
        '
        Me.mnuExportExcel.Name = "mnuExportExcel"
        Me.mnuExportExcel.Size = New System.Drawing.Size(205, 22)
        Me.mnuExportExcel.Text = "Export to Excel"
        '
        'mnuExportCodeGen
        '
        Me.mnuExportCodeGen.Name = "mnuExportCodeGen"
        Me.mnuExportCodeGen.Size = New System.Drawing.Size(205, 22)
        Me.mnuExportCodeGen.Text = "Export to CodeGen"
        '
        'mnuExportCodeGenByRow
        '
        Me.mnuExportCodeGenByRow.Name = "mnuExportCodeGenByRow"
        Me.mnuExportCodeGenByRow.Size = New System.Drawing.Size(205, 22)
        Me.mnuExportCodeGenByRow.Text = "Export to CodeGen by Row"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Margin = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'btnRun
        '
        Me.btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnRun.Image = Global.DbExplorer.My.Resources.Resources.play
        Me.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnRun.Margin = New System.Windows.Forms.Padding(0, 1, 15, 2)
        Me.btnRun.Name = "btnRun"
        Me.btnRun.Size = New System.Drawing.Size(23, 22)
        Me.btnRun.Text = "Run (F5)"
        '
        'btnStop
        '
        Me.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnStop.Enabled = False
        Me.btnStop.Image = Global.DbExplorer.My.Resources.Resources.stopbtn
        Me.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnStop.Margin = New System.Windows.Forms.Padding(0, 1, 15, 2)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(23, 22)
        Me.btnStop.Text = "Stop (Esc)"
        '
        'lblElapsedTime
        '
        Me.lblElapsedTime.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.lblElapsedTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.lblElapsedTime.Name = "lblElapsedTime"
        Me.lblElapsedTime.Padding = New System.Windows.Forms.Padding(30, 0, 0, 0)
        Me.lblElapsedTime.Size = New System.Drawing.Size(81, 22)
        Me.lblElapsedTime.Text = "00:00:00"
        '
        'ctlProgress
        '
        Me.ctlProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ctlProgress.AutoSize = False
        Me.ctlProgress.Margin = New System.Windows.Forms.Padding(20, 2, 10, 1)
        Me.ctlProgress.Name = "ctlProgress"
        Me.ctlProgress.Size = New System.Drawing.Size(100, 15)
        Me.ctlProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.ctlProgress.Visible = False
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(944, 567)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.Size = New System.Drawing.Size(944, 617)
        Me.ToolStripContainer1.TabIndex = 5
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.ToolStrip1)
        Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.ctlTabStrip)
        '
        'ctlTabStrip
        '
        Me.ctlTabStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.ctlTabStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnCloseTab})
        Me.ctlTabStrip.Location = New System.Drawing.Point(0, 25)
        Me.ctlTabStrip.Name = "ctlTabStrip"
        Me.ctlTabStrip.Size = New System.Drawing.Size(944, 25)
        Me.ctlTabStrip.Stretch = True
        Me.ctlTabStrip.TabIndex = 5
        '
        'btnCloseTab
        '
        Me.btnCloseTab.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnCloseTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnCloseTab.Image = Global.DbExplorer.My.Resources.Resources.close
        Me.btnCloseTab.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCloseTab.Name = "btnCloseTab"
        Me.btnCloseTab.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me.btnCloseTab.Size = New System.Drawing.Size(23, 22)
        Me.btnCloseTab.Text = "X"
        Me.btnCloseTab.ToolTipText = "Close Tab"
        '
        'tmrUpdateTabs
        '
        Me.tmrUpdateTabs.Interval = 500
        '
        'frmQuery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(944, 617)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmQuery"
        Me.Text = "Query"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.ctlTabStrip.ResumeLayout(False)
        Me.ctlTabStrip.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents mnuFile As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents mnuNewWindow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewTab As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuLoad As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSave As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuClose As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCloseAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuView As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents mnuViewResultsList As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuViewResultsTabs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExport As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents mnuExportCsv As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExportCodeGen As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExportCodeGenByRow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnRun As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnStop As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents ctlTabStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents ctlProgress As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents btnCloseTab As System.Windows.Forms.ToolStripButton
    Friend WithEvents mnuDuplicateTab As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuSaveAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuSaveAs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuExportExcel As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblElapsedTime As ToolStripLabel
    Friend WithEvents tmrUpdateTabs As Timer
End Class

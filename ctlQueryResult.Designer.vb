<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlQueryResult
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.ctlStatus = New System.Windows.Forms.StatusStrip()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ctlToolstripSpacer = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ctlProgress = New System.Windows.Forms.ToolStripProgressBar()
        Me.grdData = New System.Windows.Forms.DataGridView()
        Me.mnuRightClickCells = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCopyWithHeaders = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCopyAsList = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctlStatus.SuspendLayout()
        CType(Me.grdData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuRightClickCells.SuspendLayout()
        Me.SuspendLayout()
        '
        'ctlStatus
        '
        Me.ctlStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus, Me.ctlToolstripSpacer, Me.ctlProgress})
        Me.ctlStatus.Location = New System.Drawing.Point(0, 350)
        Me.ctlStatus.Name = "ctlStatus"
        Me.ctlStatus.Size = New System.Drawing.Size(541, 22)
        Me.ctlStatus.SizingGrip = False
        Me.ctlStatus.TabIndex = 3
        '
        'lblStatus
        '
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(60, 17)
        Me.lblStatus.Text = "{lblStatus}"
        '
        'ctlToolstripSpacer
        '
        Me.ctlToolstripSpacer.BorderStyle = System.Windows.Forms.Border3DStyle.Etched
        Me.ctlToolstripSpacer.Name = "ctlToolstripSpacer"
        Me.ctlToolstripSpacer.Size = New System.Drawing.Size(365, 17)
        Me.ctlToolstripSpacer.Spring = True
        '
        'ctlProgress
        '
        Me.ctlProgress.Margin = New System.Windows.Forms.Padding(1, 3, 0, 3)
        Me.ctlProgress.Name = "ctlProgress"
        Me.ctlProgress.Size = New System.Drawing.Size(100, 16)
        Me.ctlProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        '
        'grdData
        '
        Me.grdData.AllowUserToAddRows = False
        Me.grdData.AllowUserToDeleteRows = False
        Me.grdData.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Honeydew
        Me.grdData.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.grdData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdData.BackgroundColor = System.Drawing.SystemColors.Window
        Me.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.grdData.DefaultCellStyle = DataGridViewCellStyle2
        Me.grdData.Location = New System.Drawing.Point(3, 3)
        Me.grdData.Name = "grdData"
        Me.grdData.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.grdData.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.grdData.RowHeadersWidth = 40
        Me.grdData.Size = New System.Drawing.Size(535, 344)
        Me.grdData.TabIndex = 2
        '
        'mnuRightClickCells
        '
        Me.mnuRightClickCells.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCopy, Me.mnuCopyWithHeaders, Me.mnuCopyAsList})
        Me.mnuRightClickCells.Name = "ContextMenuStrip1"
        Me.mnuRightClickCells.ShowImageMargin = False
        Me.mnuRightClickCells.Size = New System.Drawing.Size(156, 92)
        '
        'mnuCopy
        '
        Me.mnuCopy.Name = "mnuCopy"
        Me.mnuCopy.Size = New System.Drawing.Size(155, 22)
        Me.mnuCopy.Text = "Copy"
        '
        'mnuCopyWithHeaders
        '
        Me.mnuCopyWithHeaders.Name = "mnuCopyWithHeaders"
        Me.mnuCopyWithHeaders.Size = New System.Drawing.Size(155, 22)
        Me.mnuCopyWithHeaders.Text = "Copy with Headers"
        '
        'mnuCopyAsList
        '
        Me.mnuCopyAsList.Name = "mnuCopyAsList"
        Me.mnuCopyAsList.Size = New System.Drawing.Size(155, 22)
        Me.mnuCopyAsList.Text = "Copy as SQL List"
        '
        'ctlQueryResult
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ctlStatus)
        Me.Controls.Add(Me.grdData)
        Me.Name = "ctlQueryResult"
        Me.Size = New System.Drawing.Size(541, 372)
        Me.ctlStatus.ResumeLayout(False)
        Me.ctlStatus.PerformLayout()
        CType(Me.grdData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuRightClickCells.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ctlStatus As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents grdData As System.Windows.Forms.DataGridView
    Friend WithEvents ctlToolstripSpacer As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ctlProgress As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents mnuRightClickCells As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCopyWithHeaders As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCopyAsList As System.Windows.Forms.ToolStripMenuItem

End Class

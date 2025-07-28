<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlQuery
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
        Me.ctlSplitter = New System.Windows.Forms.SplitContainer()
        Me.txtQuery = New DbExplorer.ctlEditSql()
        Me.ctlTabs = New System.Windows.Forms.TabControl()
        Me.tabResults = New System.Windows.Forms.TabPage()
        Me.ctlResultsSplitter = New DbExplorer.ctlTabSplitter()
        Me.tabMessages = New System.Windows.Forms.TabPage()
        Me.txtMessages = New System.Windows.Forms.TextBox()
        Me.tmrCheckThread = New System.Windows.Forms.Timer(Me.components)
        CType(Me.ctlSplitter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctlSplitter.Panel1.SuspendLayout()
        Me.ctlSplitter.Panel2.SuspendLayout()
        Me.ctlSplitter.SuspendLayout()
        Me.ctlTabs.SuspendLayout()
        Me.tabResults.SuspendLayout()
        Me.tabMessages.SuspendLayout()
        Me.SuspendLayout()
        '
        'ctlSplitter
        '
        Me.ctlSplitter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctlSplitter.Location = New System.Drawing.Point(0, 0)
        Me.ctlSplitter.Name = "ctlSplitter"
        Me.ctlSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'ctlSplitter.Panel1
        '
        Me.ctlSplitter.Panel1.Controls.Add(Me.txtQuery)
        '
        'ctlSplitter.Panel2
        '
        Me.ctlSplitter.Panel2.Controls.Add(Me.ctlTabs)
        Me.ctlSplitter.Size = New System.Drawing.Size(944, 617)
        Me.ctlSplitter.SplitterDistance = 144
        Me.ctlSplitter.TabIndex = 0
        '
        'txtQuery
        '
        Me.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtQuery.Location = New System.Drawing.Point(0, 0)
        Me.txtQuery.Name = "txtQuery"
        Me.txtQuery.Size = New System.Drawing.Size(944, 144)
        Me.txtQuery.SqlText = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.txtQuery.TabIndex = 5
        '
        'ctlTabs
        '
        Me.ctlTabs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ctlTabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.ctlTabs.Controls.Add(Me.tabResults)
        Me.ctlTabs.Controls.Add(Me.tabMessages)
        Me.ctlTabs.Location = New System.Drawing.Point(3, 3)
        Me.ctlTabs.Name = "ctlTabs"
        Me.ctlTabs.SelectedIndex = 0
        Me.ctlTabs.Size = New System.Drawing.Size(938, 463)
        Me.ctlTabs.TabIndex = 2
        '
        'tabResults
        '
        Me.tabResults.Controls.Add(Me.ctlResultsSplitter)
        Me.tabResults.Location = New System.Drawing.Point(4, 25)
        Me.tabResults.Name = "tabResults"
        Me.tabResults.Padding = New System.Windows.Forms.Padding(3)
        Me.tabResults.Size = New System.Drawing.Size(930, 434)
        Me.tabResults.TabIndex = 1
        Me.tabResults.Text = "Results"
        Me.tabResults.UseVisualStyleBackColor = True
        '
        'ctlResultsSplitter
        '
        Me.ctlResultsSplitter.AutoScroll = True
        Me.ctlResultsSplitter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ctlResultsSplitter.Location = New System.Drawing.Point(3, 3)
        Me.ctlResultsSplitter.MinTabHeight = 100
        Me.ctlResultsSplitter.Name = "ctlResultsSplitter"
        Me.ctlResultsSplitter.Size = New System.Drawing.Size(924, 428)
        Me.ctlResultsSplitter.TabIndex = 0
        '
        'tabMessages
        '
        Me.tabMessages.Controls.Add(Me.txtMessages)
        Me.tabMessages.Location = New System.Drawing.Point(4, 25)
        Me.tabMessages.Name = "tabMessages"
        Me.tabMessages.Size = New System.Drawing.Size(930, 403)
        Me.tabMessages.TabIndex = 0
        Me.tabMessages.Text = "Messages"
        Me.tabMessages.UseVisualStyleBackColor = True
        '
        'txtMessages
        '
        Me.txtMessages.BackColor = System.Drawing.SystemColors.Window
        Me.txtMessages.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtMessages.Location = New System.Drawing.Point(0, 0)
        Me.txtMessages.Multiline = True
        Me.txtMessages.Name = "txtMessages"
        Me.txtMessages.ReadOnly = True
        Me.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtMessages.Size = New System.Drawing.Size(930, 403)
        Me.txtMessages.TabIndex = 0
        Me.txtMessages.WordWrap = False
        '
        'tmrCheckThread
        '
        Me.tmrCheckThread.Interval = 500
        '
        'ctlQuery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ctlSplitter)
        Me.Name = "ctlQuery"
        Me.Size = New System.Drawing.Size(944, 617)
        Me.ctlSplitter.Panel1.ResumeLayout(False)
        Me.ctlSplitter.Panel2.ResumeLayout(False)
        CType(Me.ctlSplitter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctlSplitter.ResumeLayout(False)
        Me.ctlTabs.ResumeLayout(False)
        Me.tabResults.ResumeLayout(False)
        Me.tabMessages.ResumeLayout(False)
        Me.tabMessages.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ctlSplitter As System.Windows.Forms.SplitContainer
    Friend WithEvents ctlTabs As System.Windows.Forms.TabControl
    Friend WithEvents tabMessages As System.Windows.Forms.TabPage
    Friend WithEvents txtMessages As System.Windows.Forms.TextBox
    Friend WithEvents tmrCheckThread As System.Windows.Forms.Timer
    Friend WithEvents tabResults As System.Windows.Forms.TabPage
    Friend WithEvents ctlResultsSplitter As DbExplorer.ctlTabSplitter
    Friend WithEvents txtQuery As DbExplorer.ctlEditSql
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlEditSql
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
        Me.ctlScintilla = New ScintillaNET.Scintilla()
        Me.tmrScintillaUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'ctlScintilla
        '
        Me.ctlScintilla.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ctlScintilla.Location = New System.Drawing.Point(0, 0)
        Me.ctlScintilla.Name = "ctlScintilla"
        Me.ctlScintilla.Size = New System.Drawing.Size(728, 485)
        Me.ctlScintilla.TabIndex = 1
        '
        'tmrScintillaUpdate
        '
        Me.tmrScintillaUpdate.Interval = 500
        '
        'ctlEditSql
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ctlScintilla)
        Me.Name = "ctlEditSql"
        Me.Size = New System.Drawing.Size(731, 485)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ctlScintilla As ScintillaNET.Scintilla
    Friend WithEvents tmrScintillaUpdate As System.Windows.Forms.Timer

End Class

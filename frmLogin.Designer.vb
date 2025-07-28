<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogin
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
        Me.lblUsername = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.ckSavePassword = New System.Windows.Forms.CheckBox()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblCaption = New System.Windows.Forms.Label()
        Me.pnlUsername = New System.Windows.Forms.Panel()
        Me.pnlPassword = New System.Windows.Forms.Panel()
        Me.pnlPasswordRetype = New System.Windows.Forms.Panel()
        Me.txtPasswordRetype = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlSavePassword = New System.Windows.Forms.Panel()
        Me.lblError = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.pnlUsername.SuspendLayout()
        Me.pnlPassword.SuspendLayout()
        Me.pnlPasswordRetype.SuspendLayout()
        Me.pnlSavePassword.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblUsername
        '
        Me.lblUsername.AutoSize = True
        Me.lblUsername.Location = New System.Drawing.Point(3, 3)
        Me.lblUsername.Name = "lblUsername"
        Me.lblUsername.Size = New System.Drawing.Size(58, 13)
        Me.lblUsername.TabIndex = 0
        Me.lblUsername.Text = "Username:"
        Me.lblUsername.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Password:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(69, 0)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(196, 20)
        Me.txtUsername.TabIndex = 1
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(69, 3)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(196, 20)
        Me.txtPassword.TabIndex = 2
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'ckSavePassword
        '
        Me.ckSavePassword.AutoSize = True
        Me.ckSavePassword.Location = New System.Drawing.Point(69, 3)
        Me.ckSavePassword.Name = "ckSavePassword"
        Me.ckSavePassword.Size = New System.Drawing.Size(100, 17)
        Me.ckSavePassword.TabIndex = 4
        Me.ckSavePassword.Text = "Save Password"
        Me.ckSavePassword.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(175, 143)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(60, 23)
        Me.btnOk.TabIndex = 99
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(241, 143)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(60, 23)
        Me.btnCancel.TabIndex = 100
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.lblCaption)
        Me.FlowLayoutPanel1.Controls.Add(Me.pnlUsername)
        Me.FlowLayoutPanel1.Controls.Add(Me.pnlPassword)
        Me.FlowLayoutPanel1.Controls.Add(Me.pnlPasswordRetype)
        Me.FlowLayoutPanel1.Controls.Add(Me.pnlSavePassword)
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(12, 12)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(268, 118)
        Me.FlowLayoutPanel1.TabIndex = 8
        '
        'lblCaption
        '
        Me.lblCaption.AutoSize = True
        Me.lblCaption.Location = New System.Drawing.Point(3, 0)
        Me.lblCaption.Margin = New System.Windows.Forms.Padding(3, 0, 3, 10)
        Me.lblCaption.Name = "lblCaption"
        Me.lblCaption.Size = New System.Drawing.Size(49, 13)
        Me.lblCaption.TabIndex = 9
        Me.lblCaption.Text = "[Caption]"
        Me.lblCaption.Visible = False
        '
        'pnlUsername
        '
        Me.pnlUsername.AutoSize = True
        Me.pnlUsername.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlUsername.Controls.Add(Me.txtUsername)
        Me.pnlUsername.Controls.Add(Me.lblUsername)
        Me.pnlUsername.Location = New System.Drawing.Point(0, 23)
        Me.pnlUsername.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlUsername.Name = "pnlUsername"
        Me.pnlUsername.Size = New System.Drawing.Size(268, 23)
        Me.pnlUsername.TabIndex = 1
        '
        'pnlPassword
        '
        Me.pnlPassword.AutoSize = True
        Me.pnlPassword.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlPassword.Controls.Add(Me.txtPassword)
        Me.pnlPassword.Controls.Add(Me.Label2)
        Me.pnlPassword.Location = New System.Drawing.Point(0, 46)
        Me.pnlPassword.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlPassword.Name = "pnlPassword"
        Me.pnlPassword.Size = New System.Drawing.Size(268, 26)
        Me.pnlPassword.TabIndex = 2
        '
        'pnlPasswordRetype
        '
        Me.pnlPasswordRetype.AutoSize = True
        Me.pnlPasswordRetype.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlPasswordRetype.Controls.Add(Me.txtPasswordRetype)
        Me.pnlPasswordRetype.Controls.Add(Me.Label1)
        Me.pnlPasswordRetype.Location = New System.Drawing.Point(0, 72)
        Me.pnlPasswordRetype.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlPasswordRetype.Name = "pnlPasswordRetype"
        Me.pnlPasswordRetype.Size = New System.Drawing.Size(268, 23)
        Me.pnlPasswordRetype.TabIndex = 3
        '
        'txtPasswordRetype
        '
        Me.txtPasswordRetype.Location = New System.Drawing.Point(69, 0)
        Me.txtPasswordRetype.Name = "txtPasswordRetype"
        Me.txtPasswordRetype.Size = New System.Drawing.Size(196, 20)
        Me.txtPasswordRetype.TabIndex = 3
        Me.txtPasswordRetype.UseSystemPasswordChar = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Retype:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'pnlSavePassword
        '
        Me.pnlSavePassword.AutoSize = True
        Me.pnlSavePassword.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlSavePassword.Controls.Add(Me.ckSavePassword)
        Me.pnlSavePassword.Location = New System.Drawing.Point(0, 95)
        Me.pnlSavePassword.Margin = New System.Windows.Forms.Padding(0)
        Me.pnlSavePassword.Name = "pnlSavePassword"
        Me.pnlSavePassword.Size = New System.Drawing.Size(172, 23)
        Me.pnlSavePassword.TabIndex = 10
        '
        'lblError
        '
        Me.lblError.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblError.AutoSize = True
        Me.lblError.Location = New System.Drawing.Point(3, 161)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(35, 13)
        Me.lblError.TabIndex = 10
        Me.lblError.Text = "[Error]"
        '
        'frmLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(312, 178)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblError)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmLogin"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Login"
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.pnlUsername.ResumeLayout(False)
        Me.pnlUsername.PerformLayout()
        Me.pnlPassword.ResumeLayout(False)
        Me.pnlPassword.PerformLayout()
        Me.pnlPasswordRetype.ResumeLayout(False)
        Me.pnlPasswordRetype.PerformLayout()
        Me.pnlSavePassword.ResumeLayout(False)
        Me.pnlSavePassword.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblUsername As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents ckSavePassword As System.Windows.Forms.CheckBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents pnlUsername As System.Windows.Forms.Panel
    Friend WithEvents pnlPassword As System.Windows.Forms.Panel
    Friend WithEvents pnlPasswordRetype As System.Windows.Forms.Panel
    Friend WithEvents txtPasswordRetype As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pnlSavePassword As System.Windows.Forms.Panel
    Friend WithEvents lblCaption As System.Windows.Forms.Label
    Friend WithEvents lblError As System.Windows.Forms.Label
End Class

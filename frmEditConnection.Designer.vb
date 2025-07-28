<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditConnection
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ddlDbType = New System.Windows.Forms.ComboBox()
        Me.ddlProvider = New System.Windows.Forms.ComboBox()
        Me.txtConnString = New System.Windows.Forms.TextBox()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.txtCategory = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblError = New System.Windows.Forms.Label()
        Me.btnConnStringHelp = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(26, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Database Type:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(60, 88)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Provider:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(94, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Connection String:"
        '
        'ddlDbType
        '
        Me.ddlDbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ddlDbType.FormattingEnabled = True
        Me.ddlDbType.Location = New System.Drawing.Point(118, 58)
        Me.ddlDbType.Name = "ddlDbType"
        Me.ddlDbType.Size = New System.Drawing.Size(164, 21)
        Me.ddlDbType.TabIndex = 3
        '
        'ddlProvider
        '
        Me.ddlProvider.FormattingEnabled = True
        Me.ddlProvider.Location = New System.Drawing.Point(118, 85)
        Me.ddlProvider.Name = "ddlProvider"
        Me.ddlProvider.Size = New System.Drawing.Size(164, 21)
        Me.ddlProvider.TabIndex = 4
        '
        'txtConnString
        '
        Me.txtConnString.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtConnString.Location = New System.Drawing.Point(118, 112)
        Me.txtConnString.Multiline = True
        Me.txtConnString.Name = "txtConnString"
        Me.txtConnString.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtConnString.Size = New System.Drawing.Size(395, 67)
        Me.txtConnString.TabIndex = 5
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(410, 195)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(55, 23)
        Me.btnOk.TabIndex = 7
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(471, 195)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(55, 23)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(71, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(38, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Name:"
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(118, 6)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(314, 20)
        Me.txtName.TabIndex = 1
        '
        'txtCategory
        '
        Me.txtCategory.Location = New System.Drawing.Point(118, 32)
        Me.txtCategory.Name = "txtCategory"
        Me.txtCategory.Size = New System.Drawing.Size(314, 20)
        Me.txtCategory.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(57, 35)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(52, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Category:"
        '
        'lblError
        '
        Me.lblError.AutoSize = True
        Me.lblError.ForeColor = System.Drawing.Color.Red
        Me.lblError.Location = New System.Drawing.Point(13, 208)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(45, 13)
        Me.lblError.TabIndex = 12
        Me.lblError.Text = "[lblError]"
        '
        'btnConnStringHelp
        '
        Me.btnConnStringHelp.FlatAppearance.BorderSize = 0
        Me.btnConnStringHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConnStringHelp.Image = Global.DbExplorer.My.Resources.Resources.help
        Me.btnConnStringHelp.Location = New System.Drawing.Point(516, 112)
        Me.btnConnStringHelp.Name = "btnConnStringHelp"
        Me.btnConnStringHelp.Size = New System.Drawing.Size(16, 16)
        Me.btnConnStringHelp.TabIndex = 13
        Me.btnConnStringHelp.TabStop = False
        Me.btnConnStringHelp.UseVisualStyleBackColor = True
        '
        'frmEditConnection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(540, 230)
        Me.Controls.Add(Me.btnConnStringHelp)
        Me.Controls.Add(Me.lblError)
        Me.Controls.Add(Me.txtCategory)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.txtConnString)
        Me.Controls.Add(Me.ddlProvider)
        Me.Controls.Add(Me.ddlDbType)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmEditConnection"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Connection"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ddlDbType As System.Windows.Forms.ComboBox
    Friend WithEvents ddlProvider As System.Windows.Forms.ComboBox
    Friend WithEvents txtConnString As System.Windows.Forms.TextBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtCategory As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents btnConnStringHelp As System.Windows.Forms.Button
End Class

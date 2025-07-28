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



<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PromptFormPrv
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
        Me.lblPrompt = New System.Windows.Forms.Label()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtValue = New System.Windows.Forms.TextBox()
        Me.dtpValue = New System.Windows.Forms.DateTimePicker()
        Me.ddlValue = New System.Windows.Forms.ComboBox()
        Me.lblError = New System.Windows.Forms.Label()
        Me.lstValue = New System.Windows.Forms.CheckedListBox()
        Me.pnlControls = New System.Windows.Forms.Panel()
        Me.txtValueMultiline = New System.Windows.Forms.TextBox()
        Me.pnlControls.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblPrompt
        '
        Me.lblPrompt.AutoSize = True
        Me.lblPrompt.Location = New System.Drawing.Point(5, 9)
        Me.lblPrompt.Name = "lblPrompt"
        Me.lblPrompt.Size = New System.Drawing.Size(46, 13)
        Me.lblPrompt.TabIndex = 0
        Me.lblPrompt.Text = "[Prompt]"
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.Location = New System.Drawing.Point(262, 455)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(58, 23)
        Me.btnOk.TabIndex = 50
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(326, 455)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(58, 23)
        Me.btnCancel.TabIndex = 100
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtValue
        '
        Me.txtValue.Location = New System.Drawing.Point(0, 0)
        Me.txtValue.Name = "txtValue"
        Me.txtValue.Size = New System.Drawing.Size(373, 20)
        Me.txtValue.TabIndex = 10
        Me.txtValue.WordWrap = False
        '
        'dtpValue
        '
        Me.dtpValue.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpValue.Location = New System.Drawing.Point(0, 26)
        Me.dtpValue.Name = "dtpValue"
        Me.dtpValue.Size = New System.Drawing.Size(93, 20)
        Me.dtpValue.TabIndex = 10
        '
        'ddlValue
        '
        Me.ddlValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ddlValue.FormattingEnabled = True
        Me.ddlValue.Location = New System.Drawing.Point(-1, 52)
        Me.ddlValue.Name = "ddlValue"
        Me.ddlValue.Size = New System.Drawing.Size(373, 21)
        Me.ddlValue.TabIndex = 10
        '
        'lblError
        '
        Me.lblError.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblError.AutoSize = True
        Me.lblError.ForeColor = System.Drawing.Color.Red
        Me.lblError.Location = New System.Drawing.Point(10, 465)
        Me.lblError.Name = "lblError"
        Me.lblError.Size = New System.Drawing.Size(45, 13)
        Me.lblError.TabIndex = 6
        Me.lblError.Text = "[lblError]"
        '
        'lstValue
        '
        Me.lstValue.CheckOnClick = True
        Me.lstValue.FormattingEnabled = True
        Me.lstValue.Location = New System.Drawing.Point(-1, 79)
        Me.lstValue.Name = "lstValue"
        Me.lstValue.Size = New System.Drawing.Size(373, 109)
        Me.lstValue.TabIndex = 10
        '
        'pnlControls
        '
        Me.pnlControls.AutoSize = True
        Me.pnlControls.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlControls.Controls.Add(Me.txtValueMultiline)
        Me.pnlControls.Controls.Add(Me.txtValue)
        Me.pnlControls.Controls.Add(Me.lstValue)
        Me.pnlControls.Controls.Add(Me.dtpValue)
        Me.pnlControls.Controls.Add(Me.ddlValue)
        Me.pnlControls.Location = New System.Drawing.Point(5, 42)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(472, 400)
        Me.pnlControls.TabIndex = 8
        '
        'txtValueMultiline
        '
        Me.txtValueMultiline.Location = New System.Drawing.Point(-1, 197)
        Me.txtValueMultiline.Multiline = True
        Me.txtValueMultiline.Name = "txtValueMultiline"
        Me.txtValueMultiline.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtValueMultiline.Size = New System.Drawing.Size(470, 200)
        Me.txtValueMultiline.TabIndex = 11
        '
        'PromptFormPrv
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(389, 485)
        Me.ControlBox = False
        Me.Controls.Add(Me.pnlControls)
        Me.Controls.Add(Me.lblError)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lblPrompt)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "PromptFormPrv"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Prompt"
        Me.pnlControls.ResumeLayout(False)
        Me.pnlControls.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblPrompt As System.Windows.Forms.Label
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtValue As System.Windows.Forms.TextBox
    Friend WithEvents dtpValue As System.Windows.Forms.DateTimePicker
    Friend WithEvents ddlValue As System.Windows.Forms.ComboBox
    Friend WithEvents lblError As System.Windows.Forms.Label
    Friend WithEvents lstValue As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlControls As System.Windows.Forms.Panel
    Friend WithEvents txtValueMultiline As System.Windows.Forms.TextBox
End Class

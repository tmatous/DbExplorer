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



Public Class frmLogin

    Public Property AllowBlankPassword As Boolean = False

    Public Property ShowUsername() As Boolean
        Get
            Return pnlUsername.Visible
        End Get
        Set(ByVal value As Boolean)
            pnlUsername.Visible = value
            ResizeForm()
        End Set
    End Property

    Public Property ShowPasswordRetype() As Boolean
        Get
            Return pnlPasswordRetype.Visible
        End Get
        Set(ByVal value As Boolean)
            pnlPasswordRetype.Visible = value
            ResizeForm()
        End Set
    End Property

    Public Property ShowSavePassword() As Boolean
        Get
            Return pnlSavePassword.Visible
        End Get
        Set(ByVal value As Boolean)
            pnlSavePassword.Visible = value
            ResizeForm()
        End Set
    End Property

    Public Property Username() As String
        Get
            Return txtUsername.Text
        End Get
        Set(ByVal value As String)
            txtUsername.Text = value
        End Set
    End Property

    Public Property Caption() As String
        Get
            Return lblCaption.Text
        End Get
        Set(ByVal value As String)
            lblCaption.Text = value
            lblCaption.Visible = Not String.IsNullOrEmpty(value)
            ResizeForm()
        End Set
    End Property

    Public Property Password() As String
        Get
            Return txtPassword.Text
        End Get
        Set(ByVal value As String)
            txtPassword.Text = value
        End Set
    End Property

    Public Property SetSavePassword() As Boolean
        Get
            Return ckSavePassword.Checked
        End Get
        Set(ByVal value As Boolean)
            ckSavePassword.Checked = value
        End Set
    End Property




    Private Sub ResizeForm()
        Me.Height = 110 + FlowLayoutPanel1.Height
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        If ((Me.txtPassword.Text = "") AndAlso (Not AllowBlankPassword)) Then Return
        If (Me.ShowUsername AndAlso (Me.txtUsername.Text = "")) Then Return
        If (Me.ShowPasswordRetype AndAlso (Me.txtPassword.Text <> Me.txtPasswordRetype.Text)) Then lblError.Text = "Passwords must match" : Return
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblError.Text = ""
    End Sub

    Private Sub txtPassword_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles txtPassword.PreviewKeyDown
        If (e.KeyCode = Keys.Enter) Then btnOk.PerformClick()
    End Sub

End Class
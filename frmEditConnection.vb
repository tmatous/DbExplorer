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



Imports System.Linq

Public Class frmEditConnection

    Private Const AUTO_PROVIDER = "[Automatic]"
    Private _origConn As Settings.Connection

    Public Shared Function Launch(pConn As Settings.Connection) As DialogResultWithPayload(Of Settings.Connection)
        Using frm As New frmEditConnection
            frm._origConn = pConn
            frm.ShowDialog()
            Dim conn As Settings.Connection = Nothing
            If (frm.DialogResult = Windows.Forms.DialogResult.OK) Then conn = frm.GetNewConn()

            Return New DialogResultWithPayload(Of Settings.Connection)(frm.DialogResult, conn)
        End Using
    End Function

    Private Sub btnConnStringHelp_Click(sender As Object, e As EventArgs) Handles btnConnStringHelp.Click
        Dim res = frmConnStringHelp.Launch()
        If (res.DialogResult = Windows.Forms.DialogResult.OK) Then
            txtConnString.Text = res.Payload.ConnString
            ddlDbType.Text = res.Payload.DbType.ToString
            If (Not String.IsNullOrEmpty(res.Payload.Provider)) Then
                ddlProvider.Text = res.Payload.Provider
            Else
                ddlProvider.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        Dim valid = ValidateForm()
        If (valid <> "") Then
            lblError.Text = valid
            Return
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub EditConnection_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim typs = [Enum].GetNames(GetType(Rational.DB.eDbType))
        typs(0) = ""
        ddlDbType.DataSource = typs
        Dim provs = Util.GetProviders()
        provs.Insert(0, AUTO_PROVIDER)
        ddlProvider.DataSource = provs

        If (_origConn IsNot Nothing) Then
            txtName.Text = _origConn.Name
            txtCategory.Text = _origConn.Category
            txtConnString.Text = _origConn.ConnString
            ddlDbType.Text = _origConn.DbType.ToString
            ddlProvider.Text = _origConn.Provider
        End If
        lblError.Text = ""
    End Sub

    Private Function ValidateForm() As String
        If (String.IsNullOrEmpty(txtName.Text)) Then Return "Please enter a name for this connection"
        'If (String.IsNullOrEmpty(ddlDbType.Text)) Then Return "Please select database type"
        If (String.IsNullOrEmpty(txtConnString.Text)) Then Return "Please enter a connection string"

        Return ""
    End Function

    Private Function GetNewConn() As Settings.Connection
        Dim typ As Rational.DB.eDbType
        [Enum].TryParse(ddlDbType.Text, typ)
        Dim conn = New Settings.Connection With {
            .Name = txtName.Text,
            .Category = txtCategory.Text,
            .ConnString = txtConnString.Text,
            .DbType = typ,
            .Provider = ddlProvider.Text
        }
        If (conn.Provider = AUTO_PROVIDER) Then conn.Provider = ""

        If ((_origConn Is Nothing) OrElse String.IsNullOrEmpty(_origConn.Password)) Then
            Util.SetupConnectionPassword(conn)
        Else
            If (MessageBox.Show("Do you want to update the password for this connection?", "Prompt", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes) Then
                Util.SetupConnectionPassword(conn)
            Else
                conn.Password = _origConn.Password
                conn.SavePassword = _origConn.SavePassword
            End If
        End If

        Return conn
    End Function

End Class
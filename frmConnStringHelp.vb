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



Public Class frmConnStringHelp

    Private _selected As ConnStringHelp

    Private _helpData As List(Of ConnStringHelp)
    Public ReadOnly Property HelpData() As List(Of ConnStringHelp)
        Get
            If (_helpData Is Nothing) Then
                Dim hd As New List(Of ConnStringHelp)
                hd.Add(New ConnStringHelp With {
                       .Title = "SQL Server with Windows auth",
                       .DbType = Rational.DB.eDbType.SqlServer,
                       .ConnString = "Server=ServerName; Database=DatabaseName; Trusted_Connection=true;",
                       .Description = "SQL Server using Windows authentication. Fill in ServerName and DatabaseName."
                   })
                hd.Add(New ConnStringHelp With {
                       .Title = "SQL Server with password prompt",
                       .DbType = Rational.DB.eDbType.SqlServer,
                       .ConnString = "Server=ServerName; Database=DatabaseName; UID=UserName; PWD={password};",
                       .Description = "SQL Server using database authentication. Fill in ServerName, DatabaseName, and UserName. Will prompt for password."
                   })
                hd.Add(New ConnStringHelp With {
                       .Title = "Oracle",
                       .DbType = Rational.DB.eDbType.Oracle,
                       .ConnString = "User ID=UserName; Password={password}; Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = ServerName)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = ServiceName))); Persist Security Info=False;",
                       .Description = "Sample Oracle connection string. Fill in ServerName, ServiceName and UserName. Modify the port if necessary. Will prompt for password."
                   })
                hd.Add(New ConnStringHelp With {
                       .Title = "MySQL",
                       .DbType = Rational.DB.eDbType.MySql,
                       .ConnString = "Server=ServerName; Port=PortNumber; Database=DatabaseName; Uid=UserName; Pwd={password};",
                       .Description = "Sample MySQL connection string. Fill in ServerName, PortNumber, DatabaseName and UserName. Will prompt for password."
                   })
                hd.Add(New ConnStringHelp With {
                       .Title = "Access",
                       .DbType = Rational.DB.eDbType.Unknown,
                       .Provider = "System.Data.Odbc",
                       .ConnString = "Driver={Microsoft Access Driver (*.mdb, *.accdb)};Dbq=C:\Database.mdb;",
                       .Description = "Sample Access connection string. Fill in the database file path. NOTE: Access drivers require the 32-bit version."
                   })

                _helpData = hd
            End If
            Return _helpData
        End Get
    End Property


    Public Shared Function Launch() As DialogResultWithPayload(Of ConnStringHelp)
        Using frm As New frmConnStringHelp
            frm.ShowDialog()
            Return New DialogResultWithPayload(Of ConnStringHelp)(frm.DialogResult, frm._selected)
        End Using
    End Function


    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        If (_selected Is Nothing) Then Return
        _selected.ConnString = txtConnString.Text
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblDescription.Text = ""
        For Each hd In HelpData
            lstConnStrings.Items.Add(New ListViewItem With {.Text = hd.Title})
        Next
        lstConnStrings.Columns(0).Width = lstConnStrings.ClientRectangle.Width
    End Sub

    Public Class ConnStringHelp
        Public Property Title As String
        Public Property Description As String
        Public Property DbType As Rational.DB.eDbType
        Public Property Provider As String
        Public Property Category As String
        Public Property ConnString As String
    End Class

    Private Sub lstConnStrings_DoubleClick(sender As Object, e As EventArgs) Handles lstConnStrings.DoubleClick
        btnOk_Click(Nothing, Nothing)
    End Sub

    Private Sub lstConnStrings_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstConnStrings.SelectedIndexChanged
        If (lstConnStrings.SelectedItems.Count = 0) Then Return
        Dim hd = (From h In HelpData Where h.Title = lstConnStrings.SelectedItems(0).Text).FirstOrDefault
        _selected = New ConnStringHelp With {
            .DbType = hd.DbType,
            .Provider = hd.Provider,
            .ConnString = hd.ConnString
        }

        txtConnString.Text = _selected.ConnString
        lblDescription.Text = _selected.Description
    End Sub

End Class
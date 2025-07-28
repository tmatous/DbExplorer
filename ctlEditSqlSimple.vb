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



Public Class ctlEditSqlSimple

    Public Event SqlTextChanged As Action(Of Object, EventArgs)

    Public Property SqlText() As String
        Get
            Return txtQuery.Text
        End Get
        Set(ByVal value As String)
            txtQuery.Text = value
            txtQuery.Select(0, 0)
        End Set
    End Property

    Public ReadOnly Property SelectionLength() As Int32
        Get
            Return txtQuery.SelectionLength
        End Get
    End Property

    Public ReadOnly Property SelectedText() As String
        Get
            Return txtQuery.SelectedText
        End Get
    End Property




    Private Sub ctlEditSqlSimple_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub txtQuery_KeyDown(sender As Object, e As KeyEventArgs) Handles txtQuery.KeyDown
        If ((e.KeyCode = Keys.A) AndAlso e.Control) Then
            'implement select all
            txtQuery.SelectAll()
            e.SuppressKeyPress = True
            e.Handled = True
        End If
    End Sub

    Private Sub txtQuery_TextChanged(sender As Object, e As EventArgs) Handles txtQuery.TextChanged
        'always keep a newline at the end, helps for line selection using the keyboard
        If (Not txtQuery.Text.EndsWith(Environment.NewLine)) Then
            Dim position = txtQuery.SelectionStart
            txtQuery.Text += Environment.NewLine
            txtQuery.Select(position, 0)
        End If
        RaiseEvent SqlTextChanged(Me, New EventArgs)
    End Sub

End Class

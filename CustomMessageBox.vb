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


Friend Class CustomMessageBoxPrv

    Private CustomDialogResult As String
    Private Prompt As String
    Private Options As CustomMessageBox.CustomMessageBoxOptions
    Private Const BUTTON_COUNT = 15
    Private _buttons As New List(Of Button)

    Public Sub New(pOptions As CustomMessageBox.CustomMessageBoxOptions)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Options = pOptions
        Me.TopMost = Options.TopMost
        For i As Int32 = 0 To BUTTON_COUNT - 1
            Dim btn As Button = DirectCast(Me.tblButtons.Controls(String.Format("Button{0}", i)), Button)
            _buttons.Add(btn)
            AddHandler btn.Click, AddressOf CustomButton_Click
            DisplayButton(btn, False)
        Next
    End Sub

    Private Sub CustomMsgBox_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        tblButtons.Left = (Me.Width \ 2) - (tblButtons.Width \ 2)
        lblPrompt.MaximumSize = New Size(Me.Width, 1000)
        lblPrompt.Text = Me.Prompt
        tblButtons.Top = lblPrompt.Top + lblPrompt.Height + 10
        Me.Height = tblButtons.Top + tblButtons.Height + 10
        Me.Focus()
        Me.BringToFront()
    End Sub

    Private Sub CustomButton_Click(sender As System.Object, e As System.EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Me.CustomDialogResult = btn.Text
        Close()
    End Sub

    Public Function Display(pPrompt As String, pButtonLabels As IEnumerable(Of String), pTitle As String) As String
        If (pButtonLabels.Count > BUTTON_COUNT) Then Throw New ArgumentOutOfRangeException("Maximum number of buttons exceeded")
        Dim lbl As String
        For i As Int32 = 0 To BUTTON_COUNT - 1
            lbl = Nothing
            If (pButtonLabels.Count > i) Then
                lbl = pButtonLabels(i)
            End If
            If (Not String.IsNullOrEmpty(lbl)) Then
                DisplayButton(_buttons(i), True)
                _buttons(i).Text = lbl
            Else
                DisplayButton(_buttons(i), False)
            End If
        Next
        Me.Prompt = pPrompt
        Me.Text = pTitle
        Me.ShowDialog()
        Return Me.CustomDialogResult
    End Function

    Private Sub DisplayButton(pButton As Button, pDisplay As Boolean)
        If (pDisplay) Then
            pButton.AutoSize = True
            pButton.Margin = New Padding(5, 10, 5, 10)
            pButton.Padding = New Padding(5, 0, 5, 0)
            pButton.Visible = True
        Else
            pButton.Text = ""
            pButton.AutoSize = False
            pButton.Width = 1
            pButton.Visible = False
        End If
    End Sub
End Class




Public Class CustomMessageBox

    Private Sub New()
        'no public instantiation
    End Sub

    Public Shared Function Show(pPrompt As String, pButtonLabels As IEnumerable(Of String), Optional pTitle As String = "Prompt") As String
        Return Show(pPrompt, pButtonLabels, New CustomMessageBoxOptions, pTitle)
    End Function

    Public Shared Function ShowTopMost(pPrompt As String, pButtonLabels As IEnumerable(Of String), Optional pTitle As String = "Prompt") As String
        Return Show(pPrompt, pButtonLabels, New CustomMessageBoxOptions With {.TopMost = True}, pTitle)
    End Function

    Public Shared Function Show(pPrompt As String, pButtonLabels As IEnumerable(Of String), pOptions As CustomMessageBoxOptions, Optional pTitle As String = "Prompt") As String
        Using frm As New CustomMessageBoxPrv(pOptions)
            Return frm.Display(pPrompt, pButtonLabels, pTitle)
        End Using
    End Function

    Public Class CustomMessageBoxOptions
        Public Property TopMost As Boolean = False
        Public Property Width As Int32?
        Public Property Height As Int32?
        Public Property ReadOnlyMessage As Boolean = True
    End Class

End Class

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


Friend Class PromptFormPrv

    Private _validator As IValidator

    Private Sub PromptFormPrv_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Focus()
        Me.BringToFront()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        DoOkCheck()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Public Function PromptForSingleLineString(ByRef pioValue As String, ByVal pPrompt As String, ByVal pTitle As String, ByVal pValidator As IValidator) As DialogResult
        ResetForm(pPrompt, pTitle, txtValue)

        If (pioValue Is Nothing) Then pioValue = ""
        txtValue.Text = pioValue
        _validator = pValidator

        Me.ShowDialog()

        If (Me.DialogResult = DialogResult.OK) Then
            pioValue = txtValue.Text
        End If

        Return Me.DialogResult
    End Function

    Public Function PromptForMultilineString(ByRef pioValue As String, ByVal pPrompt As String, ByVal pTitle As String, ByVal pValidator As StringValidator) As DialogResult
        ResetForm(pPrompt, pTitle, txtValueMultiline)

        If (pioValue Is Nothing) Then pioValue = ""
        txtValueMultiline.Text = pioValue
        _validator = pValidator

        Me.ShowDialog()

        If (Me.DialogResult = DialogResult.OK) Then
            pioValue = txtValueMultiline.Text
        End If

        Return Me.DialogResult
    End Function

    Public Function PromptForNumber(ByRef pioValue As Int32, ByVal pPrompt As String, ByVal pTitle As String, ByVal pValidator As NumberValidator) As DialogResult
        ResetForm(pPrompt, pTitle, txtValue)

        txtValue.Text = pioValue.ToString

        _validator = pValidator
        If (_validator Is Nothing) Then _validator = New NumberValidator With {.FloatingPoint = True}

        Me.ShowDialog()

        If (Me.DialogResult = DialogResult.OK) Then
            pioValue = CInt(txtValue.Text)
        End If

        Return Me.DialogResult
    End Function

    Public Function PromptForDate(ByRef pioValue As DateTime, ByVal pPrompt As String, ByVal pTitle As String, ByVal pValidator As DateValidator) As DialogResult
        ResetForm(pPrompt, pTitle, dtpValue)

        If ((pioValue = DateTime.MinValue) OrElse (pioValue = DateTime.MaxValue)) Then pioValue = DateTime.Now
        dtpValue.Value = pioValue
        _validator = pValidator

        Me.ShowDialog()

        If (Me.DialogResult = DialogResult.OK) Then
            pioValue = dtpValue.Value
        End If

        Return Me.DialogResult
    End Function

    Public Function PromptForStringFromList(ByRef pioValue As String, ByVal pChoices As IEnumerable(Of String), ByVal pPrompt As String, ByVal pTitle As String, ByVal pValidator As StringValidator, ByVal pAllowUserEntry As Boolean) As DialogResult
        ResetForm(pPrompt, pTitle, ddlValue)
        If (pAllowUserEntry) Then ddlValue.DropDownStyle = ComboBoxStyle.DropDown

        For Each el In pChoices
            ddlValue.Items.Add(el)
        Next
        ddlValue.Text = If(pioValue, "")
        _validator = pValidator

        Me.ShowDialog()

        If (Me.DialogResult = DialogResult.OK) Then
            pioValue = ddlValue.Text
        End If

        Return Me.DialogResult
    End Function

    Public Function PromptForValueFromList(Of TPayload)(ByRef pioValue As TPayload, ByVal pChoices As IDictionary(Of TPayload, String), ByVal pPrompt As String, ByVal pTitle As String) As DialogResult
        ResetForm(pPrompt, pTitle, ddlValue)

        For Each el In pChoices
            Dim newIt = New ObjectLabelPair(el.Key, el.Value)
            ddlValue.Items.Add(newIt)
            If (el.Key.Equals(pioValue)) Then ddlValue.SelectedItem = newIt
        Next

        Me.ShowDialog()

        If (Me.DialogResult = DialogResult.OK) Then
            pioValue = ObjectLabelPair.GetPayload(Of TPayload)(ddlValue.SelectedItem)
        End If

        Return Me.DialogResult
    End Function

    Public Function PromptForValuesFromList(Of TPayload)(ByRef pioValues As List(Of TPayload), ByVal pChoices As IDictionary(Of TPayload, String), ByVal pPrompt As String, ByVal pTitle As String) As DialogResult
        ResetForm(pPrompt, pTitle, lstValue)

        For Each el In pChoices
            Dim newIt = New ObjectLabelPair(el.Key, el.Value)
            lstValue.Items.Add(newIt)
            If (pioValues.Contains(el.Key)) Then lstValue.SetItemChecked(lstValue.Items.Count - 1, True)
        Next

        Me.ShowDialog()

        If (Me.DialogResult = DialogResult.OK) Then
            pioValues.Clear()
            Dim itKey As TPayload
            For Each it In lstValue.CheckedItems
                itKey = ObjectLabelPair.GetPayload(Of TPayload)(it)
                pioValues.Add(itKey)
            Next
        End If

        Return Me.DialogResult
    End Function

    Private Sub ResetForm(pPrompt As String, pTitle As String, pVisibleControl As Control)
        _validator = Nothing
        Dim controlPos As New Point(0, 0)

        txtValue.Visible = False
        txtValue.Location = controlPos

        txtValueMultiline.Visible = False
        txtValueMultiline.Location = controlPos

        dtpValue.Visible = False
        dtpValue.Location = controlPos

        ddlValue.Visible = False
        ddlValue.Location = controlPos
        ddlValue.Items.Clear()
        ddlValue.DropDownStyle = ComboBoxStyle.DropDownList

        lstValue.Visible = False
        lstValue.Location = controlPos
        lstValue.Items.Clear()

        lblPrompt.MaximumSize = New Size(Me.Width - 10, 0)
        lblPrompt.Text = pPrompt
        Me.Text = pTitle
        lblError.Text = ""

        If (String.IsNullOrEmpty(lblPrompt.Text)) Then
            pnlControls.Top = 10
        Else
            pnlControls.Top = lblPrompt.Top + lblPrompt.Height + 10
        End If
        Me.Width = 400
        Me.AcceptButton = btnOk

        pVisibleControl.Visible = True
        If (pVisibleControl.Equals(txtValueMultiline)) Then
            Me.AcceptButton = Nothing
            Me.Width = 500
        End If

        Me.Height = pnlControls.Top + pnlControls.Height + btnOk.Height + 60
    End Sub

    Private Sub PromptForm_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.BringToFront()
        Me.Focus()
        If (ddlValue.Visible) Then ddlValue.Focus()
        If (dtpValue.Visible) Then dtpValue.Focus()
        If (txtValue.Visible) Then txtValue.Focus()
        If (txtValueMultiline.Visible) Then txtValueMultiline.Focus()
        If (lstValue.Visible) Then lstValue.Focus()
    End Sub

    Private Sub DoOkCheck()
        Dim done = False
        Dim valToCheck = ""
        If (ddlValue.Visible) Then valToCheck = ddlValue.Text
        If (dtpValue.Visible) Then valToCheck = dtpValue.Value.ToString
        If (txtValue.Visible) Then valToCheck = txtValue.Text
        If (_validator IsNot Nothing) Then
            Dim check = _validator.Validate(valToCheck)
            If (check.Success) Then
                done = True
            Else
                Me.lblError.Text = check.Description
            End If
        Else
            done = True
        End If

        If (done) Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Close()
        End If
    End Sub

End Class





Public Class PromptForm

    Private Sub New()
        'no public instantiation
    End Sub

    Public Shared Function PromptForSingleLineString(ByRef pioValue As String, Optional ByVal pPrompt As String = "", Optional ByVal pTitle As String = "Prompt", Optional ByVal pValidator As IValidator = Nothing) As DialogResult
        Using frm As New PromptFormPrv
            Return frm.PromptForSingleLineString(pioValue, pPrompt, pTitle, pValidator)
        End Using
    End Function

    Public Shared Function PromptForMultilineString(ByRef pioValue As String, Optional ByVal pPrompt As String = "", Optional ByVal pTitle As String = "Prompt", Optional ByVal pValidator As StringValidator = Nothing) As DialogResult
        Using frm As New PromptFormPrv
            Return frm.PromptForMultilineString(pioValue, pPrompt, pTitle, pValidator)
        End Using
    End Function

    Public Shared Function PromptForNumber(ByRef pioValue As Int32, Optional ByVal pPrompt As String = "", Optional ByVal pTitle As String = "Prompt", Optional ByVal pValidator As NumberValidator = Nothing) As DialogResult
        Using frm As New PromptFormPrv
            Return frm.PromptForNumber(pioValue, pPrompt, pTitle, pValidator)
        End Using
    End Function

    Public Shared Function PromptForDate(ByRef pioValue As DateTime, Optional ByVal pPrompt As String = "", Optional ByVal pTitle As String = "Prompt", Optional ByVal pValidator As DateValidator = Nothing) As DialogResult
        Using frm As New PromptFormPrv
            Return frm.PromptForDate(pioValue, pPrompt, pTitle, pValidator)
        End Using
    End Function

    Public Shared Function PromptForStringFromList(ByRef pioValue As String, ByVal pChoices As IEnumerable(Of String), Optional ByVal pPrompt As String = "", Optional ByVal pTitle As String = "Prompt", Optional ByVal pValidator As StringValidator = Nothing, Optional ByVal pAllowUserEntry As Boolean = False) As DialogResult
        Using frm As New PromptFormPrv
            Return frm.PromptForStringFromList(pioValue, pChoices, pPrompt, pTitle, pValidator, pAllowUserEntry)
        End Using
    End Function

    Public Shared Function PromptForValueFromList(Of TPayload)(ByRef pioValue As TPayload, ByVal pChoices As IDictionary(Of TPayload, String), Optional ByVal pPrompt As String = "", Optional ByVal pTitle As String = "Prompt") As DialogResult
        Using frm As New PromptFormPrv
            Return frm.PromptForValueFromList(Of TPayload)(pioValue, pChoices, pPrompt, pTitle)
        End Using
    End Function

    Public Shared Function PromptForValuesFromList(Of TPayload)(ByRef pioValues As List(Of TPayload), ByVal pChoices As IDictionary(Of TPayload, String), Optional ByVal pPrompt As String = "", Optional ByVal pTitle As String = "Prompt") As DialogResult
        Using frm As New PromptFormPrv
            Return frm.PromptForValuesFromList(Of TPayload)(pioValues, pChoices, pPrompt, pTitle)
        End Using
    End Function

End Class

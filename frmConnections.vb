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



Public Class frmConnections

    Private _settings As New Settings
    Private _settingsFileInfo As IO.FileInfo = Nothing
    Private _settingsModified As Boolean = False

    Public Function GetSelectedConnection() As Settings.Connection
        If (lstConnections.SelectedItems.Count = 0) Then Return Nothing
        Dim conn = DirectCast(lstConnections.SelectedItems(0).Tag, Settings.Connection)
        Return conn
    End Function

    Public Function GetLoadSchema() As Boolean
        Return ckLoadSchema.Checked
    End Function



    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim res = frmEditConnection.Launch(Nothing)
        If (res.DialogResult <> Windows.Forms.DialogResult.OK) Then Return
        _settings.Connections.Add(res.Payload)
        _settingsModified = True
        PopulateConnections()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnConnect_Click(sender As Object, e As EventArgs) Handles btnConnect.Click
        If (lstConnections.SelectedItems.Count = 0) Then Return
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If (lstConnections.SelectedItems.Count = 0) Then Return
        Dim conn = GetSelectedConnection()
        Dim res = frmEditConnection.Launch(conn)
        If (res.DialogResult <> Windows.Forms.DialogResult.OK) Then Return
        _settings.Connections.Remove(conn)
        _settings.Connections.Add(res.Payload)
        _settingsModified = True
        PopulateConnections()
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
        If (lstConnections.SelectedItems.Count = 0) Then Return
        Dim conn = GetSelectedConnection()
        Dim res = frmEditConnection.Launch(conn)
        If (res.DialogResult <> Windows.Forms.DialogResult.OK) Then Return
        _settings.Connections.Add(res.Payload)
        _settingsModified = True
        PopulateConnections()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If (lstConnections.SelectedItems.Count = 0) Then Return
        If (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel) <> Windows.Forms.DialogResult.OK) Then Return
        _settings.Connections.Remove(GetSelectedConnection)
        _settingsModified = True
        PopulateConnections()
    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click
        PromptSaveIfChangedSettings()
        Dim fn = ""
        Using frm = New Windows.Forms.OpenFileDialog()
            frm.Filter = "Connections file|Connections.config|All Files|*.*"
            frm.InitialDirectory = Application.StartupPath
            If (frm.ShowDialog() <> Windows.Forms.DialogResult.OK) Then Return
            fn = frm.FileName
        End Using

        Try
            LoadSettings(fn)
            PopulateConnections()
        Catch ex As Exception
            Util.ShowError(String.Format("Error loading settings: {0}", ex.Message))
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        DoSaveSettings(pConfirmFilename:=True)
    End Sub

    Private Sub btnPassword_Click(sender As Object, e As EventArgs) Handles btnPassword.Click
        DoChangePassword()
    End Sub

    Private Sub lstConnections_KeyDown(sender As Object, e As KeyEventArgs) Handles lstConnections.KeyDown
        If (e.KeyCode = Keys.Enter) Then btnConnect_Click(Nothing, Nothing)
    End Sub

    Private Sub lstConnections_DoubleClick(sender As Object, e As EventArgs) Handles lstConnections.DoubleClick
        btnConnect_Click(Nothing, Nothing)
    End Sub

    Private Sub txtFilter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFilter.TextChanged
        PopulateConnections()
    End Sub



    Private Sub PopulateConnections()
        lstConnections.Groups.Clear()
        lstConnections.Items.Clear()

        'sort connections by category, name
        _settings.Connections = (From c In _settings.Connections Select c Order By If(c.Category, ""), c.Name).ToList

        Dim filteredConnections = _settings.Connections
        If (Not String.IsNullOrWhiteSpace(txtFilter.Text)) Then
            filteredConnections = (From c In _settings.Connections Where c.Name.ToUpper.Contains(txtFilter.Text.ToUpper) Select c).ToList
        End If
        Dim grps = New Dictionary(Of String, ListViewGroup)(StringComparer.OrdinalIgnoreCase)
        For Each catName In (From c In filteredConnections Where Not String.IsNullOrEmpty(c.Category) Select c.Category Distinct)
            Dim grp = New ListViewGroup(catName, HorizontalAlignment.Left)
            grps.Add(catName, grp)
            lstConnections.Groups.Add(grps.Item(catName))
        Next
        If (grps.Count = 0) Then
            lstConnections.ShowGroups = False
        Else
            lstConnections.ShowGroups = True
        End If
        For Each conn In filteredConnections
            Dim lvi As New ListViewItem(conn.Name)
            lvi.Tag = conn
            If (Not String.IsNullOrEmpty(conn.Category)) Then lvi.Group = grps.Item(conn.Category)
            lstConnections.Items.Add(lvi)
        Next

        lstConnections.Columns(0).Width = lstConnections.ClientRectangle.Width
    End Sub

    Private Sub Connections_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopulateConnections()
        lstConnections.Focus()
    End Sub

    Private Sub frmConnections_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If (_settingsFileInfo Is Nothing) Then
            _settingsFileInfo = g_DefaultSettingsFileInfo
        End If

        Try
            LoadSettings(_settingsFileInfo.FullName)
            PopulateConnections()
        Catch ex As Exception
            Util.ShowError(String.Format("Error loading settings: {0}", ex.Message))
        End Try
    End Sub

    Private Const MASTER_PASSWORD_CHECK As String = "{50E972A5-47B1-44D2-94D1-1481AFE95840}"

    Public Sub LoadSettings(pFilename As String)
        If (Not IO.File.Exists(pFilename)) Then Throw New Exception("File not found")

        Dim xml = IO.File.ReadAllText(pFilename)
        Dim settngs = Util.DeserializeObjectXmlContract(Of Settings)(xml)
        If (Not String.IsNullOrEmpty(settngs.MasterPassword)) Then
            Dim typedPass = ""
            Dim passOk = False
            Do
                Using frm As New frmLogin
                    frm.ShowUsername = False
                    frm.ShowPasswordRetype = False
                    frm.ShowSavePassword = False
                    frm.Caption = "Enter the Master Password"
                    frm.StartPosition = FormStartPosition.CenterParent
                    If (frm.ShowDialog(Me) <> DialogResult.OK) Then Return
                    typedPass = frm.Password
                End Using
                Dim checkPass = Util.Decrypt(settngs.MasterPassword, typedPass)
                If (checkPass = MASTER_PASSWORD_CHECK) Then
                    passOk = True
                Else
                    MessageBox.Show("Incorrect password")
                End If
            Loop Until (passOk)

            settngs.MasterPassword = typedPass
            For Each cn In settngs.Connections
                If (cn.SavePassword) Then
                    Try
                        cn.Password = Util.Decrypt(cn.Password, settngs.MasterPassword)
                    Catch ex As Exception
                        cn.Password = ""
                    End Try
                Else
                    cn.Password = ""
                End If
            Next
        End If

        'sort connections by category, name
        settngs.Connections = (From c In settngs.Connections Select c Order By If(c.Category, ""), c.Name).ToList

        _settingsFileInfo = New IO.FileInfo(pFilename)
        _settings = settngs
        _settingsModified = False
    End Sub

    Public Sub SaveSettings(pFilename As String)

        'back up and clear the unencrypted passwords
        Dim tempMasterPassword = If(_settings.MasterPassword, "")
        _settings.MasterPassword = ""
        Dim tempPasswords As New Dictionary(Of Settings.Connection, String)
        For Each cn In _settings.Connections
            tempPasswords.Item(cn) = If(cn.Password, "")
            cn.Password = ""
        Next

        If (Not String.IsNullOrEmpty(tempMasterPassword)) Then
            _settings.MasterPassword = Util.Encrypt(MASTER_PASSWORD_CHECK, tempMasterPassword)
            For Each cn In _settings.Connections
                If (cn.SavePassword) Then
                    Try
                        cn.Password = Util.Encrypt(tempPasswords.Item(cn), tempMasterPassword)
                    Catch ex As Exception
                        cn.Password = ""
                    End Try
                End If
            Next
        End If

        'sort connections by category, name
        _settings.Connections = (From c In _settings.Connections Select c Order By If(c.Category, ""), c.Name).ToList

        Dim xml = Util.SerializeObjectXmlContract(_settings)
        IO.File.WriteAllText(pFilename, xml)

        'restore unencrypted passwords
        _settings.MasterPassword = tempMasterPassword
        For Each cn In _settings.Connections
            cn.Password = tempPasswords.Item(cn)
        Next

        _settingsFileInfo = New IO.FileInfo(pFilename)
        _settingsModified = False
    End Sub

    Public Sub PromptSaveIfChangedSettings()
        If (Not _settingsModified) Then Return
        If (MessageBox.Show("Settings have changed. Save?", "Confirm", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes) Then DoSaveSettings(pConfirmFilename:=False)
    End Sub

    Private Sub DoSaveSettings(pConfirmFilename As Boolean)
        Dim fn = ""
        If (pConfirmFilename) Then
            Using frm = New Windows.Forms.SaveFileDialog()
                frm.Filter = "Connections file|Connections.config|All Files|*.*"
                frm.InitialDirectory = _settingsFileInfo.DirectoryName
                frm.FileName = _settingsFileInfo.Name
                If (frm.ShowDialog() <> Windows.Forms.DialogResult.OK) Then Return
                fn = frm.FileName
            End Using
        Else
            fn = _settingsFileInfo.FullName
        End If

        Dim needPw = (From cn In _settings.Connections Where cn.SavePassword).Count
        If ((needPw > 0) AndAlso (String.IsNullOrEmpty(_settings.MasterPassword))) Then
            MessageBox.Show("Some connections contain passwords. You must set a Master Password.")
            DoChangePassword()
            If (String.IsNullOrEmpty(_settings.MasterPassword)) Then Return
        End If

        Try
            SaveSettings(fn)
        Catch ex As Exception
            Util.ShowError(String.Format("Error saving settings: {0}", ex.Message))
        End Try
    End Sub

    Private Sub DoChangePassword()
        If (String.IsNullOrEmpty(_settings.MasterPassword)) Then
            Dim message = "Please note: The Master Password will only encrypt passwords using the {password} keyword. Passwords entered directly into a connection string will not be encrypted."
            message += Environment.NewLine + Environment.NewLine
            message += "If you are unsure, please Cancel and re-check your connection strings."
            If (MessageBox.Show(message, "Warning", MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.Cancel) Then Return
        End If
        Using frm As New frmLogin
            frm.ShowUsername = False
            frm.ShowPasswordRetype = True
            frm.ShowSavePassword = False
            frm.Caption = "Set the Master Password"
            If (frm.ShowDialog() <> DialogResult.OK) Then Return
            _settings.MasterPassword = frm.Password
            _settingsModified = True
        End Using
    End Sub

End Class
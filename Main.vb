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



Public Module Main

    Private _embedDll As EmbedDll
    Private _connectionsFrm As frmConnections
    Private _appContext As ApplicationContext

    Public g_DefaultSettingsFileInfo As IO.FileInfo

    Public Sub Main()
        Application.EnableVisualStyles()

        _embedDll = New EmbedDll("EmbedDllData")
        '_embedDll.Register("DbSchemaTools")
        '_embedDll.Register("DbCodeGen")
        '_embedDll.Register("Rational.DB")
        _embedDll.Register("EPPlus")
        _embedDll.Register("ScintillaNET")
        _embedDll.Register("LumenWorks.Framework.IO")
        _embedDll.Register("Westwind.RazorHosting")
        _embedDll.Register("System.Web.Razor")

        Dim settingsPath = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DbExplorer")
        If (Not IO.Directory.Exists(settingsPath)) Then
            Try
                IO.Directory.CreateDirectory(settingsPath)
            Catch ex As Exception
            End Try
        End If
        If (Not IO.Directory.Exists(settingsPath)) Then
            settingsPath = Application.StartupPath
        End If

        Dim args = Environment.GetCommandLineArgs()
        Dim filePath = ""
        If ((args.Length = 2) AndAlso IO.File.Exists(args(1))) Then
            filePath = args(1)
        Else
            filePath = IO.Path.Combine(settingsPath, "Connections.config")
        End If
        If (Not IO.File.Exists(filePath)) Then
            Dim defaultFilePath = IO.Path.Combine(Application.StartupPath, "Sample.config")
            Try
                IO.File.Copy(defaultFilePath, filePath)
            Catch ex As Exception
            End Try
        End If
        g_DefaultSettingsFileInfo = New IO.FileInfo(filePath)
        Util.SetCurrentOutputFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))

        frmMain.Launch(Nothing, pLoadSchema:=True)

        _appContext = New ApplicationContext()
        Application.Run(_appContext)
    End Sub

    Public Class LaunchConnectionsDialogResult
        Public Property DialogResult As DialogResult
        Public Property Connection As Settings.Connection
        Public Property LoadSchema As Boolean
    End Class

    Public Function LaunchConnectionsDialog(pParent As Form) As LaunchConnectionsDialogResult
        If (_connectionsFrm Is Nothing) Then
            _connectionsFrm = New frmConnections()
        End If
        _connectionsFrm.StartPosition = FormStartPosition.CenterParent
        _connectionsFrm.ShowDialog(pParent)

        Return New LaunchConnectionsDialogResult With {
            .DialogResult = _connectionsFrm.DialogResult,
            .Connection = _connectionsFrm.GetSelectedConnection,
            .LoadSchema = _connectionsFrm.GetLoadSchema
        }
    End Function

    Private Function GetOpenMainForms() As IList(Of Form)
        Dim foundOpen = New List(Of Form)
        For Each frm As Form In Application.OpenForms
            If ((frm.GetType() = GetType(frmMain)) AndAlso (Not frm.IsDisposed)) Then foundOpen.Add(frm)
        Next
        Return foundOpen
    End Function

    Public Function GetOpenFormGroupChildForms(pFormGroupKey As String) As IList(Of Form)
        Dim foundOpen = New List(Of Form)
        For Each frm As Form In Application.OpenForms
            If ((TypeOf frm Is IFormGroupKey) AndAlso (Not frm.IsDisposed)) Then
                Dim fgf = DirectCast(frm, IFormGroupKey)
                If ((fgf.FormGroupKey = pFormGroupKey) AndAlso (Not fgf.IsFormGroupParent)) Then foundOpen.Add(frm)
            End If
        Next
        Return foundOpen
    End Function

    Public Sub PromptSaveSettingsIfLastWindow()
        If (GetOpenMainForms().Count <= 1) Then
            _connectionsFrm.PromptSaveIfChangedSettings()
        End If
    End Sub

    Public Sub ExitAppIfNoWindows()
        If (GetOpenMainForms().Count = 0) Then
            Util.CleanupTempFiles()
            _appContext.ExitThread()
        End If
    End Sub

End Module

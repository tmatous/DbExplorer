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


Public Class EmbedDll

    Private _registeredDlls As Dictionary(Of String, String)
    Private _resourceFileAssembly As Reflection.Assembly
    Private _resourceFilePath As String

    Public Sub New(pResourceFile As String)
        Dim callingType = (New StackFrame(1)).GetMethod().DeclaringType
        If (Not pResourceFile.Contains(".")) Then pResourceFile = String.Format("{0}.{1}", callingType.Namespace, pResourceFile)
        Init(pResourceFile, callingType.Assembly)
    End Sub

    Public Sub New(pResourceFilePath As String, pResourceFileAssembly As Reflection.Assembly)
        Init(pResourceFilePath, pResourceFileAssembly)
    End Sub

    Private Sub Init(pResourceFilePath As String, pResourceFileAssembly As Reflection.Assembly)
        _registeredDlls = New Dictionary(Of String, String)
        _resourceFileAssembly = pResourceFileAssembly
        _resourceFilePath = pResourceFilePath
        AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf AppDomain_AssemblyResolve
    End Sub

    Public Sub Register(pDllName As String, pResourceName As String)
        _registeredDlls.Add(pDllName, pResourceName)
    End Sub

    Public Sub Register(pDllName As String)
        _registeredDlls.Add(pDllName, pDllName.Replace(".", "_"))
    End Sub

    Private Function AppDomain_AssemblyResolve(sender As Object, args As ResolveEventArgs) As System.Reflection.Assembly
        Dim dllName = args.Name
        If (dllName.Contains(",")) Then dllName = dllName.Substring(0, dllName.IndexOf(","))
        If (_registeredDlls.ContainsKey(dllName)) Then
            Try
                Dim rm As New Resources.ResourceManager(_resourceFilePath, _resourceFileAssembly)
                Dim bytes As Byte() = DirectCast(rm.GetObject(_registeredDlls.Item(dllName)), Byte())
                Return System.Reflection.Assembly.Load(bytes)
            Catch ex As Exception
                Util.ShowError(String.Format("Unable to load assembly: {0}", dllName))
            End Try
        End If
        Return Nothing
    End Function


End Class

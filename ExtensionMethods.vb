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



Imports System.Runtime.CompilerServices

Module ExtensionMethods

#Region "StringBuilder"

    <Extension()>
    Public Sub exAppendFormatLine(ByVal pSb As System.Text.StringBuilder, ByVal format As String, ByVal Arg0 As Object, Optional ByVal Arg1 As Object = Nothing, Optional ByVal Arg2 As Object = Nothing, Optional ByVal Arg3 As Object = Nothing, Optional ByVal Arg4 As Object = Nothing)
        pSb.AppendFormat(format, Arg0, Arg1, Arg2, Arg3, Arg4)
        pSb.AppendLine()
    End Sub

#End Region

End Module

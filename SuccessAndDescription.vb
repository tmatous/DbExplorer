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



Public Class SuccessAndDescription
    Public Property Success As Boolean
    Public Property Description As String

    Public Sub New(pSuccess As Boolean, pDescription As String)
        Success = pSuccess
        Description = pDescription
    End Sub

    ''' <summary>
    ''' Creates a new object indicating success
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Ok() As SuccessAndDescription
        Return New SuccessAndDescription(True, "")
    End Function

    ''' <summary>
    ''' Creates a new object indicating failure
    ''' </summary>
    ''' <param name="pDescription"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function NotOk(pDescription As String) As SuccessAndDescription
        Return New SuccessAndDescription(False, pDescription)
    End Function

End Class

Public Class SuccessAndDescription(Of TPayload)
    Public Property Success As Boolean
    Public Property Description As String
    Public Property Payload As TPayload

    Public Sub New(pSuccess As Boolean, pDescription As String, pPayload As TPayload)
        Success = pSuccess
        Description = pDescription
        Payload = pPayload
    End Sub

    ''' <summary>
    ''' Creates a new object indicating success
    ''' </summary>
    ''' <param name="pPayload"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Ok(pPayload As TPayload) As SuccessAndDescription(Of TPayload)
        Return New SuccessAndDescription(Of TPayload)(True, "", pPayload)
    End Function

    ''' <summary>
    ''' Creates a new object indicating failure
    ''' </summary>
    ''' <param name="pDescription"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function NotOk(pDescription As String) As SuccessAndDescription(Of TPayload)
        Return New SuccessAndDescription(Of TPayload)(False, pDescription, Nothing)
    End Function

End Class

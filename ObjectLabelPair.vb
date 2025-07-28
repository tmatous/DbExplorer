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



Public Class ObjectLabelPair
    Public Property Payload As Object
    Public Property Label As String

    Sub New(pPayload As Object, pLabel As String)
        Payload = pPayload
        Label = pLabel
    End Sub

    Public Overrides Function ToString() As String
        Return Label
    End Function

    Public Shared Function GetPayload(Of TPayloadType)(pOLPair As Object) As TPayloadType
        Return GetPayload(Of TPayloadType)(pOLPair, Nothing)
    End Function

    Public Shared Function GetPayload(Of TPayloadType)(pOLPair As Object, pDefault As TPayloadType) As TPayloadType
        If ((pOLPair Is Nothing) OrElse (Not (TypeOf pOLPair Is ObjectLabelPair))) Then Return pDefault

        Dim olp = DirectCast(pOLPair, ObjectLabelPair)
        If (TypeOf olp.Payload Is TPayloadType) Then
            Return DirectCast(olp.Payload, TPayloadType)
        Else
            Return pDefault
        End If
    End Function

    Public Shared Function FindInList(Of TPayloadType)(pCollection As IEnumerable, pPayloadVal As TPayloadType) As ObjectLabelPair
        For Each curObj As Object In pCollection
            If (TypeOf curObj Is ObjectLabelPair) Then
                Dim val As TPayloadType = GetPayload(Of TPayloadType)(curObj)
                If (val.Equals(pPayloadVal)) Then
                    Return DirectCast(curObj, ObjectLabelPair)
                End If
            End If
        Next
        Return Nothing
    End Function

End Class

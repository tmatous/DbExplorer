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



Public Class MyDataGridView
    Inherits DataGridView


    Protected Overrides Function ProcessDataGridViewKey(e As KeyEventArgs) As Boolean
        If (Me.IsCurrentCellInEditMode) Then
            'supress these keys, they cause the dgv to exit edit mode
            If (e.KeyCode = Keys.End) Then
                Return False
            ElseIf (e.KeyCode = Keys.Home) Then
                Return False
            ElseIf (e.KeyCode = Keys.Right) Then
                Return False
            ElseIf (e.KeyCode = Keys.Left) Then
                Return False
            End If
        End If
        Return MyBase.ProcessDataGridViewKey(e)
    End Function

End Class

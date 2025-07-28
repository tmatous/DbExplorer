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



Public Class Settings

    Public Property MasterPassword As String
    Public Property Connections As New List(Of Connection)

    Public Class Connection
        Public Property Name As String
        Public Property Category As String
        Public Property ConnString As String
        Public Property DbType As Rational.DB.eDbType
        Public Property Provider As String
        Public Property Password As String
        Public Property SavePassword As Boolean
    End Class

End Class

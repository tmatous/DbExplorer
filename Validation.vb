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




Public Class Validator
    Private Sub New()

    End Sub

    Public Shared Function ValidateString(ByVal pValue As String, ByVal pMinLength As Int32?, ByVal pMaxLength As Int32?, Optional ByVal pCharset As String = Nothing, Optional ByVal pRegEx As String = Nothing) As SuccessAndDescription
        If (String.IsNullOrEmpty(pValue)) Then
            pValue = ""
        End If
        If (pMinLength.HasValue AndAlso (pValue.Length < pMinLength.Value)) Then
            Return New SuccessAndDescription(False, "Shorter than minimum (" & pMinLength.Value & ")")
        End If
        If (pMaxLength.HasValue AndAlso (pValue.Length > pMaxLength.Value)) Then
            Return New SuccessAndDescription(False, "Longer than maximum (" & pMaxLength.Value & ")")
        End If
        If (Not String.IsNullOrEmpty(pCharset)) Then
            If (Not Util.StringContainsOnly(pValue, pCharset)) Then
                Return New SuccessAndDescription(False, "Contains invalid characters")
            End If
        End If
        If (Not String.IsNullOrEmpty(pRegEx)) Then
            If (Not Util.RegexMatchExact(pValue, pRegEx)) Then
                Return New SuccessAndDescription(False, "Does not match pattern")
            End If
        End If

        Return New SuccessAndDescription(True, "")
    End Function

    Public Shared Function ValidateNumber(ByVal pValue As String, ByVal pMin As Double?, ByVal pMax As Double?, Optional ByVal pFloatingPoint As Boolean = False, Optional ByVal pDecimalDigits As Int32? = Nothing) As SuccessAndDescription
        If (String.IsNullOrEmpty(pValue)) Then
            pValue = ""
        End If
        Dim numVal As Double
        If (Not Double.TryParse(pValue, numVal)) Then
            Return New SuccessAndDescription(False, "Must be a number")
        End If

        If ((Not pFloatingPoint) AndAlso (Math.Floor(numVal) <> numVal)) Then
            Return New SuccessAndDescription(False, "Must be an integer")
        ElseIf (pFloatingPoint AndAlso pDecimalDigits.HasValue) Then
            Dim numValStr As String = numVal.ToString()
            If ((numValStr.Length - 1 - numValStr.IndexOf(".")) > pDecimalDigits.Value) Then
                Return New SuccessAndDescription(False, "Too many digits after the decimal point")
            End If
        End If

        If (pMin.HasValue AndAlso (numVal < pMin.Value)) Then
            Return New SuccessAndDescription(False, "Smaller than minimum (" & pMin.Value & ")")
        End If
        If (pMax.HasValue AndAlso (numVal > pMax.Value)) Then
            Return New SuccessAndDescription(False, "Greater than maximum (" & pMax.Value & ")")
        End If

        Return New SuccessAndDescription(True, "")
    End Function

    Public Shared Function ValidateDate(ByVal pValue As String, ByVal pMin As DateTime?, ByVal pMax As DateTime?, Optional ByVal pDateOnly As Boolean = False) As SuccessAndDescription
        If (String.IsNullOrEmpty(pValue)) Then
            pValue = ""
        End If
        Dim dtVal As DateTime
        If (Not DateTime.TryParse(pValue, dtVal)) Then
            Return New SuccessAndDescription(False, "Must be a date")
        End If

        If (pDateOnly AndAlso (dtVal.CompareTo(New DateTime(dtVal.Year, dtVal.Month, dtVal.Day)) <> 0)) Then
            Return New SuccessAndDescription(False, "Must not contain a time")
        End If

        If (pMin.HasValue AndAlso (dtVal < pMin.Value)) Then
            Return New SuccessAndDescription(False, "Earlier than minimum (" & pMin.Value & ")")
        End If
        If (pMax.HasValue AndAlso (dtVal > pMax.Value)) Then
            Return New SuccessAndDescription(False, "Later than maximum (" & pMax.Value & ")")
        End If

        Return New SuccessAndDescription(True, "")
    End Function

    'TBD:  Should we use RegEx?
    Public Shared Function ValidateEMail(pMinLength As Int32?, pMaxLength As Int32?, pValue As String) As SuccessAndDescription
        Try
            Dim resp = Validator.ValidateString(pValue, pMinLength, pMaxLength)
            If Not resp.Success Then Return resp

            Dim tmp = New System.Net.Mail.MailAddress(pValue)
            If String.IsNullOrWhiteSpace(tmp.Address) Then Return New SuccessAndDescription(False, "Address os not valid")

        Catch ex As Exception
            Return New SuccessAndDescription(False, ex.Message)
        End Try
        Return New SuccessAndDescription(True, "")
    End Function
End Class

Public Interface IValidator
    Function Validate(ByVal pValue As String) As SuccessAndDescription
End Interface

Public Class StringValidator
    Implements IValidator

    Public Property MinLength As Int32?
    Public Property MaxLength As Int32?
    Public Property Charset As String
    Public Property RegEx As String


    Sub New()
    End Sub


    Sub New(ByVal pMinLength As Int32?, ByVal pMaxLength As Int32?, Optional ByVal pCharset As String = Nothing, Optional ByVal pRegEx As String = Nothing)
        Me.MinLength = pMinLength
        Me.MaxLength = pMaxLength
        Me.Charset = pCharset
        Me.RegEx = pRegEx
    End Sub

    Public Function Validate(ByVal pValue As String) As SuccessAndDescription Implements IValidator.Validate
        Return Validator.ValidateString(pValue, Me.MinLength, Me.MaxLength, Me.Charset, Me.RegEx)
    End Function
End Class

Public Class NumberValidator
    Implements IValidator

    Public Property Min As Double?
    Public Property Max As Double?
    Public Property FloatingPoint As Boolean
    Public Property DecimalDigits As Int32?

    Sub New()
    End Sub

    Sub New(ByVal pMin As Int32, ByVal pMax As Int32)
        Me.Min = pMin
        Me.Max = pMax
        Me.FloatingPoint = False
    End Sub

    Sub New(ByVal pMin As Int32?, ByVal pMax As Int32?)
        Me.Min = pMin
        Me.Max = pMax
        Me.FloatingPoint = False
    End Sub

    Sub New(ByVal pMin As Double, ByVal pMax As Double)
        Me.Min = pMin
        Me.Max = pMax
    End Sub

    Sub New(ByVal pMin As Double?, ByVal pMax As Double?, Optional ByVal pFloatingPoint As Boolean = True, Optional ByVal pDecimalDigits As Int32? = Nothing)
        Me.Min = pMin
        Me.Max = pMax
        Me.FloatingPoint = pFloatingPoint
        Me.DecimalDigits = pDecimalDigits
    End Sub

    Public Function Validate(ByVal pValue As String) As SuccessAndDescription Implements IValidator.Validate
        Return Validator.ValidateNumber(pValue, Me.Min, Me.Max, Me.FloatingPoint, Me.DecimalDigits)
    End Function
End Class

Public Class DateValidator
    Implements IValidator

    Public Property Min As DateTime?
    Public Property Max As DateTime?
    Public Property DateOnly As Boolean

    Sub New()
    End Sub

    Sub New(ByVal pMin As DateTime?, ByVal pMax As DateTime?, Optional ByVal pDateOnly As Boolean = False)
        Me.Min = pMin
        Me.Max = pMax
        Me.DateOnly = pDateOnly
    End Sub

    Public Function Validate(ByVal pValue As String) As SuccessAndDescription Implements IValidator.Validate
        Return Validator.ValidateDate(pValue, Me.Min, Me.Max, Me.DateOnly)
    End Function
End Class

Public Class EmailValidator
    Implements IValidator

    Public Property MinLength As Int32?
    Public Property MaxLength As Int32?

    Sub New()
    End Sub

    Sub New(ByVal pMinLength As Int32?, ByVal pMaxLength As Int32?)
        Me.MinLength = pMinLength
        Me.MaxLength = pMaxLength
    End Sub

    Public Function Validate(pValue As String) As SuccessAndDescription Implements IValidator.Validate
        Return Validator.ValidateEMail(Me.MinLength, Me.MaxLength, pValue)
    End Function
End Class
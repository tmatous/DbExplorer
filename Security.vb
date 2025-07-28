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



Imports System.Security.Cryptography

Public Class Security

    Public Shared Function HashMd5(ByVal pToHash As Byte()) As Byte()
        Using crypto As New MD5CryptoServiceProvider()
            Return crypto.ComputeHash(pToHash)
        End Using
    End Function

    ''' <summary>
    ''' Hash a string using MD5. Returns string in Base64
    ''' </summary>
    Public Shared Function HashMd5ToBase64(ByVal pToHash As String) As String
        Return Convert.ToBase64String(HashMd5(StringToByteArray(pToHash)))
    End Function


    ''' <summary>
    ''' Convert a string object to a byte array using UTF8 encoding
    ''' </summary>
    Public Shared Function StringToByteArray(pStr As String) As Byte()
        Dim encoding As New System.Text.UTF8Encoding()
        Return encoding.GetBytes(pStr)
    End Function


    ''' <summary>
    ''' Generates an encryption key of specified length from a user password
    ''' </summary>
    Public Shared Function PasswordToEncryptionKey(ByVal pPassword As String, ByVal pBitSize As Int32) As String
        Dim baseKey As String = HashMd5ToBase64(pPassword)
        Dim byteReq As Int32 = CInt(pBitSize / 8)
        If (baseKey.Length < byteReq) Then
            Return baseKey.PadRight(byteReq, "x"c)
        ElseIf (baseKey.Length > byteReq) Then
            Return baseKey.Substring(0, byteReq)
        Else
            Return baseKey
        End If
    End Function


    ''' <summary>
    ''' Encrypt string with AES, 256-bit key, ECB mode
    ''' </summary>
    ''' <param name="pToEncrypt">data to encrypt</param>
    ''' <param name="pKey">key to encrypt with. must be 256-bit (32 byte)</param>
    ''' <returns>byte array</returns>
    ''' <remarks></remarks>
    Public Shared Function EncryptAes(ByVal pToEncrypt As Byte(), ByRef pKey As String) As Byte()
        Dim crypto As New AesCryptoServiceProvider()
        crypto.KeySize = 256  '128 - 256, could be a parameter
        crypto.Mode = CipherMode.ECB  'electronic codebook, good for short or random data
        'crypto.Mode = CipherMode.CBC  'cipher block chaining, better for longer or less random data. requires initialization vector
        crypto.Padding = PaddingMode.PKCS7  'seems to be the default
        crypto.Key = Util.StringToByteArray(pKey)

        Using resMs As New IO.MemoryStream
            Using enc As ICryptoTransform = crypto.CreateEncryptor()
                Using cs As New CryptoStream(resMs, enc, CryptoStreamMode.Write)
                    cs.Write(pToEncrypt, 0, pToEncrypt.Length)
                    cs.FlushFinalBlock()
                    Return resMs.ToArray()
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Encrypt string with AES, 256-bit key, ECB mode
    ''' </summary>
    ''' <param name="pToEncrypt">data to encrypt</param>
    ''' <param name="pKey">key to encrypt with. must be 256-bit (32 byte)</param>
    ''' <returns>Base64 string</returns>
    ''' <remarks></remarks>
    Public Shared Function EncryptAesToBase64(ByVal pToEncrypt As Byte(), ByRef pKey As String) As String
        Return Convert.ToBase64String(EncryptAes(pToEncrypt, pKey))
    End Function

    ''' <summary>
    ''' Encrypt string with AES, 256-bit key, ECB mode
    ''' </summary>
    ''' <param name="pToEncrypt">data to encrypt</param>
    ''' <param name="pKey">key to encrypt with. must be 256-bit (32 byte)</param>
    ''' <returns>Base64 string</returns>
    ''' <remarks></remarks>
    Public Shared Function EncryptAesToBase64(ByVal pToEncrypt As String, ByVal pKey As String) As String
        Return Convert.ToBase64String(EncryptAes(Util.StringToByteArray(pToEncrypt), pKey))
    End Function


    ''' <summary>
    ''' Decrypt a byte array with AES, 256-bit key, ECB mode.  Returns a byte array
    ''' </summary>
    ''' <param name="pEncrypted"></param>
    ''' <param name="pKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DecryptAes(ByVal pEncrypted As Byte(), ByVal pKey As String) As Byte()
        Dim BUFFER_SIZE As Int32 = 2048
        Dim crypto As New AesCryptoServiceProvider()
        crypto.KeySize = 256  '128 - 256, could be a parameter
        crypto.Mode = CipherMode.ECB 'electronic codebook, good for short or random data
        'crypto.Mode = CipherMode.CBC 'cipher block chaining, better for longer or less random data. requires initialization vector
        crypto.Padding = PaddingMode.PKCS7  'seems to be the default
        crypto.Key = Util.StringToByteArray(pKey)

        Using encMs As New IO.MemoryStream(pEncrypted, 0, pEncrypted.Length)
            Using resMs As New IO.MemoryStream
                Using dec As ICryptoTransform = crypto.CreateDecryptor()
                    Using cs As New CryptoStream(encMs, dec, CryptoStreamMode.Read)
                        Dim buf(BUFFER_SIZE) As Byte
                        Dim bytesRead As Integer
                        bytesRead = cs.Read(buf, 0, BUFFER_SIZE)
                        Do While bytesRead > 0
                            resMs.Write(buf, 0, bytesRead)
                            bytesRead = cs.Read(buf, 0, BUFFER_SIZE)
                        Loop
                        Return resMs.ToArray()
                    End Using
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Decrypt a Base64 string with AES, 256-bit key, ECB mode.  Returns a string
    ''' </summary>
    Public Shared Function DecryptAesBase64ToString(ByVal pEncryptedBase64 As String, ByVal pKey As String) As String
        Return Util.ByteArrayToString(DecryptAes(Convert.FromBase64String(pEncryptedBase64), pKey))
    End Function

End Class

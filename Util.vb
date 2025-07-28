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



Public Class Util

    Public Const CHARSET_ALPHANUMERIC As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
    ''' <summary>This charset is good for generating random strings that can't be words</summary>
    Public Const CHARSET_UPPERALPHANOVOWELS As String = "BCDFGHJKLMNPQRSTVWXYZ"

    Public Shared Sub ShowError(ex As Exception)
        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK)
    End Sub

    Public Shared Sub ShowError(msg As String)
        MessageBox.Show(msg, "Error", MessageBoxButtons.OK)
    End Sub


    Private Shared _GetLocationRelativeTo_StaggerCount As Int32 = 0
    Private Const _GetLocationRelativeTo_StaggerMax = 5
    Private Const _GetLocationRelativeTo_StaggerOffset = 20
    Public Shared Function GetLocationRelativeTo(pReferenceBounds As Rectangle, pTargetSize As Size, Optional pStagger As Boolean = False) As Point
        Dim scr = Screen.FromRectangle(pReferenceBounds)
        Dim posX = Convert.ToInt32((scr.WorkingArea.Width / 2) - (pTargetSize.Width / 2) + scr.WorkingArea.X)
        Dim posY = Convert.ToInt32((scr.WorkingArea.Height / 2.2) - (pTargetSize.Height / 2) + scr.WorkingArea.Y)
        If (pStagger) Then
            Dim backOffset = Convert.ToInt32((_GetLocationRelativeTo_StaggerMax / 2) * _GetLocationRelativeTo_StaggerOffset)
            posX -= backOffset
            posY -= backOffset
            Dim fwdOffset = Convert.ToInt32(_GetLocationRelativeTo_StaggerCount * _GetLocationRelativeTo_StaggerOffset)
            posX += fwdOffset
            posY += fwdOffset
            _GetLocationRelativeTo_StaggerCount += 1
            If (_GetLocationRelativeTo_StaggerCount >= _GetLocationRelativeTo_StaggerMax) Then _GetLocationRelativeTo_StaggerCount = 0
        End If

        Return New Point(posX, posY)
    End Function

    Public Shared Sub ShowFormRelativeTo(pForm As Form, pBounds As Rectangle)
        pForm.StartPosition = FormStartPosition.Manual
        pForm.Location = Util.GetLocationRelativeTo(pBounds, pForm.Size, pStagger:=True)
        pForm.Show()
    End Sub

    Public Shared Function ByteArrayToHexString(pData As Byte()) As String
        Return String.Concat("0x", BitConverter.ToString(pData).Replace("-", ""))
    End Function

    Public Shared Function HexStringToByteArray(pData As String) As Byte()
        pData = pData.Trim()
        If (Not pData.StartsWith("0x")) Then Throw New ArgumentException("Invalid hex string")
        pData = pData.Remove(0, 2)
        If ((pData.Length Mod 2) <> 0) Then Throw New ArgumentException("Invalid hex string")
        Dim charCount = pData.Length

        Dim bytes((charCount \ 2) - 1) As Byte
        For i = 0 To bytes.Length - 1
            bytes(i) = Convert.ToByte(pData.Substring(i * 2, 2), 16)
        Next

        Return bytes
    End Function

    Public Shared Function GetProviders() As IList(Of String)
        Dim res As New List(Of String)
        Try
            Dim provs = Common.DbProviderFactories.GetFactoryClasses()
            For Each row As DataRow In provs.Rows
                res.Add(row.Item("InvariantName").ToString)
            Next
        Catch ex As Exception
            ShowError(String.Format("Error loading database providers: {0}", ex.Message))
        End Try
        Return res
    End Function

    Public Shared Function SerializeObjectXmlContract(pObj As Object) As String
        If (pObj Is Nothing) Then Return ""
        Dim ser As New Runtime.Serialization.DataContractSerializer(pObj.GetType())

        Using ms As New IO.MemoryStream()
            ser.WriteObject(ms, pObj)
            Using rdr As New IO.StreamReader(ms, System.Text.Encoding.UTF8)
                ms.Position = 0
                Return rdr.ReadToEnd()
            End Using
        End Using
    End Function


    Public Shared Function DeserializeObjectXmlContract(pXmlStr As String, pType As System.Type) As Object
        If (String.IsNullOrEmpty(pXmlStr)) Then Return Nothing
        Dim ser As New Runtime.Serialization.DataContractSerializer(pType)
        Using sr As New IO.StringReader(pXmlStr)
            Using xr = Xml.XmlReader.Create(sr)
                Return ser.ReadObject(xr)
            End Using
        End Using
    End Function

    Public Shared Function DeserializeObjectXmlContract(Of T)(pXmlStr As String) As T
        Return DirectCast(DeserializeObjectXmlContract(pXmlStr, GetType(T)), T)
    End Function

    Public Shared Sub SetupConnectionPassword(pConn As Settings.Connection)
        If (pConn.ConnString.Contains("{password}")) Then
            Using frm As New frmLogin
                frm.ShowUsername = False
                frm.ShowPasswordRetype = False
                frm.Caption = String.Format("Enter password for connection '{0}'", pConn.Name)
                frm.SetSavePassword = True
                frm.AllowBlankPassword = True
                If (frm.ShowDialog() <> DialogResult.OK) Then Return
                pConn.Password = frm.Password
                pConn.SavePassword = frm.SetSavePassword
            End Using
        Else
            pConn.Password = ""
            pConn.SavePassword = False
            If (ConnStringMayContainInlinePassword(pConn.ConnString)) Then
                Dim message = "It appears that this connection string may contain an inline password. The Master Password will only encrypt passwords using the {password} keyword. Inline passwords will not be encrypted."
                MessageBox.Show(message, "Warning", MessageBoxButtons.OK)
            End If
        End If
    End Sub

    Public Shared Function ConnStringMayContainInlinePassword(pConnString As String) As Boolean
        Dim csUpper = pConnString.ToUpper()
        If (System.Text.RegularExpressions.Regex.IsMatch(csUpper, "PASSWORD\s*=")) Then Return True
        If (System.Text.RegularExpressions.Regex.IsMatch(csUpper, "PWD\s*=")) Then Return True
        Return False
    End Function

    Public Shared Function CreateConnInfo(pConn As Settings.Connection) As Rational.DB.DbConnectionInfo
        Dim cs = pConn.ConnString
        If (cs.Contains("{password}")) Then
            Dim pw = ""
            If (pConn.SavePassword AndAlso (Not String.IsNullOrEmpty(pConn.Password))) Then
                pw = pConn.Password
            Else
                Using frm As New frmLogin
                    frm.ShowUsername = False
                    frm.ShowPasswordRetype = False
                    frm.ShowSavePassword = False
                    frm.Caption = String.Format("Enter password for connection '{0}'", pConn.Name)
                    If (frm.ShowDialog() <> DialogResult.OK) Then Return Nothing
                    pw = frm.Password
                End Using
            End If
            cs = cs.Replace("{password}", pw)
        End If

        If (Not String.IsNullOrWhiteSpace(pConn.Provider)) Then
            Return New Rational.DB.DbConnectionInfo(cs, pConn.DbType, pConn.Provider)
        Else
            Return New Rational.DB.DbConnectionInfo(cs, pConn.DbType)
        End If
    End Function

    Private Const ENCRYPTION_SALT1 As String = "{03CDAF84-DD21-4429-8802-CE61DAF85B09}"
    Private Const ENCRYPTION_SALT2 As String = "{8CCA7257-B6E0-49AC-8C1A-856C672DFAC8}"

    Public Shared Function Encrypt(pToEncrypt As String, pPassword As String) As String
        Dim key = Security.PasswordToEncryptionKey(pPassword, 256)
        Dim payload = String.Format("{0}{1}{2}", ENCRYPTION_SALT1, pToEncrypt, ENCRYPTION_SALT2)
        Return Security.EncryptAesToBase64(payload, key)
    End Function

    Public Shared Function Decrypt(pToDecrypt As String, pPassword As String) As String
        Try
            Dim key = Security.PasswordToEncryptionKey(pPassword, 256)
            Dim payload = Security.DecryptAesBase64ToString(pToDecrypt, key)
            Dim decrypted = payload.Substring(ENCRYPTION_SALT1.Length, payload.Length - ENCRYPTION_SALT1.Length - ENCRYPTION_SALT2.Length)
            Return decrypted
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Class SelectHelper

        Private _cmd As IDbCommand
        Private _requestCancel As Boolean

        Public Sub SelectIntoAction(pConnInfo As Rational.DB.DbConnectionInfo, pSql As String, pAction As Action(Of IDataReader))
            If (_cmd IsNot Nothing) Then Throw New Exception("Already in use")
            Dim fact = pConnInfo.GetFactory()
            Using conn = fact.CreateConnection()
                conn.ConnectionString = pConnInfo.ConnectionString
                conn.Open()
                Try
                    _cmd = conn.CreateCommand()
                    _cmd.CommandType = CommandType.Text
                    _cmd.CommandText = pSql
                    _cmd.CommandTimeout = GetMaxTimeout(pConnInfo)
                    Dim rdr = _cmd.ExecuteReader()
                    pAction.Invoke(rdr)
                Finally
                    _cmd.Dispose()
                    _cmd = Nothing
                    _requestCancel = False
                End Try
            End Using
        End Sub

        ''' <summary>
        ''' Designed to be called from another thread to cancel the currently running query
        ''' </summary>
        Public Sub Cancel()
            _requestCancel = True
            _cmd.Cancel()
        End Sub

    End Class

    Public Shared Function GetDefaultTimeout(db As Rational.DB.DbConnectionInfo) As Int32
        If (db.DbType = Rational.DB.eDbType.SqlServerCe) Then
            'SqlServerCe doesn't support timeout
            Return 0
        Else
            Return 300
        End If
    End Function

    Public Shared Function GetMaxTimeout(db As Rational.DB.DbConnectionInfo) As Int32
        If (db.DbType = Rational.DB.eDbType.SqlServerCe) Then
            'SqlServerCe doesn't support timeout
            Return 0
        Else
            Return Int32.MaxValue
        End If
    End Function

    Public Shared Function DataReaderToCsvFile(data As IDataReader, includeHeader As Boolean, fileName As String, Optional delimiter As String = ",") As Int64
        Dim aLine As New System.Text.StringBuilder()
        Dim rowCount As Int64 = 0
        Dim colCount = data.FieldCount
        Dim headers As New List(Of String)()
        For curCol = 0 To colCount - 1
            headers.Add(data.GetName(curCol))
        Next
        headers = CleanupFieldList(headers)

        Using fil As New IO.StreamWriter(fileName)
            If (includeHeader) Then
                For curCol = 0 To colCount - 1
                    If (curCol > 0) Then aLine.Append(delimiter)
                    aLine.Append(headers(curCol))
                Next
                fil.WriteLine(aLine.ToString())
                aLine.Clear()
            End If

            Dim values(colCount) As Object
            While data.Read()
                data.GetValues(values)
                For curCol = 0 To colCount - 1
                    If (curCol > 0) Then aLine.Append(delimiter)
                    AppendCsvFieldData(aLine, values(curCol))
                Next
                fil.WriteLine(aLine.ToString())
                aLine.Clear()
                rowCount += 1
            End While

            fil.Close()
        End Using

        Return rowCount
    End Function

    Public Shared Function DataReaderToExcelFile(data As IDataReader, includeHeader As Boolean, fileName As String, sheetName As String) As Int64
        Dim rowCount = 0
        Using pck As New OfficeOpenXml.ExcelPackage(New IO.FileInfo(fileName))
            Dim ws As OfficeOpenXml.ExcelWorksheet = pck.Workbook.Worksheets.Add(sheetName)
            Dim colCount = data.FieldCount
            Dim headers As New List(Of String)()
            For curCol = 0 To colCount - 1
                headers.Add(data.GetName(curCol))
            Next
            headers = CleanupFieldList(headers)

            Dim curRow = 1
            If (includeHeader) Then
                ws.Cells.Item(curRow, 1, curRow, colCount).Style.Font.Bold = True
                For curCol = 1 To colCount
                    ws.Cells.Item(curRow, curCol).Value = headers(curCol - 1)
                Next
                curRow += 1
            End If

            Dim values(colCount) As Object
            While data.Read()
                data.GetValues(values)
                For curCol = 1 To colCount
                    Dim dat = values(curCol - 1)
                    If (dat IsNot Nothing) Then dat = dat.ToString()
                    ws.Cells.Item(curRow, curCol).Value = dat
                Next
                curRow += 1
                rowCount += 1
            End While

            ws.Cells.Item(1, 1, curRow, colCount).AutoFitColumns(ws.DefaultColWidth, 50)
            pck.Save()
        End Using

        Return rowCount
    End Function

    Public Shared Sub DataReaderToCodeGen(data As IDataReader, templateFile As String, outputFile As String)
        Dim templateSource = IO.File.ReadAllText(templateFile)

        Using fil As New IO.StreamWriter(outputFile)
            DbCodeGen.CodeGenerator.RenderTemplateForDataReader(templateSource, data, fil)
            fil.Close()
        End Using
    End Sub

    Public Shared Sub DataReaderToCodeGenByRow(data As IDataReader, templateFile As String, outputPath As String, filenameColumn As String)
        Dim templateSource = IO.File.ReadAllText(templateFile)
        Dim generatedFiles = New List(Of String)

        Dim rowNum = 0
        While data.Read()
            rowNum += 1
            Dim filename = ""
            If (Not String.IsNullOrEmpty(filenameColumn)) Then
                filename = CleanFileName(data.GetString(data.GetOrdinal(filenameColumn)))
            End If
            If (String.IsNullOrWhiteSpace(filename)) Then filename = String.Format("result_{0}.out", rowNum)
            If (Not IO.Path.IsPathRooted(filename)) Then filename = IO.Path.Combine(outputPath, filename)
            If (generatedFiles.Contains(filename, StringComparer.OrdinalIgnoreCase)) Then Throw New Exception(String.Format("Duplicate filename: {0}", filename))
            If (IO.File.Exists(filename)) Then Throw New Exception(String.Format("File exists: {0}", filename))

            Using fil As New IO.StreamWriter(filename)
                DbCodeGen.CodeGenerator.RenderTemplateForDataRecord(templateSource, data, fil)
                fil.Close()
            End Using
            generatedFiles.Add(filename)

        End While
    End Sub

    Public Shared Function CleanupFieldList(pList As IList(Of String)) As List(Of String)
        Dim newList = New List(Of String)()
        Dim curIdx = 1
        For Each curName In pList
            If (String.IsNullOrWhiteSpace(curName)) Then curName = String.Format("Column{0}", curIdx)
            Dim dupIdx = 1
            Dim origName = curName
            While (newList.Contains(curName, StringComparer.OrdinalIgnoreCase))
                dupIdx += 1
                curName = String.Format("{0}_{1}", origName, dupIdx)
            End While
            newList.Add(curName)

            curIdx += 1
        Next
        Return newList
    End Function

    Public Shared Sub AppendCsvFieldData(sb As System.Text.StringBuilder, fieldData As Object)
        If (TypeOf fieldData Is System.DBNull) Then
            'don't add anything
        ElseIf (fieldData Is Nothing) Then
            'don't add anything
        Else
            sb.Append("""")

            If (TypeOf fieldData Is Byte()) Then
                sb.Append("0x")
                sb.Append(ByteArrayToHexString(DirectCast(fieldData, Byte())))
            Else
                sb.Append(fieldData.ToString().Replace("""", """"""))
            End If

            sb.Append("""")
        End If
    End Sub

    Private Shared _numericTypes As New HashSet(Of Type)(
        {
        GetType(SByte), GetType(Int16), GetType(Int32), GetType(Int64),
        GetType(Byte), GetType(UInt16), GetType(UInt32), GetType(UInt64),
        GetType(Decimal),
        GetType(Single), GetType(Double)
        })

    Public Shared Sub AppendSqlListFieldData(sb As System.Text.StringBuilder, fieldData As Object, fieldType As Type)
        If (TypeOf fieldData Is System.DBNull) Then
            sb.Append("NULL")
        ElseIf (fieldData Is Nothing) Then
            sb.Append("NULL")
        Else
            If (fieldType Is Nothing) Then fieldType = GetType(String)
            If (_numericTypes.Contains(fieldType)) Then
                sb.Append(fieldData.ToString())
            Else
                sb.Append("'")

                If (TypeOf fieldData Is Byte()) Then
                    sb.Append("0x")
                    sb.Append(ByteArrayToHexString(DirectCast(fieldData, Byte())))
                Else
                    sb.Append(fieldData.ToString().Replace("'", "''"))
                End If

                sb.Append("'")
            End If
        End If
    End Sub


    Public Shared Function SplitSqlBatches(pSql As String) As List(Of String)
        Dim results = New List(Of String)()
        If (String.IsNullOrWhiteSpace(pSql)) Then Return results

        Using sr As New System.IO.StringReader(pSql)
            Dim curBatch = New System.Text.StringBuilder()
            Dim curLine = sr.ReadLine()
            While (curLine IsNot Nothing)
                If (curLine.Trim().Equals("GO", StringComparison.OrdinalIgnoreCase)) Then
                    Dim curBatchStr = curBatch.ToString()
                    If (curBatchStr.Trim().Length > 0) Then results.Add(curBatchStr)
                    curBatch.Clear()
                Else
                    curBatch.AppendLine(curLine)
                End If
                curLine = sr.ReadLine()
            End While
            'add last batch
            Dim lastBatchStr = curBatch.ToString()
            If (lastBatchStr.Trim().Length > 0) Then results.Add(lastBatchStr)
        End Using

        Return results
    End Function

    Private Shared _currentCodeGenFolder As String = Nothing
    Public Shared Function GetCurrentCodeGenFolder() As String
        If (_currentCodeGenFolder Is Nothing) Then _currentCodeGenFolder = GetDefaultCodeGenFolder()
        Return _currentCodeGenFolder
    End Function

    Public Shared Function GetDefaultCodeGenFolder() As String
        Dim cgf = IO.Path.Combine(Application.StartupPath, "CodeGen")
        If (Not IO.Directory.Exists(cgf)) Then cgf = Application.StartupPath
        Return cgf
    End Function

    Public Shared Sub SetCurrentCodeGenFolder(pPath As String)
        If (IO.Directory.Exists(pPath)) Then _currentCodeGenFolder = pPath
    End Sub

    Private Shared _currentOutputFolder As String = Nothing
    Public Shared Function GetCurrentOutputFolder() As String
        If (_currentOutputFolder Is Nothing) Then _currentOutputFolder = Application.StartupPath
        Return _currentOutputFolder
    End Function

    Public Shared Sub SetCurrentOutputFolder(pPath As String)
        If (IO.Directory.Exists(pPath)) Then _currentOutputFolder = pPath
    End Sub

    Public Shared Function CleanFileName(fileName As String) As String
        If (String.IsNullOrWhiteSpace(fileName)) Then Return ""
        Return System.IO.Path.GetInvalidFileNameChars().Aggregate(fileName,
                                                                  Function(current, c)
                                                                      Return current.Replace(c.ToString(), String.Empty)
                                                                  End Function)
    End Function

    Private Shared _tempFiles As New List(Of String)
    Public Shared Function GetTempFilename(Optional pExtension As String = "") As String
        If (String.IsNullOrWhiteSpace(pExtension)) Then pExtension = "tmp"
        Dim fname = ""
        Do
            fname = IO.Path.Combine(IO.Path.GetTempPath(), String.Format("{0}.{1}", Util.RandomString(10, CHARSET_UPPERALPHANOVOWELS), pExtension))
        Loop Until Not IO.File.Exists(fname)
        _tempFiles.Add(fname)

        Return fname
    End Function


    Public Shared Sub CleanupTempFiles()
        For Each fil In _tempFiles
            Try
                If (IO.File.Exists(fil)) Then IO.File.Delete(fil)
            Catch ex As Exception
                'ignore
            End Try
        Next
    End Sub

    Public Shared Function PromptDelimiter() As String
        Dim delim = "Comma (,)"
        Dim choices = {"Comma (,)", "Pipe (|)", "Tab"}
        If (PromptForm.PromptForStringFromList(delim, choices, "Select the field delimiter.") <> Windows.Forms.DialogResult.OK) Then Return Nothing
        Select Case delim
            Case "Comma (,)" : Return ","
            Case "Pipe (|)" : Return "|"
            Case "Tab" : Return ControlChars.Tab
            Case Else
                Throw New Exception("Invalid delimiter")
        End Select
    End Function

    Public Shared Function PromptRazorTemplate() As String
        Dim fn = ""
        Using frm = New Windows.Forms.OpenFileDialog()
            frm.Title = "Select a template file"
            frm.Filter = "Razor template (*.csrzr)|*.csrzr|All Files|*.*"
            frm.RestoreDirectory = True
            frm.InitialDirectory = Util.GetCurrentCodeGenFolder()
            If (frm.ShowDialog() <> Windows.Forms.DialogResult.OK) Then Return Nothing
            fn = frm.FileName
            Util.SetCurrentCodeGenFolder(IO.Path.GetDirectoryName(frm.FileName))
        End Using
        Return fn
    End Function

    Public Shared Function PromptSaveFile(pFileType As String, Optional pFileTypeDesc As String = Nothing) As String
        If (String.IsNullOrEmpty(pFileTypeDesc)) Then pFileTypeDesc = String.Format("{0} Files", pFileType.ToUpper)
        pFileType = pFileType.ToLower()
        Dim outputFilename = ""
        Using frm As New SaveFileDialog()
            frm.Filter = String.Format("{0}|*.{1}|All Files|*.*", pFileTypeDesc, pFileType)
            frm.RestoreDirectory = True
            frm.InitialDirectory = Util.GetCurrentOutputFolder()
            If (frm.ShowDialog() <> DialogResult.OK) Then Return Nothing
            outputFilename = frm.FileName
            Util.SetCurrentOutputFolder(IO.Path.GetDirectoryName(frm.FileName))
        End Using
        Return outputFilename
    End Function

    Public Shared Sub CopyToClipboard(pText As String)
        Dim success = False
        Try
            Clipboard.Clear()
        Catch ex As Exception
            'ignore
        End Try
        Try
            Clipboard.SetDataObject(pText, False, 5, 250)
            success = True
        Catch ex As Exception
            'ignore
        End Try
        If (Not success) Then
            Util.ShowError("Error copying to the clipboard")
        End If
    End Sub

    Public Shared Sub CopyToClipboard(pData As DataObject)
        Dim success = False
        Try
            Clipboard.Clear()
        Catch ex As Exception
            'ignore
        End Try
        Try
            Clipboard.SetDataObject(pData, False, 5, 250)
            success = True
        Catch ex As Exception
            'ignore
        End Try
        If (Not success) Then
            Util.ShowError("Error copying to the clipboard")
        End If
    End Sub

    ''' <summary>
    ''' Convert a string object to a byte array using UTF8 encoding
    ''' </summary>
    Public Shared Function StringToByteArray(pStr As String) As Byte()
        Dim encoding As New System.Text.UTF8Encoding()
        Return encoding.GetBytes(pStr)
    End Function

    ''' <summary>
    ''' Convert a byte array to a string object assuming UTF8 encoding
    ''' </summary>
    Public Shared Function ByteArrayToString(pArr As Byte()) As String
        Dim enc As New System.Text.UTF8Encoding()
        Return enc.GetString(pArr)
    End Function

    Private Shared _randomSeed As New Random()  'single seed value prevents duplicate random chains when done in quick succession

    ''' <summary>
    ''' Generates a random string of the specified length, optionally with a limited character set
    ''' </summary>
    ''' <param name="pLength"></param>
    ''' <param name="pAllowChars">string containing the allowed characters, defaults to CHARSET_ALPHANUMERIC</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function RandomString(pLength As Integer, Optional pAllowChars As String = CHARSET_ALPHANUMERIC) As String
        Dim sb As New System.Text.StringBuilder()
        Dim nextChar As String
        Dim maxInd As Int32 = pAllowChars.Length - 1

        For i As Int32 = 0 To pLength - 1
            nextChar = pAllowChars.Substring(_randomSeed.Next(0, maxInd), 1)
            sb.Append(nextChar)
        Next i

        Return sb.ToString()
    End Function

    ''' <summary>
    ''' Check whether a string contains only characters from the specified list of characters
    ''' </summary>
    ''' <param name="pToCheck">string to check for validity</param>
    ''' <param name="pAllowChars">string of all the valid characters. CHARSET_xxx constants are available</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function StringContainsOnly(pToCheck As String, pAllowChars As String) As Boolean
        If (String.IsNullOrEmpty(pToCheck)) Then Return True
        If (String.IsNullOrEmpty(pAllowChars)) Then Return False
        'is there a faster way to do this?
        For x As Int32 = 0 To pToCheck.Length - 1
            If (Not pAllowChars.Contains(pToCheck.Substring(x, 1))) Then Return False
        Next
        Return True
    End Function

    ''' <summary>
    ''' Check whether the entire string matches the regular expression. REGEX_xxx constants are available
    ''' </summary>
    Public Shared Function RegexMatchExact(pToCheck As String, pPattern As String) As Boolean
        'this forces RegEx to match the entire string rather than a substring
        If (Not pPattern.StartsWith("^")) Then
            pPattern = "^" + pPattern
        End If
        If (Not pPattern.EndsWith("$")) Then
            pPattern += "$"
        End If

        Return System.Text.RegularExpressions.Regex.IsMatch(pToCheck, pPattern)
    End Function

    Public Shared Function EnumToDict(pEnumType As Type) As Dictionary(Of Int32, String)
        Dim res As New Dictionary(Of Int32, String)
        Dim names() As String = [Enum].GetNames(pEnumType)
        Dim vals As Array = [Enum].GetValues(pEnumType)
        For x As Int32 = 0 To names.Length - 1
            res.Add(DirectCast(vals.GetValue(x), Int32), names(x))
        Next

        Return res
    End Function

    Private Class DelayDoObj
        Public Property MyAction As Action
        Public Property Delay As TimeSpan
    End Class

    Public Shared Sub DelayDo(pAction As Action, pDelay As TimeSpan)
        Dim obj As New DelayDoObj
        obj.MyAction = pAction
        obj.Delay = pDelay
        Dim doThrd As New Threading.Thread(New Threading.ParameterizedThreadStart(AddressOf DelayDoImpl))
        doThrd.Priority = Threading.ThreadPriority.BelowNormal
        doThrd.Start(obj)
    End Sub

    Private Shared Sub DelayDoImpl(pDelayDoObj As Object)
        Dim args = DirectCast(pDelayDoObj, DelayDoObj)
        Threading.Thread.Sleep(CInt(args.Delay.TotalMilliseconds))
        args.MyAction.Invoke()
    End Sub

End Class

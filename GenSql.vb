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



Imports DbSchemaTools

Public Class GenSql

    Private Const SQL_INDENT = "    "

    Private _connInfo As Rational.DB.DbConnectionInfo
    Private _db As Rational.DB.Database

    Public Sub New(pConnInfo As Rational.DB.DbConnectionInfo)
        _connInfo = pConnInfo
        _db = New Rational.DB.Database(_connInfo)
    End Sub


    Public Function QuoteIdentifier(pIdentifier As String) As String
        Return String.Format("{0}{1}{2}", _db.QuotePrefix, pIdentifier, _db.QuoteSuffix)
    End Function

    Public Function GetSchemaText(pTable As TableInfo) As String
        Dim res As New System.Text.StringBuilder
        res.AppendLine(pTable.Name)
        For Each col In pTable.Columns
            res.AppendLine(SQL_INDENT + GetSchemaText(col, pIncludeTableName:=False, pIncludeNullSetting:=True))
        Next

        Return res.ToString
    End Function

    Public Function GetSchemaText(pColumn As ColumnInfo, pIncludeTableName As Boolean, pIncludeNullSetting As Boolean) As String
        Dim nameColWidth = 30
        Dim spacing = vbTab
        Dim name As String
        If (pIncludeTableName) Then
            name = String.Format("{0}.{1}", pColumn.Table.Name, pColumn.Name)
            nameColWidth = 60
        Else
            name = pColumn.Name
        End If
        If (name.Length < nameColWidth) Then
            spacing = StrDup(nameColWidth - name.Length, " "c)
        End If
        Dim nullSetting = ""
        If (pIncludeNullSetting) Then
            nullSetting = If(pColumn.Nullable, " NULL", " NOT NULL")
        End If

        Dim res = String.Format("{0}{1}{2}{3}{4}{5}", name, spacing, If(pColumn.InPrimaryKey, "PK ", ""), If(pColumn.InForeignKey, "FK ", ""), pColumn.DdlDataType, nullSetting)

        Return res
    End Function

    Public Function GenerateSelectSql(pTable As TableInfo, pAlias As String, Optional pIncludeWhere As Boolean = True) As String
        Dim schemaPrefix = ""
        If (Not String.IsNullOrWhiteSpace(pTable.SchemaName)) Then schemaPrefix = QuoteIdentifier(pTable.SchemaName) + "."

        Dim aliasDot = ""
        If (String.IsNullOrWhiteSpace(pAlias)) Then
            pAlias = ""
            aliasDot = ""
        Else
            pAlias = pAlias.Trim()
            aliasDot = pAlias + "."
            pAlias = " " + pAlias
        End If

        Dim cond = ""
        Dim fldTemp = ""
        Dim fldsLine = ""
        Dim fldsArr = New List(Of String)
        Dim delim = ""
        For Each col In pTable.Columns
            fldsLine += delim
            delim = ", "
            fldTemp = String.Format("{0}{1}", aliasDot, QuoteIdentifier(col.Name))

            'split to readable lines
            If ((fldsLine.Length > 0) AndAlso ((fldsLine.Length + fldTemp.Length) > 80)) Then
                fldsArr.Add(fldsLine)
                fldsLine = ""
            End If

            fldsLine += fldTemp

            If (col.InPrimaryKey) Then
                If (cond.Length > 0) Then cond += " AND "
                cond += String.Format("{0}{1} = @{2}", aliasDot, QuoteIdentifier(col.Name), col.Name)
            End If
        Next
        fldsArr.Add(fldsLine)

        If (cond = "") Then cond = "<CONDITION>"

        Dim res As New System.Text.StringBuilder
        res.AppendLine("SELECT")
        res.Append(SQL_INDENT)
        res.AppendLine(Join(fldsArr.ToArray, vbCrLf + SQL_INDENT))
        res.AppendLine("FROM")
        res.AppendLine(String.Format("{0}{1}{2}{3}", SQL_INDENT, schemaPrefix, QuoteIdentifier(pTable.Name), pAlias))
        If (pIncludeWhere) Then
            res.AppendLine("WHERE")
            res.AppendLine(String.Format("{0}{1}", SQL_INDENT, cond))
        End If

        Return res.ToString
    End Function

    Public Function GenerateInsertWithParams(pTable As TableInfo, pIncludeIdentity As Boolean) As String
        If (Not TypeOf pTable Is TableInfo) Then Return "Not supported for this table type"

        Dim schemaPrefix = ""
        If (Not String.IsNullOrWhiteSpace(pTable.SchemaName)) Then schemaPrefix = QuoteIdentifier(pTable.SchemaName) + "."

        Dim fldTemp = ""
        Dim fldsLine = ""
        Dim fldsArr = New List(Of String)
        Dim valTemp = ""
        Dim valsLine = ""
        Dim valsArr = New List(Of String)
        Dim delim = ""
        For Each col In pTable.Columns
            If (col.IsIdentity AndAlso (Not pIncludeIdentity)) Then Continue For

            fldsLine += delim
            valsLine += delim
            delim = ", "
            fldTemp = String.Format("{0}", QuoteIdentifier(col.Name))
            valTemp = String.Format("@{0}", col.Name)

            'split to readable lines
            If ((fldsLine.Length > 0) AndAlso (((fldsLine.Length + fldTemp.Length) > 80) OrElse ((valsLine.Length + valTemp.Length) > 80))) Then
                fldsArr.Add(fldsLine)
                fldsLine = ""
                valsArr.Add(valsLine)
                valsLine = ""
            End If

            fldsLine += fldTemp
            valsLine += valTemp
        Next
        fldsArr.Add(fldsLine)
        valsArr.Add(valsLine)

        Dim res As New System.Text.StringBuilder
        res.AppendLine(String.Format("INSERT INTO {0}{1}", schemaPrefix, QuoteIdentifier(pTable.Name)))
        res.AppendLine("(")
        res.Append(SQL_INDENT)
        res.AppendLine(Join(fldsArr.ToArray, vbCrLf + SQL_INDENT))
        res.AppendLine(")")
        res.AppendLine("VALUES")
        res.AppendLine("(")
        res.Append(SQL_INDENT)
        res.AppendLine(Join(valsArr.ToArray, vbCrLf + SQL_INDENT))
        res.AppendLine(")")

        Return res.ToString
    End Function

    Public Function GenerateUpdateWithParams(pTable As TableInfo, Optional pUpdateKeyFields As Boolean = False, Optional pIncludeWhere As Boolean = True) As String
        If (Not TypeOf pTable Is TableInfo) Then Return "Not supported for this table type"

        Dim schemaPrefix = ""
        If (Not String.IsNullOrWhiteSpace(pTable.SchemaName)) Then schemaPrefix = QuoteIdentifier(pTable.SchemaName) + "."

        Dim updTemp = ""
        Dim updsLine = ""
        Dim updsArr = New List(Of String)
        Dim delim = ""
        Dim cond = ""
        For Each col In pTable.Columns
            If (col.InPrimaryKey) Then
                If (cond.Length > 0) Then cond += " AND "
                cond += String.Format("{0} = @{1}", QuoteIdentifier(col.Name), col.Name)
            End If
            If ((Not col.InPrimaryKey) OrElse pUpdateKeyFields) Then
                updsLine += delim
                delim = ", "
                updTemp = String.Format("{0} = @{1}", QuoteIdentifier(col.Name), col.Name)
            End If

            'split to readable lines
            If ((updsLine.Length > 0) AndAlso ((updsLine.Length + updTemp.Length) > 80)) Then
                updsArr.Add(updsLine)
                updsLine = ""
            End If

            updsLine += updTemp
        Next
        updsArr.Add(updsLine)

        If (cond = "") Then cond = "<CONDITION>"

        Dim res As New System.Text.StringBuilder
        res.AppendLine(String.Format("UPDATE {0}{1} SET", schemaPrefix, QuoteIdentifier(pTable.Name)))
        res.Append(SQL_INDENT)
        res.AppendLine(Join(updsArr.ToArray, vbCrLf + SQL_INDENT))
        If (pIncludeWhere) Then
            res.AppendLine("WHERE")
            res.AppendLine(String.Format("{0}{1}", SQL_INDENT, cond))
        End If

        Return res.ToString
    End Function

    Public Function GenerateInsertWithValues(pTable As TableInfo, pIncludeIdentity As Boolean) As String
        If (Not TypeOf pTable Is TableInfo) Then Return "Not supported for this table type"

        Dim schemaPrefix = ""
        If (Not String.IsNullOrWhiteSpace(pTable.SchemaName)) Then schemaPrefix = QuoteIdentifier(pTable.SchemaName) + "."

        Dim fldTemp = ""
        Dim fldsLine = ""
        Dim fldsArr = New List(Of String)
        Dim valTemp = ""
        Dim valsLine = ""
        Dim valsArr = New List(Of String)
        Dim delim = ""
        For Each col In pTable.Columns
            If (col.IsIdentity AndAlso (Not pIncludeIdentity)) Then Continue For

            Dim curVal = "NULL"
            If (Not col.Nullable) Then
                Select Case col.DataClass
                    Case eDataClass.StringType
                        curVal = "''"
                    Case eDataClass.IntegerType
                        curVal = "0"
                    Case eDataClass.FloatingPointType
                        curVal = "0.0"
                    Case eDataClass.DateTimeType
                        curVal = "GETDATE()"
                    Case Else
                        curVal = "?"
                End Select
            End If

            fldsLine += delim
            valsLine += delim
            delim = ", "
            fldTemp = String.Format("{0}", QuoteIdentifier(col.Name))
            valTemp = String.Format("{0}", curVal)

            'split to readable lines
            If ((fldsLine.Length > 0) AndAlso (((fldsLine.Length + fldTemp.Length) > 80) OrElse ((valsLine.Length + valTemp.Length) > 80))) Then
                fldsArr.Add(fldsLine)
                fldsLine = ""
                valsArr.Add(valsLine)
                valsLine = ""
            End If

            fldsLine += fldTemp
            valsLine += valTemp
        Next
        fldsArr.Add(fldsLine)
        valsArr.Add(valsLine)

        Dim res As New System.Text.StringBuilder
        res.AppendLine(String.Format("INSERT INTO {0}{1}", schemaPrefix, QuoteIdentifier(pTable.Name)))
        res.AppendLine("(")
        res.Append(SQL_INDENT)
        res.AppendLine(Join(fldsArr.ToArray, vbCrLf + SQL_INDENT))
        res.AppendLine(")")
        res.AppendLine("VALUES")
        res.AppendLine("(")
        res.Append(SQL_INDENT)
        res.AppendLine(Join(valsArr.ToArray, vbCrLf + SQL_INDENT))
        res.AppendLine(")")

        Return res.ToString
    End Function

End Class

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



Imports ScintillaNET

Public Class ctlEditSql

    Public Event SqlTextChanged As Action(Of Object, EventArgs)

    Public Property SqlText() As String
        Get
            Return ctlScintilla.Text
        End Get
        Set(ByVal value As String)
            ctlScintilla.Text = value
            ctlScintilla.SetSelection(0, 0)
        End Set
    End Property

    Public Property Modified() As Boolean
        Get
            Return ctlScintilla.Modified
        End Get
        Set(ByVal value As Boolean)
            If (value = False) Then
                ctlScintilla.SetSavePoint()
            Else
                Throw New NotImplementedException("ctlEditSql: Setting Modified to True is not supported")
            End If
        End Set
    End Property

    Public ReadOnly Property SelectionLength() As Int32
        Get
            Return ctlScintilla.SelectionEnd - ctlScintilla.SelectionStart
        End Get
    End Property

    Public ReadOnly Property SelectedText() As String
        Get
            Return ctlScintilla.SelectedText
        End Get
    End Property




    Private Sub ctlEditSql_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupEditorCmdKeys()
        SetupSqlEditor()
        tmrScintillaUpdate.Interval = 1000
        tmrScintillaUpdate.Start()
    End Sub


    Private Sub SetupEditorCmdKeys()
        Dim ctl = Me.ctlScintilla
        'pScintilla.ClearAllCmdKeys() 'doesn't work
        'these keys create control characters, remove them
        ctl.ClearCmdKey(Keys.Control Or Keys.Q)
        ctl.ClearCmdKey(Keys.Control Or Keys.W)
        ctl.ClearCmdKey(Keys.Control Or Keys.E)
        ctl.ClearCmdKey(Keys.Control Or Keys.R)
        ctl.ClearCmdKey(Keys.Control Or Keys.S)
        ctl.ClearCmdKey(Keys.Control Or Keys.F)
        ctl.ClearCmdKey(Keys.Control Or Keys.G)
        ctl.ClearCmdKey(Keys.Control Or Keys.H)
        ctl.ClearCmdKey(Keys.Control Or Keys.K)
        ctl.ClearCmdKey(Keys.Control Or Keys.O)
        ctl.ClearCmdKey(Keys.Control Or Keys.P)
        ctl.ClearCmdKey(Keys.Control Or Keys.B)
        ctl.ClearCmdKey(Keys.Control Or Keys.N)

        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.Q)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.W)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.E)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.R)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.Y)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.O)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.P)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.A)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.S)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.D)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.F)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.G)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.H)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.K)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.Z)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.X)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.C)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.V)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.B)
        ctl.ClearCmdKey(Keys.Control Or Keys.Shift Or Keys.N)
    End Sub


    Private Sub SetupSqlEditor()
        Dim ctl = Me.ctlScintilla
        ctl.StyleResetDefault()
        ctl.Styles(Style.Default).Font = "Courier New"
        ctl.Styles(Style.Default).Size = 10
        ctl.StyleClearAll()

        ctl.Lexer = Lexer.Sql

        'set up line numbers
        ctl.Margins(0).Width = 20
        RemoveHandler ctl.Insert, AddressOf Scintilla_ResizeLineNumbers
        RemoveHandler ctl.Delete, AddressOf Scintilla_ResizeLineNumbers
        AddHandler ctl.Insert, AddressOf Scintilla_ResizeLineNumbers
        AddHandler ctl.Delete, AddressOf Scintilla_ResizeLineNumbers

        'reset scroll width
        ctl.ScrollWidth = 1
        ctl.ScrollWidthTracking = True

        'setup styles
        ctl.Styles(Style.LineNumber).ForeColor = Color.FromArgb(255, 128, 128, 128)  'Dark Gray
        ctl.Styles(Style.LineNumber).BackColor = Color.FromArgb(255, 228, 228, 228)  'Light Gray
        ctl.Styles(Style.Sql.Comment).ForeColor = Color.Green
        ctl.Styles(Style.Sql.CommentLine).ForeColor = Color.Green
        ctl.Styles(Style.Sql.CommentLineDoc).ForeColor = Color.Green
        ctl.Styles(Style.Sql.Number).ForeColor = Color.Maroon
        ctl.Styles(Style.Sql.Word).ForeColor = Color.Blue
        ctl.Styles(Style.Sql.Word2).ForeColor = Color.Fuchsia
        ctl.Styles(Style.Sql.User1).ForeColor = Color.Gray
        ctl.Styles(Style.Sql.User2).ForeColor = Color.FromArgb(255, 0, 128, 192)    'Medium Blue-Green
        ctl.Styles(Style.Sql.String).ForeColor = Color.Red
        ctl.Styles(Style.Sql.Character).ForeColor = Color.Red
        ctl.Styles(Style.Sql.Operator).ForeColor = Color.Black

        ' Set keyword lists
        ' Word = 0
        ctl.SetKeywords(0, "add alter as authorization backup begin bigint binary bit break browse bulk by cascade case catch check checkpoint close clustered column commit compute constraint containstable continue create current cursor cursor database date datetime datetime2 datetimeoffset dbcc deallocate decimal declare default delete deny desc disk distinct distributed double drop dump else end errlvl escape except exec execute exit external fetch file fillfactor float for foreign freetext freetexttable from full function goto grant group having hierarchyid holdlock identity identity_insert identitycol if image index insert int intersect into key kill lineno load merge money national nchar nocheck nocount nolock nonclustered ntext numeric nvarchar of off offsets on open opendatasource openquery openrowset openxml option order over percent plan precision primary print proc procedure public raiserror read readtext real reconfigure references replication restore restrict return revert revoke rollback rowcount rowguidcol rule save schema securityaudit select set setuser shutdown smalldatetime smallint smallmoney sql_variant statistics table table tablesample text textsize then time timestamp tinyint to top tran transaction trigger truncate try union unique uniqueidentifier update updatetext use user values varbinary varchar varying view waitfor when where while with writetext xml ")
        ' Word2 = 1
        ctl.SetKeywords(1, "ascii cast char charindex ceiling coalesce collate contains convert current_date current_time current_timestamp current_user floor isnull max min nullif object_id session_user substring system_user tsequal ")
        ' User1 = 4
        ctl.SetKeywords(4, "all and any between cross exists in inner is join left like not null or outer pivot right some unpivot ( ) * ")
        ' User2 = 5
        ctl.SetKeywords(5, "sys objects sysobjects ")
    End Sub

    Public Sub HighlightWord(pToHighlight As String)
        Dim ctl = Me.ctlScintilla
        ' Indicators 0-7 could be in use by a lexer
        ' so we'll use indicator 8 to highlight words.
        Dim indNum = 8

        ' Remove all uses of our indicator
        ctl.IndicatorCurrent = indNum
        ctl.IndicatorClearRange(0, ctl.TextLength)

        If (String.IsNullOrWhiteSpace(pToHighlight)) Then Return
        If (pToHighlight.Length < 3) Then Return

        ' Update indicator appearance
        ctl.Indicators(indNum).Style = IndicatorStyle.StraightBox
        ctl.Indicators(indNum).Under = True
        ctl.Indicators(indNum).ForeColor = Color.Green
        ctl.Indicators(indNum).OutlineAlpha = 50
        ctl.Indicators(indNum).Alpha = 30

        ' Search the document
        ctl.TargetStart = 0
        ctl.TargetEnd = ctl.TextLength
        ctl.SearchFlags = SearchFlags.WholeWord
        While (ctl.SearchInTarget(pToHighlight) <> -1)
            ' Mark the search results with the current indicator
            ctl.IndicatorFillRange(ctl.TargetStart, ctl.TargetEnd - ctl.TargetStart)

            ' Search the remainder of the document
            ctl.TargetStart = ctl.TargetEnd
            ctl.TargetEnd = ctl.TextLength
        End While
    End Sub

    Private Shared Sub Scintilla_ResizeLineNumbers(sender As Object, e As ModificationEventArgs)
        Dim ctl = DirectCast(sender, Scintilla)
        If (e.LinesAdded > 0) Then
            Dim maxLineNumberCharLength = ctl.Lines.Count.ToString().Length

            ' Calculate the width required to display the last line number
            ' and include some padding for good measure.
            Dim padding = 2
            ctl.Margins(0).Width = ctl.TextWidth(Style.LineNumber, New String("9"c, maxLineNumberCharLength + 1)) + padding
        End If
    End Sub


    Private Sub tmrScintillaUpdate_Tick(sender As Object, e As EventArgs) Handles tmrScintillaUpdate.Tick
        Dim ctl = ctlScintilla

        'always keep a newline at the end, helps for line selection using the keyboard
        If (Not ctl.Text.EndsWith(Environment.NewLine)) Then
            'but don't mess up redo history, or change an unmodified file
            If ((Not ctl.CanRedo) AndAlso Modified) Then ctl.AppendText(Environment.NewLine)
        End If
    End Sub

    Private Sub ctlScintilla_CharAdded(sender As Object, e As CharAddedEventArgs) Handles ctlScintilla.CharAdded
        'Dim ctl = ctlScintilla

        '' Find the word start
        'Dim currentPos = ctl.CurrentPosition
        'Dim wordStartPos = ctl.WordStartPosition(currentPos, True)

        '' Display the autocompletion list
        'Dim lenEntered = currentPos - wordStartPos
        'If (lenEntered > 0) Then
        '    ctl.AutoCShow(lenEntered, "test junk blah hey")
        'End If
    End Sub

    Private Sub ctlScintilla_TextChanged(sender As Object, e As EventArgs) Handles ctlScintilla.TextChanged
        Dim ctl = ctlScintilla

        'update scroll width
        If ((ctl.TextLength = 0) AndAlso (ctl.ScrollWidth > ctl.Width)) Then
            'causes a flash if done every time
            ctl.ScrollWidth = 1
            ctl.ScrollWidthTracking = True
        End If
        RaiseEvent SqlTextChanged(Me, New EventArgs())
    End Sub


    Private _isHighlight As Boolean = False
    Private Sub ctlScintilla_UpdateUI(sender As Object, e As UpdateUIEventArgs) Handles ctlScintilla.UpdateUI
        Dim ctl = ctlScintilla

        If (e.Change = UpdateChange.Selection) Then
            If (_isHighlight) Then
                HighlightWord(Nothing)
                _isHighlight = False
            End If
            If (Not String.IsNullOrWhiteSpace(ctl.SelectedText)) Then
                If (ctl.IsRangeWord(ctl.SelectionStart, ctl.SelectionEnd)) Then
                    HighlightWord(ctl.SelectedText)
                    _isHighlight = True
                End If
            End If
        End If
    End Sub


End Class

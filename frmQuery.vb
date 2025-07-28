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



Imports System.Data.Common

Public Class frmQuery
    Implements IFormGroupKey

    Public Property FormGroupKey As String Implements IFormGroupKey.FormGroupKey
    Public Property IsFormGroupParent As Boolean Implements IFormGroupKey.IsFormGroupParent

    Private _queryTabs As New List(Of ctlQuery)
    Private _currentQueryTab As ctlQuery
    'maintain list of history, for selecting previously selected
    Private _currentQueryTabHistory As New List(Of ctlQuery)
    Private _queryTabCounter As Int32 = 1
    Private _alwaysShowTabBar As Boolean = True


    Private _connInfo As Rational.DB.DbConnectionInfo
    Public Property ConnInfo() As Rational.DB.DbConnectionInfo
        Get
            Return _connInfo
        End Get
        Set(ByVal value As Rational.DB.DbConnectionInfo)
            If (Not Object.Equals(value, ConnInfo)) Then
                _connInfo = value
                For Each ctl In _queryTabs
                    ctl.ConnInfo = _connInfo
                Next
            End If
        End Set
    End Property


    Private _viewResultsAsList As Boolean = True
    Public Property ViewResultsAsList() As Boolean
        Get
            Return _viewResultsAsList
        End Get
        Set(ByVal value As Boolean)
            _viewResultsAsList = value
            SetViewResultsAs()
        End Set
    End Property


    Public Sub AddQueryTab(Optional pSql As String = Nothing, Optional pQuickView As Boolean = False)
        Dim ctl As New ctlQuery()
        ctl.SetQuery(pSql)
        ctl.QuickView = pQuickView
        AddQueryTab(ctl)
    End Sub


    Public Sub UpdateRunningTabs()
        If (_queryTabs.Count = 0) Then Return
        Dim curTabRunning = CurrentQueryTab.IsRunning()
        btnRun.Enabled = Not curTabRunning
        btnStop.Enabled = curTabRunning
        mnuView.Enabled = Not curTabRunning
        mnuExport.Enabled = Not curTabRunning
        ctlProgress.Visible = curTabRunning
        lblElapsedTime.Text = CurrentQueryTab.RunTimeString

        'set loading image on tabs that are running
        For Each tb In _queryTabs
            Dim btn = DirectCast(tb.Tag, ToolStripButton)
            If (tb.IsRunning) Then
                btn.Image = My.Resources.wait
            Else
                btn.Image = Nothing
            End If
        Next
    End Sub



#Region "Private methods"


    Private Function CurrentQueryTab() As ctlQuery
        Return _currentQueryTab
    End Function


    Private Sub AddQueryTab(pControl As ctlQuery)
        pControl.ParentQueryForm = Me
        pControl.ConnInfo = _connInfo
        pControl.Dock = DockStyle.Fill
        ToolStripContainer1.ContentPanel.Controls.Add(pControl)
        Dim btn = New ToolStripButton(String.Format("Query {0}", _queryTabCounter))
        btn.Tag = pControl
        btn.AutoToolTip = False
        btn.Padding = New Padding(0, 0, 20, 0)
        pControl.Tag = btn
        AddHandler btn.Click, AddressOf QueryTab_Click
        AddHandler btn.MouseDown, AddressOf QueryTab_MouseDown
        ctlTabStrip.Items.Add(btn)
        If (_alwaysShowTabBar OrElse (_queryTabs.Count >= 1)) Then
            ctlTabStrip.Visible = True
        Else
            ctlTabStrip.Visible = False
        End If
        _queryTabs.Add(pControl)
        _queryTabCounter += 1
        SelectQueryTab(pControl)
    End Sub


    Private Function RemoveQueryTab(pControl As ctlQuery) As Boolean
        If (pControl.IsRunning()) Then
            pControl.DoStop()
            Return False
        End If
        If (pControl.Modified AndAlso (Not String.IsNullOrEmpty(pControl.FilePath))) Then
            Dim saveResp = MessageBox.Show("File modified, do you wish to save?", "Confirm", MessageBoxButtons.YesNoCancel)
            If (saveResp = Windows.Forms.DialogResult.Cancel) Then Return False
            If (saveResp = Windows.Forms.DialogResult.Yes) Then DoSave(pControl, pForceSaveAs:=False)
        End If

        Dim removingCurrent = (pControl.Equals(CurrentQueryTab))
        pControl.CleanupCurrentResult()
        _queryTabs.Remove(pControl)
        If (_currentQueryTabHistory.Contains(pControl)) Then _currentQueryTabHistory.Remove(pControl)
        ToolStripContainer1.ContentPanel.Controls.Remove(pControl)
        ctlTabStrip.Items.Remove(DirectCast(pControl.Tag, ToolStripButton))
        If (_queryTabs.Count = 0) Then
            Me.Close()
        Else
            If (removingCurrent) Then SelectQueryTab(_currentQueryTabHistory.Last())
            If (_alwaysShowTabBar OrElse (_queryTabs.Count > 1)) Then
                ctlTabStrip.Visible = True
            Else
                ctlTabStrip.Visible = False
            End If
        End If
        Return True
    End Function


    Private Sub SelectQueryTab(pControl As ctlQuery)
        Dim btn = DirectCast(pControl.Tag, ToolStripButton)
        For Each tsItem As ToolStripItem In ctlTabStrip.Items
            If ((tsItem.Tag IsNot Nothing) AndAlso (tsItem.Tag.GetType() = GetType(ctlQuery))) Then
                Dim tsBtn = DirectCast(tsItem, ToolStripButton)
                tsBtn.Checked = False
            End If
        Next
        btn.Checked = True
        pControl.BringToFront()
        pControl.txtQuery.Focus()
        _currentQueryTab = pControl
        If (_currentQueryTabHistory.Contains(_currentQueryTab)) Then _currentQueryTabHistory.Remove(_currentQueryTab)
        _currentQueryTabHistory.Add(_currentQueryTab)
        UpdateRunningTabs()
    End Sub


    Private Sub SelectQueryTab(pButton As ToolStripButton)
        Dim ctl = DirectCast(pButton.Tag, ctlQuery)
        SelectQueryTab(ctl)
    End Sub


    Private Sub SetQueryTabTitle(pControl As ctlQuery, pTitle As String, pTooltip As String)
        Dim btn = DirectCast(pControl.Tag, ToolStripButton)
        btn.Text = pTitle
        If (Not String.IsNullOrEmpty(pTooltip)) Then
            btn.AutoToolTip = True
            btn.ToolTipText = pTooltip
        Else
            btn.AutoToolTip = False
            btn.ToolTipText = Nothing
        End If
    End Sub


    Private Sub SetViewResultsAs()
        mnuViewResultsList.Checked = ViewResultsAsList
        mnuViewResultsTabs.Checked = Not ViewResultsAsList
        For Each ctl In _queryTabs
            ctl.ViewResultsAsList = ViewResultsAsList
        Next
    End Sub


    Private Sub DoLoad(pFiles As List(Of String))
        For x = 0 To pFiles.Count - 1
            Dim fn = pFiles(x)
            Dim ctl As ctlQuery
            If ((x = 0) AndAlso String.IsNullOrWhiteSpace(CurrentQueryTab.GetFullQuery())) Then
                ctl = CurrentQueryTab()
            Else
                ctl = New ctlQuery()
                AddQueryTab(ctl)
            End If
            ctl.DoLoad(fn)
            SetQueryTabTitle(ctl, ctl.Filename, ctl.FilePath)
        Next
    End Sub


    Private Sub DoSave(pControl As ctlQuery, pForceSaveAs As Boolean)
        Dim fn = ""
        If (pForceSaveAs OrElse String.IsNullOrEmpty(pControl.FilePath)) Then
            fn = Util.PromptSaveFile("sql")
            If (String.IsNullOrEmpty(fn)) Then Return
        Else
            fn = pControl.FilePath
        End If
        pControl.DoSave(fn)
        SetQueryTabTitle(CurrentQueryTab, pControl.Filename, pControl.FilePath)
    End Sub


#End Region



#Region "Events"

    Private Sub frmQuery_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetViewResultsAs()
        Me.AllowDrop = True
        AddHandler Me.DragEnter, AddressOf frmQuery_DragEnter
        AddHandler Me.DragDrop, AddressOf frmQuery_DragDrop
    End Sub


    Private Sub frmQuery_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If (_queryTabs.Count = 0) Then AddQueryTab()
        tmrUpdateTabs.Enabled = True
    End Sub


    Private Sub frmQuery_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim cancel = False
        For Each ctl In _queryTabs
            If (ctl.IsRunning()) Then
                ctl.DoStop()
                cancel = True
            End If
        Next
        If (cancel) Then
            e.Cancel = True
            MessageBox.Show("Running queries were cancelled.", "Warning", MessageBoxButtons.OK)
            Return
        End If

        While (_queryTabs.Count > 0)
            Dim ctl = _queryTabs(0)
            SelectQueryTab(ctl)
            Dim closeOk = RemoveQueryTab(ctl)
            If (Not closeOk) Then
                e.Cancel = True
                Return
            End If
        End While
    End Sub


    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        Dim ctl = CurrentQueryTab()
        If (ctl IsNot Nothing) Then
            If (ctl.IsRunning()) Then
                If (keyData = Keys.Escape) Then ctl.DoStop()
            Else
                If (keyData = Keys.F5) Then ctl.DoQuery()
                If ((keyData = Keys.Escape) AndAlso ctl.QuickView) Then RemoveQueryTab(ctl)
            End If
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function


    Private Sub frmQuery_DragEnter(sender As Object, e As DragEventArgs)
        If (e.Data.GetDataPresent(DataFormats.FileDrop)) Then e.Effect = DragDropEffects.Copy
    End Sub


    Private Sub frmQuery_DragDrop(sender As Object, e As DragEventArgs)
        Dim files = DirectCast(e.Data.GetData(DataFormats.FileDrop), String()).ToList()
        DoLoad(files)
    End Sub


    Private Sub QueryTab_Click(sender As Object, e As EventArgs)
        Dim btn = DirectCast(sender, ToolStripButton)
        SelectQueryTab(btn)
    End Sub


    Private Sub QueryTab_MouseDown(sender As Object, e As MouseEventArgs)
        If (e.Button = Windows.Forms.MouseButtons.Middle) Then
            Dim btn = DirectCast(sender, ToolStripButton)
            Dim ctl = DirectCast(btn.Tag, ctlQuery)
            RemoveQueryTab(ctl)
        End If
    End Sub


    Private Sub mnuNewWindow_Click(sender As Object, e As EventArgs) Handles mnuNewWindow.Click
        Dim frm As New frmQuery()
        frm.FormGroupKey = Me.FormGroupKey
        frm.ConnInfo = Me.ConnInfo
        frm.ViewResultsAsList = Me.ViewResultsAsList
        frm.Text = Me.Text
        Util.ShowFormRelativeTo(frm, Me.Bounds)
    End Sub


    Private Sub mnuNewTab_Click(sender As Object, e As EventArgs) Handles mnuNewTab.Click
        AddQueryTab("", pQuickView:=False)
    End Sub


    Private Sub mnuDuplicateTab_Click(sender As Object, e As EventArgs) Handles mnuDuplicateTab.Click
        AddQueryTab(CurrentQueryTab.GetActiveQuery, pQuickView:=False)
    End Sub


    Private Sub mnuLoad_Click(sender As Object, e As EventArgs) Handles mnuLoad.Click
        Dim files As List(Of String)
        Using frm = New Windows.Forms.OpenFileDialog()
            frm.Filter = "SQL file (*.sql)|*.sql|All Files|*.*"
            frm.RestoreDirectory = True
            frm.InitialDirectory = Util.GetCurrentOutputFolder()
            frm.Multiselect = True
            If (frm.ShowDialog() <> Windows.Forms.DialogResult.OK) Then Return
            files = frm.FileNames.ToList()
            Util.SetCurrentOutputFolder(IO.Path.GetDirectoryName(frm.FileName))
        End Using

        DoLoad(files)
    End Sub


    Private Sub mnuSave_Click(sender As Object, e As EventArgs) Handles mnuSave.Click
        DoSave(CurrentQueryTab, pForceSaveAs:=False)
    End Sub


    Private Sub mnuSaveAs_Click(sender As Object, e As EventArgs) Handles mnuSaveAs.Click
        DoSave(CurrentQueryTab, pForceSaveAs:=True)
    End Sub


    Private Sub mnuSaveAll_Click(sender As Object, e As EventArgs) Handles mnuSaveAll.Click
        Dim curTab = CurrentQueryTab()
        For Each tb In _queryTabs
            SelectQueryTab(tb)
            DoSave(tb, pForceSaveAs:=False)
            If (Not String.IsNullOrEmpty(tb.FilePath)) Then
                SetQueryTabTitle(tb, tb.Filename, tb.FilePath)
            End If
        Next
        SelectQueryTab(curTab)
    End Sub


    Private Sub mnuClose_Click(sender As Object, e As EventArgs) Handles mnuClose.Click
        RemoveQueryTab(CurrentQueryTab)
    End Sub


    Private Sub mnuCloseAll_Click(sender As Object, e As EventArgs) Handles mnuCloseAll.Click
        Me.Close()
    End Sub


    Private Sub mnuExportCsv_Click(sender As Object, e As EventArgs) Handles mnuExportCsv.Click
        CurrentQueryTab.DoExport(ctlQuery.eExportMode.Csv)
    End Sub


    Private Sub mnuExportExcel_Click(sender As Object, e As EventArgs) Handles mnuExportExcel.Click
        CurrentQueryTab.DoExport(ctlQuery.eExportMode.Excel)
    End Sub


    Private Sub mnuExportCodeGen_Click(sender As Object, e As EventArgs) Handles mnuExportCodeGen.Click
        CurrentQueryTab.DoExport(ctlQuery.eExportMode.CodeGen)
    End Sub


    Private Sub mnuExportCodeGenByRow_Click(sender As Object, e As EventArgs) Handles mnuExportCodeGenByRow.Click
        CurrentQueryTab.DoExport(ctlQuery.eExportMode.CodeGenByRow)
    End Sub


    Private Sub btnRun_Click(sender As Object, e As EventArgs) Handles btnRun.Click
        CurrentQueryTab.DoQuery()
        CurrentQueryTab.txtQuery.Focus()
    End Sub


    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        CurrentQueryTab.DoStop()
    End Sub


    Private Sub mnuViewResultsList_Click(sender As Object, e As EventArgs) Handles mnuViewResultsList.Click
        ViewResultsAsList = True
        SetViewResultsAs()
    End Sub


    Private Sub mnuViewResultsTabs_Click(sender As Object, e As EventArgs) Handles mnuViewResultsTabs.Click
        ViewResultsAsList = False
        SetViewResultsAs()
    End Sub


    Private Sub btnCloseTab_Click(sender As Object, e As EventArgs) Handles btnCloseTab.Click
        RemoveQueryTab(CurrentQueryTab)
    End Sub

    Private Sub tmrUpdateTabs_Tick(sender As Object, e As EventArgs) Handles tmrUpdateTabs.Tick
        UpdateRunningTabs()
    End Sub


#End Region


End Class
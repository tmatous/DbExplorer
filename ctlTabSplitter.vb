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



Public Class ctlTabSplitter

    Private Const SCROLLBAR_PADDING = 10
    Private Const TAB_RESIZE_PADDING = 100

    Private _splitterList As New List(Of Splitter)

    Private _tabList As New List(Of Panel)
    Public ReadOnly Property TabPages() As ObjectModel.ReadOnlyCollection(Of Panel)
        Get
            Return _tabList.AsReadOnly()
        End Get
    End Property

    Public ReadOnly Property TabCount() As Int32
        Get
            Return _tabList.Count
        End Get
    End Property

    Public Property MinTabHeight As Int32 = 150

    Public Function InsertTab(index As Int32) As Panel
        If (index < 0) Then Throw New Exception("Invalid index")
        If (index > TabCount) Then Throw New Exception("Invalid index")

        If (TabCount = 1) Then AddSplitter()
        If (TabCount >= 1) Then AddSplitter()

        Dim newTab = New Panel()
        newTab.Dock = DockStyle.Top
        newTab.Padding = New Padding(0, 0, SCROLLBAR_PADDING, 0)
        _tabList.Insert(index, newTab)

        RedrawTabs()

        Return newTab
    End Function

    Private Sub AddSplitter()
        Dim newSplit = New Splitter()
        newSplit.Dock = DockStyle.Top
        newSplit.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
        newSplit.BackColor = Color.DarkGray
        AddHandler newSplit.SplitterMoved, AddressOf SplitterMoved
        _splitterList.Add(newSplit)
    End Sub

    Private Sub RemoveSplitter()
        Dim spl = _splitterList.Item(0)
        _splitterList.Remove(spl)
        spl.Dispose()
    End Sub

    Public Sub RemoveTab(control As Panel)
        If (Not _tabList.Contains(control)) Then Throw New Exception("Tab not found")

        _tabList.Remove(control)

        If (TabCount > 1) Then RemoveSplitter()
        If (TabCount = 1) Then RemoveSplitter()

        RedrawTabs()
    End Sub

    Public Sub AutosizeTabs()
        Dim availHeight = Me.ClientSize.Height - SCROLLBAR_PADDING
        If (TabCount = 0) Then
            Return
        ElseIf (TabCount = 1) Then
            Me.pnlContent.Height = availHeight
            _tabList.Item(0).Height = availHeight
        Else
            Dim sizeEach = availHeight \ _tabList.Count
            If (sizeEach < MinTabHeight) Then sizeEach = MinTabHeight

            Me.pnlContent.SuspendLayout()
            For Each tb In _tabList
                tb.Height = sizeEach
            Next
            Me.pnlContent.ResumeLayout()
            RedrawTabs()
        End If
    End Sub



    Private Sub RedrawTabs()
        Me.pnlContent.Controls.Clear()

        If (TabCount = 0) Then
            Me.pnlContent.Dock = DockStyle.Fill
            Return
        ElseIf (TabCount = 1) Then
            Me.pnlContent.Dock = DockStyle.Fill
            Dim ctl = _tabList.Item(0)
            Me.pnlContent.Controls.Add(ctl)
            ctl.Dock = DockStyle.Fill
        Else
            Me.pnlContent.Dock = DockStyle.Top
            Dim controlList = New List(Of Control)
            For tabInd = 0 To _tabList.Count - 1
                _tabList.Item(tabInd).Dock = DockStyle.Top
                controlList.Add(_tabList.Item(tabInd))
                If (TabCount > 1) Then controlList.Add(_splitterList.Item(tabInd))
            Next

            Me.pnlContent.SuspendLayout()
            Dim totalHeight = 0
            For Each ctl In controlList
                Me.pnlContent.Controls.Add(ctl)
                ctl.BringToFront()
                totalHeight += ctl.Height
            Next
            Me.pnlContent.ResumeLayout()
            Me.pnlContent.Height = totalHeight + TAB_RESIZE_PADDING
        End If
    End Sub


    Private Sub ctlTabSplitter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.AutoScroll = True
        RedrawTabs()
    End Sub

    Private Sub SplitterMoved(sender As Object, e As SplitterEventArgs)
        RedrawTabs()
    End Sub

End Class

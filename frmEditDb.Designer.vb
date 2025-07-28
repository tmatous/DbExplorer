<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditDb
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEditDb))
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnQuery = New System.Windows.Forms.Button()
        Me.mnuContextRow = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuRowCopy = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRowPaste = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuRowSaveChanges = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRowUndoChanges = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuContextCell = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCellSetNull = New System.Windows.Forms.ToolStripMenuItem()
        Me.grdEdit = New DbExplorer.MyDataGridView()
        Me.mnuContextRow.SuspendLayout()
        Me.mnuContextCell.SuspendLayout()
        CType(Me.grdEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(5, 6)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(57, 21)
        Me.btnLoad.TabIndex = 30
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(68, 6)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(57, 21)
        Me.btnSave.TabIndex = 50
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnQuery
        '
        Me.btnQuery.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnQuery.Location = New System.Drawing.Point(924, 5)
        Me.btnQuery.Name = "btnQuery"
        Me.btnQuery.Size = New System.Drawing.Size(70, 21)
        Me.btnQuery.TabIndex = 40
        Me.btnQuery.Text = "Edit Query"
        Me.btnQuery.UseVisualStyleBackColor = True
        '
        'mnuContextRow
        '
        Me.mnuContextRow.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRowCopy, Me.mnuRowPaste, Me.ToolStripSeparator1, Me.mnuRowSaveChanges, Me.mnuRowUndoChanges})
        Me.mnuContextRow.Name = "mnuContextRow"
        Me.mnuContextRow.ShowImageMargin = False
        Me.mnuContextRow.Size = New System.Drawing.Size(120, 98)
        '
        'mnuRowCopy
        '
        Me.mnuRowCopy.Name = "mnuRowCopy"
        Me.mnuRowCopy.Size = New System.Drawing.Size(119, 22)
        Me.mnuRowCopy.Text = "Copy"
        '
        'mnuRowPaste
        '
        Me.mnuRowPaste.Name = "mnuRowPaste"
        Me.mnuRowPaste.Size = New System.Drawing.Size(119, 22)
        Me.mnuRowPaste.Text = "Paste"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(116, 6)
        '
        'mnuRowSaveChanges
        '
        Me.mnuRowSaveChanges.Name = "mnuRowSaveChanges"
        Me.mnuRowSaveChanges.Size = New System.Drawing.Size(119, 22)
        Me.mnuRowSaveChanges.Text = "Save Changes"
        '
        'mnuRowUndoChanges
        '
        Me.mnuRowUndoChanges.Name = "mnuRowUndoChanges"
        Me.mnuRowUndoChanges.Size = New System.Drawing.Size(119, 22)
        Me.mnuRowUndoChanges.Text = "Undo Changes"
        '
        'mnuContextCell
        '
        Me.mnuContextCell.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCellSetNull})
        Me.mnuContextCell.Name = "mnuCellRightClick"
        Me.mnuContextCell.ShowImageMargin = False
        Me.mnuContextCell.Size = New System.Drawing.Size(106, 26)
        '
        'mnuCellSetNull
        '
        Me.mnuCellSetNull.Name = "mnuCellSetNull"
        Me.mnuCellSetNull.Size = New System.Drawing.Size(105, 22)
        Me.mnuCellSetNull.Text = "Set to NULL"
        '
        'grdEdit
        '
        Me.grdEdit.AllowUserToResizeRows = False
        Me.grdEdit.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdEdit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdEdit.Location = New System.Drawing.Point(5, 33)
        Me.grdEdit.MultiSelect = False
        Me.grdEdit.Name = "grdEdit"
        Me.grdEdit.Size = New System.Drawing.Size(989, 485)
        Me.grdEdit.TabIndex = 54
        '
        'frmEditDb
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(997, 521)
        Me.Controls.Add(Me.grdEdit)
        Me.Controls.Add(Me.btnQuery)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnLoad)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmEditDb"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Data"
        Me.mnuContextRow.ResumeLayout(false)
        Me.mnuContextCell.ResumeLayout(false)
        CType(Me.grdEdit,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)

End Sub
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnQuery As System.Windows.Forms.Button
    Friend WithEvents mnuContextRow As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuRowSaveChanges As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRowUndoChanges As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuContextCell As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCellSetNull As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents grdEdit As MyDataGridView
    Friend WithEvents mnuRowCopy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuRowPaste As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
End Class

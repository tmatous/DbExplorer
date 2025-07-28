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


Friend Class WaitFormPrv
    Private _parentForm As Form
    Private _handler As WaitForm
    Private Const _baseHeight As Integer = 50

    Friend Sub New(pHandler As WaitForm, pParentForm As Form)
        InitializeComponent()

        _handler = pHandler
        _parentForm = pParentForm

        If (_parentForm IsNot Nothing) Then
            Left = _parentForm.Left + (_parentForm.Width \ 2) - (Width \ 2)
            Top = _parentForm.Top + (_parentForm.Height \ 2) - (Height \ 2)
        Else
            Me.StartPosition = FormStartPosition.CenterScreen
        End If
    End Sub

    Private Sub WaitFormPrv_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        DoRefresh()
        tmrRefresh.Enabled = True
    End Sub

    Private Sub StatusLabel_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles StatusLabel.TextChanged
        Dim gfx As Graphics = StatusLabel.CreateGraphics
        Dim textSize As SizeF = gfx.MeasureString(StatusLabel.Text, StatusLabel.Font, StatusLabel.DisplayRectangle.Width)
        gfx.Dispose()
        Height = CInt(_baseHeight + textSize.Height + StatusLabel.Font.Height)
    End Sub




    Public Sub UpdateProgress(pProgressPercent As Int32)
        If InvokeRequired Then
            Invoke(New Action(Of Int32)(AddressOf UpdateProgress), pProgressPercent)
        Else
            ProgressBar.Value = pProgressPercent
            DoRefresh()
        End If
    End Sub

    Public Sub UpdateMessage(pMessage As String)
        If InvokeRequired Then
            Invoke(New Action(Of String)(AddressOf UpdateMessage), pMessage)
        Else
            StatusLabel.Text = pMessage
            DoRefresh()
        End If
    End Sub

    Public Sub DoRefresh()
        If InvokeRequired Then
            Invoke(New Action(AddressOf DoRefresh))
        Else
            Me.Refresh()
            Me.Focus()
            Me.BringToFront()
        End If
    End Sub


#Region "Timing issues"

    'this is all to handle the timing issues involved with creating a form and destroying it automatically very quickly

    Private Sub tmrRefresh_Tick(sender As System.Object, e As System.EventArgs) Handles tmrRefresh.Tick
        If (_handler.IsDisposed) Then
            DoStartClose()
        Else
            DoRefresh()
        End If
    End Sub

    Public Sub DoStartClose()
        If InvokeRequired Then
            Invoke(New Action(AddressOf DoStartClose))
        Else
            tmrRefresh.Enabled = False
            Me.Hide()
            Util.DelayDo(New Action(AddressOf Me.DoClose), TimeSpan.FromMilliseconds(1000))
        End If
    End Sub

    Private Sub DoClose()
        If InvokeRequired Then
            Invoke(New Action(AddressOf DoClose))
        Else
            Me.Close()
        End If
    End Sub

#End Region

End Class


Public Class WaitForm
    Implements IDisposable

    Private _form As WaitFormPrv
    Private _parentForm As Form
    Private _message As String
    Private _showProgress As Boolean
    Private _settings As Settings

    Public Class Settings
        Public Property DelayMs As Int32 = 250
    End Class

    ''' <summary>
    ''' Creates a new WaitForm and displays it to the user
    ''' </summary>
    Public Sub New(pParentForm As Form, pMessage As String, Optional pShowProgress As Boolean = False, Optional pSettings As Settings = Nothing)
        _parentForm = pParentForm
        _message = pMessage
        _showProgress = pShowProgress
        _settings = pSettings
        If (_settings Is Nothing) Then _settings = New Settings()
        Util.DelayDo(New Action(AddressOf DoShowForm), TimeSpan.FromMilliseconds(_settings.DelayMs))
    End Sub

    ''' <summary>
    ''' Updates the status message
    ''' </summary>
    ''' <param name="pMessage">Status message to display</param>
    ''' <remarks></remarks>
    Public Sub UpdateMessage(pMessage As String)
        _message = pMessage
        If (_form IsNot Nothing) Then
            _form.UpdateMessage(_message)
        End If
    End Sub

    ''' <summary>
    ''' Updates the progress bar value
    ''' </summary>
    ''' <param name="pProgressPercent">A value from 0 to 100 representing the percentage complete</param>
    ''' <remarks></remarks>
    Public Sub UpdateProgress(pProgressPercent As Int32)
        If (_form IsNot Nothing) Then
            If (pProgressPercent > 100) Then pProgressPercent = 100
            If (pProgressPercent < 0) Then pProgressPercent = 0
            _form.UpdateProgress(pProgressPercent)
        End If
    End Sub

    ''' <summary>
    ''' Updates the progress bar value
    ''' </summary>
    ''' <param name="pCurrent">Current progress value between 0 and pMax</param>
    ''' <param name="pMax">Maximum progress value</param>
    ''' <remarks></remarks>
    Public Sub UpdateProgress(pCurrent As Double, pMax As Double)
        UpdateProgress(CInt((pCurrent / pMax) * 100))
    End Sub

    Private Sub DoShowForm()
        If (IsDisposed) Then Return

        _form = New WaitFormPrv(Me, _parentForm)
        _form.StatusLabel.Text = _message
        If (Not _showProgress) Then
            _form.ProgressBar.Visible = False
            _form.StatusLabel.Top -= 15
        End If

        'starts a new message pump for this form
        Application.Run(_form)

        '_form.ShowDialog()

        'this resulted in cross-thread error
        '_form.ShowDialog(_parentForm)
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If (_form IsNot Nothing) Then _form.DoStartClose()
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Public ReadOnly Property IsDisposed As Boolean
        Get
            Return Me.disposedValue
        End Get
    End Property

#End Region

End Class

'import .net namespace(s) required
Imports System.Threading
Imports System.Windows.Threading

Namespace WPF.MVVM

    ''' <summary>
    ''' dialog viemodel base class
    ''' </summary>
    Public MustInherit Class DialogViewModel

        Inherits ViewModel

#Region "private variable(s)"

        Private _dialogResult As Boolean = False

        Private _myBlock As EventWaitHandle = Nothing

#End Region

#Region "public properties"

        Public Property DialogResult As Boolean
            Get

                Return _dialogResult

            End Get
            Set(value As Boolean)

                _dialogResult = value
                OnPropertyChanged()

            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()

            'create a new base-class instance
            MyBase.New

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method showing up the overlay as dialog
        ''' </summary>
        ''' <returns></returns>
        Public Function ShowDialog() As Boolean

            'show the view
            ShowView()

            'wait for user input
            _myBlock = New EventWaitHandle(False, EventResetMode.ManualReset)
            WaitForEvent(_myBlock, New TimeSpan(24, 0, 0))

            'remove the wait handle
            _myBlock.Dispose()

            'return the dialog closing indicator
            Return Me.DialogResult

        End Function

#End Region

#Region "protected method(s)"

        ''' <summary>
        ''' method showing the view
        ''' </summary>
        Protected MustOverride Sub ShowView()

        ''' <summary>
        ''' method closing the view
        ''' </summary>
        Protected MustOverride Sub CloseView()

        ''' <summary>
        ''' method closing the dialog, setting the dialog result
        ''' </summary>
        Protected Sub Close(ByVal result As Boolean)

            'set the dialog result
            Me.DialogResult = result

            'close the view
            CloseView()

            'release the blocking
            If Not IsNothing(_myBlock) Then

                _myBlock.Set()

            End If

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method allowing to wait for an event (non-blocking the application thread)
        ''' </summary>
        ''' <param name="eventHandle"></param>
        ''' <param name="timeout"></param>
        ''' <returns></returns>
        ''' <remarks>based on the article found at 'https://medium.com/home-wireless/a-non-blocking-wait-for-event-in-c-6662ba47373'</remarks>
        Private Function WaitForEvent(eventHandle As EventWaitHandle, Optional timeout As TimeSpan = Nothing) As Boolean

            Dim didWait As Boolean = False
            Dim frame = New DispatcherFrame()
            Dim start As New ParameterizedThreadStart(Sub()

                                                          didWait = eventHandle.WaitOne(timeout)
                                                          frame.[Continue] = False

                                                      End Sub)

            Dim thread = New Thread(start)
            thread.Start()
            Dispatcher.PushFrame(frame)
            Return didWait

        End Function

#End Region

    End Class

End Namespace
'import .net namespace(s) required
Imports System.Threading
Imports System.Windows.Threading

Namespace WPF.MVVM

    Public MustInherit Class GUIThreadViewModel(Of T)

        Inherits ViewModel

#Region "public event(s)"

        Public Event Closed(sender As GUIThreadViewModel(Of T))

#End Region

#Region "protected variable(s)"

        Protected _myView As Window = Nothing
        Protected _myThread As Thread = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property View As Window
            Get

                Return _myView

            End Get
        End Property

        Public ReadOnly Property ThreadID As Integer
            Get

                Return _myThread.ManagedThreadId

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="show"></param>
        Public Sub New(Optional ByVal show As Boolean = True)

            'create a new base-class instance
            MyBase.New()

            'set the variable(s)
            If show Then

                Me.Show()

            End If

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method showing up the GUI
        ''' </summary>
        Public Sub Show()

            'clear from a possibly running instance
            If Not IsNothing(_myThread) Then

                RaiseEvent Closed(Me)

                _myThread.Abort()
                _myThread = Nothing

            End If

            'start a new thread
            _myThread = New Thread(AddressOf ShowWindow)
            _myThread.SetApartmentState(ApartmentState.STA)
            _myThread.Start()

        End Sub

        ''' <summary>
        ''' method terminating the thread
        ''' </summary>
        Public Sub Close()

            If Not IsNothing(_myThread) Then

                RaiseEvent Closed(Me)

                _myThread.Abort()

            End If

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method showing up the GUI initially
        ''' </summary>
        Private Sub ShowWindow()

            'create a new view instance
            _myView = DirectCast(Activator.CreateInstance(GetType(T)), Window)
            _myView.DataContext = Me
            _myView.Show()

            AddHandler _myView.Closed, AddressOf OnWindowClosed

            Dispatcher.Run()

        End Sub

        ''' <summary>
        ''' event handler for method on GUI window closed 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub OnWindowClosed(sender, e)

            RaiseEvent Closed(Me)

            _myView.Dispatcher.InvokeShutdown()

        End Sub

#End Region

    End Class

End Namespace
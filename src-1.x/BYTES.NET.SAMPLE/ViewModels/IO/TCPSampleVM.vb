'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.IO.TCP
Imports BYTES.NET.Logging
Imports BYTES.NET.WPF.MVVM

'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.IO

Namespace ViewModels.IO

    Public Class TCPSampleVM

        Inherits SampleVM

#Region "private avriable(s)"

        Private _myView As TCPSampleView = Nothing

        Private _server As Transmitter = New Transmitter
        Private _client As Transmitter = New Transmitter

        Private _log As Log = New Log

        Private _targetAddress As String = "127.0.0.1"
        Private _targetPort As Integer = 8080

        Private _sourcePort As Integer = 5555

        Private _message As String = "Hello World!"

#End Region

#Region "public properties inherited from base-class"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "TCP Communication"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New TCPSampleView
                    _myView.DataContext = Me

                End If

                Return _myView

            End Get
            Set(value As UserControl)

                _myView = value
                _myView.DataContext = Me

                OnPropertyChanged()

            End Set
        End Property

#End Region

#Region "public properties"

        Public ReadOnly Property Log As LogEntry()
            Get

                Return _log.Cache
            End Get
        End Property

        Public ReadOnly Property Listeners As Integer()
            Get

                Return _server.Listeners
            End Get
        End Property

        Public Property TargetAddress As String
            Get

                Return _targetAddress

            End Get
            Set(value As String)

                _targetAddress = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property TargetPort As Integer
            Get

                Return _targetPort

            End Get
            Set(value As Integer)

                _targetPort = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property SourcePort As Integer
            Get

                Return _sourcePort

            End Get
            Set(value As Integer)

                _sourcePort = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property Message As String
            Get

                Return _message

            End Get
            Set(value As String)

                _message = value
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

            'add the event handler(s)
            AddHandler _log.Logged, AddressOf HandleLogUpdated

            AddHandler _server.Logged, AddressOf HandleTransmitterLogged
            AddHandler _client.Logged, AddressOf HandleTransmitterLogged

            AddHandler _server.MessageReceived, AddressOf HandleMessageReceived
            AddHandler _client.MessageSent, AddressOf HandleMessageSent

            'add the command(s)
            Me.Commands.Add("AddListenerCmd", New ViewModelRelayCommand(New Action(AddressOf AddListener)))
            Me.Commands.Add("SendMsgCmd", New ViewModelRelayCommand(New Action(AddressOf SendMessage)))

        End Sub

#End Region

#Region "private method(s)"

        Private Sub HandleLogUpdated(ByRef entry As LogEntry)

            OnPropertyChanged("Log")

        End Sub

        Private Sub HandleTransmitterLogged(ByVal message As String)

            _log.Write(message)

        End Sub

        Private Sub HandleMessageReceived(ByRef message As Message)

            Dim text As String = "Message received from " & message.Meta("SourceAddress").ToString & ":" & message.Meta("SourcePort").ToString & " at " & message.Meta("Timestamp").ToString & vbNewLine & vbNewLine
            text &= Channel.GetStringData(message.Data, message.Meta("MessageBytes"))

            _log.Write("*****" & vbNewLine & text & vbNewLine & "*****")
            OnPropertyChanged("Log")

        End Sub

        Private Sub HandleMessageSent(ByRef message As Message)

            Dim text As String = "Message sent to " & message.Meta("TargetAddress").ToString & ":" & message.Meta("TargetPort").ToString & " at " & message.Meta("Timestamp").ToString & vbNewLine & vbNewLine
            text &= Channel.GetStringData(message.Data)

            _log.Write("*****" & vbNewLine & text & vbNewLine & "*****")
            OnPropertyChanged("Log")

        End Sub

        Private Sub AddListener()

            _server.StartListening(Me.TargetPort)
            OnPropertyChanged("Listeners")

        End Sub

        Private Sub SendMessage()

            _client.Send(Me.TargetAddress, Me.TargetPort, Channel.ParseStringData(Me.Message), Me.SourcePort)
            OnPropertyChanged("Listeners")

        End Sub

#End Region

    End Class

End Namespace
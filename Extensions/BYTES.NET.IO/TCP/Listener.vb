'import .net namespace(s) required
Imports System.Net
Imports System.Net.Sockets

Imports System.Threading

Namespace TCP

    Public Class Listener

        Inherits TransmissionBase

#Region "public events"

        Public Event InboundReceived(ByRef channel As Channel)

#End Region

#Region "private variable(s)"

        Private _port As Integer = Nothing

        Private _listener As TcpListener = Nothing
        Private _listen As Boolean = True

        Private _thread As Thread = Nothing

#End Region

#Region "public properties inherited from base-class instance"

        Public Overrides ReadOnly Property ID As String
            Get

                Return "Listener on port " & Me.Port
            End Get
        End Property

#End Region

#Region "public properties"

        Public ReadOnly Property Port As Integer
            Get

                Return _port

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="port"></param>
        Public Sub New(ByVal port As Integer)

            'create a new base-class instance
            MyBase.New

            'set the variable(s)
            _port = port

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method for starting port listening
        ''' </summary>
        ''' <returns></returns>
        Public Function Start() As Boolean

            'check for an existing listener
            If Not IsNothing(_listener) Then

                Log("INFO", "Port listening already started")

                Return False

            End If

            Try

                'create a new listener
                _listener = New TcpListener(New IPEndPoint(IPAddress.Any, _port))

                'create and start the inbound waiting thread
                _thread = New Thread(AddressOf Listen)
                _thread.IsBackground = True
                _thread.Start()

                'return the output value
                Log("INFO", "Port listening started")

                Return True

            Catch ex As Exception

                Log("EXCEPTION", "Starting port listening failed: " & ex.Message)

                Return False

            End Try

        End Function

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method listening to incomming transmissions
        ''' </summary>
        Private Sub Listen()

            _listener.Start() 'start the tcp listener

            While _listen 'do an endless loop

                Dim inbound As Channel = New Channel(_listener.AcceptTcpClient, True)

                AddHandler inbound.Logged, AddressOf Log

                Log("INFO", "New inbound received from '" & inbound.RemoteAddress.ToString & ":" & inbound.RemotePort.ToString & "'")

                RaiseEvent InboundReceived(inbound)

            End While

        End Sub

#End Region

    End Class

End Namespace
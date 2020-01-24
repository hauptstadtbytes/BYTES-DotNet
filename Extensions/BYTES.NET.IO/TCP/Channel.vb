'import .net namespace(s) required
Imports System.Net
Imports System.Net.Sockets

Imports System.Threading
Imports System.Text

Namespace TCP

    Public Class Channel

        Inherits TransmissionBase

#Region "public events"

        Public Event MessageReceived(ByRef message As Message, ByRef channel As Channel)

#End Region

#Region "private variable(s)"

        Private _client As TcpClient = Nothing

        Private _connected As DateTime = Nothing
        Private _released As DateTime = Nothing

        Private _waitingThread As Thread = Nothing

        Private _isListening As Boolean = True

#End Region

#Region "public properties inherited from base-class instance"

        Public Overrides ReadOnly Property ID As String
            Get

                Return "Channel to " & Me.RemoteAddress.ToString & ":" & Me.RemotePort.ToString

            End Get
        End Property

#End Region

#Region "public properties"

        Public ReadOnly Property RemoteAddress As IPAddress
            Get

                Return DirectCast(_client.Client.RemoteEndPoint, IPEndPoint).Address()

            End Get
        End Property

        Public ReadOnly Property RemotePort As Integer
            Get

                Return DirectCast(_client.Client.RemoteEndPoint, IPEndPoint).Port()

            End Get
        End Property

        Public ReadOnly Property RemoteHost As String
            Get

                Return Dns.GetHostEntry(Me.RemoteAddress).HostName

            End Get
        End Property

        Public ReadOnly Property LocalPort As Integer
            Get

                Return DirectCast(_client.Client.LocalEndPoint, IPEndPoint).Port()
            End Get
        End Property

        Public ReadOnly Property Connected As DateTime
            Get

                Return _connected

            End Get
        End Property

        Public ReadOnly Property Released As DateTime
            Get

                Return _released

            End Get
        End Property

        Public Property IsListening As Boolean
            Get

                Return _isListening

            End Get
            Set(value As Boolean)

                _isListening = value

                If Not _isListening Then

                    _released = DateTime.Now

                End If

            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="remoteHost"></param>
        ''' <param name="remotePort"></param>
        Public Sub New(ByVal remoteHost As String, ByVal remotePort As Integer, Optional ByVal localPort As Integer = Nothing, Optional ByVal waitforMessages As Boolean = True)

            'create a new base-class instance
            MyBase.New

            'create a new tcp client
            If IsNothing(localPort) Then

                _client = New TcpClient

            Else

                _client = New TcpClient(New IPEndPoint(IPAddress.Any, localPort))

            End If

            _client.Connect(Dns.GetHostAddresses(remoteHost).First, remotePort)

            'start waiting for for incomming messages
            If waitforMessages Then

                AcceptInboundMessages()

            End If

        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="client"></param>
        ''' <param name="waitforMessages"></param>
        Public Sub New(ByRef client As TcpClient, Optional ByVal waitforMessages As Boolean = True)

            'create a new base-class instance
            MyBase.New

            'set the variable(s)
            _client = client

            'start waiting for for incomming messages
            If waitforMessages Then

                AcceptInboundMessages()

            End If

        End Sub

#End Region

#Region "shared method(s)"

        ''' <summary>
        ''' method parsing an input string to byte data
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        Shared Function ParseStringData(ByVal data As String) As Byte()

            Dim encoder As New ASCIIEncoding()
            Return encoder.GetBytes(data)

        End Function

        ''' <summary>
        ''' method parsing byte data to string
        ''' </summary>
        ''' <param name="data"></param>
        ''' <param name="length"></param>
        ''' <returns></returns>
        Shared Function GetStringData(ByVal data As Byte(), ByVal length As Integer) As String

            Dim encoder As ASCIIEncoding = New ASCIIEncoding
            Return encoder.GetString(data, 0, length)

        End Function

        Shared Function GetStringData(ByVal data As Byte()) As String

            Dim encoder As ASCIIEncoding = New ASCIIEncoding
            Return encoder.GetString(data)

        End Function

#End Region

#Region "public method(s) inherited from base-class instance"

        ''' <summary>
        ''' oervriding for the object default 'equals' method
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        Public Overrides Function Equals(obj As Object) As Boolean

            If obj.GetType = GetType(String) Then

                Dim arg As String = DirectCast(obj, String)

                If arg.Contains(":") Then

                    Dim args As String() = arg.Split(":")

                    For Each address As IPAddress In Dns.GetHostAddresses(args(0))

                        If Me.RemoteAddress.Equals(address) Then

                            If CInt(args(1)) = Me.RemotePort Then

                                Return True

                            End If

                        End If

                    Next

                    Return False

                Else

                    Return Me.RemoteAddress.Equals(arg)

                End If

            Else

                Return MyBase.Equals(obj)

            End If

        End Function

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method starting to wait for incomming messages
        ''' </summary>
        Public Sub AcceptInboundMessages()

            If IsNothing(_waitingThread) Then

                Me.IsListening = True

                _waitingThread = New Thread(AddressOf WaitForIncommingMessages)
                _waitingThread.IsBackground = True
                _waitingThread.Start()

            End If

        End Sub

        ''' <summary>
        ''' method removing the inbound listening waiting thread
        ''' </summary>
        Public Sub IgnoreInboundgMessages()

            If Not IsNothing(_waitingThread) Then

                Me.IsListening = False

                _waitingThread = Nothing

            End If

        End Sub

        ''' <summary>
        ''' method for sending data
        ''' </summary>
        ''' <param name="data"></param>
        Public Sub Send(ByRef data As Byte())

            'send the data
            Dim stream As NetworkStream = _client.GetStream

            stream.Write(data, 0, data.Length)
            stream.Flush()

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method waiting for inbound messages
        ''' </summary>
        Private Sub WaitForIncommingMessages()

            Try

                'get the network stream
                Dim stream As NetworkStream = _client.GetStream

                'read the incomming message(s)
                Dim buffer(4096) As Byte
                Dim bytesRead As Integer

                Log("DEBUG", "Start waiting for incomming messages")

                While Me.IsListening

                    'read the data from network stream
                    bytesRead = 0

                    Try

                        bytesRead = stream.Read(buffer, 0, buffer.Length)

                    Catch ex As Exception

                        'a socket error has occured
                        Log("Exception", "Failed to read network stream")

                        Me.IsListening = False

                    End Try

                    'the client has disconnected
                    If bytesRead = 0 Then

                        Log("INFO", "Connection closed")

                        Me.IsListening = False

                    End If

                    'throw a 'message received' event
                    Dim meta As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

                    With meta
                        .Add("Timestamp", DateTime.Now)
                        .Add("SourceAddress", Me.RemoteAddress)
                        .Add("SourcePort", Me.RemotePort)
                        .Add("TargetAddress", "localhost")
                        .Add("TargetPort", Me.LocalPort)
                        .Add("MessageBytes", bytesRead)
                    End With

                    Log("INFO", "Message received")

                    RaiseEvent MessageReceived(New Message(buffer, meta), Me)

                End While

                Log("DEBUG", "Waiting for incomming messages finalized")

            Catch ex As Exception

                'a general error has occured
                Log("Exception", "Failed to read network stream: " & ex.Message)

            End Try

        End Sub

#End Region

    End Class

End Namespace
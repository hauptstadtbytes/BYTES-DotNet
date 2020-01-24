Namespace TCP

    Public Class Transmitter

        Inherits TransmissionBase

#Region "public events"

        Public Event MessageReceived(ByRef message As Message)
        Public Event MessageSent(ByRef message As Message)

#End Region

#Region "private variable(s)"

        Private _id As String = Nothing
        Private _listeners As Dictionary(Of Integer, Listener) = New Dictionary(Of Integer, Listener)

        Private _inbounds As List(Of Channel) = New List(Of Channel)
        Private _outbounds As Dictionary(Of Integer, List(Of Channel)) = New Dictionary(Of Integer, List(Of Channel))

#End Region

#Region "public properties inherited from base-class instance"

        Public Overrides ReadOnly Property ID As String
            Get

                Return "Transmitter " & _id

            End Get
        End Property

#End Region

#Region "public properties"

        Public ReadOnly Property Listeners As Integer()
            Get

                Return _listeners.Keys.ToArray

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()

            'create a new base-class instance
            MyBase.New

            'set the variable(s)
            Dim id As Guid = Guid.NewGuid
            _id = id.ToString

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method adding a new inbound port listener
        ''' </summary>
        ''' <param name="port"></param>
        ''' <returns></returns>
        Public Function StartListening(ByVal port As Integer) As Boolean

            If _listeners.ContainsKey(port) Then

                Log("INFO", "Listening on port '" & port & "' already started")

                Return True

            End If

            Try

                'create and start a new inbound listener
                Dim listener As Listener = New Listener(port)

                AddHandler listener.Logged, AddressOf Log
                AddHandler listener.InboundReceived, AddressOf HandleInboundReceived

                _listeners.Add(port, listener)
                listener.Start()

                'return the output value
                Log("INFO", "Listening on port '" & port & "' started")

                Return True

            Catch ex As Exception

                Log("EXCEPTION", "Failed to start listening to port '" & port & "': " & ex.Message)

                Return False

            End Try

        End Function

        ''' <summary>
        ''' method for sending data to a remote host 
        ''' </summary>
        ''' <param name="remoteHost"></param>
        ''' <param name="remotePort"></param>
        ''' <param name="data"></param>
        ''' <param name="localPort"></param>
        ''' <returns></returns>
        Public Function Send(ByVal remoteHost As String, ByVal remotePort As Integer, ByRef data As Byte(), Optional ByVal localPort As Integer = Nothing)

            Dim channel As Channel = GetOutboundChannel(remoteHost, remotePort, localPort)

            If IsNothing(channel) Then

                Log("Exception", "Failed to send message: Unable to establish outgoing connection")

                Return False

            End If

            Try

                'send the message data
                channel.Send(data)

                'add a log entry
                Log("INFO", "Message of length '" & data.Length.ToString & "' sent")

                'throw and event
                Dim meta As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

                With meta
                    .Add("Timestamp", DateTime.Now)
                    .Add("SourceAddress", "localhost")
                    .Add("SourcePort", channel.LocalPort)
                    .Add("TargetAddress", channel.RemoteAddress)
                    .Add("TargetPort", channel.RemotePort)
                End With

                RaiseEvent MessageSent(New Message(data, meta))

                'return the output value
                Return True

            Catch ex As Exception

                Log("Exception", "Failed to send message: " & ex.Message)

                Return False

            End Try

        End Function

        ''' <summary>
        ''' method resetting the transmitter
        ''' </summary>
        Public Sub Reset()

            _listeners.Clear()

            For Each inbound As Channel In _inbounds

                inbound.IsListening = False

            Next

            _inbounds.Clear()

            _outbounds.Clear()

            'add a log entry
            Log("INFO", "Transmitter reset")

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method handling a listener's 'InboundReceived' event
        ''' </summary>
        ''' <param name="channel"></param>
        Private Sub HandleInboundReceived(ByRef channel As Channel)

            AddHandler channel.MessageReceived, AddressOf HandleMessageReceived

            _inbounds.Add(channel)

        End Sub

        ''' <summary>
        ''' method handling an incomming message
        ''' </summary>
        ''' <param name="message"></param>
        Private Sub HandleMessageReceived(ByRef message As Message, ByRef channel As Channel)

            'raise the event
            RaiseEvent MessageReceived(message)

        End Sub

        ''' <summary>
        ''' method getting an outbound connection channel (from list of existing channels or creating a new one) 
        ''' </summary>
        ''' <param name="remoteHost"></param>
        ''' <param name="remotePort"></param>
        ''' <param name="localPort"></param>
        ''' <returns></returns>
        Private Function GetOutboundChannel(ByVal remoteHost As String, ByVal remotePort As Integer, Optional ByVal localPort As Integer = Nothing) As Channel

            Try

                'parse the argument(a)
                If IsNothing(localPort) Then

                    localPort = -1

                End If

                'check for an existing channel
                If _outbounds.ContainsKey(localPort) Then

                    For Each channel As Channel In _outbounds(localPort)

                        If channel.Equals(remoteHost & ":" & remotePort) Then

                            Return channel

                        End If

                    Next

                End If

                'create a new channel
                Dim output As Channel

                If localPort = -1 Then

                    output = New Channel(remoteHost, remotePort, Nothing, False)

                Else

                    output = New Channel(remoteHost, remotePort, localPort, False)

                End If

                AddHandler output.Logged, AddressOf Log

                'add the channel to the list of existing connections
                If Not _outbounds.ContainsKey(output.LocalPort) Then

                    _outbounds.Add(output.LocalPort, New List(Of Channel))

                End If

                _outbounds(output.LocalPort).Add(output)

                Log("INFO", "Outbound connection to '" & remoteHost & ":" & remotePort & "' via local port '" & output.LocalPort & "' created")

                'return the output value
                Return output

            Catch ex As Exception

                Log("Exception", "Failed to create outbound connection to '" & remoteHost & ":" & remotePort & "' via local port '" & localPort & "': " & ex.Message)

                Return Nothing

            End Try

        End Function

#End Region

    End Class

End Namespace
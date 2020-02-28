'import internal namespace(s) required
Imports BYTES.NET.Logging.API

Namespace Logging

    Public Class Log

#Region "public event(s)"

        Public Event Logged(ByRef entry As LogEntry)

#End Region

#Region "private variable(s)"

        Private _cache As List(Of LogEntry) = New List(Of LogEntry)

        Private _cacheLimit As Integer = 100
        Private _threshold As LogEntry.InformationLevel = LogEntry.InformationLevel.Info

        Private _appenders As List(Of ILogAppender) = New List(Of ILogAppender)

#End Region

#Region "public properties"

        Public ReadOnly Property Cache As LogEntry()
            Get

                Return _cache.ToArray

            End Get
        End Property

        ''' <summary>
        ''' The number of log entries cached in-memory
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>'Nothing' means 'cache all/ no limit'; by default set to 100</remarks>
        Public Property CacheLimit As Integer
            Get

                Return _cacheLimit

            End Get
            Set(value As Integer)

                _cacheLimit = value

            End Set
        End Property

        ''' <summary>
        ''' The minimum information level for a log entry to be added
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>by default set to 'Info'</remarks>
        Public Property InformationThreshold As LogEntry.InformationLevel
            Get

                Return _threshold

            End Get
            Set(value As LogEntry.InformationLevel)

                _threshold = value

            End Set
        End Property

        Public ReadOnly Property Appenders As List(Of ILogAppender)
            Get

                Return _appenders

            End Get
        End Property

#End Region

#Region "shared method(s)"

        ''' <summary>
        ''' method validating a log entry by threshold
        ''' </summary>
        ''' <param name="entry"></param>
        ''' <param name="threshold"></param>
        ''' <returns></returns>
        Shared Function ValidateByThreshold(ByRef entry As LogEntry, ByVal threshold As LogEntry.InformationLevel)

            ' DEBUG messages will always be written
            If threshold = LogEntry.InformationLevel.Debug Then

                Return True

            End If

            'validate by threshold and entry
            Select Case True

                Case ((threshold = LogEntry.InformationLevel.Info) And (Not (entry.Level = LogEntry.InformationLevel.Debug)))
                    Return True

                Case ((threshold = LogEntry.InformationLevel.Warning) And (Not (entry.Level = LogEntry.InformationLevel.Debug)) And (Not (entry.Level = LogEntry.InformationLevel.Info)))
                    Return True

                Case (threshold = LogEntry.InformationLevel.Exception And (entry.Level = LogEntry.InformationLevel.Exception OrElse entry.Level = LogEntry.InformationLevel.Fatal))
                    Return True

                Case ((threshold = LogEntry.InformationLevel.Fatal) And (entry.Level = LogEntry.InformationLevel.Fatal))
                    Return True

            End Select

            'return the default output value
            Return False

        End Function

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()
        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method for adding an 'ILogAppender' instance
        ''' </summary>
        ''' <param name="appender"></param>
        ''' <param name="dumpCache"></param>
        Public Sub AddAppender(ByRef appender As ILogAppender, Optional ByVal dumpCache As Boolean = False)

            'add the appenders
            _appenders.Add(appender)

            'call the 'OnAppend' method
            appender.OnAppend(Me, dumpCache)

        End Sub

        ''' <summary>
        ''' default method for writing to the log
        ''' </summary>
        ''' <param name="entry"></param>
        Public Sub Write(ByRef entry As LogEntry)

            If entry.Level >= _threshold Then 'check for the information level

                'add the entry (to the cache)
                If (IsNothing(_cacheLimit) OrElse (_cacheLimit > 0)) Then

                    _cache.Add(entry)

                End If

                'cleanup the cache
                If Not IsNothing(_cacheLimit) Then

                    While _cache.Count > _cacheLimit

                        _cache.RemoveAt(0)

                    End While

                End If

                'write the entry to all appenders registrated
                For Each appender As ILogAppender In Me.Appenders

                    appender.Write(entry)

                Next

                'raise the notification event
                RaiseEvent Logged(entry)

            End If

        End Sub

        ''' <summary>
        ''' overloaded method for log writing
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="level"></param>
        ''' <param name="details"></param>
        Public Sub Write(ByVal message As String, ByVal level As LogEntry.InformationLevel, ByRef details As Object)

            Me.Write(New LogEntry(message, level, details))

        End Sub

        ''' <summary>
        ''' overloaded method for log writing
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="level"></param>
        ''' <remarks>for increasing C# compatibility</remarks>
        Public Sub Write(ByVal message As String, ByVal level As LogEntry.InformationLevel)

            Me.Write(New LogEntry(message, level))

        End Sub

        ''' <summary>
        ''' overloaded method for log writing
        ''' </summary>
        ''' <param name="message"></param>
        ''' <remarks>for increasing C# compatibility</remarks>
        Public Sub Write(ByVal message As String)

            Me.Write(New LogEntry(message))

        End Sub

#End Region

    End Class

End Namespace
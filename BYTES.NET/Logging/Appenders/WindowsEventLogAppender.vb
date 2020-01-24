'import internal namspace(s) required
Imports BYTES.NET.Logging
Imports BYTES.NET.Logging.API

Namespace Logging.Appenders

    <LogAppenderMetadata(ID:="BYTES_LogAppender_WindowsEventLog", Name:="Windows Event Log Appender")>
    Public Class WindowsEventLogAppender

        Implements ILogAppender

#Region "private avriable(s)"

        Private _log As String = Nothing
        Private _source As String = Nothing
        Private _threshold As LogEntry.InformationLevel = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property LogName As String
            Get

                Return _log

            End Get
        End Property

        Public ReadOnly Property Source As String
            Get

                Return _source

            End Get
        End Property

        Public ReadOnly Property Threshold As LogEntry.InformationLevel
            Get

                Return _threshold

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="log"></param>
        ''' <param name="threshold"></param>
        Public Sub New(ByVal source As String, Optional ByVal log As String = "Application", Optional ByVal threshold As LogEntry.InformationLevel = LogEntry.InformationLevel.Info)

            'set the variable(s)
            _source = source
            _log = log
            _threshold = threshold

        End Sub

#End Region

#Region "public method(s) inherited from base-class instance"

        ''' <summary>
        ''' method called on appending a log entry
        ''' </summary>
        ''' <param name="entry"></param>
        Public Sub Write(ByRef entry As LogEntry) Implements ILogAppender.Write

            If Log.ValidateByThreshold(entry, _threshold) Then 'check for the threshold

                'parse the event log entry type
                Dim type As EventLogEntryType = EventLogEntryType.Information

                Select Case True

                    Case (entry.Level = LogEntry.InformationLevel.Debug OrElse entry.Level = LogEntry.InformationLevel.Info)
                        ' do nothing since 'Information' is the default type

                    Case entry.Level = LogEntry.InformationLevel.Warning
                        type = EventLogEntryType.Warning

                    Case (entry.Level = LogEntry.InformationLevel.Exception OrElse entry.Level = LogEntry.InformationLevel.Fatal)
                        type = EventLogEntryType.Error

                End Select

                'parse the message
                Dim msg As String = entry.Message

                If Not IsNothing(entry.Details) Then

                    If entry.Details.GetType.Equals(GetType(Exception)) Then

                        Dim details As Exception = DirectCast(entry.Details, Exception)

                        msg &= vbNewLine & details.Message & vbNewLine & details.StackTrace

                    End If

                End If

                'write the log entry
                WriteToEventLog(msg, type)

            End If

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method writing to the Windows event log
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="type"></param>
        Private Sub WriteToEventLog(ByVal message As String, ByVal type As EventLogEntryType)

            'create the source (if required)
            If Not EventLog.SourceExists(_source) Then

                EventLog.CreateEventSource(_source, _log)

            End If

            'add the log entry
            Dim myLog As EventLog = New EventLog(_log) With {.Source = _source}
            myLog.WriteEntry(message, type)

        End Sub

#End Region

    End Class

End Namespace
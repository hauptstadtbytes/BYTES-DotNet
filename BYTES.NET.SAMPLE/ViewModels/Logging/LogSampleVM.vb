'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.WPF.MVVM

Imports BYTES.NET.Logging
Imports BYTES.NET.Logging.Appenders

'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.Logging

Namespace ViewModels.Logging

    Public Class LogSampleVM

        Inherits SampleVM

#Region "private variable(s)"

        Private _myView As LogSampleView = Nothing

        Private _log As Log = New Log

        Private _newEntryLevel As LogEntry.InformationLevel = LogEntry.InformationLevel.Warning
        Private _newEntryMessage As String = Nothing

        Private _logFilePath As String = "D:\BYTES.NET.SAMPLE\MyLog.LOG"
        Private _eventLogsource As String = "BYTES.NET.SAMPLE"

#End Region

#Region "public properties inherited from base-class instance"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "Logging"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New LogSampleView
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

        Public ReadOnly Property Levels As LogEntry.InformationLevel()
            Get

                Return {LogEntry.InformationLevel.Info, LogEntry.InformationLevel.Warning, LogEntry.InformationLevel.Exception}

            End Get
        End Property

        Public Property Level As LogEntry.InformationLevel
            Get

                Return _newEntryLevel

            End Get
            Set(value As LogEntry.InformationLevel)

                _newEntryLevel = value
                OnPropertyChanged()

            End Set
        End Property

        Public ReadOnly Property Entries As LogEntry()
            Get

                Return _log.Cache

            End Get
        End Property

        Public ReadOnly Property LastEntry As LogEntry
            Get

                Return _log.Cache.Last

            End Get
        End Property

        Public Property Message As String
            Get

                Return _newEntryMessage

            End Get
            Set(value As String)

                _newEntryMessage = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property LogFilePath As String
            Get

                Return _logFilePath

            End Get
            Set(value As String)

                _logFilePath = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property EventLogSource As String
            Get

                Return _eventLogsource

            End Get
            Set(value As String)

                _eventLogsource = value
                OnPropertyChanged()

            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        Public Sub New()

            MyBase.New

            AddHandler _log.Logged, AddressOf HandleLogged

            _log.Write("Initialized", LogEntry.InformationLevel.Info)

            Me.Commands.Add("WriteToLogCmd", New ViewModelRelayCommand(New Action(AddressOf WriteEntry)))
            Me.Commands.Add("AppendLogFileCmd", New ViewModelRelayCommand(New Action(AddressOf AppendLogFile)))
            Me.Commands.Add("AppendEventLogCmd", New ViewModelRelayCommand(New Action(AddressOf AppendEventLog)))

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method handling the log's 'Logged' event
        ''' </summary>
        ''' <param name="entry"></param>
        Private Sub HandleLogged(ByRef entry As LogEntry)

            OnPropertyChanged("Entries")
            OnPropertyChanged("LastEntry")

        End Sub

        ''' <summary>
        ''' method for writing a log entry
        ''' </summary>
        Private Sub WriteEntry()

            'validate the message
            If IsNothing(_newEntryMessage) OrElse String.IsNullOrEmpty(_newEntryMessage) Then

                MsgBox("The message (text) must not be empty")

                Exit Sub

            End If

            'write to the log
            _log.Write(_newEntryMessage, _newEntryLevel)

        End Sub

        ''' <summary>
        ''' method for appending a log file
        ''' </summary>
        Private Sub AppendLogFile()

            'validate the file path
            If IsNothing(_logFilePath) OrElse String.IsNullOrEmpty(_logFilePath) Then

                MsgBox("The log file path must not be empty")

                Exit Sub

            End If

            'add the rolling file appender
            '_log.AddAppender(New RollingFileAppender(_logFilePath))

            Dim args As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"FilePath", _logFilePath}}

            Dim fileAppender As RollingFileAppender = New RollingFileAppender()
            fileAppender.Initialize(args)

            _log.AddAppender(fileAppender, True)

            'write a message to the log
            _log.Write("Rolling file appender added for file at '" & _logFilePath & "'", LogEntry.InformationLevel.Info)

        End Sub

        ''' <summary>
        ''' method for appending a log to the Windows Event Log
        ''' </summary>
        Private Sub AppendEventLog()

            'validate the file path
            If IsNothing(_eventLogsource) OrElse String.IsNullOrEmpty(_eventLogsource) Then

                MsgBox("The logging source must not be empty")

                Exit Sub

            End If

            'add the Windows event log instance
            '_log.AddAppender(New WindowsEventLogAppender(_eventLogsource))

            Dim args As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"Source", _eventLogsource}, {"Threshold", "Debug"}}

            Dim eventLogAppender As WindowsEventLogAppender = New WindowsEventLogAppender
            eventLogAppender.Initialize(args)

            _log.AddAppender(eventLogAppender)

            'write a message to the log
            _log.Write("Windows event log appender added, using source '" & _eventLogsource & "'", LogEntry.InformationLevel.Info)

        End Sub

#End Region

    End Class

End Namespace
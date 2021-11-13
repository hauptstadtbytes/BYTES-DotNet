'import .net namespace(s) registered
Imports System.IO

'import namspace(s) required from 'BYTES.NET' library
Imports BYTES.NET.Logging.API

'import namespace(s) required from 'log4net' library
Imports log4net
Imports log4net.Repository.Hierarchy
Imports log4net.Layout
Imports log4net.Core

Namespace Appenders

    <LogAppenderMetadata(ID:="BYTES_LogAppender_RollingFile", Name:="Rolling File Log Appender")>
    Public Class RollingFileAppender

        Implements ILogAppender

#Region "private variable(s)"

        Private _filePath As String = Nothing

        Private _logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Private _loggingPattern As String = "%date %level [%thread] - %message%newline"
        Private _maxFileSize As String = "5MB"
        Private _maxBackupsCount As Integer = 3

#End Region

#Region "public properties"

        Public ReadOnly Property FilePath As String
            Get

                Return _filePath

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="filePath"></param>
        ''' <param name="pattern"></param>
        ''' <param name="maxFileSize"></param>
        ''' <param name="maxBackupCounts"></param>
        Public Sub New(ByVal filePath As String, Optional ByVal pattern As String = Nothing, Optional ByVal maxFileSize As String = Nothing, Optional ByVal maxBackupCounts As Integer = Nothing)

            'create a new base-class instance
            MyBase.New

            'set the variable(s)
            _filePath = filePath

            If Not IsNothing(pattern) Then
                _loggingPattern = pattern
            End If

            If Not IsNothing(maxFileSize) Then
                _maxFileSize = maxFileSize
            End If

            If Not IsNothing(maxBackupCounts) Then
                _maxBackupsCount = maxBackupCounts
            End If

            'initialize 'log4net'
            ConfigureLog4Net()

        End Sub

        ''' <summary>
        ''' overloaded constructor, supporting parameterless construction
        ''' </summary>
        Public Sub New()
        End Sub

#End Region

#Region "public method(s) inherited from base-class instance"

        ''' <summary>
        ''' method initializing the appender
        ''' </summary>
        ''' <param name="parameters"></param>
        Public Sub Initialize(parameters As Dictionary(Of String, String)) Implements ILogAppender.Initialize

            'parse the parameters
            For Each param As KeyValuePair(Of String, String) In parameters

                Select Case param.Key.ToLower

                    Case "filepath"
                        _filePath = param.Value

                    Case "pattern"
                        _loggingPattern = param.Value

                    Case "maxfilesize"
                        _maxFileSize = param.Value

                    Case "maxbackupscount"
                        Try

                            _maxBackupsCount = CInt(param.Value)

                        Catch ex As Exception

                            Throw New ArgumentException("Unable to parse '" & param.Value & "' to '" & GetType(Integer).ToString & "' for maximum backups count")

                        End Try

                End Select

            Next

            'initialize 'log4net'
            ConfigureLog4Net()

        End Sub

        ''' <summary>
        ''' method called on appending the appender to the parent log
        ''' </summary>
        ''' <param name="parent"></param>
        ''' <param name="dumpCache"></param>
        Public Sub OnAppend(ByRef parent As Log, ByVal dumpCache As Boolean) Implements ILogAppender.OnAppend

            'dump the cache
            If dumpCache Then

                _logger.Info("### Starting Log Cache Dump ###")

                For Each entry As LogEntry In parent.Cache

                    Write(entry)

                Next

                _logger.Info("### Cache Dump Finished ###")

            End If

        End Sub

        ''' <summary>
        ''' method called on writing a log entry
        ''' </summary>
        ''' <param name="entry"></param>
        Public Sub Write(ByRef entry As LogEntry) Implements ILogAppender.Write

            Select Case (entry.Level)

                Case LogEntry.InformationLevel.Debug
                    _logger.Debug(entry.Message, entry.Details)

                Case LogEntry.InformationLevel.Info
                    _logger.Info(entry.Message, entry.Details)

                Case LogEntry.InformationLevel.Warning
                    _logger.Warn(entry.Message, entry.Details)

                Case LogEntry.InformationLevel.Exception
                    _logger.Error(entry.Message, entry.Details)

                Case LogEntry.InformationLevel.Fatal
                    _logger.Fatal(entry.Message, entry.Details)

            End Select

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method for configuring the 'log4net''RollingFileAppender' at runtime
        ''' </summary>
        ''' <remarks>based on the article found at 'https://stackoverflow.com/questions/16336917/can-you-configure-log4net-in-code-instead-of-using-a-config-file'</remarks>
        Private Sub ConfigureLog4Net()

            'validate the file path
            If IsNothing(_filePath) OrElse String.IsNullOrEmpty(_filePath) OrElse String.IsNullOrWhiteSpace(_filePath) Then

                Throw New ArgumentException("The file path must not be empty")

            End If

            'check for the output folder (or create)
            Dim dirPath As String = _filePath.Substring(0, _filePath.LastIndexOf("\"))

            If Not Directory.Exists(dirPath) Then

                Directory.CreateDirectory(dirPath)

            End If

            'create the layout pattern
            Dim patternLayout As New PatternLayout()
            patternLayout.ConversionPattern = _loggingPattern
            patternLayout.ActivateOptions()

            'enable the rolling file appender
            Dim roller As Appender.RollingFileAppender = New Appender.RollingFileAppender()

            With roller
                .Layout = patternLayout

                .AppendToFile = True
                .File = _filePath
                .StaticLogFileName = True

                .RollingStyle = Appender.RollingFileAppender.RollingMode.Size
                .MaxSizeRollBackups = _maxBackupsCount
                .MaximumFileSize = _maxFileSize
            End With

            roller.ActivateOptions()

            'add the appender and configure the 'log4net' hirarchy
            Dim hierarchy As Hierarchy = DirectCast(LogManager.GetRepository(), Hierarchy)

            hierarchy.Root.AddAppender(roller)
            hierarchy.Root.Level = Level.All
            hierarchy.Configured = True

        End Sub

#End Region

    End Class

End Namespace
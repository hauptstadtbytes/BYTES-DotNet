Namespace Logging.API

    Public Interface ILogAppender

        Sub Initialize(ByVal parameters As Dictionary(Of String, String))

        Sub OnAppend(ByRef parent As Log, ByVal dumpCache As Boolean)

        Sub Write(ByRef entry As LogEntry)

    End Interface

End Namespace
'import .net namespace(s) required
Imports System.IO

'import internal namespace(s) required
Imports BYTES.NET.Logging

Namespace IO

    Public Class ManagedProcess

#Region "public event(s)"

        Public Event OutputReceived(data As LogEntry)
        Public Event Exited(exitCode As Integer)

#End Region

#Region "private variable(s)"

        Private _exe As String = String.Empty

        Private _args As SortedDictionary(Of Integer, String) = New SortedDictionary(Of Integer, String)

        Private _process As Process = Nothing
        Private _showUI As Boolean = False

#End Region

#Region "public properties"

        Public Property Executable As String
            Get

                Return _exe

            End Get
            Set(value As String)

                _exe = value

            End Set
        End Property

        Public Property Arguments As SortedDictionary(Of Integer, String)
            Get

                Return _args

            End Get
            Set(value As SortedDictionary(Of Integer, String))

                _args = value

            End Set
        End Property

        Public Property ShowUI As Integer
            Get

                Return _showUI

            End Get
            Set(value As Integer)

                _showUI = value

            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method(s)
        ''' </summary>
        Public Sub New()
        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method running the process sync
        ''' </summary>
        Public Function Run(Optional ByVal timeout As Integer = Nothing) As Integer

            _process = GetProcess()

            If IsNothing(_process) Then
                Return -1
            End If

            RunProcess(timeout)

            Return 0

        End Function

        ''' <summary>
        ''' method running the process async
        ''' </summary>
        ''' <returns></returns>
        Public Async Function RunAsync() As Task(Of Integer)

            _process = GetProcess()

            If IsNothing(_process) Then
                Return -1
            End If

            Return Await RunProcessAsync().ConfigureAwait(False)

        End Function

        Public Sub Terminate()

            If Not IsNothing(_process) Then

                _process.Kill()

            End If

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method returning a pre-configured process instance
        ''' </summary>
        ''' <returns></returns>
        Private Function GetProcess() As Process

            'validate the executable path
            If IsNothing(_exe) OrElse String.IsNullOrEmpty(_exe) OrElse Not File.Exists(_exe) Then

                CreateOutput("Unable to find target file at '" & Helper.ExpandPath(_exe) & "'", LogEntry.InformationLevel.Warning)
                Return Nothing

            End If

            'create a new process instance
            Dim myStartInfo As ProcessStartInfo = New ProcessStartInfo(Helper.ExpandPath(_exe))

            With myStartInfo

                .Arguments = Join(_args.Values.ToArray, " ")

                .UseShellExecute = False
                .RedirectStandardOutput = True
                .RedirectStandardError = True

                If Not ShowUI Then
                    .CreateNoWindow = True
                End If

            End With

            _process = New Process
            _process.StartInfo = myStartInfo
            _process.EnableRaisingEvents = True

            'return the output
            Return _process

        End Function

        ''' <summary>
        ''' (internal) method for starting the process and reading output(s) and error(s) synchroniously
        ''' </summary>
        ''' <param name="timeout"></param>
        Private Sub RunProcess(Optional ByVal timeout As Integer = Nothing)

            AddHandler _process.Exited, Sub()
                                            RaiseEvent Exited(_process.ExitCode)
                                        End Sub
            AddHandler _process.OutputDataReceived, AddressOf HandleOutput
            AddHandler _process.ErrorDataReceived, AddressOf HandleError

            'start the process
            _process.Start()

            _process.BeginOutputReadLine()
            _process.BeginErrorReadLine()

            If IsNothing(timeout) Then

                _process.WaitForExit()

            Else

                _process.WaitForExit(timeout)

            End If

        End Sub

        ''' <summary>
        ''' (internal) method for starting the process and reading output(s) and error(s) async
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>based on the article found at 'https://stackoverflow.com/questions/10788982/is-there-any-async-equivalent-of-process-start'</remarks>
        Private Function RunProcessAsync() As Task(Of Integer)

            Dim completionSource As TaskCompletionSource(Of Integer) = New TaskCompletionSource(Of Integer)

            AddHandler _process.Exited, Sub()

                                            completionSource.SetResult(_process.ExitCode)

                                            RaiseEvent Exited(_process.ExitCode)

                                        End Sub

            AddHandler _process.OutputDataReceived, AddressOf HandleOutput

            AddHandler _process.ErrorDataReceived, AddressOf HandleError

            Dim started As Boolean = _process.Start()

            _process.BeginOutputReadLine()
            _process.BeginErrorReadLine()

            Return completionSource.Task

        End Function

        ''' <summary>
        ''' method handling the process-attached output received event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub HandleOutput(sender As Object, e As DataReceivedEventArgs)

            'ignore empty strings
            If String.IsNullOrEmpty(e.Data) Then

                Exit Sub

            End If

            'raise an event on output received
            Dim entry As LogEntry = New LogEntry(e.Data, LogEntry.InformationLevel.Info)
            RaiseEvent OutputReceived(entry)

        End Sub

        ''' <summary>
        ''' method handling process-attached error received event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub HandleError(sender As Object, e As DataReceivedEventArgs)

            'ignore empty strings
            If String.IsNullOrEmpty(e.Data) Then

                Exit Sub

            End If

            'raise an event on output received
            Dim entry As LogEntry = New LogEntry(e.Data, LogEntry.InformationLevel.Warning)
            RaiseEvent OutputReceived(entry)

        End Sub

        ''' <summary>
        ''' method creating a custom output
        ''' </summary>
        ''' <param name="message"></param>
        ''' <param name="level"></param>
        Private Sub CreateOutput(ByVal message As String, Optional ByVal level As LogEntry.InformationLevel = LogEntry.InformationLevel.Debug)

            RaiseEvent OutputReceived(New LogEntry(message, level))

        End Sub

#End Region

    End Class

End Namespace
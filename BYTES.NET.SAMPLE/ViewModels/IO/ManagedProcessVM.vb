'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.IO

'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.IO
Imports BYTES.NET.WPF.MVVM
Imports BYTES.NET.Logging

Namespace ViewModels

    Public Class ManagedProcessVM

        Inherits SampleVM

#Region "private variable(s)"

        Private _examples As List(Of String) = New List(Of String)
        Private _example As String = Nothing

        Private _myView As ManagedProcessView = Nothing

        Private _exe As String = "C:\Windows\System32\cmd.exe"

        Private _args As List(Of String) = New List(Of String)
        Private _arg As String = "/c " & """" & "%InstallationDir%\..\..\..\..\samples\Scripts\LongRunningSampleScript.cmd" & """" & " TestValue"

        Private _buttonTxt As String = "Run"

        Private _process As ManagedProcess = Nothing
        Private _runAsync As Boolean = True
        Private _showGUI As Boolean = True

        Private _log As List(Of LogEntry) = New List(Of LogEntry)

#End Region

#Region "public properties inherited from base-class instance"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "Managed Process"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New ManagedProcessView
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

        Public ReadOnly Property Examples As String()
            Get
                Return _examples.ToArray
            End Get
        End Property

        Public Property Example As String
            Get

                If IsNothing(_example) And Me.Examples.Length > 0 Then

                    _example = Examples.First

                End If

                Return _example

            End Get
            Set(value As String)

                _example = value
                OnPropertyChanged()
                UpdateExample()

            End Set
        End Property

        Public Property Executable As String
            Get

                Return _exe

            End Get
            Set(value As String)

                _exe = value
                OnPropertyChanged()

            End Set
        End Property

        Public ReadOnly Property Arguments As String()
            Get

                Return _args.ToArray()

            End Get
        End Property

        Public Property Argument As String
            Get

                Return _arg

            End Get
            Set(value As String)

                _arg = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property RunAsync As Boolean
            Get

                Return _runAsync

            End Get
            Set(value As Boolean)

                _runAsync = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property ShowUI As Boolean
            Get

                Return _showGUI

            End Get
            Set(value As Boolean)

                _showGUI = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property RunButtonText As String
            Get

                Return _buttonTxt

            End Get
            Set(value As String)

                _buttonTxt = value
                OnPropertyChanged()

            End Set
        End Property

        Public ReadOnly Property Log As LogEntry()
            Get

                Return _log.ToArray

            End Get
        End Property

        Public ReadOnly Property LastLogEntry As LogEntry
            Get

                If Log.Length > 0 Then

                    Return Log.Last

                End If

                Return Nothing

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
            _examples.Add("Windows Batch (*.cmd)")
            _examples.Add("Anaconda Python (*.py)")

            'add the command(s)
            Me.Commands.Add("ToggleRunCmd", New ViewModelRelayCommand(New Action(AddressOf ToggleScriptRun)))
            Me.Commands.Add("AddArgumentCmd", New ViewModelRelayCommand(New Action(AddressOf AddArguent)))
            Me.Commands.Add("ClearArgumentsCmd", New ViewModelRelayCommand(New Action(AddressOf ClearArguments)))

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method updating the properties, based on the example selected
        ''' </summary>
        Private Sub UpdateExample()

            Select Case Me.Example

                Case "Windows Batch (*.cmd)"
                    Me.Executable = "C:\Windows\System32\cmd.exe"
                    Me.Argument = "/c " & """" & "%InstallationDir%..\..\..\..\samples\Scripts\LongRunningSampleScript.cmd" & """" & " TestValue"
                    ClearArguments()

                Case "Anaconda Python (*.py)"
                    Me.Executable = "D:\Application_Data\Git\Net Application Platform\bytes.net\samples\Scripts\Anaconda3\python.exe"
                    Me.Argument = """" & "%InstallationDir%..\..\..\..\samples\Scripts\SampleScript.py" & """" & " TestValue"

            End Select

        End Sub

        ''' <summary>
        ''' method running the script
        ''' </summary>
        Private Async Sub ToggleScriptRun()

            If IsNothing(_process) Then 'run a new process

                'reset the log
                _log = New List(Of LogEntry)
                OnPropertyChanged("Log")
                OnPropertyChanged("LastLogEntry")

                'parse the arguments
                Dim args As SortedDictionary(Of Integer, String) = New SortedDictionary(Of Integer, String)

                Dim counter As Integer = 0
                For Each arg As String In Me.Arguments

                    counter += 1

                    args.Add(counter, Helper.ExpandPath(arg))

                Next

                'setup a new process
                _process = New ManagedProcess

                With _process
                    .Executable = Me.Executable
                    .Arguments = args
                    .ShowUI = Me.ShowUI
                End With

                AddHandler _process.OutputReceived, Sub(data As LogEntry)

                                                        _log.Add(data)
                                                        OnPropertyChanged("Log")
                                                        OnPropertyChanged("LastLogEntry")

                                                    End Sub

                AddHandler _process.Exited, Sub(code As Integer)

                                                MsgBox("Process finished with: " & code)
                                                OnPropertyChanged("Log")
                                                OnPropertyChanged("LastLogEntry")

                                            End Sub

                'update the GUI
                Me.RunButtonText = "Terminate"

                'run the process
                If Me.RunAsync Then

                    Await _process.RunAsync()

                Else

                    _process.Run()

                End If

            Else 'exit a running process

                _process.Terminate()
                _process = Nothing

                'update the GUI
                Me.RunButtonText = "Run"

                MsgBox("Process terminated")

            End If

        End Sub

        ''' <summary>
        ''' method for adding an argument
        ''' </summary>
        Private Sub AddArguent()

            _args.Add(Me.Argument)
            OnPropertyChanged("Arguments")

            'Me.Argument = Nothing

        End Sub

        ''' <summary>
        ''' method clearing the list of arguments
        ''' </summary>
        Private Sub ClearArguments()

            _args = New List(Of String)
            OnPropertyChanged("Arguments")

        End Sub

#End Region

    End Class

End Namespace
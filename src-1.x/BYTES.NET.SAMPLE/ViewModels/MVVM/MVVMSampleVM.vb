'import .net namespace(s) required
Imports System.ComponentModel

'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.WPF.MVVM
Imports BYTES.NET.Collections

'import internal namepsace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.MVVM

Namespace ViewModels.MVVM

    Public Class MVVMSampleVM

        Inherits SampleVM

#Region "private avriable(s)"

        Private _myView As MVVMSampleView = Nothing

        Private _dialogText As String = "This text might be modified by dialog"

        Private _validationAnswer As Integer = Nothing
        Private _validationResults As List(Of String) = New List(Of String)
        Private _validationNestedVM As ValidationVM = New ValidationVM

        Private _conversonSampleString = Nothing
        Private _conversionSampleBoolean = False
        Private _conversionSampleList As List(Of String) = New List(Of String)

        Private _sampleDictionary As ObservableDictionary(Of String, String) = New ObservableDictionary(Of String, String)
        Private _sampleDictionaryItemKey As String = Nothing
        Private _sampleDictionaryItemValue As String = Nothing

        Private _guiThreads As Dictionary(Of Integer, GUIThreadSampleVM) = New Dictionary(Of Integer, GUIThreadSampleVM)

#End Region

#Region "public properties inherited from base-class"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "MVVM"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New MVVMSampleView
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

        Public Property DialogText As String
            Get

                Return _dialogText

            End Get
            Set(value As String)

                _dialogText = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property ValidationAnswer As Integer
            Get

                Return _validationAnswer

            End Get
            Set(value As Integer)

                _validationAnswer = value

                OnPropertyChanged(True)

            End Set
        End Property

        Public ReadOnly Property ValidationResults As String()
            Get

                Return _validationResults.ToArray()

            End Get
        End Property

        Public ReadOnly Property ValidationNestedVM As ValidationVM
            Get

                Return _validationNestedVM

            End Get
        End Property

        Public Property ConversionSampleString As String
            Get

                Return _conversonSampleString

            End Get
            Set(value As String)

                _conversonSampleString = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property ConversionSampleBoolean As Boolean
            Get

                Return _conversionSampleBoolean

            End Get
            Set(value As Boolean)

                _conversionSampleBoolean = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property ConversionSampleList As List(Of String)
            Get

                Return _conversionSampleList

            End Get
            Set(value As List(Of String))

                _conversionSampleList = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property SampleDictionary As ObservableDictionary(Of String, String)
            Get

                Return _sampleDictionary

            End Get
            Set(value As ObservableDictionary(Of String, String))

                _sampleDictionary = value

                OnPropertyChanged()

            End Set
        End Property

        Public Property SampleDictionaryItemKey As String
            Get

                Return _sampleDictionaryItemKey

            End Get
            Set(value As String)

                _sampleDictionaryItemKey = value

                OnPropertyChanged()

            End Set
        End Property

        Public Property SampleDictionaryItemValue As String
            Get

                Return _sampleDictionaryItemValue

            End Get
            Set(value As String)

                _sampleDictionaryItemValue = value

                OnPropertyChanged()

            End Set
        End Property

        Public ReadOnly Property Threads As GUIThreadSampleVM()
            Get

                Return _guiThreads.Values.ToArray

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

            'add the command(s)
            Me.Commands.Add("SayHelloCmd", New ViewModelRelayCommand(New Action(Of Object)(AddressOf SayHello)))
            Me.Commands.Add("OpenDialogCmd", New ViewModelRelayCommand(New Action(AddressOf OpenDialogCmd)))
            Me.Commands.Add("ResetConversionSampleStringCmd", New ViewModelRelayCommand(New Action(AddressOf ResetConversionSampleString)))
            Me.Commands.Add("ToggleConversionSampleBooleanCmd", New ViewModelRelayCommand(New Action(AddressOf ToggleConversionSampleBoolean)))
            Me.Commands.Add("ConversionSampleListAddCmd", New ViewModelRelayCommand(New Action(AddressOf AddConversionSampleListItem)))
            Me.Commands.Add("ConversionSampleListClearCmd", New ViewModelRelayCommand(New Action(AddressOf ClearConversionSampleList)))
            Me.Commands.Add("SampleDictionaryAddItemCmd", New ViewModelRelayCommand(New Action(AddressOf SampleDictionaryAdd)))

            Me.Commands.Add("OpenGUIThreadCmd", New ViewModelRelayCommand(New Action(AddressOf GUIThreadOpenCmd)))
            Me.Commands.Add("CloseGUIThreadsCmd", New ViewModelRelayCommand(New Action(AddressOf GUIThreadsCloseCmd)))
            Me.Commands.Add("GUIThreadsChangeGreetingCmd", New ViewModelRelayCommand(New Action(AddressOf GUIThreadChangeGreetingsCmd)))

            'add the validation rule(s)
            Me.ValidationRules.Add(New ViewModelValidationRule("ValidationAnswer", Function() Not Me.ValidationAnswer = 42, "'" & Me.ValidationAnswer & "' was not 'Deep Thought's' result."))

            AddHandler Me.ErrorsChanged, AddressOf HandleValidation
            AddHandler _validationNestedVM.ErrorsChanged, AddressOf HandleValidation

            'do an initial validation
            Validate() 'not working as expected -> known bug to be fixed

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method prompting a message box on button click
        ''' </summary>
        ''' <param name="args"></param>
        Private Sub SayHello(args As Object)

            MsgBox(DirectCast(args, String))

        End Sub

        ''' <summary>
        ''' method opening a dialog to modify the text
        ''' </summary>
        Private Sub OpenDialogCmd()

            Dim dialog As DialogVM = New DialogVM With {.Text = Me.DialogText}

            If dialog.ShowDialog Then

                MsgBox("Dialog applied. Your message: " & vbNewLine & vbNewLine & dialog.Text)
                Me.DialogText = dialog.Text

            Else

                MsgBox("Dialog canceled...")

            End If

            MsgBox("Dialog closed...")

        End Sub

        ''' <summary>
        ''' method opening a (custom) GUI thread
        ''' </summary>
        Private Sub GUIThreadOpenCmd()

            Dim thread As GUIThreadSampleVM = New GUIThreadSampleVM()
            AddHandler thread.Closed, AddressOf GUIThreadOnClosed

            _guiThreads.Add(thread.ThreadID, thread)
            OnPropertyChanged("Threads")

        End Sub

        ''' <summary>
        ''' event handler on closing a (custom) GUI thread
        ''' </summary>
        ''' <param name="sender"></param>
        Private Sub GUIThreadOnClosed(sender As GUIThreadSampleVM)

            'inform the user
            MsgBox("Thread '" & sender.ThreadID & "' closed")

            'update the listing
            If _guiThreads.ContainsKey(sender.ThreadID) Then

                _guiThreads.Remove(sender.ThreadID)

            End If

            OnPropertyChanged("Threads")

        End Sub

        ''' <summary>
        ''' method closing all (custom) GUI threads
        ''' </summary>
        Private Sub GUIThreadsCloseCmd()

            'close all thread(s)
            Dim counter As Integer = 0

            Dim threads As GUIThreadSampleVM() = _guiThreads.Values.ToArray

            For Each thread As GUIThreadSampleVM In threads

                counter += 1

                thread.Close()

            Next

            MsgBox(counter.ToString + " GUI thread(s) closed successfully")

            'reset the listing
            _guiThreads = New Dictionary(Of Integer, GUIThreadSampleVM)
            OnPropertyChanged("Threads")

        End Sub

        ''' <summary>
        ''' method modifying the greetings of the first GUI thread instance
        ''' </summary>
        Private Sub GUIThreadChangeGreetingsCmd()

            'check for a thread
            If _guiThreads.Count < 1 Then

                MsgBox("There are not threads to be altered")

                Exit Sub

            End If

            'modify the greetings
            _guiThreads.First.Value.Greetings = "Modified by request"

        End Sub

        ''' <summary>
        ''' method called on validation result(s) change
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub HandleValidation(sender As Object, e As DataErrorsChangedEventArgs)

            _validationResults = New List(Of String)

            'get the error(s) from current view model
            For Each err As ViewModelValidationResult In Me.GetErrors

                _validationResults.Add("'" & err.Level.ToString & "' from <Current ViewModel>: " & err.Message)

            Next

            'get the error(s) from nested view model
            For Each err As ViewModelValidationResult In Me.ValidationNestedVM.GetErrors

                _validationResults.Add("'" & err.Level.ToString & "' from <Nested ViewModel>: " & err.Message)

            Next

            'notify on property update
            OnPropertyChanged("ValidationResults")

        End Sub

        ''' <summary>
        ''' method resetting the conversion sample string
        ''' </summary>
        Private Sub ResetConversionSampleString()

            Me.ConversionSampleString = Nothing

        End Sub

        ''' <summary>
        ''' method toggling the conversion sample boolean
        ''' </summary>
        Private Sub ToggleConversionSampleBoolean()

            If Me.ConversionSampleBoolean Then

                Me.ConversionSampleBoolean = False

            Else

                Me.ConversionSampleBoolean = True

            End If

        End Sub

        ''' <summary>
        ''' method adding an item to the conversion sample list
        ''' </summary>
        Private Sub AddConversionSampleListItem()

            Dim list As List(Of String) = Me.ConversionSampleList

            list.Add(DateTime.Now.ToString)

            Me.ConversionSampleList = list

        End Sub

        ''' <summary>
        ''' method clearing the conversion sample list
        ''' </summary>
        Private Sub ClearConversionSampleList()

            Me.ConversionSampleList = New List(Of String)

        End Sub

        ''' <summary>
        ''' method adding a new value to the sample dictionary
        ''' </summary>
        Private Sub SampleDictionaryAdd()

            'validate the key
            If IsNothing(Me.SampleDictionaryItemKey) OrElse String.IsNullOrEmpty(Me.SampleDictionaryItemKey) Then

                MsgBox("The item key must not be empty")

                Return

            End If

            If Me.SampleDictionary.ContainsKey(Me.SampleDictionaryItemKey) Then

                MsgBox("The '" & Me.SampleDictionaryItemKey & "' key already exits in the dictionary")

                Return

            End If

            'validate the value
            If IsNothing(Me.SampleDictionaryItemValue) OrElse String.IsNullOrEmpty(Me.SampleDictionaryItemValue) Then

                MsgBox("The item value must not be empty")

                Return

            End If

            'add the item
            Me.SampleDictionary.Add(Me.SampleDictionaryItemKey, Me.SampleDictionaryItemValue)

        End Sub

#End Region

    End Class

End Namespace
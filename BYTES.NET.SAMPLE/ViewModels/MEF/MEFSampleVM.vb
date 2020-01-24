'import namespace(s) required from 'BYTES.NET' assembly
Imports BYTES.NET.WPF.MVVM

Imports BYTES.NET.MEF

'import internal namepsace(s) required
Imports BYTES.NET.SAMPLE.API.MEF

Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.MEF

Namespace ViewModels.MEF

    Public Class MEFSampleVM

        Inherits SampleVM

#Region "private variable(s)"

        Private _myView As MEFSampleView = Nothing

        Private _extensionsManager As ExtendedExtensionsManager(Of ICalculate, ICalculateMetadata) = New ExtendedExtensionsManager(Of ICalculate, ICalculateMetadata)

        Private _value1 As Double = 6
        Private _value2 As Double = 3

        Private _result As Double = Nothing

#End Region

#Region "public properties inherited from base-class instance"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "MEF"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New MEFSampleView
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

        Public ReadOnly Property OperatorSigns As String()
            Get

                Dim output As List(Of String) = New List(Of String)

                For Each extension As Lazy(Of ICalculate, ICalculateMetadata) In _extensionsManager.Extensions

                    output.Add(extension.Metadata.OperatorSign)

                Next

                Return output.ToArray

            End Get
        End Property

        Public Property Value1 As Double
            Get

                Return _value1

            End Get
            Set(value As Double)

                _value1 = value

                OnPropertyChanged()

            End Set
        End Property

        Public Property Value2 As Double
            Get

                Return _value2

            End Get
            Set(value As Double)

                _value2 = value

                OnPropertyChanged()

            End Set
        End Property

        Public ReadOnly Property Result As Double
            Get

                Return _result

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

            'load the extensions
            Dim settings As ExtensionsSettings = New ExtensionsSettings With {.Source = "%InstallationDir%\*.exe"}
            _extensionsManager.Settings = {settings, New ExtensionsSettings With {.Source = "C:\UnableToFind.dll"}}

            'add the command(s)
            Me.Commands.Add("CalculateResultCmd", New ViewModelRelayCommand(New Action(Of Object)(AddressOf CalculateResult)))

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method calculating the result value
        ''' </summary>
        ''' <param name="args"></param>
        Private Sub CalculateResult(args As Object)

            'validate the argument(s)
            If IsNothing(args) OrElse String.IsNullOrEmpty(args) Then

                MsgBox("The operator must not be empty.")

                Return

            End If

            'get the extension (instance)
            Dim extension As Lazy(Of ICalculate, ICalculateMetadata) = GetExtensionByOperator(args)

            If IsNothing(extension) Then

                MsgBox("Unable to find related extension")

            End If

            'calculate the result
            Try

                _result = extension.Value.Calculate(Me.Value1, Me.Value2)

            Catch ex As Exception

                _result = Nothing

                MsgBox("Error: Unable to calculate result: " & ex.Message)

            End Try

            'update the GUI
            OnPropertyChanged("Result")

        End Sub

        ''' <summary>
        ''' method returning the extension instance by operator sign
        ''' </summary>
        ''' <param name="operatorSign"></param>
        ''' <returns></returns>
        Private Function GetExtensionByOperator(ByVal operatorSign As String) As Lazy(Of ICalculate, ICalculateMetadata)

            For Each extension As Lazy(Of ICalculate, ICalculateMetadata) In _extensionsManager.Extensions

                If extension.Metadata.OperatorSign = operatorSign Then

                    Return extension

                End If

            Next

            Return Nothing

        End Function

#End Region

    End Class

End Namespace
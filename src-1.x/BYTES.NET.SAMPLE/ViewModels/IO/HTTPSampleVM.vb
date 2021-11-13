'import .net namespace(s) required
Imports System.Net
Imports System.IO

'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.WPF.MVVM

'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.IO

Namespace ViewModels.IO

    Public Class HTTPSampleVM

        Inherits SampleVM

#Region "private avriable(s)"

        Private _myView As HTTPSampleView = Nothing

        Private _url As String = Nothing

        Private _response As WebResponse = Nothing

#End Region

#Region "public properties inherited from base-class"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "HTTP Client"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New HTTPSampleView
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

        Public Property URL As String
            Get

                Return _url

            End Get
            Set(value As String)

                _url = value
                OnPropertyChanged()

            End Set
        End Property

        Public ReadOnly Property Headers As String
            Get

                If IsNothing(_response) Then
                    Return Nothing
                End If

                Return _response.Headers.ToString

            End Get
        End Property

        Public ReadOnly Property Body As String
            Get

                If IsNothing(_response) Then
                    Return Nothing
                End If

                Try

                    Dim reader As New StreamReader(_response.GetResponseStream())

                    Return "Type: " & _response.ContentType & vbNewLine & "Value: " & reader.ReadToEnd

                Catch ex As Exception

                    Return "<Failed to parse response>"

                End Try

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
            Me.Commands.Add("UpdateCmd", New ViewModelRelayCommand(New Action(AddressOf Update)))

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method requesting web response
        ''' </summary>
        Private Sub Update()

            'parse the argument(s)
            If IsNothing(_url) OrElse String.IsNullOrEmpty(_url) Then

                MsgBox("Unable to perform request: The URL must not be empty")

            End If

            'perform the web request
            Dim client As NET.IO.HTTP.Client = New NET.IO.HTTP.Client()
            _response = client.Request(_url)

            OnPropertyChanged("Headers")
            OnPropertyChanged("Body")

        End Sub

#End Region

    End Class

End Namespace
'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.IO.FTP
Imports BYTES.NET.WPF.MVVM

'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.IO

Namespace ViewModels.IO

    Public Class FTPSampleVM

        Inherits SampleVM

#Region "private variable(s)"

        Private _myView As FTPSampleView = Nothing

        Private _uri As String = Nothing
        Private _user As String = Nothing
        Private _password As String = Nothing

        Private _connection As Connection = Nothing
        Private _items As FTPRemoteItem() = Nothing

#End Region

#Region "public properties inherited from base-class"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "FTP Browser"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New FTPSampleView
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

        Public Property URI As String
            Get

                Return _uri

            End Get
            Set(value As String)

                _uri = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property User As String
            Get

                Return _user

            End Get
            Set(value As String)

                _user = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property Password As String
            Get

                Return _password

            End Get
            Set(value As String)

                _password = value
                OnPropertyChanged()

            End Set
        End Property

        Public ReadOnly Property Items As FTPRemoteItem()
            Get

                Return _items

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
            Me.Commands.Add("ConnectCmd", New ViewModelRelayCommand(New Action(AddressOf Connect)))

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method connecting to the network share
        ''' </summary>
        Private Sub Connect()

            'validate the values
            If IsNothing(_uri) Then

                MsgBox("Failed to connect: The source URI must not be empty")

            End If

            'establish the connection
            If IsNothing(User) OrElse String.IsNullOrEmpty(User) Then

                _connection = New Connection(New Uri(Uri))

            End If

            _connection = New Connection(New Uri(Uri), New NET.IO.User.Info(_user, _password))

            'update the list of remote items
            _items = _connection.GetItems()
            OnPropertyChanged("Items")

        End Sub

#End Region

    End Class

End Namespace
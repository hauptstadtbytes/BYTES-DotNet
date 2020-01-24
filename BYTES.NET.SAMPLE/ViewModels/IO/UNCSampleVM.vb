'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.IO.UNC

Imports BYTES.NET.WPF.MVVM

'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.IO

Namespace ViewModels.IO

    Public Class UNCSampleVM

        Inherits SampleVM

#Region "private avriable(s)"

        Private _myView As UNCSampleView = Nothing

        Private _sourcePath As String = Nothing
        Private _connection As Connection = Nothing

        Private _user As NET.IO.User.Info = Nothing
        Private _folders As FolderVM() = Nothing

#End Region

#Region "public properties inherited from base-class"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "Remote Directory Browser"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New UNCSampleView
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

        Public Property SourcePath As String
            Get

                Return _sourcePath

            End Get
            Set(value As String)

                _sourcePath = value
                OnPropertyChanged()

            End Set
        End Property

        Public Property User As String
            Get

                If IsNothing(_user) Then
                    Return Nothing
                End If

                If IsNothing(_user.Domain) Then
                    Return _user.Name
                End If

                Return _user.Domain & "\" & _user.Name

            End Get
            Set(value As String)

                UpdateUser(value, Nothing)

                OnPropertyChanged()

            End Set
        End Property

        Public Property Password As String
            Get

                If IsNothing(_user) Then
                    Return Nothing
                End If

                Return _user.Password

            End Get
            Set(value As String)

                UpdateUser(Nothing, value)

                OnPropertyChanged()

            End Set
        End Property

        Public ReadOnly Property Folders As FolderVM()
            Get

                Return _folders

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
        ''' method updating the user properties
        ''' </summary>
        ''' <param name="user"></param>
        ''' <param name="password"></param>
        Private Sub UpdateUser(Optional ByVal user As String = Nothing, Optional ByVal password As String = Nothing)

            'parse the user properties
            Dim domain As String = Nothing
            Dim usr As String = Nothing
            Dim passwd As String = Nothing

            If IsNothing(user) And Not IsNothing(password) Then

                domain = _user.Domain
                usr = _user.Name
                passwd = password

            End If

            If Not IsNothing(user) And IsNothing(password) Then

                passwd = password

                If user.Contains("\") Then

                    domain = user.Split("\")(0)
                    usr = user.Split("\")(1)

                Else

                    usr = user
                    domain = Nothing

                End If

            End If

            _user = New NET.IO.User.Info(usr, passwd, domain)

        End Sub

        ''' <summary>
        ''' method connecting to the network share
        ''' </summary>
        Private Sub Connect()

            'validate the values
            If IsNothing(_sourcePath) Then

                MsgBox("Failed to connect: The source path must not be empty")

            End If

            'establish a connection
            _connection = New Connection(_sourcePath, _user)

            'update the folders
            _folders = {New FolderVM(_connection)}
            OnPropertyChanged("Folders")

        End Sub

#End Region

    End Class

End Namespace
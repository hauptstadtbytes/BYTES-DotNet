'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.Collections.API
Imports BYTES.NET.Collections.MSSQL

Imports BYTES.NET.WPF.MVVM

'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.Collections.MSSQL

Namespace ViewModels.Collections.MSSQL

    Public Class MSSQLServerSampleVM

        Inherits SampleVM

#Region "private variable(s)"

        Private _myView As MSSQLServerSampleView = Nothing

        Private _host As String = "localhost"
        Private _catalog As String = "DemoDB"
        Private _user As String = "sa"
        Private _password As String = Nothing

        Private _connection As SQLServerConnection = Nothing

        Private _tables As Dictionary(Of String, ISQLTable) = Nothing
        Private _table As String = Nothing

#End Region

#Region "public properties inherited from base-class instance"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "SQL Server Browser"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New MSSQLServerSampleView
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

        Public Property Host As String
            Get

                Return _host

            End Get
            Set(value As String)

                _host = value

                OnPropertyChanged()

            End Set
        End Property

        Public Property Catalog As String
            Get

                Return _catalog

            End Get
            Set(value As String)

                _catalog = value

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

        Public ReadOnly Property Tables As String()
            Get

                If IsNothing(_tables) Then

                    Return {"<Not Connected>"}

                Else

                    Return _tables.Keys.ToArray

                End If

            End Get
        End Property

        Public Property Table As String
            Get

                Return _table

            End Get
            Set(value As String)

                _table = value

                OnPropertyChanged()
                OnPropertyChanged("Columns")
                OnPropertyChanged("Count")

            End Set
        End Property

        Public ReadOnly Property Columns As String()
            Get

                If IsNothing(_tables) Then

                    Return {}

                End If

                Dim table As SQLServerTable = Nothing

                If _tables.ContainsKey(Me.Table) Then
                    table = _tables(Me.Table)
                End If

                If Not IsNothing(table) Then

                    Return table.Columns

                Else

                    Return {}

                End If

            End Get
        End Property

        Public ReadOnly Property Count As Integer
            Get

                If IsNothing(_tables) Then

                    Return 0

                End If

                Dim table As SQLServerTable = Nothing

                If _tables.ContainsKey(Me.Table) Then
                    table = _tables(Me.Table)
                End If

                If Not IsNothing(table) Then

                    Return table.Items.Count

                Else

                    Return 0

                End If

            End Get
        End Property

#End Region

#Region "public new instance method"

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

#Region "private variable(s)"

        ''' <summary>
        ''' method establishing a DB connection
        ''' </summary>
        Private Sub Connect()

            Try

                _connection = New SQLServerConnection(Me.Host, Me.Catalog, Me.User, Me.Password)
                _tables = _connection.Tables

                OnPropertyChanged("Tables")

                Me.Table = Me.Tables.First

            Catch ex As Exception

                MsgBox("Establishing a database connection to '" & Me.Host & "\" & Me.Catalog & "' failed: " & ex.Message)

            End Try

        End Sub

#End Region

    End Class

End Namespace
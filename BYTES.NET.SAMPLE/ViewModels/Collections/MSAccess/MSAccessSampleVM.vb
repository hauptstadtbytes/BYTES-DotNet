'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.Collections.API
Imports BYTES.NET.Collections.MSAccess

Imports BYTES.NET.WPF.MVVM

'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.ViewModels.API

Imports BYTES.NET.SAMPLE.Views.Collections.MSAccess

Namespace ViewModels.Collections.MSAccess

    Public Class MSSAccessSampleVM

        Inherits SampleVM

#Region "private variable(s)"

        Private _myView As MSAccessSampleView = Nothing

        Private _path As String = Nothing

        Private _connection As AccessDBConnection = Nothing

        Private _tables As Dictionary(Of String, ISQLTable) = Nothing
        Private _table As String = Nothing

#End Region

#Region "public properties inherited from base-class instance"

        Public Overrides ReadOnly Property Name As String
            Get

                Return "Access File Browser"

            End Get
        End Property

        Public Overrides Property View As UserControl
            Get

                If IsNothing(_myView) Then

                    _myView = New MSAccessSampleView
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

        Public Property Path As String
            Get

                Return _path

            End Get
            Set(value As String)

                _path = value

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

                Dim table As AccessDBTable = Nothing

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

                Dim table As AccessDBTable = Nothing

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

                _connection = New AccessDBConnection(Me.Path)
                _tables = _connection.Tables

                OnPropertyChanged("Tables")

                Me.Table = Me.Tables.First

            Catch ex As Exception

                MsgBox("Establishing a connection to '" & Me.Path & "' failed: " & ex.Message)

            End Try

        End Sub

#End Region

    End Class

End Namespace
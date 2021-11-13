'import .net namespace(s) needed
Imports System.Data.SqlClient

'import internal namespace(s) required
Imports BYTES.NET.Collections.API

Namespace Collections.MSSQL

    <SQLConnectionMetadata(ID:="BYTES_SQLConnection_MSSQL", Name:="Microsoft SQL Server Connection")>
    Public Class SQLServerConnection

        Implements ISQLConnection

#Region "private variable(s)"

        Private _host As String = String.Empty
        Private _catalog As String = String.Empty
        Private _dbUser As String = String.Empty
        Private _dbPassword As String = String.Empty

        Private _dbConnection As SqlConnection = Nothing

#End Region

#Region "public properties inherited from 'ISQLConnection' interface"

        Public ReadOnly Property Tables As Dictionary(Of String, ISQLTable) Implements ISQLConnection.Tables
            Get

                Dim output As Dictionary(Of String, ISQLTable) = New Dictionary(Of String, ISQLTable)

                For Each name As String In ListTables()

                    output.Add(name, New SQLServerTable(Me, name))

                Next

                Return output

            End Get
        End Property

#End Region

#Region "public properties"

        Public ReadOnly Property Host As String
            Get

                Return _host

            End Get
        End Property

        Public ReadOnly Property Catalog As String
            Get

                Return _catalog

            End Get
        End Property

        Public ReadOnly Property Connection As SqlConnection
            Get

                Return _dbConnection

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="host"></param>
        ''' <param name="catalog"></param>
        ''' <param name="dbUser"></param>
        ''' <param name="dbPassword"></param>
        Public Sub New(ByVal host As String, ByVal catalog As String, ByVal dbUser As String, ByVal dbPassword As String)

            'set the variable(s)
            _host = host
            _catalog = catalog
            _dbUser = dbUser
            _dbPassword = dbPassword

            'initialize the database connection
            _dbConnection = GetConnection()
            _dbConnection.Open()
            _dbConnection.Close()

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method listing all (non-system) database table(s)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>returning a list of property strings using 'name' as default</remarks>
        Public Function ListTables(Optional ByVal prop As String = "name") As String()

            'create the output value
            Dim output As List(Of String) = New List(Of String)

            'open the database connection
            _dbConnection.Open()

            'query the database
            Using queryCmd As New SqlCommand

                With queryCmd
                    .Connection = _dbConnection
                    .CommandText = "SELECT " & prop & " FROM sys.tables"
                End With

                Dim reader As SqlDataReader = queryCmd.ExecuteReader
                While reader.Read
                    output.Add(reader(0).ToString)
                End While

            End Using

            'close the database connection
            _dbConnection.Close()

            'return the output value
            Return output.ToArray()

        End Function

        ''' <summary>
        ''' method listing all table columns
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>returning a list of property strings using 'COLUMN_NAME' as default</remarks>
        Public Function ListColumns(ByVal table As String, Optional ByVal prop As String = "COLUMN_NAME") As String()

            'create the output value
            Dim output As List(Of String) = New List(Of String)

            'check for the table
            If Not ListTables().Contains(table) Then

                Return output.ToArray()

            End If

            'open the database connection
            _dbConnection.Open()

            'query the database
            Using queryCmd As New SqlCommand("exec sp_columns " & table, _dbConnection)

                Dim reader As SqlDataReader = queryCmd.ExecuteReader
                While reader.Read
                    output.Add(reader(prop).ToString)
                End While

            End Using

            'close the database connection
            _dbConnection.Close()

            'return the output value
            Return output.ToArray()

        End Function

        ''' <summary>
        ''' method querying the database
        ''' </summary>
        ''' <param name="statement"></param>
        ''' <returns></returns>
        Public Function Query(statement As String) As Dictionary(Of String, Object)()

            Dim output As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'open the database connection
            _dbConnection.Open()

            'query the database
            Using queryCmd As New SqlCommand(statement, _dbConnection)

                Dim reader As SqlDataReader = queryCmd.ExecuteReader
                While reader.Read

                    Dim values As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

                    If reader.FieldCount > 0 Then

                        For i = 0 To reader.FieldCount - 1 Step 1 'loop for each column

                            values.Add(reader.GetName(i), reader(i))

                        Next

                    End If

                    output.Add(values)

                End While

            End Using

            'close the database connection
            _dbConnection.Close()

            Return output.ToArray()

        End Function


        ''' <summary>
        ''' method executing a SQL statement without doing a query
        ''' </summary>
        ''' <param name="statement"></param>
        ''' <returns></returns>
        Public Function NoQuery(statement As String) As Boolean

            'open the database connection
            _dbConnection.Open()

            'query the database
            Using queryCmd As New SqlCommand(statement, _dbConnection)

                Dim adapter As SqlDataAdapter = New SqlDataAdapter(queryCmd)
                queryCmd.ExecuteNonQuery()

            End Using

            'close the database connection
            _dbConnection.Close()

            Return True

        End Function

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method returning a SQL server connection
        ''' </summary>
        ''' <returns></returns>
        Private Function GetConnection() As SqlConnection

            Try

                If IsNothing(_dbConnection) Then

                    _dbConnection = New SqlConnection

                    With _dbConnection
                        .ConnectionString = "Data Source=" & _host & "; Initial Catalog=" & _catalog & "; Persist Security Info=True;User ID=" & _dbUser & ";Password=" & _dbPassword & ";Pooling=true;Connection Timeout=2"
                    End With

                End If

                Return _dbConnection

            Catch ex As Exception

                Throw New ArgumentException(ex.Message, ex)

            End Try

        End Function

#End Region

    End Class

End Namespace
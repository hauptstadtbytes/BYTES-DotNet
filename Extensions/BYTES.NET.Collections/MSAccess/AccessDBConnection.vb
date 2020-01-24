'import .net namespace(s) required
Imports System.IO

Imports System.Data
Imports System.Data.OleDb

'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.Collections.API

Imports BYTES.NET.Collections.Registry

Namespace MSAccess

    <SQLConnectionMetadata(ID:="BYTES_SQLConnection_Access", Name:="Microsoft Access File Connection")>
    Public Class AccessDBConnection

        Implements ISQLConnection

#Region "private variable(s)"

        Private _path As String = Nothing

        Private _dbConnection As OleDbConnection = Nothing

#End Region

#Region "public properties inherited from base-class instance"

        Public ReadOnly Property Tables As Dictionary(Of String, ISQLTable) Implements ISQLConnection.Tables
            Get

                Dim output As Dictionary(Of String, ISQLTable) = New Dictionary(Of String, ISQLTable)

                For Each name As String In ListTables()

                    output.Add(name, New AccessDBTable(Me, name))

                Next

                Return output

            End Get
        End Property

#End Region

#Region "public properties"

        Public ReadOnly Property Connection As OleDbConnection
            Get

                Return _dbConnection

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="filePath"></param>
        Public Sub New(ByVal filePath As String)

            'validate the file path
            If Not File.Exists(filePath) Then

                Throw New ArgumentException("Unable to find Access file at '" & filePath & "'")

            End If

            'check for the requirements
            If Not CheckForRequirements() Then

                Throw New Exception("Unable to create a 'AccessDBConnection' class instance: The Microsoft Access Database Engine is missing on this system. Please install first.")

            End If

            'set the variable(s)
            _path = filePath
            _dbConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source = " & filePath)

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method listing all database table(s)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>based on the article found at 'https://social.msdn.microsoft.com/Forums/en-US/1415dc19-b526-415e-b8d8-73ac872caf73/get-a-list-of-tables?forum=csharpgeneral'</remarks>
        Public Function ListTables() As String()

            'create the output value
            Dim output As List(Of String) = New List(Of String)

            'query the DB file
            Dim restrictions As String() = New String() {Nothing, Nothing, Nothing, "Table"}
            Dim conn As OleDbConnection = Connection

            conn.Open()
            Dim dataTable As DataTable = conn.GetSchema("Tables", restrictions)
            conn.Close()

            'prepare the output
            For Each dataRow As DataRow In dataTable.Rows

                output.Add(dataRow("TABLE_NAME"))

            Next

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
            Using queryCmd As New OleDbCommand(statement, _dbConnection)

                Dim reader As OleDbDataReader = queryCmd.ExecuteReader
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

#End Region

#Region "private methods(s)"

        ''' <summary>
        ''' method validating the current environment for 
        ''' </summary>
        ''' <returns></returns>
        Private Function CheckForRequirements() As Boolean

            'create the parent node manager instance
            Dim software As Node = New Node("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall")

            'search for the 'Microsoft Access database engine'
            Dim filter As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"DisplayName", "Microsoft Access database engine"}}

            If software.SearchForChildren(filter, {Node.EnumerationOptions.IgnoreCase, Node.EnumerationOptions.ContainsSearch}).Length > 0 Then

                Return True

            End If

            'return the default output value
            Return False

        End Function

#End Region

    End Class

End Namespace
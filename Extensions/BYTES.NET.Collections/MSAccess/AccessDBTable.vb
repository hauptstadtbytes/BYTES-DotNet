'import .net namespace(s) required
Imports System.Data
Imports System.Data.OleDb

'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.Collections.API

Namespace MSAccess

    Public Class AccessDBTable

        Implements ISQLTable

#Region "protected variable(s)"

        Protected _parent As AccessDBConnection = Nothing
        Protected _name As String = Nothing

#End Region

#Region "public properties inherited from base-class instance"

        Public ReadOnly Property Columns As String() Implements ISQLTable.Columns
            Get

                Return GetColumns()

            End Get
        End Property

        Public ReadOnly Property Items As Dictionary(Of String, Object)() Implements ISQLTable.Items
            Get

                Return _parent.Query("SELECT * FROM " & _name)

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="parent"></param>
        ''' <param name="name"></param>
        Public Sub New(ByRef parent As AccessDBConnection, ByVal name As String)

            'set the variable(s)
            _parent = parent
            _name = name

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method returning a list of all column names
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>based on the article found at 'https://stackoverflow.com/questions/3502741/is-there-a-query-that-will-return-all-of-the-column-names-in-a-microsoft-access'</remarks>
        Private Function GetColumns() As String()

            Dim output As List(Of String) = New List(Of String)

            'query the DB file
            Dim restrictions As String() = New String() {Nothing, Nothing, _name, Nothing}
            Dim conn As OleDbConnection = _parent.Connection

            conn.Open()
            Dim dataTable As DataTable = conn.GetSchema("Columns", restrictions)
            conn.Close()

            'prepare the output
            For Each dataRow As DataRow In dataTable.Rows

                output.Add(dataRow("Column_Name"))

            Next

            'return the output value
            Return output.ToArray

        End Function

#End Region

    End Class

End Namespace
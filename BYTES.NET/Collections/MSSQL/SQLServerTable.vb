'import internal namespace(s) required
Imports BYTES.NET.Collections.API

Namespace Collections.MSSQL

    Public Class SQLServerTable

        Implements ISQLTable

#Region "protected variable(s)"

        Protected _parent As SQLServerConnection = Nothing
        Protected _name As String = Nothing

#End Region

#Region "public properties inherited from base-class instance"

        Public ReadOnly Property Columns As String() Implements ISQLTable.Columns
            Get

                Return _parent.ListColumns(_name)

            End Get
        End Property

        Public ReadOnly Property Items As Dictionary(Of String, Object)() Implements ISQLTable.Items
            Get

                Return _parent.Query("SELECT * FROM " & _name)

            End Get
        End Property

#End Region

#Region "public properties"

        Public ReadOnly Property Server As SQLServerConnection
            Get

                Return _parent

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="parent"></param>
        ''' <param name="name"></param>
        Public Sub New(ByRef parent As SQLServerConnection, ByVal name As String)

            'set the variable(s)
            _parent = parent
            _name = name

        End Sub

#End Region

#Region "shared method(s)"

        ''' <summary>
        ''' method accessing a table by it's connection properties
        ''' </summary>
        ''' <param name="host"></param>
        ''' <param name="catalog"></param>
        ''' <param name="dbUser"></param>
        ''' <param name="dbPassword"></param>
        ''' <param name="table"></param>
        ''' <returns></returns>
        Shared Function Open(ByVal host As String, ByVal catalog As String, ByVal dbUser As String, ByVal dbPassword As String, ByVal table As String, Optional ByVal creationQuery As String = Nothing) As SQLServerTable

            'create a new connection
            Dim conn As SQLServerConnection = New SQLServerConnection(host, catalog, dbUser, dbPassword)

            If Not conn.ListTables.Contains(table) Then

                If Not IsNothing(creationQuery) Then

                    If conn.NoQuery(creationQuery) Then

                        Return New SQLServerTable(conn, table)

                    End If

                Else

                    Throw New ArgumentException("Unable to find table '" & table & "' at '" & host & ":" & catalog & "'")

                End If

            End If

            'return the output value
            Return New SQLServerTable(conn, table)

        End Function

#End Region

    End Class

End Namespace
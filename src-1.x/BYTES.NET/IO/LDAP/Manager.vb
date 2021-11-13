'import .net namespace(s) required
Imports System.DirectoryServices

Namespace IO.LDAP

    ''' <summary>
    ''' the LDAP manager
    ''' </summary>
    ''' <remarks>based on the article found at 'https://www.codemag.com/article/1312041?fb_comment_id=1421144321437986_1620428098176273'</remarks>
    Public Class Manager

#Region "private variable(s)"

        Private _domainPath As String = Nothing

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="domainPath"></param>
        Public Sub New(Optional ByVal domainPath As String = Nothing)

            'set the variable(s)
            If IsNothing(domainPath) Then

                _domainPath = GetCurrentDomain(True)

            Else

                _domainPath = domainPath

            End If

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method authenticating a user by user name and password
        ''' </summary>
        ''' <param name="user"></param>
        ''' <param name="password"></param>
        ''' <returns></returns>
        Public Function Authenticate(ByVal user As String, ByVal password As String)

            Try

                Dim entry As DirectoryEntry = New DirectoryEntry(_domainPath, user, password)

                Dim searcher As DirectorySearcher = New DirectorySearcher(entry)

                searcher.FindOne()

                Return True

            Catch ex As Exception

                Return False

            End Try

        End Function

        ''' <summary>
        ''' method searching for entities, using the 
        ''' </summary>
        ''' <param name="filter"></param>
        ''' <param name="properties"></param>
        ''' <returns></returns>
        Public Function Search(ByVal filter As String, Optional ByVal properties As String() = Nothing) As Dictionary(Of String, Object)()

            Dim output As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))

            'get the search results and parse the output
            For Each result As SearchResult In GetSearchResult(_domainPath, filter)

                output.Add(ParseProperties(result, properties))

            Next

            'return the output value
            Return output.ToArray

        End Function

        ''' <summary>
        ''' method returning a list of all property names
        ''' </summary>
        ''' <param name="filter"></param>
        ''' <returns></returns>
        Public Function GetProperties(ByVal filter As String) As String()

            Dim output As List(Of String) = New List(Of String)

            'get the search results and parse the output
            For Each result As SearchResult In GetSearchResult(_domainPath, filter)

                For Each propName As String In ParseProperties(result).Keys

                    If Not output.Contains(propName) Then

                        output.Add(propName)

                    End If

                Next

            Next

            Return output.ToArray

        End Function

#End Region

#Region "shared method(s)"

        ''' <summary>
        ''' method returning the current domain properties
        ''' </summary>
        ''' <returns></returns>
        Shared Function GetCurrentDomain() As ActiveDirectory.Domain

            Return ActiveDirectory.Domain.GetCurrentDomain

        End Function

        ''' <summary>
        ''' method returning the domain's "distinguished name"
        ''' </summary>
        ''' <param name="addPrefix"></param>
        ''' <returns></returns>
        Shared Function GetCurrentDomain(ByVal addPrefix As Boolean) As String

            Dim entry As DirectoryEntry = New DirectoryEntry("LDAP://RootDSE")

            Dim prefix As String = String.Empty

            If addPrefix Then
                prefix = "LDAP://"
            End If

            Return prefix & entry.Properties("defaultNamingContext")(0).ToString()

        End Function

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method searching the domain given, applying the filter given
        ''' </summary>
        ''' <param name="domainPath"></param>
        ''' <param name="filter"></param>
        ''' <returns></returns>
        Private Function GetSearchResult(ByVal domainPath As String, Optional ByVal filter As String = "(objectClass=simpleSecurityObject)") As SearchResult()

            Dim entry As DirectoryEntry = New DirectoryEntry(domainPath)

            Dim searcher As DirectorySearcher = New DirectorySearcher(entry)
            searcher.Filter = filter

            Dim output As List(Of SearchResult) = New List(Of SearchResult)
            For Each result As SearchResult In searcher.FindAll

                output.Add(result)

            Next

            Return output.ToArray

        End Function

        ''' <summary>
        ''' method parsing a search result for the properties given
        ''' </summary>
        ''' <param name="result"></param>
        ''' <param name="properties"></param>
        ''' <returns></returns>
        Private Function ParseProperties(ByRef result As SearchResult, Optional ByVal properties As String() = Nothing) As Dictionary(Of String, Object)

            Dim output As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

            If Not IsNothing(properties) Then 'only some specific pproperties have been requested

                For Each name As String In properties

                    If result.Properties(name).Count > 0 Then

                        output.Add(name, result.Properties(name)(0))

                    Else

                        output.Add(name, Nothing)

                    End If

                Next

            Else 'all properties have been requested

                For Each prop As DictionaryEntry In result.Properties

                    If prop.Value.Count > 0 Then

                        output.Add(prop.Key, prop.Value)

                    Else

                        output.Add(prop.Key, Nothing)

                    End If

                Next

            End If

            Return output

        End Function

#End Region

    End Class

End Namespace
Namespace Collections.Registry

    Public Class Node

#Region "shared variable(s)"

        Public Enum EnumerationOptions
            IgnoreCase
            ContainsSearch
        End Enum

#End Region

#Region "private variable(s)"

        Private _root As Microsoft.Win32.RegistryKey = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property Root As Microsoft.Win32.RegistryKey
            Get

                Return _root

            End Get
        End Property

        Public ReadOnly Property Path As String
            Get
                Return _root.Name
            End Get
        End Property

        Public ReadOnly Property Values As Dictionary(Of String, Object)
            Get

                Dim output As Dictionary(Of String, Object) = New Dictionary(Of String, Object)

                For Each name As String In _root.GetValueNames()

                    output.Add(name, _root.GetValue(name))

                Next

                Return output

            End Get
        End Property

        Public ReadOnly Property Children As Node()
            Get

                Dim output As List(Of Node) = New List(Of Node)

                For Each name As String In _root.GetSubKeyNames()

                    output.Add(New Node(_root.OpenSubKey(name)))

                Next

                Return output.ToArray

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="path"></param>
        Public Sub New(ByVal path As String)

            'set the variable(s)
            _root = Node.GetKey(path)

        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="key"></param>
        Public Sub New(ByRef key As Microsoft.Win32.RegistryKey)

            'set the variable(s)
            _root = key

        End Sub

#End Region

#Region "shared method(s)"

        ''' <summary>
        ''' method creating a registry key from path
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        Shared Function GetKey(ByVal path As String) As Microsoft.Win32.RegistryKey

            Select Case True

                Case path = "HKEY_LOCAL_MACHINE"
                    Return Microsoft.Win32.Registry.LocalMachine

                Case path.StartsWith("HKEY_LOCAL_MACHINE")
                    Return GetSubKey(Microsoft.Win32.Registry.LocalMachine, path.Replace("HKEY_LOCAL_MACHINE", ""))

            End Select

            Return Nothing

        End Function

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method searching for children matching the filter (options) given
        ''' </summary>
        ''' <param name="filter"></param>
        ''' <param name="options"></param>
        ''' <returns></returns>
        Public Function SearchForChildren(Optional ByVal filter As Dictionary(Of String, String) = Nothing, Optional ByVal options As EnumerationOptions() = Nothing) As Node()

            'create the output value
            Dim output As List(Of Node) = New List(Of Node)

            'check for the filter
            If IsNothing(filter) Then
                Return Me.Children
            End If

            'parse the options
            Dim ignoreCase As Boolean = True

            If Not options.Contains(EnumerationOptions.IgnoreCase) Then
                ignoreCase = False
            End If

            Dim containsSearch As Boolean = True

            If Not options.Contains(EnumerationOptions.ContainsSearch) Then
                containsSearch = False
            End If

            'validate the children
            For Each child As Node In Me.Children

                If ValidateNodeByFilter(child, filter, ignoreCase, containsSearch) Then

                    output.Add(child)

                End If

            Next

            'return the output value
            Return output.ToArray

        End Function

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method opening a sub key 
        ''' </summary>
        ''' <param name="root"></param>
        ''' <param name="path"></param>
        ''' <returns></returns>
        Private Shared Function GetSubKey(ByRef root As Microsoft.Win32.RegistryKey, ByVal path As String)

            If path.StartsWith("\") Then
                path = path.Substring(1)
            End If

            Return root.OpenSubKey(path)

        End Function

        ''' <summary>
        ''' method validating a key by the filter criteria given
        ''' </summary>
        ''' <param name="node"></param>
        ''' <param name="filter"></param>
        ''' <returns></returns>
        Private Function ValidateNodeByFilter(ByRef node As Node, filter As Dictionary(Of String, String), ByVal ignoreCase As Boolean, ByVal containsSearch As Boolean) As Boolean

            'get the values
            Dim vals As Dictionary(Of String, Object) = node.Values

            'validate against the filter parameters
            For Each pair As KeyValuePair(Of String, String) In filter

                If Not vals.ContainsKey(pair.Key) Then

                    Return False

                End If

                If ignoreCase Then

                    If containsSearch Then

                        If Not vals(pair.Key).ToString.ToLower.Contains(pair.Value.ToLower) Then
                            Return False
                        End If

                    Else

                        If Not vals(pair.Key).ToString.ToLower = pair.Value.ToLower Then
                            Return False
                        End If

                    End If

                Else

                    If containsSearch Then

                        If Not vals(pair.Key).ToString.Contains(pair.Value) Then
                            Return False
                        End If

                    Else

                        If Not vals(pair.Key).ToString = pair.Value Then
                            Return False
                        End If

                    End If

                End If

            Next

            'return the default value
            Return True

        End Function

#End Region

    End Class

End Namespace
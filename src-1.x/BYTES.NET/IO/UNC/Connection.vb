'import .net namespace(s) required
Imports System.IO

Namespace IO.UNC

    Public Class Connection

#Region "private variable(s)"

        Private _path As String = Nothing

        Private _connection As NetworkConnection = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property Path As String
            Get

                Return _path

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="user"></param>
        Public Sub New(ByVal path As String, Optional ByVal user As User.Info = Nothing)

            'parse the argument(s)
            If Not path.EndsWith("\") Then

                path = path & "\"

            End If

            'set the variable(s)
            _path = path

            If Not IsNothing(user) Then

                _connection = New NetworkConnection(_path, user)

            End If

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method returning the file information
        ''' </summary>
        ''' <param name="path">path relative to the UNC root folder</param>
        ''' <returns></returns>
        Public Function GetFileInfo(ByVal path As String) As FileInfo

            'parse the path
            path = _path & ParsePath(path)

            'return the output value
            Try

                If IsNothing(_connection) Then

                    Return New FileInfo(path)

                Else

                    Using _connection

                        Return New FileInfo(path)

                    End Using

                End If

            Catch ex As Exception

                Throw New Exception("Unable to get 'FileInfo' for '" & path & "': " & ex.Message, ex)

            End Try

        End Function

        ''' <summary>
        ''' method returning the directory information
        ''' </summary>
        ''' <param name="path">path relative to the UNC root folder</param>
        ''' <returns></returns>
        Public Function GetFolderInfo(Optional ByVal path As String = Nothing) As DirectoryInfo

            'parse the path
            If Not IsNothing(path) Then

                path = _path & ParsePath(path)

            Else

                path = _path

            End If

            'return the output value
            Try

                If IsNothing(_connection) Then

                    Return New DirectoryInfo(path)

                Else

                    Using _connection

                        Return New DirectoryInfo(path)

                    End Using

                End If

            Catch ex As Exception

                Throw New Exception("Unable to get 'DirectoryInfo' for '" & path & "': " & ex.Message, ex)

            End Try

        End Function

        ''' <summary>
        ''' method returning an array of sub-folders
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        Public Function GetFolders(Optional ByVal path As String = Nothing) As DirectoryInfo()

            'parse the path
            If Not IsNothing(path) Then

                path = ParsePath(path)

            End If

            'return the output value
            Return Me.GetFolderInfo(path).GetDirectories()

        End Function

        ''' <summary>
        ''' method returning an array of files inside a given folder
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="searchPattern"></param>
        ''' <returns></returns>
        Public Function GetFiles(Optional ByVal path As String = Nothing, Optional ByVal searchPattern As String = Nothing) As FileInfo()

            'parse the path given
            If Not IsNothing(path) Then

                path = ParsePath(path)

            End If

            'return the output value
            If Not IsNothing(searchPattern) Then

                Return Me.GetFolderInfo(path).GetFiles(searchPattern)

            Else

                Return Me.GetFolderInfo(path).GetFiles

            End If

        End Function

        ''' <summary>
        ''' method checking wheather the location is readable
        ''' </summary>
        ''' <returns></returns>
        Public Function IsReadable() As Boolean

            Try

                If IsNothing(_connection) Then

                    Dim dirInfo As DirectoryInfo = New DirectoryInfo(_path)

                Else

                    Using _connection

                        Dim dirInfo As DirectoryInfo = New DirectoryInfo(_path)

                    End Using

                End If

                Return True

            Catch ex As Exception

                Return False

            End Try

        End Function

        ''' <summary>
        ''' method checking wheather a file exists or not
        ''' </summary>
        ''' <param name="path">path relative to the UNC root folder</param>
        ''' <returns></returns>
        Public Function FileExists(ByVal path As String) As Boolean

            'parse the path
            path = _path & ParsePath(path)

            'return the output value
            If IsNothing(_connection) Then

                Return File.Exists(path)

            Else

                Using _connection

                    Return File.Exists(path)

                End Using

            End If

        End Function

        ''' <summary>
        ''' method checking wheather a folder exists or not
        ''' </summary>
        ''' <param name="path">path relative to the UNC root folder</param>
        ''' <returns></returns>
        Public Function FolderExists(Optional ByVal path As String = Nothing) As Boolean

            'parse the path
            If Not IsNothing(path) Then

                path = _path & ParsePath(path)

            Else

                path = path

            End If

            'return the output value
            If IsNothing(_connection) Then

                Return Directory.Exists(path)

            Else

                Using _connection

                    Return Directory.Exists(path)

                End Using

            End If

        End Function

        ''' <summary>
        ''' method reading all bytes from a given file
        ''' </summary>
        ''' <param name="path">path relative to the UNC root folder</param>
        ''' <returns></returns>
        Public Function ReadBytes(ByVal path As String) As Byte()

            'parse the path
            path = _path & ParsePath(path)

            'return the output value
            Try

                If IsNothing(_connection) Then

                    Return File.ReadAllBytes(path)

                Else

                    Using _connection

                        Return File.ReadAllBytes(path)

                    End Using

                End If

            Catch ex As Exception

                Throw New Exception("Unable to read bytes from '" & path & "': " & ex.Message, ex)

            End Try

        End Function

        ''' <summary>
        ''' method fro copying a file to a specific location
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="destination"></param>
        Public Sub CopyFileTo(ByVal source As String, ByVal destination As String)

            'parse the source path
            source = _path & ParsePath(source)

            'copy the file
            Try

                If IsNothing(_connection) Then

                    FileCopy(source, destination)

                Else

                    Using _connection

                        FileCopy(source, destination)

                    End Using

                End If

            Catch ex As Exception

                Throw New Exception("Unable to copy file from '" & source & "' to '" & destination & "': " & ex.Message, ex)

            End Try

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method clearing the path string to be used as relative path
        ''' </summary>
        ''' <param name="path"></param>
        ''' <returns></returns>
        Private Function ParsePath(ByVal path As String) As String

            'remove the UNC location path substring (e.g. if a full path is given)
            If path.StartsWith(_path) Then

                path = path.Replace(_path, "")

            End If

            'remove a leading '\' char
            If path.StartsWith("\") Then

                path = path.Substring(1)

            End If

            'return the output value
            Return path

        End Function

#End Region

    End Class

End Namespace
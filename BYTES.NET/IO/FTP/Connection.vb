'import .net namespace(s) required
Imports System.IO
Imports System.Net

Namespace IO.FTP

    Public Class Connection

#Region "private variable(s)"

        Private _uri As Uri = Nothing
        Private _user As User.Info = Nothing

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="uri"></param>
        ''' <param name="user"></param>
        Public Sub New(ByVal uri As Uri, Optional ByVal user As User.Info = Nothing)

            'set the variable(s)
            _uri = uri
            _user = New User.Info("anonymous")

            If Not IsNothing(user) Then
                _user = user
            End If

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method returning a list of remote (file or folder) items
        ''' </summary>
        ''' <returns></returns>
        Public Function GetItems() As FTPRemoteItem()

            'create the output value
            Dim output As List(Of FTPRemoteItem) = New List(Of FTPRemoteItem)

            'create a new request
            Dim request As FtpWebRequest = DirectCast(WebRequest.Create(_uri), FtpWebRequest)

            With request
                .Method = WebRequestMethods.Ftp.ListDirectoryDetails
                .Credentials = _user.ToNetworkCredential()
            End With

            'get the response
            Dim response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)

            'read the stream data
            Dim strm As Stream = response.GetResponseStream()
            Dim reader As StreamReader = New StreamReader(strm)

            Do While reader.EndOfStream = False

                Dim item As FTPRemoteItem = New FTPRemoteItem(reader.ReadLine, Me)

                If Not (item.Name = ".") And Not (item.Name = "..") Then 'ignore file system item(s)

                    output.Add(item)

                End If

            Loop

            'return the output value
            Return output.ToArray

        End Function

#End Region

    End Class

End Namespace
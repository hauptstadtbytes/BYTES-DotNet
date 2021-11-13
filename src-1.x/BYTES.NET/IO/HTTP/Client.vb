'import .net namespace(s) required
Imports System.IO
Imports System.Net
Imports System.Text

Namespace IO.HTTP

    Public Class Client

#Region "private variable(s)"

        Private _auth As User.Info = Nothing

        Private _proxy As IWebProxy = Nothing
        Private _proxyCredentials As NetworkCredential = Nothing

#End Region

#Region "public properties"

        Public WriteOnly Property Authentication As User.Info
            Set(value As User.Info)
                _auth = value
            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()
        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method applying the system proxy
        ''' </summary>
        Public Sub SetProxy()

            _proxy = WebRequest.GetSystemWebProxy
            _proxyCredentials = CredentialCache.DefaultNetworkCredentials

        End Sub

        ''' <summary>
        ''' overloaded method applying a custom proxy
        ''' </summary>
        ''' <param name="address"></param>
        ''' <param name="user"></param>
        Public Sub SetProxy(ByVal address As String, ByVal user As User.Info)

            _proxy = New WebProxy With {.Address = New Uri(address)}
            _proxyCredentials = New NetworkCredential With {.UserName = user.Name, .Password = user.Password, .Domain = user.Domain}

        End Sub

        ''' <summary>
        ''' method performing a web request
        ''' </summary>
        ''' <param name="url"></param>
        ''' <param name="data"></param>
        ''' <returns></returns>
        Public Function Request(ByVal url As String, Optional ByVal data As String = Nothing) As WebResponse

            'create a new web request
            Dim req As WebRequest = WebRequest.Create(url)

            'set the proxy
            If Not IsNothing(_proxy) Then

                With req
                    .Proxy = _proxy
                    .Proxy.Credentials = _proxyCredentials
                End With

            End If

            'set the authentication
            If Not IsNothing(_auth) Then

                Dim cache As CredentialCache = New CredentialCache()

                cache.Add(New Uri(url), "Basic", New NetworkCredential(_auth.Name, _auth.Password, _auth.Domain))
                req.Credentials = cache

            End If

            'add POST data
            If Not IsNothing(data) Then

                req.Method = "POST"
                req.ContentType = "application/x-www-form-urlencoded"
                req.ContentLength = data.Length

                Dim writer As New StreamWriter(req.GetRequestStream, Encoding.ASCII)
                writer.Write(data)
                writer.Close()

            End If

            'return the output value
            Return req.GetResponse()

        End Function

#End Region

    End Class

End Namespace
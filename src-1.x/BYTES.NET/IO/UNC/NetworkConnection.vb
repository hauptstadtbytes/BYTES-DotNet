'import .net namespace(s) required
Imports System.Runtime.InteropServices

Namespace IO.UNC

    ''' <summary>
    ''' the 'NetworkConnection' class
    ''' </summary>
    ''' <remarks>based on the article found at 'http://stackoverflow.com/questions/295538/how-to-provide-user-name-and-password-when-connecting-to-a-network-share'</remarks>
    Public Class NetworkConnection

        Implements IDisposable

#Region "import unmanaged code"

        <DllImport("mpr.dll")>
        Private Shared Function WNetAddConnection2(netResource As NetworkResource, password As String, username As String, flags As Integer) As Integer
        End Function

        <DllImport("mpr.dll")>
        Private Shared Function WNetCancelConnection2(name As String, flags As Integer, force As Boolean) As Integer
        End Function

#End Region

#Region "private variable(s)"

        Private _path As String = String.Empty
        Private _user As User.Info = Nothing

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
        Public Sub New(ByVal path As String, ByVal user As User.Info)

            'set variable(s)
            _path = path
            _user = user

            'create the networkresource
            Dim netResource = New NetworkResource() With {.Scope = ResourceScope.GlobalNetwork, .ResourceType = ResourceType.Disk, .DisplayType = ResourceDisplaytype.Share, .RemoteName = _path}

            Dim result = WNetAddConnection2(netResource, _user.Password, _user.FullName, 0)

            'check not always working (= to be done)
            'If Not result <> 0 Then

            'Throw New Exception("Failed to connect to '" & path & "'.")

            'End If

        End Sub

#End Region

#Region "protected method(s) supporting 'IDisposable'"

        Protected Overridable Sub Dispose(disposing As Boolean)

            WNetCancelConnection2(_path, 0, True)

        End Sub

        Protected Overrides Sub Finalize()

            Try
                Dispose(False)
            Finally
                MyBase.Finalize()
            End Try

        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose

            Dispose(True)
            GC.SuppressFinalize(Me)

        End Sub

#End Region

    End Class

End Namespace
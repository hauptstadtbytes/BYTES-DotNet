Namespace IO.User

    Public Class Info

#Region "private variable(s)"

        Private _userName As String = Nothing
        Private _userDomain As String = Nothing

        Private _userPassword As String = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property Name As String
            Get
                Return _userName
            End Get
        End Property

        Public ReadOnly Property FullName As String
            Get

                If Not IsNothing(_userDomain) Then

                    Return _userDomain & "\" & _userName

                End If

                Return _userName

            End Get
        End Property

        Public ReadOnly Property Domain As String
            Get
                Return _userDomain
            End Get
        End Property

        Public ReadOnly Property Password As String
            Get

                Return _userPassword

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' public new instance method
        ''' </summary>
        ''' <param name="user"></param>
        ''' <param name="password"></param>
        ''' <param name="domain"></param>
        Public Sub New(ByVal user As String, Optional ByVal password As String = Nothing, Optional ByVal domain As String = Nothing)

            'set the variable(s)
            _userName = user
            _userPassword = password
            _userDomain = domain

        End Sub

#End Region

    End Class

End Namespace
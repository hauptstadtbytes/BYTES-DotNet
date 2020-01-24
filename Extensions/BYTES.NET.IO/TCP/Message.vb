Namespace TCP

    Public Class Message

#Region "private variable(s)"

        Private _data As Byte() = Nothing
        Private _meta As Dictionary(Of String, Object) = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property Data As Byte()
            Get

                Return _data

            End Get
        End Property

        Public ReadOnly Property Meta(ByVal key As String) As Object
            Get

                If _meta.ContainsKey(key) Then

                    Return _meta(key)

                Else

                    Return Nothing

                End If

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="data"></param>
        ''' <param name="meta"></param>
        Public Sub New(ByRef data As Byte(), ByVal meta As Dictionary(Of String, Object))

            'set the variable(s)
            _data = data
            _meta = meta

        End Sub

#End Region

    End Class

End Namespace
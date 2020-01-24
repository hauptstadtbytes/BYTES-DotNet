'import internal namespace(s) required
Imports BYTES.NET.MEF.API

Namespace Collections.API

    Public Class SQLConnectionMetadata

        Inherits Metadata

#Region "public properties"

        Public Property Name As String

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()

            MyBase.New(GetType(ISQLConnection))

        End Sub

#End Region

    End Class

End Namespace
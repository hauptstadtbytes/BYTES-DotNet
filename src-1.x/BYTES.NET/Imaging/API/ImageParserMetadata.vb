'import internal namespace(s) required
Imports BYTES.NET.MEF.API

Namespace Imaging.API

    Public Class ImageParserMetadata

        Inherits Metadata

#Region "public properties"

        Public Property Name As String

        Public Property FileExtensions As String()

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()

            MyBase.New(GetType(IImageParser))

        End Sub

#End Region

    End Class

End Namespace
'import internal namespace(s) required
Imports BYTES.NET.MEF.API

Namespace Logging.API

    Public Class LogAppenderMetadata

        Inherits Metadata

#Region "public properties"

        Public Property Name As String

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()

            MyBase.New(GetType(ILogAppender))

        End Sub

#End Region

    End Class

End Namespace
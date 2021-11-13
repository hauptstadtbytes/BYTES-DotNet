'import internal namespace(s) required
Imports BYTES.NET.MEF.API

Namespace Imaging.API

    Public Interface IImageParserMetadata

        Inherits IMetadata

        ReadOnly Property Name As String

        ReadOnly Property FileExtensions As String()

    End Interface

End Namespace
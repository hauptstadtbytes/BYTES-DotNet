'import namespace(s) required from 'BYTES.NET' assembly
Imports BYTES.NET.MEF.API

Namespace API.MEF

    Public Interface ICalculateMetadata

        Inherits IMetadata

        ReadOnly Property OperatorSign As String

    End Interface

End Namespace
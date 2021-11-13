'import namespace(s) required from 'BYTES.NET' assembly
Imports BYTES.NET.MEF.API

Namespace API.MEF

    Public Class CalculateMetadata

        Inherits Metadata

        Public Property OperatorSign As String

        Public Sub New()

            MyBase.New(GetType(ICalculate))

        End Sub

    End Class

End Namespace
'import internal namespace(s) required
Imports BYTES.NET.SAMPLE.API.MEF

Namespace Implementations.MEF

    <CalculateMetadata(ID:="MEFExample_Addition", OperatorSign:="+")>
    Public Class Addition

        Implements ICalculate

        Public Function Calculate(val1 As Double, val2 As Double) As Double Implements ICalculate.Calculate

            Return val1 + val2

        End Function

    End Class

End Namespace
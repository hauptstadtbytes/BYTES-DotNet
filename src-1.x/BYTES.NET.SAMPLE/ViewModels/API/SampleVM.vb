'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.WPF.MVVM

Namespace ViewModels.API

    Public MustInherit Class SampleVM

        Inherits ViewModel

#Region "public properties"

        Public MustOverride ReadOnly Property Name As String

        Public MustOverride Property View As UserControl

#End Region

    End Class

End Namespace
'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.WPF.MVVM

Imports BYTES.NET.Collections.API
Imports BYTES.NET.Collections.MSSQL

Namespace ViewModels.Collections

    Public Class TableSampleVM

        Inherits ViewModel

#Region "private variable(s)"

        Private _table As ISQLTable = Nothing

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="table"></param>
        Public Sub New(ByRef table As ISQLTable)

            'create a new base-class instance
            MyBase.New

            'set the variable(s)
            _table = table

        End Sub

#End Region

    End Class

End Namespace
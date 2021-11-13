'import namespace(s) required from 'BYTES.NET' library
Imports BYTES.NET.WPF.MVVM

Namespace ViewModels.MVVM

    Public Class ValidationVM

        Inherits ViewModel

#Region "private variable(s)"

        Private _operatorSign As String = "-"

#End Region

#Region "public properties"

        Public Property OperatorSign As String
            Get

                Return _operatorSign

            End Get
            Set(value As String)

                _operatorSign = value

                OnPropertyChanged(True)

            End Set
        End Property

#End Region

#Region "public new instance method"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        Public Sub New()

            'create a new base-class instance
            MyBase.New

            'add the validation rule(s)
            Me.ValidationRules.Add(New ViewModelValidationRule("OperatorSign", Function() Not Me.OperatorSign = "+", "'" & Me.OperatorSign & "' is the wrong operator. Think about once more!"))

            'do an initial validation
            Validate()

        End Sub

#End Region

    End Class

End Namespace
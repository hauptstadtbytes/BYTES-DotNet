Namespace WPF.MVVM

    ''' <summary>
    ''' property validation result class
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ViewModelValidationResult

#Region "private variable(s)"

        Private _propertyName As String = Nothing

        Private _resultLevel As ViewModelValidationRule.ResultLevels = ViewModelValidationRule.ResultLevels.IsUndefnied
        Private _outputMessage As String = ""

#End Region

#Region "public properties"

        Public ReadOnly Property PropertyName As String
            Get

                Return _propertyName

            End Get
        End Property

        Public ReadOnly Property Message As String
            Get

                Return _outputMessage

            End Get
        End Property

        Public ReadOnly Property Level As ViewModelValidationRule.ResultLevels
            Get

                Return _resultLevel

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="propertyName"></param>
        ''' <param name="resultLevel"></param>
        ''' <param name="message"></param>
        Public Sub New(ByVal propertyName As String, ByVal resultLevel As ViewModelValidationRule.ResultLevels, ByVal message As String)

            'set variable(s)
            _propertyName = propertyName
            _resultLevel = resultLevel
            _outputMessage = message

        End Sub

#End Region

    End Class

End Namespace
Namespace WPF.MVVM

    ''' <summary>
    ''' property validation rule class
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ViewModelValidationRule

#Region "public variable(s)"

        Public Enum ResultLevels
            IsNoError = 0
            IsWarning = 1
            IsError = 2
            IsUndefnied = 3
        End Enum

        Public Method As Func(Of Boolean) = Function() False

#End Region

#Region "private variable(s)"

        Private _propertyName As String = Nothing
        Private _outputMessage As String = ""
        Private _resultLevel As ViewModelValidationRule.ResultLevels = ResultLevels.IsError

#End Region

#Region "public properties"

        Public Property PropertyName As String
            Get
                Return _propertyName
            End Get
            Set(value As String)
                _propertyName = value
            End Set
        End Property

        Public Property Message As String
            Get
                Return _outputMessage
            End Get
            Set(value As String)
                _outputMessage = value
            End Set
        End Property

        Public Property Level As ViewModelValidationRule.ResultLevels
            Get
                Return _resultLevel
            End Get
            Set(value As ViewModelValidationRule.ResultLevels)
                _resultLevel = value
            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="propertyName"></param>
        ''' <param name="validationMethod"></param>
        ''' <param name="message"></param>
        ''' <param name="resultLevel"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal propertyName As String, ByVal validationMethod As Func(Of Boolean), ByVal message As String, Optional ByVal resultLevel As ViewModelValidationRule.ResultLevels = ResultLevels.IsError)

            'set the properties
            With Me
                .PropertyName = propertyName
                .Method = validationMethod
                .Message = message
                .Level = resultLevel
            End With

        End Sub

#End Region

    End Class

End Namespace
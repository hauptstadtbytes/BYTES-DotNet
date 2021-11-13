Namespace WPF.MVVM

    ''' <summary>
    ''' class for a managed set of 'ViewmodelvalidationRule' objects
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ViewModelValidationRulesSet

#Region "private variable(s)"

        Private _validationRules As Dictionary(Of String, List(Of ViewModelValidationRule)) = New Dictionary(Of String, List(Of ViewModelValidationRule))

#End Region

#Region "public Properties"

        Public ReadOnly Property Rules(ByVal propertyName As String) As List(Of ViewModelValidationRule)
            Get

                If _validationRules.ContainsKey(propertyName) Then
                    Return _validationRules(propertyName)
                Else
                    Return New List(Of ViewModelValidationRule)
                End If

            End Get
        End Property

        Public ReadOnly Property PropertyNames() As IEnumerable(Of String)
            Get

                Return _validationRules.Keys

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

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method for adding a new rule
        ''' </summary>
        ''' <param name="rule"></param>
        ''' <remarks></remarks>
        Public Sub Add(ByVal rule As ViewModelValidationRule)

            'create a new dictionary entry if required
            If Not _validationRules.ContainsKey(rule.PropertyName) Then
                _validationRules.Add(rule.PropertyName, New List(Of ViewModelValidationRule))
            End If

            'add the new item
            _validationRules(rule.PropertyName).Add(rule)

        End Sub

        ''' <summary>
        ''' method for clearing the set from rules (for a specific property)
        ''' </summary>
        ''' <param name="propertyName"></param>
        ''' <remarks></remarks>
        Public Sub Clear(Optional ByVal propertyName As String = Nothing)

            If Not IsNothing(propertyName) Then

                If _validationRules.ContainsKey(propertyName) Then
                    _validationRules.Remove(propertyName)
                End If

            Else
                _validationRules.Clear()
            End If

        End Sub

#End Region

    End Class

End Namespace
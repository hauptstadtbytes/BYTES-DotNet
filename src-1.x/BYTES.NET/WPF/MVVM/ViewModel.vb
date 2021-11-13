'import .net namespace(s) required
Imports System.ComponentModel

Imports System.Runtime.CompilerServices

Namespace WPF.MVVM

    ''' <summary>
    ''' view model base class
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class ViewModel

        Inherits Observable

        Implements INotifyDataErrorInfo

#Region "method(s) supporting the 'INotifypropertyChanged' interface (inherited from 'Observable)"

        ''' <summary>
        ''' overloaded method for raising the 'PropertyChanged' event
        ''' </summary>
        ''' <param name="doReValidation"></param>
        ''' <param name="propertyName"></param>
        Public Overloads Sub OnPropertyChanged(ByVal doReValidation As Boolean, <CallerMemberName> Optional ByVal propertyName As String = Nothing)

            'raise the 'PropertyChanged' event
            OnPropertyChanged(propertyName)

            'do a re-validation (if requested)
            If doReValidation Then

                Validate(propertyName)

            End If

        End Sub

        ''' <summary>
        ''' method for notifying on all properties changed
        ''' </summary>
        ''' <param name="doReValidation"></param>
        ''' <remarks>based on the article found at 'http://jobijoy.blogspot.de/2009/07/easy-way-to-update-all-ui-property.html'</remarks>
        Public Overloads Sub OnAllPropertiesChanged(Optional ByVal doReValidation As Boolean = False)

            'raise the 'PropertyChanged' event
            OnAllPropertiesChanged(doReValidation)

            'do a re-validation (if requested)
            If doReValidation Then

                Validate(Nothing)

            End If

        End Sub

#End Region

#Region "method(s) implementing the 'INotifyDataErrorInfo' interface"

        Public Event ErrorsChanged(sender As Object, e As DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

        ''' <summary>
        ''' method returning the errors for a dedicated property
        ''' </summary>
        ''' <param name="propertyName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors

            If _validationResults.ContainsKey(propertyName) Then

                Return _validationResults(propertyName)

            Else

                Return Nothing

            End If

        End Function

        ''' <summary>
        ''' method returning all errors or all errors for properties listed
        ''' </summary>
        ''' <param name="propertyNames"></param>
        ''' <returns></returns>
        Public Function GetErrors(Optional ByVal propertyNames As String() = Nothing) As ViewModelValidationResult()

            'create the output value
            Dim output As List(Of ViewModelValidationResult) = New List(Of ViewModelValidationResult)

            If IsNothing(propertyNames) Then 'no specific property was requested

                For Each pair As KeyValuePair(Of String, List(Of ViewModelValidationResult)) In _validationResults

                    For Each result As ViewModelValidationResult In pair.Value

                        output.Add(result)

                    Next

                Next

            Else 'specific properties have been requested

                For Each prop As String In propertyNames

                    Dim errors As IEnumerable = GetErrors(prop)

                    If Not IsNothing(errors) Then

                        For Each err As ViewModelValidationResult In errors

                            output.Add(err)

                        Next

                    End If

                Next

            End If

            'return the output value
            Return output.ToArray

        End Function

        ''' <summary>
        ''' method returning a list of errors created by the 'Validate' method
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
            Get

                Return _validationResults.Count > 0

            End Get
        End Property

#End Region

#Region "method(s) supporting the 'INotifyDataErrorInfo' interface"

        Private _validationRules As ViewModelValidationRulesSet = New ViewModelValidationRulesSet
        Private _validationResults As Dictionary(Of String, List(Of ViewModelValidationResult)) = New Dictionary(Of String, List(Of ViewModelValidationResult))

        Public Property ValidationRules As ViewModelValidationRulesSet
            Get
                Return _validationRules
            End Get
            Set(value As ViewModelValidationRulesSet)

                _validationRules = value
                OnPropertyChanged()

            End Set
        End Property

        ''' <summary>
        ''' method for validating the properties using the validation rules set
        ''' </summary>
        ''' <param name="propertyName"></param>
        ''' <remarks></remarks>
        Public Sub Validate(Optional ByVal propertyName As String = Nothing)

            If Not IsNothing(propertyName) Then 'validate the rules for a specific property

                If Me.ValidationRules.PropertyNames.Contains(propertyName) Then 'check if there are rules listed for the respective property

                    'remove existing results for that property
                    If _validationResults.ContainsKey(propertyName) Then
                        _validationResults.Remove(propertyName)
                    End If

                    'validate all rules for that property
                    For Each singleRule As ViewModelValidationRule In Me.ValidationRules.Rules(propertyName)

                        If singleRule.Method() Then 'check if the method returns 'true'

                            'check for the dictionary entry
                            If Not _validationResults.ContainsKey(propertyName) Then
                                _validationResults.Add(propertyName, New List(Of ViewModelValidationResult))
                            End If

                            'add the result
                            _validationResults(propertyName).Add(New ViewModelValidationResult(propertyName, singleRule.Level, singleRule.Message))

                        End If

                    Next

                End If

            Else 'validate all rules

                Dim outputValue As Dictionary(Of String, List(Of ViewModelValidationResult)) = New Dictionary(Of String, List(Of ViewModelValidationResult))

                For Each singleProperty As String In Me.ValidationRules.PropertyNames 'loop for each single property

                    For Each singleRule As ViewModelValidationRule In Me.ValidationRules.Rules(singleProperty) 'loop for each single rule

                        If singleRule.Method() Then 'check if the method returns 'true'

                            'check for the dictionary entry
                            If Not outputValue.ContainsKey(singleRule.PropertyName) Then
                                outputValue.Add(singleRule.PropertyName, New List(Of ViewModelValidationResult))
                            End If

                            'add the result
                            outputValue(singleRule.PropertyName).Add(New ViewModelValidationResult(singleProperty, singleRule.Level, singleRule.Message))

                        End If

                    Next

                Next

                'set the validation results
                _validationResults = outputValue

            End If

            'notify the WPF framework on errors changed
            OnErrorsChanged()
            OnPropertyChanged("HasErrors")

        End Sub

        ''' <summary>
        ''' method for raising the 'ErrorsChanged' event on errors list changed
        ''' </summary>
        ''' <param name="propertyName"></param>
        ''' <remarks></remarks>
        Private Sub OnErrorsChanged(Optional ByVal propertyName As String = Nothing)

            If Not IsNothing(propertyName) Then

                RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(propertyName))

            Else

                RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(Nothing))

            End If

        End Sub

#End Region

#Region "method(s) supporting command-handling"

        Private _commands As Dictionary(Of String, ViewModelRelayCommand) = New Dictionary(Of String, ViewModelRelayCommand)

        Public Property Commands() As Dictionary(Of String, ViewModelRelayCommand)
            Get
                Return _commands
            End Get
            Set(value As Dictionary(Of String, ViewModelRelayCommand))

                _commands = value

                OnPropertyChanged()

            End Set
        End Property

        Public Property Commands(ByVal identifyer As String) As ViewModelRelayCommand
            Get

                If _commands.ContainsKey(identifyer) Then
                    Return _commands(identifyer)
                Else
                    Return Nothing
                End If

            End Get
            Set(value As ViewModelRelayCommand)

                If _commands.ContainsKey(identifyer) Then
                    _commands(identifyer) = value
                Else
                    _commands.Add(identifyer, value)
                End If

                OnPropertyChanged()

            End Set
        End Property

#End Region


    End Class

End Namespace
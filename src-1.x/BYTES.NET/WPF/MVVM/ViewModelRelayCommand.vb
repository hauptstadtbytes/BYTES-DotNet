Namespace WPF.MVVM

    ''' <summary>
    ''' base class for a flexible view model relay command
    ''' </summary>
    ''' <remarks>based on the article found at 'http://www.cocktailsandcode.de/2012/04/mvvm-tutorial-part-3-viewmodelbase-und-relaycommand/'</remarks>
    Public Class ViewModelRelayCommand

#Region "method(s) implementing 'ICommand' interface"

        Implements ICommand

        Public Event CanExecuteChanged(sender As Object, e As EventArgs) Implements ICommand.CanExecuteChanged

        ''' <summary>
        ''' method for retrieving a boolean indicating wheather a the command can be executed or not
        ''' </summary>
        ''' <param name="parameter"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute

            Return _canExecute

        End Function

        ''' <summary>
        ''' method for executing the command
        ''' </summary>
        ''' <param name="parameter"></param>
        ''' <remarks></remarks>
        Public Sub Execute(parameter As Object) Implements ICommand.Execute

            If Not IsNothing(_action) Then
                _action(parameter)
            Else

                If Not IsNothing(_parameterLessAction) Then
                    _parameterLessAction()
                End If

            End If

        End Sub

#End Region

#Region "private variable(s)"

        Private ReadOnly _action As Action(Of Object) = Nothing
        Private ReadOnly _parameterLessAction As Action = Nothing

        Private _canExecute As Boolean = Nothing

#End Region

#Region "public poperties"

        ''' <summary>
        ''' property determining wheather the command can be executed or not
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsEnabled As Boolean
            Get

                Return _canExecute

            End Get
            Set(value As Boolean)

                _canExecute = value
                Me.OnCanExecuteChanged()

            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="action"></param>
        ''' <param name="canExecute"></param>
        ''' <remarks></remarks>
        Public Sub New(action As Action(Of Object), Optional ByVal canExecute As Boolean = True)

            'set the variable(s)
            _action = action
            _canExecute = canExecute

        End Sub

        ''' <summary>
        ''' overvloaded new instance method, accepting a parameter-less action
        ''' </summary>
        ''' <param name="action"></param>
        ''' <param name="canExecute"></param>
        ''' <remarks></remarks>
        Public Sub New(action As Action, Optional ByVal canExecute As Boolean = True)

            'set the variable(s)
            _parameterLessAction = action
            _canExecute = canExecute

        End Sub

#End Region

#Region "public method(s)"

        ''' <summary>
        ''' method for updating the 'CanExecute' status
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub OnCanExecuteChanged()

            RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)

        End Sub

#End Region

    End Class

End Namespace
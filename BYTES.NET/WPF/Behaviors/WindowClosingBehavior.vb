Imports System.ComponentModel

Namespace WPF.Behaviors

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>based on the article found at 'https://www.codeproject.com/Articles/73251/Handling-a-Window-s-Closed-and-Closing-events-in-t'</remarks>
    Public Class WindowClosingBehavior

#Region "define the attached properties"

        Public Shared ReadOnly ClosedProperty As DependencyProperty = DependencyProperty.RegisterAttached("Closed", GetType(ICommand), GetType(WindowClosingBehavior), New UIPropertyMetadata(New PropertyChangedCallback(AddressOf ClosedChanged)))
        Public Shared ReadOnly ClosingProperty As DependencyProperty = DependencyProperty.RegisterAttached("Closing", GetType(ICommand), GetType(WindowClosingBehavior), New UIPropertyMetadata(New PropertyChangedCallback(AddressOf ClosingChanged)))
        Public Shared ReadOnly CancelClosingProperty As DependencyProperty = DependencyProperty.RegisterAttached("CancelClosing", GetType(ICommand), GetType(WindowClosingBehavior))

        Public Shared Function GetClosed(ByVal obj As DependencyObject) As ICommand
            Return CType(obj.GetValue(ClosedProperty), ICommand)
        End Function

        Public Shared Sub SetClosed(ByVal obj As DependencyObject, ByVal value As ICommand)
            obj.SetValue(ClosedProperty, value)
        End Sub

        Public Shared Function GetClosing(ByVal obj As DependencyObject) As ICommand
            Return CType(obj.GetValue(ClosingProperty), ICommand)
        End Function

        Public Shared Sub SetClosing(ByVal obj As DependencyObject, ByVal value As ICommand)
            obj.SetValue(ClosingProperty, value)
        End Sub

        Public Shared Function GetCancelClosing(ByVal obj As DependencyObject) As ICommand
            Return CType(obj.GetValue(CancelClosingProperty), ICommand)
        End Function

        Public Shared Sub SetCancelClosing(ByVal obj As DependencyObject, ByVal value As ICommand)
            obj.SetValue(CancelClosingProperty, value)
        End Sub

#End Region

#Region "private property changed method(s)"

        Private Shared Sub ClosedChanged(ByVal target As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            Dim window As Window = TryCast(target, Window)

            If window IsNot Nothing Then

                If e.NewValue IsNot Nothing Then

                    AddHandler window.Closed, AddressOf Window_Closed

                Else

                    RemoveHandler window.Closed, AddressOf Window_Closed

                End If
            End If
        End Sub

        Private Shared Sub ClosingChanged(ByVal target As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            Dim window As Window = TryCast(target, Window)

            If window IsNot Nothing Then

                If e.NewValue IsNot Nothing Then

                    AddHandler window.Closing, AddressOf Window_Closing

                Else

                    RemoveHandler window.Closing, AddressOf Window_Closing

                End If
            End If
        End Sub

#End Region

#Region "private method(s)"

        Private Shared Sub Window_Closed(ByVal sender As Object, ByVal e As EventArgs)
            Dim closed As ICommand = GetClosed(TryCast(sender, Window))

            If closed IsNot Nothing Then
                closed.Execute(Nothing)
            End If
        End Sub

        Private Shared Sub Window_Closing(ByVal sender As Object, ByVal e As CancelEventArgs)
            Dim closing As ICommand = GetClosing(TryCast(sender, Window))

            If closing IsNot Nothing Then

                If closing.CanExecute(Nothing) Then
                    closing.Execute(Nothing)
                Else
                    Dim cancelClosing As ICommand = GetCancelClosing(TryCast(sender, Window))

                    If cancelClosing IsNot Nothing Then
                        cancelClosing.Execute(Nothing)
                    End If

                    e.Cancel = True
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
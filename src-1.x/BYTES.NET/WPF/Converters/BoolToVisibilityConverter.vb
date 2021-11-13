'import .net namespace(s) needed
Imports System.Globalization

Namespace WPF.Converters

    ''' <summary>
    ''' converter returning the visibility state by boolean value
    ''' </summary>
    Public Class BoolToVisibilityConverter

        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

            If IsNothing(parameter) OrElse CBool(parameter) Then 'return 'visible' on 'true'

                If CBool(value) Then

                    Return Visibility.Visible

                Else

                    Return Visibility.Collapsed

                End If

            Else

                If Not CBool(value) Then

                    Return Visibility.Visible

                Else

                    Return Visibility.Collapsed

                End If

            End If

        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack

            Return Binding.DoNothing

        End Function

    End Class

End Namespace
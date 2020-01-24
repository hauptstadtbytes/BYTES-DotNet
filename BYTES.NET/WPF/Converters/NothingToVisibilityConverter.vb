'import .net namespace(s) needed
Imports System.Globalization

Namespace WPF.Converters

    ''' <summary>
    ''' converter returning the visibility state based on an object being nothing or not nothing
    ''' </summary>
    ''' <remarks>'Visible' will be returned by default, if value is 'Nothing'; if parameter is set to 'false', 'Visible' will be returned if value is not 'Nothing'; if para´meter is set to 'true', strings of length 0 will be handled like 'Nothing'</remarks>
    Public Class NothingToVisibilityConverter

        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

            If IsNothing(parameter) OrElse String.IsNullOrEmpty(parameter) Then 'no parameter was given

                If IsNothing(value) Then

                    Return Visibility.Visible

                Else

                    Return Visibility.Collapsed

                End If

            Else 'a parameter was given

                Try

                    Dim comparing As Boolean = CBool(parameter)

                    If comparing Then

                        If IsNothing(value) = comparing OrElse String.IsNullOrEmpty(value) Then

                            Return Visibility.Visible

                        Else

                            Return Visibility.Collapsed

                        End If

                    Else

                        If Not IsNothing(value) Then

                            Return Visibility.Visible

                        Else

                            Return Visibility.Collapsed

                        End If

                    End If

                Catch ex As Exception

                    Throw New ArgumentException("Unable to convert value")

                End Try

            End If

        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack

            Return Binding.DoNothing

        End Function

    End Class

End Namespace
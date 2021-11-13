'import .net namespace(s) needed
Imports System.Globalization

Namespace WPF.Converters

    ''' <summary>
    ''' converter returning the visibility state by the items count of a given object
    ''' </summary>
    Public Class CountToVisibilityConverter

        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

            If IsNothing(parameter) Then

                Try

                    If value.Count >= 1 Then 'check for 'Count'

                        Return Visibility.Visible

                    Else

                        Return Visibility.Collapsed

                    End If

                Catch ex As Exception

                    Try

                        If value.length >= 1 Then 'check for 'Length' (e.g. in arrays)

                            Return Visibility.Visible

                        Else

                            Return Visibility.Collapsed

                        End If

                    Catch exer As Exception

                        Return Visibility.Collapsed

                    End Try

                End Try

            Else

                Try

                    If value.count >= CInt(parameter) Then 'check for 'Count'

                        Return Visibility.Visible

                    Else

                        Return Visibility.Collapsed

                    End If

                Catch ex As Exception

                    Try

                        If value.length >= CInt(parameter) Then 'check for 'Length' (e.g. in arrays)

                            Return Visibility.Visible

                        Else

                            Return Visibility.Collapsed

                        End If

                    Catch exer As Exception

                        Return Visibility.Collapsed

                    End Try

                End Try

            End If

        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack

            Return Binding.DoNothing

        End Function

    End Class

End Namespace
'import .net namespace(s) required
Imports System.Globalization

Namespace WPF.Converters

    ''' <summary>
    ''' converter returning a boolean based on an object being nothing or not nothing
    ''' </summary>
    ''' <remarks>by default 'true' will be returned if the object is nothing; if parameter is set to 'false', 'true' will be returned if value is not 'Nothing'; if parameter is set to 'true', 'true' will be returned for an "empty string" also</remarks>
    Public Class NothingToBoolConverter

        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

            If IsNothing(parameter) OrElse String.IsNullOrEmpty(parameter) Then 'no parameter was given

                If IsNothing(value) Then

                    Return True

                Else

                    Return False

                End If

            Else 'a parameter was given

                Try

                    Dim comparing As Boolean = CBool(parameter)

                    If comparing Then

                        If IsNothing(value) = comparing OrElse String.IsNullOrEmpty(value) Then

                            Return True

                        Else

                            Return False

                        End If

                    Else

                        If Not IsNothing(value) Then

                            Return True

                        Else

                            Return False

                        End If

                    End If

                Catch ex As Exception

                    Throw New ArgumentException("Unable to convert value")

                End Try

            End If

        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack

            Return Nothing

        End Function

    End Class

End Namespace
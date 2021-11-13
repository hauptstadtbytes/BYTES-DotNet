Imports System.Globalization

Namespace WPF.Converters

    ''' <summary>
    ''' converter inverting a boolean value
    ''' </summary>
    Public Class BoolInvertConverter

        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert

            Try

                If CBool(value) Then

                    Return False

                End If

                Return True

            Catch ex As Exception

                Return False

            End Try

        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack

            Try

                If CBool(value) Then

                    Return False

                End If

                Return True

            Catch ex As Exception

                Return False

            End Try

        End Function

    End Class

End Namespace
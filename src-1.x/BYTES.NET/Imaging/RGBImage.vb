'import internal namespace(s) required
Imports BYTES.NET.Imaging.API

Namespace Imaging

    Public Class RGBImage

        Implements IImage

#Region "private method(s)"

        Private _name As String = Guid.NewGuid.ToString

        Private _red As GraylevelImage = Nothing
        Private _green As GraylevelImage = Nothing
        Private _blue As GraylevelImage = Nothing

#End Region

#Region "public properties implementing the 'IImage' interface"

        Public Property Name As String Implements IImage.Name
            Get

                Return _name

            End Get
            Set(value As String)

                _name = value

            End Set
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="red"></param>
        ''' <param name="green"></param>
        ''' <param name="blue"></param>
        Public Sub New(ByRef red As GraylevelImage, ByRef green As GraylevelImage, ByRef blue As GraylevelImage)

            'set the variable(s)
            _red = red
            _green = green
            _blue = blue

        End Sub

#End Region

#Region "public method(s) implementing the 'IImage' interface"

        ''' <summary>
        ''' method returning the BitmapSource for displaying the image
        ''' </summary>
        ''' <returns></returns>
        Public Function GetBitmapSource() As BitmapSource Implements IImage.GetBitmapSource

            'get the pixel values
            Dim redValues As Integer(,) = _red.Values
            Dim greenValues As Integer(,) = _green.Values
            Dim blueValues As Integer(,) = _blue.Values

            'get the dimensions
            Dim width As Integer = _red.Width
            Dim Height As Integer = _red.Height

            'assemble the image data
            Dim stride As Integer = width * 3
            Dim numPaddingBytes As Integer = 0

            While True
                numPaddingBytes = stride Mod 4
                If (stride Mod 4) = 0 Then
                    Exit While
                End If

                stride += 1
                numPaddingBytes += 1
            End While

            Dim bits((stride - numPaddingBytes) * Height - 1) As Byte

            For col = 0 To width - 1
                For row = 0 To Height - 1

                    Dim pos As Integer = row * (stride - numPaddingBytes) + col * 3

                    bits(pos) = redValues(col, row)
                    bits(pos + 1) = greenValues(col, row)
                    bits(pos + 2) = blueValues(col, row)

                Next
            Next

            'return the output image
            Return BitmapSource.Create(width, Height, Nothing, Nothing, PixelFormats.Rgb24, Nothing, bits, stride)

        End Function

#End Region

    End Class

End Namespace
'import .net namespace(s) required
Imports System.Drawing

'import internal namespace(s) required
Imports BYTES.NET.Imaging.API

Namespace Imaging.Parsers

    <ImageParserMetadata(ID:="BYTES_ImageParser_JPG", Name:="JPG Image File Parser", FileExtensions:={"JPG"})>
    Public Class ParseJPG

        Implements IImageParser

#Region "Public method(s) implementing the 'IImageParser' interface"

        ''' <summary>
        ''' method loading the image(s) from disk file
        ''' </summary>
        ''' <param name="diskPath"></param>
        ''' <returns></returns>
        Public Function Load(diskPath As String) As IImage() Implements IImageParser.Load

            'load the file
            Dim source As Bitmap = New Bitmap(diskPath)

            'read the pixel values for the color channels
            Dim redValues As Integer(,) = New Integer(source.Width, source.Height) {}
            Dim greenValues As Integer(,) = New Integer(source.Width, source.Height) {}
            Dim blueValues As Integer(,) = New Integer(source.Width, source.Height) {}


            For i = 0 To source.Width - 1 Step 1

                For k = 0 To source.Height - 1 Step 1

                    redValues(i, k) = source.GetPixel(i, k).R
                    greenValues(i, k) = source.GetPixel(i, k).G
                    blueValues(i, k) = source.GetPixel(i, k).B

                Next

            Next

            'create the image instance(s)
            Dim red As GraylevelImage = New GraylevelImage(redValues) With {.Name = "The red channel"}
            Dim green As GraylevelImage = New GraylevelImage(greenValues) With {.Name = "The green channel"}
            Dim blue As GraylevelImage = New GraylevelImage(blueValues) With {.Name = "The blue channel"}

            'assemble the output value
            Dim output As List(Of IImage) = New List(Of IImage)

            output.Add(red)
            output.Add(green)
            output.Add(blue)
            output.Add(New RGBImage(red, green, blue) With {.Name = "The RGB image"})

            'return the output value
            Return output.ToArray

        End Function

#End Region

    End Class

End Namespace
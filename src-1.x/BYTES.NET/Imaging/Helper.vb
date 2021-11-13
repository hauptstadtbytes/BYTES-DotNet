'import .net namespace(s) required
Imports System.IO

Namespace Imaging

    Public Class Helper

#Region "shared conversion method(s) byte() <> 'System.Drwaing.Bitmap'"

        ''' <summary>
        ''' method for converting a 'Bitmap' to byte array
        ''' </summary>
        ''' <param name="srcData"></param>
        ''' <returns></returns>
        ''' <remarks>based on the thread on 'http://stackoverflow.com/questions/12961144/how-to-convert-system-drawing-image-to-byte-array'</remarks>
        Shared Function CastBitmapToByte(ByVal srcData As System.Drawing.Bitmap) As Byte()

            Try

                Dim memStream As New MemoryStream

                srcData.Save(memStream, System.Drawing.Imaging.ImageFormat.Bmp)

                Dim bytes As Byte() = memStream.ToArray

                Return bytes

            Catch ex As Exception

                Return Nothing

            End Try

        End Function

        ''' <summary>
        ''' method for converting a byte array to 'Bitmap'
        ''' </summary>
        ''' <param name="srcData"></param>
        ''' <returns></returns>
        ''' <remarks>based on the article found at 'http://dotnet-snippets.de/snippet/bytearray-to-image-image-to-bytearray/677'</remarks>
        Shared Function CastByteToBitmap(ByVal srcData As Byte()) As System.Drawing.Bitmap

            Try

                Dim memStream As MemoryStream = New MemoryStream(srcData)
                Dim newBitmap As System.Drawing.Bitmap = System.Drawing.Bitmap.FromStream(memStream)

                Return newBitmap

            Catch ex As Exception

                Return Nothing

            End Try

        End Function

#End Region

#Region "shared conversion method(s) byte() <> 'BitmapImage'"

        ''' <summary>
        ''' method for converting a byte array to 'BitmapImage'
        ''' </summary>
        ''' <param name="srcData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function CastByteToBitmapImage(ByVal srcData As Byte()) As BitmapImage

            Try

                Dim outputValue As BitmapImage = New BitmapImage

                Dim memStream As MemoryStream = New MemoryStream(srcData)

                With outputValue
                    .BeginInit()
                    .StreamSource = memStream
                    .EndInit()
                End With

                Return outputValue

            Catch ex As Exception

                Return Nothing

            End Try

        End Function

        ''' <summary>
        ''' method for converting a 'BitmapImage' to byte array
        ''' </summary>
        ''' <param name="srcData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function CastBitmapImageToByte(ByVal srcData As BitmapImage) As Byte()

            Try

                Return CastBitmapToByte(CastBitmapImageToBitmap(srcData))

            Catch ex As Exception

                Return Nothing

            End Try

        End Function

#End Region

#Region "shared conversion method(s) byte() <> 'System.Drawing.Image'"

        ''' <summary>
        ''' method for converting a 'System.Drawing.Image' to Byte()
        ''' </summary>
        ''' <param name="srcData"></param>
        ''' <param name="srcFormat"></param>
        ''' <returns></returns>
        ''' <remarks>based on the article found at 'http://dotnet-snippets.de/snippet/bytearray-to-image-image-to-bytearray/677'</remarks>
        Shared Function CastImageToByte(ByVal srcData As System.Drawing.Image, Optional ByVal srcFormat As System.Drawing.Imaging.ImageFormat = Nothing) As Byte()

            Try

                If IsNothing(srcFormat) Then
                    srcFormat = System.Drawing.Imaging.ImageFormat.Png
                End If

                Dim memStream As MemoryStream = New MemoryStream

                srcData.Save(memStream, srcFormat)
                memStream.Flush()

                Return memStream.ToArray

            Catch ex As Exception

                Return Nothing

            End Try

        End Function

        ''' <summary>
        ''' method for converting a Byte() to 'System.Drawing.Image'
        ''' </summary>
        ''' <param name="srcData"></param>
        ''' <returns></returns>
        ''' <remarks>based on the article found at 'http://dotnet-snippets.de/snippet/bytearray-to-image-image-to-bytearray/677'</remarks>
        Shared Function CastByteToImage(ByVal srcData As Byte()) As System.Drawing.Image

            Try

                Dim memStream As MemoryStream = New MemoryStream(srcData)

                Dim outputValue As System.Drawing.Image = System.Drawing.Image.FromStream(memStream)

                Return outputValue

            Catch ex As Exception

                Return Nothing

            End Try
        End Function

#End Region

#Region "shared conversion method(s) 'BitmapImage' <> 'System.Drawing.Bitmap'"

        ''' <summary>
        ''' method for converting 'BitmapImage' to 'System.Drawing.Bitmap'
        ''' </summary>
        ''' <param name="srcData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function CastBitmapImageToBitmap(ByVal srcData As BitmapImage) As System.Drawing.Bitmap

            Try

                Dim memStream As MemoryStream = New MemoryStream
                Dim imgEncoder As BitmapEncoder = New PngBitmapEncoder

                With imgEncoder
                    .Frames.Add(BitmapFrame.Create(srcData))
                    .Save(memStream)
                End With

                Dim outputValue As New System.Drawing.Bitmap(memStream)

                Return outputValue

            Catch ex As Exception

                Return Nothing

            End Try

        End Function

        ''' <summary>
        ''' method for converting 'System.Drawing.Bitmap' to 'BitmapImage'
        ''' </summary>
        ''' <param name="srcData"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function CastBitmapToBitmapImage(ByVal srcData As System.Drawing.Bitmap) As BitmapImage

            Try

                Dim outputValue As BitmapImage = New BitmapImage

                Dim stream As MemoryStream = New MemoryStream

                srcData.Save(stream, System.Drawing.Imaging.ImageFormat.Png)

                With outputValue
                    .BeginInit()
                    .StreamSource = stream
                    .EndInit()
                End With

                Return outputValue

            Catch ex As Exception

                Return Nothing

            End Try

        End Function

#End Region

#Region "shared conversion method(s) 'System.Drawing.Bitmap' <> 'Bitmapsource'"

        ''' <summary>
        ''' method for converting a 'System.Drawing.Bitmap' to 'BitmapSource'
        ''' </summary>
        ''' <param name="srcData"></param>
        ''' <returns></returns>
        ''' <remarks>based on the article found at 'http://stackoverflow.com/questions/2284353/is-there-a-good-way-to-convert-between-bitmapsource-and-bitmap'</remarks>
        Shared Function CastBitmapToBitmapSource(ByVal srcData As System.Drawing.Bitmap) As BitmapSource

            Try

                Return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(srcData.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())

            Catch ex As Exception

                Return Nothing

            End Try

        End Function

        ''' <summary>
        ''' method for converting a 'BitmapSource' to 'System.Drawing.Bitmap'
        ''' </summary>
        ''' <param name="srcData"></param>
        ''' <returns></returns>
        ''' <remarks>based on the article found at 'http://stackoverflow.com/questions/2284353/is-there-a-good-way-to-convert-between-bitmapsource-and-bitmap'</remarks>
        Shared Function CastBitmapSourceToBitmap(ByVal srcData As BitmapSource) As System.Drawing.Bitmap

            Try

                Dim outputValue As System.Drawing.Bitmap

                Using outStream = New MemoryStream()

                    Dim enc As BitmapEncoder = New BmpBitmapEncoder()
                    enc.Frames.Add(BitmapFrame.Create(srcData))
                    enc.Save(outStream)

                    outputValue = New System.Drawing.Bitmap(outStream)

                End Using

                Return outputValue

            Catch ex As Exception

                Return Nothing

            End Try

        End Function

#End Region

    End Class

End Namespace
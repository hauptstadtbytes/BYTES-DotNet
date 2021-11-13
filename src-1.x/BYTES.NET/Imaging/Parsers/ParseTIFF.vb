'import .net namespace(s) required
Imports System.IO

Imports System.Drawing

'import internal namespace(s) required
Imports BYTES.NET.Imaging.API

Namespace Imaging.Parsers

    <ImageParserMetadata(ID:="BYTES_ImageParser_TIFF", Name:="TIFF Image File Parser", FileExtensions:={"TIFF", "TIF"})>
    Public Class ParseTIFF

        Implements IImageParser

#Region "Public method(s) implementing the 'IImageParser' interface"

        ''' <summary>
        ''' method loading the image(s) from disk file
        ''' </summary>
        ''' <param name="diskPath"></param>
        ''' <returns></returns>
        Public Function Load(diskPath As String) As IImage() Implements IImageParser.Load

            'open the file
            Dim fileReader As FileStream = New FileStream(diskPath, FileMode.Open)

            'get the image properties
            Dim width As Single
            Dim height As Single
            Dim format As String

            Using tif As Image = System.Drawing.Image.FromStream(fileReader, False, False)

                width = tif.PhysicalDimension.Width
                height = tif.PhysicalDimension.Height

                Select Case True
                    Case tif.PixelFormat.ToString.StartsWith("Format8")
                        format = "8Bit"
                    Case Else
                        format = "16Bit"
                End Select

            End Using

            'MsgBox("Width: " & width & "; Height:" & height & "; Format:" & format)

            'read all bytes
            Dim bytes As Byte()

            Dim byteData(fileReader.Length - 1) As Byte
            fileReader.Read(byteData, 0, fileReader.Length - 1)
            bytes = byteData

            fileReader.Close()

            'read the frame(s)
            Dim frames As List(Of GraylevelImage) = New List(Of GraylevelImage)
            Dim memStream As MemoryStream = New MemoryStream(bytes)
            Dim imagesDecoder As TiffBitmapDecoder = New TiffBitmapDecoder(memStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default)

            Dim frameIndex As Integer = -1

            For Each singleFrame As BitmapFrame In imagesDecoder.Frames 'loop for each frame inside the file

                'count up the frame index
                frameIndex += 1

                'convert the frame to byte data
                Dim newMemStream As MemoryStream = New MemoryStream
                Dim imageEncoder As New TiffBitmapEncoder
                Dim frameBytes As Byte() = Nothing

                With imageEncoder
                    .Compression = TiffCompressOption.None
                    .Frames.Add(imagesDecoder.Frames(frameIndex))
                End With

                imageEncoder.Save(newMemStream)
                frameBytes = newMemStream.ToArray

                newMemStream.Close()

                'add the channel
                Dim frame As GraylevelImage

                If format = "8Bit" Then

                    frame = New GraylevelImage(To8bitInteger(frameBytes, CInt(width), CInt(height))) With {.Name = "Frame " & frameIndex.ToString}

                Else

                    frame = New GraylevelImage(To16bitInteger(frameBytes, CInt(width), CInt(height)))

                End If

                frames.Add(frame)

            Next

            'create a new image 
            Return frames.ToArray

        End Function

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method converting a byte array integers, using the 8-bit schema
        ''' </summary>
        ''' <param name="bytes"></param>
        ''' <param name="width"></param>
        ''' <param name="height"></param>
        ''' <returns></returns>
        Private Function To8bitInteger(ByRef bytes As Byte(), ByVal width As Integer, ByVal height As Integer) As Integer(,)

            Dim output(width - 1, height - 1) As Integer

            Dim stride As Integer = width

            For col = 0 To width - 1 Step 1

                For row = 0 To height - 1 Step 1

                    Dim index As Integer = (row * stride) + col

                    output(col, row) = BitConverter.ToUInt16(bytes, index)

                Next

            Next

            Return output

        End Function

        ''' <summary>
        ''' method converting a byte array integers, using the 16-bit schema
        ''' </summary>
        ''' <param name="bytes"></param>
        ''' <param name="width"></param>
        ''' <param name="height"></param>
        ''' <returns></returns>
        Private Function To16bitInteger(ByRef bytes As Byte(), ByVal width As Integer, ByVal height As Integer) As Integer(,)

            Dim output(width - 1, height - 1) As Integer

            Dim stride As Integer = (width * 16) / 8

            For col = 0 To width - 1 Step 1

                For row = 0 To height - 1 Step 1

                    Dim index As Integer = 8 + (row * stride + 2 * col) 'prepend additional bits

                    output(col, row) = BitConverter.ToUInt16(bytes, index)

                Next

            Next

            Return output

        End Function

#End Region

    End Class

End Namespace
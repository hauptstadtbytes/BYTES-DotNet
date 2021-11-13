'import internal namespace(s) required
Imports BYTES.NET.Imaging.API

Imports BYTES.NET.Math

Namespace Imaging

    Public Class GraylevelImage

        Implements IImage

#Region "private variable(s)"

        Private _name As String = Guid.NewGuid.ToString

        Private _pxValues As Integer(,) = Nothing
        Private _pxStatistics As MatrixStatistics(Of Integer) = Nothing

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

#Region "public properties"

        Public ReadOnly Property Values As Integer(,)
            Get

                Return _pxValues

            End Get
        End Property

        Public ReadOnly Property Statistics As MatrixStatistics(Of Integer)
            Get

                Return _pxStatistics

            End Get
        End Property

        Public ReadOnly Property Width As Integer
            Get

                Return _pxStatistics.xLength

            End Get
        End Property

        Public ReadOnly Property Height As Integer
            Get

                Return _pxStatistics.yLength

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="px"></param>
        Public Sub New(ByVal px As Integer(,))

            'set the variable(s)
            _pxValues = px
            _pxStatistics = New MatrixStatistics(Of Integer)(_pxValues)

        End Sub

        ''' <summary>
        ''' overloaded new instance method
        ''' </summary>
        ''' <param name="values"></param>
        ''' <param name="width"></param>
        ''' <param name="height"></param>
        Public Sub New(ByVal values As Integer(), ByVal width As Integer, ByVal height As Integer)

            'set the variable(s)
            Dim pxValues(width - 1, height - 1) As Integer

            For col = 0 To width - 1 Step 1

                For row = 0 To height - 1 Step 1

                    pxValues(col, row) = values(col * height + row)

                Next

            Next

            _pxValues = pxValues
            _pxStatistics = New MatrixStatistics(Of Integer)(_pxValues)

        End Sub

#End Region

#Region "shared method(s)"

        ''' <summary>
        ''' method returning a binary sample image
        ''' </summary>
        ''' <returns></returns>
        Shared Function SampleBinary() As GraylevelImage

            Dim counter As Integer = 0

            Dim values As Integer(,) = New Integer(99, 99) {}

            For col = 0 To 99 Step 1

                For row = 0 To 99 Step 1

                    counter += 1

                    If counter Mod 4 = 0 Then

                        values(col, row) = 1

                    Else

                        values(col, row) = 0

                    End If

                Next

            Next

            Return New GraylevelImage(values)

        End Function

        ''' <summary>
        ''' method returning a 8-bit sample image
        ''' </summary>
        ''' <returns></returns>
        Shared Function Sample8Bit() As GraylevelImage

            Dim values As Integer(,) = New Integer(16, 16) {}

            For col = 0 To 16 Step 1

                For row = 0 To 16 Step 1

                    values(col, row) = col * row

                Next

            Next

            Return New GraylevelImage(values)

        End Function

        ''' <summary>
        ''' method returning a 16-bit sample image
        ''' </summary>
        ''' <returns></returns>
        Shared Function Sample16Bit() As GraylevelImage

            Dim values As Integer(,) = New Integer(255, 255) {}

            For col = 0 To 255 Step 1

                For row = 0 To 255 Step 1

                    values(col, row) = col * row

                Next

            Next

            Return New GraylevelImage(values)

        End Function

#End Region

#Region "public method(s) implementing the 'IImage' interface"

        ''' <summary>
        ''' method returning the BitmapSource for displaying the image
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>based on the article found at 'http://www.i-programmer.info/programming/wpf-workings/500-bitmapsource-wpf-bitmaps-1.html?start=1'</remarks>
        Public Function GetBitmapSource() As BitmapSource Implements IImage.GetBitmapSource

            Dim imgFormat As PixelFormat = PixelFormats.Gray8

            Dim stride As Integer = Me.Statistics.xLength + (Me.Statistics.xLength) Mod 4

            Dim bits As Byte() = New Byte(Me.Height * stride - 1) {}

            For col = 0 To Me.Width - 1 Step 1

                For row = 0 To Me.Height - 1 Step 1

                    Select Case Me.Statistics.Maximum

                        Case <= 1 'binary image
                            bits(col + row * stride) = CByte(Values(col, row) * 255)

                        Case 2 To 255 '8-bit image
                            bits(col + row * stride) = CByte(Values(col, row))

                        Case Is > 255 '12- to 16-bit image
                            bits(col + row * stride) = CByte((Values(col, row) - Me.Statistics.Minimum) / (Me.Statistics.Maximum - Me.Statistics.Minimum) * 255) 'add the scaled value


                    End Select

                Next

            Next

            'return the output image
            Return BitmapSource.Create(Me.Width, Me.Height, Nothing, Nothing, imgFormat, Nothing, bits, stride)

        End Function

#End Region

    End Class


End Namespace
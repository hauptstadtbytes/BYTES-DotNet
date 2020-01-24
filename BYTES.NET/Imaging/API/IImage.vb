Namespace Imaging.API

    Public Interface IImage

        ''' <summary>
        ''' the name property
        ''' </summary>
        ''' <returns></returns>
        Property Name As String

        ''' <summary>
        ''' method returning a 'BitmapSource' class instance (i.e. for displaying the image in a WPF-based application) 
        ''' </summary>
        ''' <returns></returns>
        Function GetBitmapSource() As BitmapSource

    End Interface

End Namespace
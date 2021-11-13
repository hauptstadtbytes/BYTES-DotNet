Namespace MEF.API

    ''' <summary>
    ''' the interface for accessing the metadata defined by the 'Metadata' class type
    ''' </summary>
    ''' <remarks>based on the article found at 'https://stefanhenneken.wordpress.com/2011/06/05/mef-teil-2-metadaten-und-erstellungsrichtlinien'</remarks>
    Public Interface IMetadata

        ReadOnly Property ID As String

        ReadOnly Property InterfaceType As Type

    End Interface

End Namespace
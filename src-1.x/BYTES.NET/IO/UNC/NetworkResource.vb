Namespace IO.UNC

    ''' <summary>
    ''' the 'NetworkResource' class
    ''' </summary>
    ''' <remarks>based on the article found at 'http://stackoverflow.com/questions/295538/how-to-provide-user-name-and-password-when-connecting-to-a-network-share'</remarks>
    <Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)>
    Public Class NetworkResource

        Public Scope As ResourceScope
        Public ResourceType As ResourceType
        Public DisplayType As ResourceDisplaytype
        Public Usage As Integer
        Public LocalName As String
        Public RemoteName As String
        Public Comment As String
        Public Provider As String
    End Class

    Public Enum ResourceScope As Integer
        Connected = 1
        GlobalNetwork
        Remembered
        Recent
        Context
    End Enum

    Public Enum ResourceType As Integer
        Any = 0
        Disk = 1
        Print = 2
        Reserved = 8
    End Enum

    Public Enum ResourceDisplaytype As Integer
        Generic = &H0
        Domain = &H1
        Server = &H2
        Share = &H3
        File = &H4
        Group = &H5
        Network = &H6
        Root = &H7
        Shareadmin = &H8
        Directory = &H9
        Tree = &HA
        Ndscontainer = &HB
    End Enum

End Namespace
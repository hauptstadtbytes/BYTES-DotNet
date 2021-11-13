'import .net namespace(s) required
Imports System.IO
Imports System.Net
Imports System.Environment
Imports System.Net.NetworkInformation

Imports Microsoft.VisualBasic.Devices

Namespace IO.System

    Public Class Info

#Region "public properties"

        Public ReadOnly Property Name As String
            Get

                Return Dns.GetHostName

            End Get
        End Property

        Public ReadOnly Property Domain As String
            Get

                Dim localIPProperties As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties
                Return localIPProperties.DomainName

            End Get
        End Property

        Public ReadOnly Property Memory(Optional ByVal displayUnit As String = "GB", Optional ByVal fullUnitsOnly As Boolean = False) As Double
            Get

                Dim machine As ComputerInfo = New ComputerInfo()
                Return Helper.FormatMemory(machine.TotalPhysicalMemory, displayUnit, fullUnitsOnly)

            End Get
        End Property

        Public ReadOnly Property Processors As Integer
            Get

                Return Environment.ProcessorCount

            End Get
        End Property

        Public Overloads ReadOnly Property Drives As Dictionary(Of DriveType, List(Of Drive))
            Get

                Return GetClusteredDrives()

            End Get
        End Property

        Public Overloads ReadOnly Property Drives(ByVal driveType As DriveType) As Drive()
            Get

                If Not Me.Drives.Keys.Contains(driveType) Then

                    Return {}

                End If

                Return Me.Drives(driveType).ToArray

            End Get
        End Property

        Public Overloads ReadOnly Property Adapters As Dictionary(Of NetworkInterfaceType, List(Of Adapter))
            Get

                Return GetClusteredAdapters()

            End Get
        End Property

        Public Overloads ReadOnly Property Adapters(ByVal adapterType As NetworkInterfaceType) As Adapter()
            Get

                If Not Me.Adapters.Keys.Contains(adapterType) Then

                    Return {}

                End If

                Return Me.Adapters(adapterType).ToArray

            End Get
        End Property


        Public ReadOnly Property CurrentUser As User.Info
            Get

                Return New User.Info(UserName, Nothing, UserDomainName)

            End Get
        End Property

#End Region

#Region "private methods"

        ''' <summary>
        ''' function returning a list of all local drives, clustered by drive type
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetClusteredDrives() As Dictionary(Of DriveType, List(Of Drive))

            Dim output As Dictionary(Of DriveType, List(Of Drive)) = New Dictionary(Of DriveType, List(Of Drive))

            For Each singleDrive As DriveInfo In DriveInfo.GetDrives

                If Not (output.ContainsKey(singleDrive.DriveType)) Then

                    output.Add(singleDrive.DriveType, New List(Of Drive))

                End If

                output(singleDrive.DriveType).Add(New Drive(singleDrive))

            Next

            Return output

        End Function

        ''' <summary>
        ''' method retrieving a list of all network adapters
        ''' </summary>
        ''' <returns></returns>
        Private Function GetClusteredAdapters() As Dictionary(Of NetworkInterfaceType, List(Of Adapter))

            Dim output As Dictionary(Of NetworkInterfaceType, List(Of Adapter)) = New Dictionary(Of NetworkInterfaceType, List(Of Adapter))

            For Each adapter As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces

                If Not output.ContainsKey(adapter.NetworkInterfaceType) Then

                    output.Add(adapter.NetworkInterfaceType, New List(Of Adapter))

                End If

                output(adapter.NetworkInterfaceType).Add(New Adapter(adapter))

            Next

            Return output

        End Function

#End Region

    End Class

End Namespace
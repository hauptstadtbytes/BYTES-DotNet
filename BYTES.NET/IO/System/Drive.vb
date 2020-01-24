'import .net namespace(s) required
Imports System.IO
Imports System.Runtime.InteropServices

Namespace IO.System

    Public Class Drive

#Region "Win API reference(s)"

        <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
        Private Shared Function GetDiskFreeSpaceEx(lpDirectoryName As String, ByRef lpFreeBytesAvailable As ULong, ByRef lpTotalNumberOfBytes As ULong, ByRef lpTotalNumberOfFreeBytes As ULong) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

#End Region

#Region "private variable(s)"

        Private _drive As DriveInfo = Nothing

        Private _isRemoveable As Boolean = Nothing

        Private _totalSpace As Double = Nothing
        Private _freeSpace As Double = Nothing

#End Region

#Region "public properties"

        Public ReadOnly Property Type As DriveType
            Get

                Return _drive.DriveType

            End Get
        End Property

        Public ReadOnly Property IsRemoveable As Boolean
            Get

                Return _isRemoveable

            End Get
        End Property

        Public ReadOnly Property Path As String
            Get

                Return _drive.ToString

            End Get
        End Property

        Public ReadOnly Property IsReady As Boolean
            Get

                Return _drive.IsReady

            End Get
        End Property

        Public ReadOnly Property TotalSpace(Optional ByVal displayUnit As String = "GB", Optional ByVal fullUnitsOnly As Boolean = False) As Double
            Get

                If Not Me.IsReady Then

                    Return 0

                End If

                Return Helper.FormatMemory(_totalSpace, displayUnit, fullUnitsOnly)

            End Get
        End Property

        Public ReadOnly Property FreeSpace(Optional ByVal displayUnit As String = "GB", Optional ByVal fullUnitsOnly As Boolean = False) As Double
            Get

                If Not Me.IsReady Then

                    Return 0

                End If

                Return Helper.FormatMemory(_freeSpace, displayUnit, fullUnitsOnly)

            End Get
        End Property

#End Region

#Region "public new instance method(s)"

        ''' <summary>
        ''' default new instance method
        ''' </summary>
        ''' <param name="drive"></param>
        Public Sub New(ByVal drive As DriveInfo)

            'set variable(s)
            _drive = drive

            'initialize the data model
            Initialize()

        End Sub

        ''' <summary>
        ''' overloaded new instance method, allowing to access the drive by drive letter
        ''' </summary>
        ''' <param name="letter"></param>
        Public Sub New(ByVal letter As String)

            'set variable(s)
            _drive = New DriveInfo(letter)

            'initialize the data model
            Initialize()

        End Sub

#End Region

#Region "private method(s)"

        ''' <summary>
        ''' method initializing the data model
        ''' </summary>
        Private Sub Initialize()

            'set variable(s)
            _isRemoveable = GetIsRemoveable(_drive)

            'Get the drive space(s)
            GetDriveSpace(_drive.ToString, _freeSpace, _totalSpace)

        End Sub

        ''' <summary>
        ''' method checking for a removeable drive
        ''' </summary>
        ''' <param name="drive"></param>
        ''' <returns></returns>
        Private Function GetIsRemoveable(ByVal drive As DriveInfo) As Boolean

            Select Case drive.DriveType

                Case DriveType.CDRom
                    Return True

                Case DriveType.Removable
                    Return True

            End Select

            'return the default output value
            Return False

        End Function

        ''' <summary>
        ''' method determining the (free and total) disk space of a path given 
        ''' </summary>
        ''' <param name="folderName"></param>
        ''' <param name="freespace"></param>
        ''' <param name="totalspace"></param>
        ''' <remarks>based on the article found at 'https://stackoverflow.com/questions/1799984/finding-out-total-and-free-disk-space-in-net'</remarks>
        Private Sub GetDriveSpace(folderName As String, ByRef freespace As ULong, ByRef totalspace As ULong)

            If Not String.IsNullOrEmpty(folderName) Then

                If Not folderName.EndsWith("\") Then
                    folderName += "\"
                End If

                Dim free As ULong = 0
                Dim total As ULong = 0
                Dim dummy2 As ULong = 0

                If GetDiskFreeSpaceEx(folderName, free, total, dummy2) Then

                    freespace = free
                    totalspace = total

                End If

            End If

        End Sub

#End Region

    End Class

End Namespace
'import .net namespace(s) required
Imports System.Reflection

'import internal namespace(s) required
Imports BYTES.NET.Primitives

Namespace IO

    Public Class Helper

#Region "shared method(s)"

        ''' <summary>
        ''' method parsing a path string, expanding various masks
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="masks"></param>
        ''' <returns></returns>
        Shared Function ExpandPath(Optional ByVal path As String = Nothing, Optional ByVal masks As Dictionary(Of String, String) = Nothing) As String

            'check for empty path
            If IsNothing(path) Then

                Return Nothing

            End If

            'assemble the mask(s) collection
            If IsNothing(masks) Then

                masks = New Dictionary(Of String, String)

            End If

            If Not masks.ContainsKey("InstallationDir") Then
                masks.Add("InstallationDir", GetAppDirPath())
            End If

            If Not masks.ContainsKey("%BYTES.NET%") Then
                masks.Add("BYTES.NET%", GetLibraryAssemblyPath())
            End If

            'return the output, expanding default Windows environment variables as well as generic masks
            Return Environment.ExpandEnvironmentVariables(path.ExpandMasks(masks))

        End Function

        ''' <summary>
        ''' method returning the (executing) application's installation directory path
        ''' </summary>
        ''' <returns></returns>
        Shared Function GetAppDirPath() As String

            Return AppDomain.CurrentDomain.BaseDirectory

        End Function

        ''' <summary>
        ''' method returning the path to the current (library) assembly
        ''' </summary>
        ''' <returns></returns>
        Shared Function GetLibraryAssemblyPath() As String

            Dim uri As Uri = New Uri(Assembly.GetExecutingAssembly().GetName().CodeBase)
            Return uri.LocalPath

        End Function

        ''' <summary>
        ''' function formatting a given amount (of memory) to the unit given
        ''' </summary>
        ''' <param name="byteAmount"></param>
        ''' <param name="displayUnit"></param>
        ''' <param name="fullUnitsOnly"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function FormatMemory(ByVal byteAmount As ULong, ByVal displayUnit As String, ByVal fullUnitsOnly As Boolean) As Double

            If (displayUnit = "byte") Or (displayUnit = "b") Then

                Return Convert.ToDouble(byteAmount)

            Else


                Dim outputValue As ULong = 0

                Select Case displayUnit.ToLower

                    Case "kb"
                        outputValue = byteAmount / 1024

                    Case "mb"
                        outputValue = byteAmount / (1024 ^ 2)

                    Case "gb"
                        outputValue = byteAmount / (1024 ^ 3)

                    Case "tb"
                        outputValue = byteAmount / (1024 ^ 4)

                End Select

                If (fullUnitsOnly And (outputValue.ToString.Contains("."))) Then

                    Dim valueSplit() As String = outputValue.ToString.Split(".")

                    outputValue = CULng(valueSplit(0))

                End If

                Return Convert.ToDouble(outputValue)

            End If

        End Function

#End Region

    End Class

End Namespace
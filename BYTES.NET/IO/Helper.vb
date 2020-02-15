'import .net namespace(s) required
Imports System.Reflection

Imports System.Text.RegularExpressions

Namespace IO

    Public Class Helper

#Region "shared method(s)"

        ''' <summary>
        ''' method parsing a path string, expanding various masks
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="additionalMasks"></param>
        ''' <returns></returns>
        Shared Function ExpandPath(ByVal path As String, Optional ByVal additionalMasks As Dictionary(Of String, String) = Nothing) As String

            If IsNothing(additionalMasks) Then

                additionalMasks = New Dictionary(Of String, String)

            End If

            If Not additionalMasks.ContainsKey("%InstallationDir%") Then
                additionalMasks.Add("%InstallationDir%", GetAppDirPath())
            End If

            If Not additionalMasks.ContainsKey("%BYTES.NET%") Then
                additionalMasks.Add("%BYTES.NET%", GetLibraryAssemblyPath())
            End If

            For Each mask As KeyValuePair(Of String, String) In additionalMasks

                Dim toReplace As String = mask.Key

                If Not toReplace.StartsWith("%") Then

                    toReplace = "%" & toReplace

                End If

                If Not toReplace.EndsWith("%") Then

                    toReplace = toReplace & "%"

                End If

                path = Regex.Replace(path, toReplace, mask.Value, RegexOptions.IgnoreCase)

            Next

            'return the output, expanding default Windows environment variables
            Return Environment.ExpandEnvironmentVariables(path)

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
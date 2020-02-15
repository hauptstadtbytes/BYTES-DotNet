'import .net namespace(s) required
Imports System.Runtime.CompilerServices

Namespace Primitives

    Public Module StringExtensions

        ''' <summary>
        ''' a 'String' type extension method calculating the trigram-based similarity to another string
        ''' </summary>
        ''' <param name="theString"></param>
        ''' <param name="reference"></param>
        ''' <returns></returns>
        ''' <remarks>see 'https://www.innodox.com/de/blog/meier-ist-nicht-gleich-maier-ist-nicht-gleich-mayer-aehnlichkeitssuche-im-alltagstest/' for more details</remarks>
        <Extension()>
        Public Function TrigramSimilarityTo(theString As String, ByVal reference As String) As Double

            'get the trigrams
            Dim wordTrigrams As String() = GetTrigrams(theString)
            Dim queryTrigrams As String() = GetTrigrams(reference)

            'calculate the output
            Dim counter As Integer = 0
            For Each trigram As String In queryTrigrams

                If wordTrigrams.Contains(trigram) Then

                    counter += 1

                End If

            Next

            Return (2 * counter) / ((2 + reference.Length) + (2 + theString.Length))

        End Function

        Public Function TrigramSimilarityTo(theString As String, ByVal reference As String()) As Dictionary(Of String, Double)

            'create the output value
            Dim output As Dictionary(Of String, Double) = New Dictionary(Of String, Double)

            For Each val As String In reference

                output.Add(val, val.TrigramSimilarityTo(theString))

            Next

            'return the output value
            Return output

        End Function

        ''' <summary>
        ''' method returning the best match from a list of candidates
        ''' </summary>
        ''' <param name="theString"></param>
        ''' <param name="list"></param>
        ''' <returns></returns>
        <Extension()>
        Public Function GetBestTrigramSimilarityMatch(theString As String, ByVal list As String(), Optional ByVal threshold As Double = Nothing) As String

            'get the best match from options list
            Dim tmp As KeyValuePair(Of String, Double) = Nothing

            For Each opt As String In list

                If IsNothing(tmp) Then

                    tmp = New KeyValuePair(Of String, Double)(opt, opt.TrigramSimilarityTo(theString))

                Else

                    Dim similarity As Double = opt.TrigramSimilarityTo(theString)

                    If similarity > tmp.Value Then

                        tmp = New KeyValuePair(Of String, Double)(opt, similarity)

                    End If

                End If

            Next

            'return the output value
            If IsNothing(threshold) Then

                Return tmp.Key

            End If

            If tmp.Value >= threshold Then

                Return tmp.Key

            Else

                Return String.Empty

            End If

        End Function

#Region "private method(s)"

        ''' <summary>
        ''' method splitting a string to an array of trigrams
        ''' </summary>
        ''' <param name="source"></param>
        ''' <returns></returns>
        Private Function GetTrigrams(ByVal source As String) As String()

            Dim output As List(Of String) = New List(Of String)

            For i = 0 To source.Length + 1 Step 1

                Dim indx As Integer = i - 2

                Dim trigram As String = String.Empty

                Select Case indx

                    Case -2
                        trigram = "  " & source.Substring(0, 1).ToUpper

                    Case -1
                        trigram = " " & source.Substring(0, 2).ToUpper

                    Case (source.Length - 2)
                        trigram = source.Substring(indx, 2).ToUpper & " "

                    Case (source.Length - 1)
                        trigram = source.Substring(indx, 1).ToUpper & "  "

                    Case Else
                        trigram = source.Substring(indx, 3).ToUpper

                End Select

                output.Add(trigram)

            Next

            Return output.ToArray

        End Function

#End Region

    End Module

End Namespace
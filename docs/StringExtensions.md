# StringExtensions Class Documentation
## Overview
The StringExtensions class in the BYTES.NET.Primitives namespace provides various extension methods for string type operations. These methods include functionalities such as trimming characters, finding indexes of characters or patterns, checking for string containment, expanding variables, matching patterns using regular expressions, parsing key-value pairs, and calculating string similarities using different algorithms.

### Namespace
```csharp
//import .net (default) namespace(s)
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BYTES.NET.Primitives;

//import namespace(s) required from 'BYTES.NET.WPF' framework
using BYTES.NET.WPF.MVVM;
```

## Class Definition
```csharp
namespace BYTES.NET.Primitives
{
    public static class StringExtensions
    {
        // Methods defined here
    }
}
```

## Methods
### Trim (Available only in .NET Full)
```csharp
public static string Trim(this string text, char character)
```
Removes all leading or trailing instances of a character from the given string.

### Parameters:

text (string): The input string.
character (char): The character to trim.

### Returns:

string: The trimmed string.
### AllIndexesOf (Character)
```csharp
public static int[] AllIndexesOf(this string str, char character)
```
Returns a zero-based list of all indexes of a character inside a given string.

### Parameters:

str (string): The input string.
character (char): The character to find.

### Returns:

int[]: Array of indexes where the character occurs.

## AllIndexesOf (Regex)
```csharp
public static int[] AllIndexesOf(this string str, Regex expression)
```
Returns a zero-based list of all indexes of strings matching the given regular expression.

### Parameters:

str (string): The input string.
expression (Regex): The regular expression to match.

### Returns:

int[]: Array of indexes where the pattern occurs.

### Contains
```csharp
public static bool Contains(this string[] strings, string match, bool ignoreCase = true)
```
Checks if a string array contains a specific value, optionally ignoring case.

### Parameters:

strings (string[]): The array of strings.
match (string): The string to match.
ignoreCase (bool, optional): Ignore case when comparing. Default is true.

### Returns:

bool: true if the string is found, otherwise false.

## Expand
```csharp
public static string Expand(this string text, Dictionary<string, string> variables, bool ignoreCase = true)
```
Expands variables inside a given string.

### Parameters:

text (string): The input string.
variables (Dictionary<string, string>): Dictionary of variables to expand.
ignoreCase (bool, optional): Ignore case when matching variables. Default is true.

### Returns:

string: The expanded string.

## MatchesPattern (with out Match)
```csharp
public static bool MatchesPattern(this string text, Regex pattern, out Match? match)
```
Checks if the string matches the given pattern using regular expressions, returning the match.

### Parameters:

text (string): The input string.
pattern (Regex): The regular expression pattern.
match (Match, out): The match found.

### Returns:

bool: true if a match is found, otherwise false.

## MatchesPattern
```csharp
public static bool MatchesPattern(this string text, Regex pattern)
```
Checks if the string matches the given pattern using regular expressions.

### Parameters:

text (string): The input string.
pattern (Regex): The regular expression pattern.

### Returns:

bool: true if a match is found, otherwise false.

## ParseKeyValue
```csharp
public static KeyValuePair<string, string> ParseKeyValue(this string text, char[] equalitySigns = null)
```
Returns a key-value pair from a string containing an equality character.

### Parameters:

text (string): The input string.
equalitySigns (char[], optional): Array of equality signs. Default is ['=', ':'].

### Returns:

KeyValuePair<string, string>: The parsed key-value pair.

## SimilarityTo (Single Reference)
```csharp
public static double? SimilarityTo(this string text, string reference, string algorithm = "trigram")
```
Returns the string similarity to a given reference string using the specified algorithm.

### Parameters:

text (string): The input string.
reference (string): The reference string.
algorithm (string, optional): The algorithm to use ('trigram' or 'levenshtein'). Default is trigram.

### Returns:

double?: The similarity score.

## SimilarityTo (Multiple References)
```csharp
public static Dictionary<string, double> SimilarityTo(this string text, IEnumerable<string> reference, string algorithm = "trigram")
```
Returns the string similarity for an array of reference strings using the specified algorithm.

### Parameters:

text (string): The input string.
reference (IEnumerable<string>): The array of reference strings.
algorithm (string, optional): The algorithm to use ('trigram' or 'levenshtein'). Default is trigram.

### Returns:

Dictionary<string, double>: Dictionary of reference strings and their similarity scores.

## BestMatch
```csharp
public static string? BestMatch(this string text, IEnumerable<string> options, out double dist, string algorithm = "trigram", double? threshold = null)
```
Returns the best match from a list of options given, respecting an optional threshold.

### Parameters:

text (string): The input string.
options (IEnumerable<string>): The list of options.
dist (double, out): The similarity score of the best match.
algorithm (string, optional): The algorithm to use ('trigram' or 'levenshtein'). Default is trigram.
threshold (double?, optional): The threshold value for the similarity score. Default is null.

### Returns:

string?: The best match if it meets the threshold, otherwise null.

## BestMatch (Without out double)
```csharp
public static string? BestMatch(this string text, IEnumerable<string> options, string algorithm = "trigram", double? threshold = null)
```
Returns the best match from a list of options given, respecting an optional threshold

### Parameters:

text (string): The input string.
options (IEnumerable<string>): The list of options.
algorithm (string, optional): The algorithm to use ('trigram' or 'levenshtein'). Default is trigram.
threshold (double?, optional): The threshold value for the similarity score. Default is null.

### Returns:

string?: The best match if it meets the threshold, otherwise null.

## Private Helper Methods
### GetTrigrams
```csharp
private static string[] GetTrigrams(string source)
```
Calculates all trigrams from a given string.

### Parameters:

source (string): The input string.

### Returns:

string[]: Array of trigrams.

## TrigramSimilarityTo
```csharp
private static double TrigramSimilarityTo(string text, string reference)
```
Calculates the trigram-based similarity between two strings.

### Parameters:

text (string): The input string.
reference (string): The reference string.

### Returns:

double: The similarity score.

### GetBestMatchUsingTrigrams
```csharp
private static string? GetBestMatchUsingTrigrams(string text, IEnumerable<string> options, out double dist, double threshold = 0)
```
Returns the trigram-based best match from a list of string options, respecting a threshold value optionally.

### Parameters:

text (string): The input string.
options (IEnumerable<string>): The list of options.
dist (double, out): The similarity score of the best match.
threshold (double, optional): The threshold value for the similarity score. Default is 0.

### Returns:

string?: The best match if it meets the threshold, otherwise null.

### GetBestMatchUsingTrigrams(Without out double)
```csharp
private static string? GetBestMatchUsingTrigrams(string text, IEnumerable<string> options, double threshold = 0)
```
Returns the trigram-based best match from a list of string options, respecting a threshold value optionally.

### Parameters:

text (string): The input string.
options (IEnumerable<string>): The list of options.
threshold (double, optional): The threshold value for the similarity score. Default is 0.

### Returns:

string?: The best match if it meets the threshold, otherwise null.

## LevenshteinDistanceNormalizedTo
```csharp
private static double LevenshteinDistanceNormalizedTo(string first, string second)
```
Calculates the normalized Levenshtein distance between two strings.

### Parameters:

first (string): The first string.
second (string): The second string.

### Returns:

double: The normalized Levenshtein distance.

## GetBestMatchUsingLevenshteinDistanceNormalized
```csharp
private static string? GetBestMatchUsingLevenshteinDistanceNormalized(string text, IEnumerable<string> options, out double dist, double? threshold = 0)
```
Returns the Levenshtein-based best match from a list of string options, respecting a threshold value optionally.

### Parameters:

text (string): The input string.
options (IEnumerable<string>): The list of options.
dist (double, out): The similarity score of the best match.
threshold (double, optional): The threshold value for the similarity score. Default is 0.

### Returns:

string?: The best match if it meets the threshold, otherwise null.

## GetBestMatchUsingLevenshteinDistanceNormalized (Without out double)
```csharp
private static string? GetBestMatchUsingLevenshteinDistanceNormalized(string text, IEnumerable<string> options, double? threshold = 0)
```
Returns the Levenshtein-based best match from a list of string options, respecting a threshold value optionally.

### Parameters:

text (string): The input string.
options (IEnumerable<string>): The list of options.
threshold (double, optional): The threshold value for the similarity score. Default is 0.

### Returns:

string?: The best match if it meets the threshold, otherwise null.

## Pros and Cons of Algorithms
### Trigram Algorithm
#### Pros:

Speed: Trigram comparison is generally faster, especially for longer strings.

Efficiency: It works well for approximate matching and can handle misspellings or variations in text efficiently.

Simplicity: Easy to implement and understand.

#### Cons:

Accuracy: May not be as accurate for short strings or strings with significant differences.

Memory Usage: Can be memory-intensive for very long strings due to the generation of many trigrams.

### Levenshtein Algorithm
#### Pros:

Accuracy: Provides a precise measure of difference by considering insertions, deletions, and substitutions.

Flexibility: Effective for short strings and can accurately measure similarity even with significant differences.

Standard Metric: Widely recognized and used in various applications for string comparison.

#### Cons:

Speed: Can be slower compared to trigram, especially for longer strings.

Complexity: More computationally intensive, which can affect performance.

Implementation: More complex to implement compared to trigram.
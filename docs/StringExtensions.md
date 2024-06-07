# BYTES.NET.Primitives.StringExtensions Documentation

## Overview
The `BYTES.NET.Primitives.StringExtensions` class provides a collection of extension methods for the `System.String` class. These methods enhance the functionality of strings by offering utilities for trimming characters, finding character or regex pattern occurrences, checking if an array of strings contains a specific value, expanding variables within a string, and calculating string similarity using trigram and Levenshtein distance algorithms.

## Methods

### Trim
```csharp
Trim(this string text, char character)
```
Removes all leading or trailing instances of a specified character from the string.

#### Parameters:

text: The original string.
character: The character to remove.
Returns:

A string with leading and trailing instances of the specified character removed.
### AllIndexesOf(Character)
```csharp
AllIndexesOf(this string str, char character)
```
Returns a zero-based list of all indexes of a character inside a string.

#### Parameters:

str: The input string.
character: The character to find indexes for.
#### Returns:

An array of integers representing the indexes of the character in the string.
### AllIndexesOf (Regex)
```csharp
AllIndexesOf(this string str, Regex expression)
```
Returns a zero-based list of all indexes of strings matching the given regex expression.

#### Parameters:

str: The input string.
expression: The regex pattern to match.
#### Returns:

An array of integers representing the indexes of matches in the string.
### Contains
```csharp
Contains(this string[] strings, string match, bool ignoreCase = true)
```
Checks if a string array contains a specific value, optionally ignoring case.

#### Parameters:

strings: The array of strings to check.
match: The value to search for.
ignoreCase: A boolean indicating whether to ignore case during the search.
#### Returns:

A boolean indicating whether the value was found in the array.
### Expand
```csharp
Expand(this System.String text, Dictionary<string, string> variables, bool ignoreCase = true)
```
Expands variables inside a string based on a dictionary of variable names and their replacements.

#### Parameters:

text: The input string.
variables: A dictionary mapping variable names to their replacements.
ignoreCase: A boolean indicating whether to ignore case during replacement.
#### Returns:

A string with variables expanded according to the provided dictionary.
### MatchesPattern
```csharp
MatchesPattern(this string text, Regex pattern, out Match? match)
```
Eases regex-based matching for string values, returning the first successful match along with the match object.

#### Parameters:

text: The input string.
pattern: The regex pattern to match against.
match: An out parameter receiving the first successful match.
#### Returns:

A boolean indicating whether a match was found.
### SimilarityTo (String)
```csharp
SimilarityTo(this System.String text, string reference)
```
Calculates the trigram-based similarity between two strings.

#### Parameters:

text: The input string.
reference: The reference string to compare against.
#### Returns:

A double representing the similarity score.
### SimilarityTo (String Array)
```csharp
SimilarityTo(this System.String text, string[] reference)
```
Calculates the trigram-based similarity between a string and an array of reference strings.

#### Parameters:

text: The input string.
reference: An array of reference strings to compare against.
#### Returns:

A dictionary mapping reference strings to their similarity scores.
### GetBestMatch
```csharp
GetBestMatch(this System.String text, string[] options, double threshold = 0)
```
Finds the best match from a list of string options based on similarity, optionally considering a threshold.

#### Parameters:

text: The input string.
options: An array of potential matches.
threshold: The minimum similarity score to consider a match valid.
#### Returns:

The best matching string or an empty string if no match exceeds the threshold.
### LevenshteinDistanceNormalized
```csharp
LevenshteinDistanceNormalized(this string first, string second)
```
Calculates the normalized Levenshtein distance between two strings.

#### Parameters:

first: The first string.
second: The second string.
#### Returns:

The normalized Levenshtein distance between the two strings.
### MinimumLevenshteinDistanceNormalized
```csharp
MinimumLevenshteinDistanceNormalized(this string target, IEnumerable<string> candidates)
```
Finds the minimum normalized Levenshtein distance among a collection of strings to a target string.

#### Parameters:

target: The target string.
candidates: A collection of candidate strings.
#### Returns:

A dictionary with each candidate string and its corresponding normalized Levenshtein distance to the target string.
### LevenstheinIsSimilarWithinThreshold
```csharp
LevenstheinIsSimilarWithinThreshold(this string first, string second, double threshold)
```
Determines whether two strings are similar within a specified threshold.

#### Parameters:

first: The first string.
second: The second string.
threshold: The maximum allowed normalized Levenshtein distance for the strings to be considered similar.
#### Returns:

A boolean indicating whether the strings are similar within the threshold.
### GetBestMatchUsingLevenshtein
```csharp
GetBestMatchUsingLevenshtein(this string target, string[] options)
```
Determines the best match between the array of strings and the target.

#### Parameters:

target: The target string.
options: The array of string options.
#### Returns:

The best matching string.
### GetBestMatch
```csharp
GetBestMatch(string algorythm, string target, string[] options)
```
Selects which algorithm to use to determine the best match.

#### Parameters:

algorythm: The algorithm to use ("Trigram" or "Levensthein").
target: The target string.
options: The array of string options.
#### Returns:

The best matching string or a message indicating no algorithm was found.
### GetSimilarity
```csharp
GetSimilarity(string algorythm, string target, string option)
```
Selects which algorithm to use to determine the similarity.

#### Parameters:

algorythm: The algorithm to use ("Trigram" or "Levensthein").
target: The target string.
option: The option string.
#### Returns:

The similarity score or -1 if no algorithm was found.

### Trigram vs. Levenshtein Distance
#### Pros of Trigram Analysis
##### Performance: 
Generally faster for large datasets due to lower computational complexity.
##### Simplicity: 
Easier to implement and understand.
Memory Efficiency: Uses less memory as it operates on smaller substrings.
#### Cons of Trigram Analysis
##### Accuracy: 
Less accurate for short strings or strings with unique words, as common phrases might not share many trigrams.
##### Context Sensitivity: 
May not capture semantic meaning well, especially in languages with complex grammar.
#### Pros of Levenshtein Distance
##### Accuracy: 
More accurate for comparing strings, especially in cases where small changes lead to significant differences.
##### Flexibility: 
Can handle insertions, deletions, and substitutions more gracefully.
#### Cons of Levenshtein Distance
##### Performance: 
Significantly slower for large datasets due to higher computational complexity.
##### Complexity:
More complex to implement and understand.
Memory Usage: Higher memory usage due to processing entire strings.

In summary, trigram analysis is suitable for quick, approximate comparisons over large datasets, while Levenshtein distance offers a more precise but computationally intensive approach for detailed string comparisons.
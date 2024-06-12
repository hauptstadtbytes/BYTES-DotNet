# StringExtensions
### Namespace: BYTES.NET.Primitives
## Overview
The _StringExtensions_ class provides various extension methods for _string_ type operations, like 
- trimming characters (for .net framework environment)
- finding indexes for characters or patterns
- checking if a  substring is contained in a string
- parsing values and expanding (envornment) variables
- comparing strings and finding similar ones e.g. from a list of options

## Usage
You can import the methods by importing the respective namespace to your project class..
```csharp
//import namespace(s) required from 'BYTES.NET' framework
using BYTES.NET.Primitives;
```
...and use it like this:
```csharp
//find all indexes for the asterix sign inside an input string given
string inputString = "D:\\Das*\\ist\\das\\H*";
char character = '*';

int[] result = inputString.AllIndexesOf(character);
```

## Availabe Methods
### Trim (available only for the .NET framework)
Removes all leading or trailing instances of a character from the given string.
```csharp
public static string Trim(this string text, char character)
```
#### Parameters:
text (string): The input string.
character (char): The character to trim.
#### Returns:
string: The trimmed string.

### AllIndexesOf (Character)
Returns a zero-based list of all indexes of a character inside a given string.
```csharp
public static int[] AllIndexesOf(this string str, char character)
```
#### Parameters:
str (string): The input string.
character (char): The character to find.
#### Returns:
int[]: Array of indexes where the character occurs.

## AllIndexesOf (Regex)
Returns a zero-based list of all indexes of strings matching the given regular expression.
```csharp
public static int[] AllIndexesOf(this string str, Regex expression)
```
#### Parameters:
str (string): The input string.
expression (Regex): The regular expression to match.
#### Returns:
int[]: Array of indexes where the pattern occurs.

### Contains
Checks if a string array contains a specific value, optionally ignoring case.
```csharp
public static bool Contains(this string[] strings, string match, bool ignoreCase = true)
```
#### Parameters:
strings (string[]): The array of strings.
match (string): The string to match.
ignoreCase (bool, optional): Ignore case when comparing. Default is true.
#### Returns:
bool: true if the string is found, otherwise false.

### Expand
Expands variables inside a given string.
```csharp
public static string Expand(this string text, Dictionary<string, string> variables, bool ignoreCase = true)
```
#### Parameters:
text (string): The input string.
variables (Dictionary<string, string>): Dictionary of variables to expand.
ignoreCase (bool, optional): Ignore case when matching variables. Default is true.
#### Returns:
string: The expanded string.

## MatchesPattern
Checks if the string matches the given pattern using regular expressions, returning the match.
```csharp
public static bool MatchesPattern(this string text, Regex pattern, out Match? match)
```
#### Parameters:
text (string): The input string.
pattern (Regex): The regular expression pattern.
match (Match, out): The match found.
#### Returns:
bool: true if a match is found, otherwise false.
#### Overloads:
```csharp
public static bool MatchesPattern(this string text, Regex pattern)
```

### ParseKeyValue
Returns a key-value pair from a string containing an equality character.
```csharp
public static KeyValuePair<string, string> ParseKeyValue(this string text, char[] equalitySigns = null)
```
#### Parameters:
text (string): The input string.
equalitySigns (char[], optional): Array of equality signs. Default is ['=', ':'].
#### Returns:
KeyValuePair<string, string>: The parsed key-value pair.

### SimilarityTo
Returns the string similarity to a given reference string using the specified algorithm.
```csharp
public static double? SimilarityTo(this string text, string reference, string algorithm = "trigram")
```
#### Parameters:
text (string): The input string.
reference (string): The reference string.
algorithm (string, optional): The algorithm to use ('trigram' or 'levenshtein'). Default is trigram.
#### Returns:
double?: The similarity score.
#### Overloads:
```csharp
public static Dictionary<string, double> SimilarityTo(this string text, IEnumerable<string> reference, string algorithm = "trigram")
```

### BestMatch
Returns the best match from a list of options given, respecting an optional threshold.
```csharp
public static string? BestMatch(this string text, IEnumerable<string> options, out double dist, string algorithm = "trigram", double? threshold = null)
```
#### Parameters:
text (string): The input string.
options (IEnumerable<string>): The list of options.
dist (double, out): The similarity score of the best match.
algorithm (string, optional): The algorithm to use ('trigram' or 'levenshtein'). Default is trigram.
threshold (double?, optional): The threshold value for the similarity score. Default is null.
#### Returns:
string?: The best match if it meets the threshold, otherwise null.
#### Overloads:
```csharp
public static string? BestMatch(this string text, IEnumerable<string> options, string algorithm = "trigram", double? threshold = null)
```

## A Note on the Similarity Algorithms
By now there are two algorithms - different pros and cons implemented:
- Trigram-based algorithm
- Levenshtein Distance algorithm

If none of them is matching your needs, you may find additional algorithms e.g. at https://stackoverflow.com/questions/15303631/what-are-some-algorithms-for-comparing-how-similar-two-strings-are

### Trigram Algorithm
The trigam-based algorithm is determining the similarity between two strings by the number of matching letter triples. You may find additional details at https://lhncbc.nlm.nih.gov/ii/tools/MTI/trigram.html.

The higher the score, the more two strings equal each other.

#### Pros:
- Speed: Trigram comparison is generally faster, especially for longer strings.
- Efficiency: It works well for approximate matching and can handle misspellings or variations in text efficiently.
- Simplicity: It is easy to implement and understand.

#### Cons:
- Requirements: The reference string has to be at least three characters long.
- Accuracy: It may not be as accurate for short strings or strings with significant differences (which might also be a pro for lists with high levels of variation).
- Memory Usage: It can be memory-intensive for very long strings due to the generation of many trigrams.

### Levenshtein Distance Algorithm
The Levenshtein algorithm is one of the fuzzy matching techniques that measure between two strings, with the given number representing how far the two strings are from being an exact match. 

The higher the number of the Levenshtein distance, the further the two terms are from being identical.

#### Pros:
- Accuracy: It provides a precise measure of difference by considering insertions, deletions, and substitutions.
- Flexibility: Effective for short strings and can accurately measure similarity even with significant differences.
- Standard Metric: Widely recognized and used in various applications for string comparison.

#### Cons:
- Speed: Can be slower compared to trigram, especially for longer strings.
- Complexity: More computationally intensive, which can affect performance.
- Implementation: More complex to implement compared to trigram.
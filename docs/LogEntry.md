# LogEntry Class Documentation

## Overview

The `LogEntry` class represents individual log entries in the logging system. It encapsulates the essential information needed for logging, including timestamp, level, message, and optional details. This class extends `EventArgs`, allowing it to be used as an argument in event handlers.

## Enum: InformationLevel

### Purpose

Defines the severity levels for log entries.

### Values

- `Debug`: Lowest severity level
- `Info`: General information
- `Warning`: Potential issues
- `Exception`: Errors or exceptions
- `Fatal`: Critical errors

## Properties

### TimeStamp

- Type: `DateTime`
- Description: The timestamp when the log entry was created.
- Access: Protected (read-only)

### Level

- Type: `InformationLevel`
- Description: The severity level of the log entry.
- Access: Protected (read-only)

### Message

- Type: `string`
- Description: The main content of the log entry.
- Access: Protected (read-only)

### Details

- Type: `object?`
- Description: Additional information associated with the log entry.
- Access: Protected (read-only)

## Constructors

### Default Constructor

```csharp 
public LogEntry(string message, InformationLevel? level = null)
```

Parameters:
- `message`: The log message.
- `level`: Optional severity level (default: `null`).

### Overloaded Constructor

```csharp 
public LogEntry(string message, InformationLevel level, object details)
```

Parameters:
- `message`: The log message.
- `level`: The severity level.
- `details`: Additional information (can be null).

## Methods

### IsMoreImportant

Checks if the current log entry's level is more important than the given threshold level.

```csharp 
public bool IsMoreImportant(LogEntry.InformationLevel threshold)
```

Parameters:
- `threshold`: The severity level to compare against.

Returns: `true` if the current entry's level is more important than the threshold, `false` otherwise.

### ToString

Creates a formatted string representation of the log entry.

```csharp 
public string ToString(string pattern = "%TimeStamp% %Level% [] - %Message%", bool supportEmptyLines = true)
```

Parameters:
- `pattern`: The formatting pattern (default: "%TimeStamp% %Level% [] - %Message%").
- `supportEmptyLines`: Whether to return empty lines for entries with no message (default: true).

Returns: A formatted string representation of the log entry.

## Usage Examples

1. Basic usage:

```csharp 
var entry = new LogEntry("User logged in successfully", InformationLevel.Info); Console.WriteLine(entry.ToString());
```

2. With details:

```csharp 
var exception = new Exception("Database connection failed"); var entry = new LogEntry("Failed to connect to database", InformationLevel.Exception, exception); Console.WriteLine(entry.ToString());
```

3. Custom pattern:

```csharp 
var entry = new LogEntry("Authentication attempt", InformationLevel.Warning, "Invalid credentials"); Console.WriteLine(entry.ToString("%Date% [%Time%] - %Level%: %Message% - Details: %Details%"));
```

## Best Practices

1. Use the appropriate `InformationLevel` for each situation.
2. Provide meaningful messages and details for each log entry.
3. Utilize the `IsMoreImportant` method when filtering logs based on severity levels.
4. Customize the `ToString` method's pattern to suit your logging requirements.
5. When using exceptions as details, consider including both the exception message and stack trace.

## Event Handling

As `LogEntry` extends `EventArgs`, it can be used as an argument in event handlers. For example:

```csharp 
log.Logged += (sender, entry) => { Console.WriteLine($"Logged: {entry.TimeStamp} - {entry.Level}: {entry.Message}"); };
```

This documentation provides an overview of the `LogEntry` class, its properties, constructors, methods, and usage examples. It also includes some best practices for utilizing the class effectively in your logging system.
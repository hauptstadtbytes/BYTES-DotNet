# Log Class Documentation

## Overview

The `Log` class is a generic application logging utility designed to handle various types of log entries and route them to registered appenders. It provides methods for adding log entries at different levels (Trace, Info, Warning, Error), managing a cache of recent logs, and configuring logging behavior.

## Properties

### ID

- Type: `string`
- Description: A unique identifier for the log instance.
- Access: Public
- Default Value: Automatically generated UUID

### CacheLimit

- Type: `int`
- Description: The maximum number of log entries to store in the cache.
- Access: Public
- Default Value: 100

### LogModifications

- Type: `bool`
- Description: Determines whether modifications to the log configuration should be logged.
- Access: Public
- Default Value: True

### Threshold

- Type: `LogEntry.InformationLevel`
- Description: The minimum severity level for log entries to be recorded.
- Access: Public
- Default Value: `LogEntry.InformationLevel.Info`

### Appenders

- Type: `ILogAppender[]`
- Description: An array of registered log appenders.
- Access: Public
- Default Value: Empty array

## Methods

### Constructor

```csharp 
public Log(string? identifier = null)
```

Creates a new `Log` instance. Optionally takes an identifier parameter.

### AddAppender

Adds a new appender to the list of registered appenders.

```csharp 
public void AddAppender(ILogAppender appender)
```

Parameters:
- `appender`: The `ILogAppender` to add.

### Write

Writes a log entry to the cache and registered appenders.

```csharp 
public void Write(LogEntry entry)
```

Parameters:
- `entry`: The `LogEntry` to write.

### Write

Overloaded version of `Write` that creates a `LogEntry` internally.

```csharp 
public void Write(string message, LogEntry.InformationLevel level, object? details = null)
```

Parameters:
- `message`: The log message.
- `level`: The severity level of the log entry.
- `details`: Optional additional information (default: null).

### Trace

Logs a Debug-level entry.

```csharp 
public void Trace(string message, object? details = null)
```

Parameters:
- `message`: The log message.
- `details`: Optional additional information (default: null).

### Inform

Logs an Info-level entry.

```csharp 
public void Inform(string message, object? details = null)
```

Parameters:
- `message`: The log message.
- `details`: Optional additional information (default: null).

### Warn

Logs a Warning-level entry.

```csharp 
public void Warn(string message, object? details = null)
```

Parameters:
- `message`: The log message.
- `details`: Optional additional information (default: null).

### ReportError

Logs an Error-level entry.

```csharp 
public void ReportError(string message, object? details = null, bool isFatal = false)
```

Parameters:
- `message`: The log message.
- `details`: Optional additional information (default: null).
- `isFatal`: Indicates if the error is fatal (default: false).

### GetCache

Retrieves cached log entries based on a threshold level.

```csharp 
public LogEntry[] GetCache(LogEntry.InformationLevel? threshold = null)
```

Parameters:
- `threshold`: Optional minimum severity level for returned entries (default: null).

Returns: An array of `LogEntry` objects matching the threshold criteria.

## Events

### Logged

Fired when a log entry is written. Can be subscribed to using the `LoggedEventHandler`.

```csharp 
public event LoggedEventHandler Logged
```

## Best Practices

1. Use the appropriate log level for each situation (e.g., `Trace` for debugging, `Inform` for general information, `Warn` for potential issues, `ReportError` for errors).
2. Configure the `Threshold` property to control the verbosity of your logs.
3. Utilize the `AddAppender` method to integrate with your preferred logging destination (file, console, database, etc.).
4. Consider setting `LogModifications` to `false` if you don't want to clutter your logs with configuration changes.
5. Regularly clear the cache using the `GetCache` method to manage memory usage.
6. Subscribe to the `Logged` event to perform actions when logs are written.

## Example Usage

```csharp 
var log = new Log("MyApplication");

log.AddAppender(new FileAppender()); log.AddAppender(new ConsoleAppender());

log.Trace("This is a debug message", "Some details"); log.Inform("User logged in successfully"); log.Warn("Potential issue detected"); log.ReportError("Critical error occurred", "Detailed error information", true);
```
This documentation provides an overview of the `Log` class, its properties, methods, and events. It also includes some best practices and an example of how to use the class. You can further customize this documentation to fit your specific needs or project requirements.
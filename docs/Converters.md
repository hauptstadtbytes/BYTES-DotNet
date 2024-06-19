# Converters
### Namespace: BYTES.NET.WPF.Converters
## Overview
The inverters are a set of classes in the BYTES.NET.WPF (sub-)package, intended to ease value binding when working with WPF (Windows Presentation Foundation). 

All converters are based on the [IValueConverter](https://learn.microsoft.com/de-de/dotnet/api/system.windows.data.ivalueconverter) interface of the .NET framework, expressing the following methods:
```csharp
public object Convert(object value, Type targetType, object parameter, CultureInfo culture);
public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
```

## Usage
The converters are intended to be used in the application views, via importing the namespace...
```xml
<Window x:Class="BYTES.NET.WPF.App.Views.MainView"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:converters="clr-namespace:BYTES.NET.WPF.Converters;assembly=BYTES.NET.WPF"
        mc:Ignorable="d"
        Title="{Binding Title, Mode=OneWay}">
```
...defining the converters (static) ressources...
```xml
<Window.Resources>
    <converters:BooleanToIndexConverter x:Key="BoolToIndexConverter" />
    <converters:BoolInvertConverter x:Key="BoolInvertConverter"/>
    <converters:BoolToVisibilityConverter x:Key="BoolToVisibilityConverter"/>
    <converters:NothingToVisibilityConverter x:Key="NothingToVisibilityConverter"/>
    <converters:NothingToBoolConverter x:Key="NothingToBoolConverter"/>
    <converters:CountToVisibilityConverter x:Key="CountToVisibilityConverter"/>
</Window.Resources>
```

...and uused when binding the value.
```xml
<TextBlock Text="{Binding IsChecked, Converter={StaticResource BoolInvertConverter}}"/>
```

## Availabe Classes
### BoolInvertConverter
It inverts boolean values. When used in XAML bindings, it converts a `true` boolean value to `false` and vice versa. This inversion allows you to control e.g. visibility, enabled state, or other properties of UI elements based on the negation of a boolean property in your ViewModel.

### BoolToIndexConverter
It converts a boolean `true` or `false` to a '1' or '0', intended to be used for enabling users to select a boolean value from a combobox.

### BoolToVisibilityConverter
It converts a boolean value to a [visibilty](https://learn.microsoft.com/de-de/uwp/api/windows.ui.xaml.uielement.visibility?view=winrt-22621) status in XAML, returning 'Collapsed' when 'false' to save space.

#### Parameters
This converter supports a boolean value, inverting the output to show items for a 'false' value. Please see [this article](https://stackoverflow.com/questions/4997446/boolean-commandparameter-in-xaml) on how to use booleans propertly in XAML.
```xml
<TextBlock Text="Sample" Visibility="{Binding ElementName=VisCheckbox, Path=IsChecked, Converter={StaticResource BoolToVisibilityConverter},ConverterParameter={StaticResource False}}" />
```

### NothingToVisibilityConverter
It converts an object type value to a [visibilty](https://learn.microsoft.com/de-de/uwp/api/windows.ui.xaml.uielement.visibility?view=winrt-22621) status in XAML, returning 'Visible' for a 'null' value or empty string.

#### Parameters
This converter supports a boolean value, inverting the output to hide when 'null' or empty. Please see [this article](https://stackoverflow.com/questions/4997446/boolean-commandparameter-in-xaml) on how to use booleans propertly in XAML.
```xml
<TextBlock Text="Sample" Visibility="{Binding ElementName=VisTextbox, Path=Text, Converter={StaticResource NothingToVisibilityConverter},ConverterParameter={StaticResource False}}" />
```

### NothingToBoolConverter
It converts an object-type value to a boolean value, returning 'true' for a 'null' value or empty string.

### CountToVisibilityConverter
It converts an enumable of objects to a [visibilty](https://learn.microsoft.com/de-de/uwp/api/windows.ui.xaml.uielement.visibility?view=winrt-22621) status in XAML, returning 'Visible' for if the list contains more than one item(s).

#### Parameters
An (string type) integer parameter might be added to define the minimum number of items to return 'Visible'. There will always be performed a ">=" comparison.
```xml
<TextBlock Text=" + One more" Visibility="{Binding SampleStringList,Mode=OneWay, Converter={StaticResource CountToVisibilityConverter},ConverterParameter=3}" />
```
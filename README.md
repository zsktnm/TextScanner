# TextScanner
Simple text scanner, built on `IParsable<T>`. 
Inspired by Java Scanner

## Usage

### Create the scanner

You can use constructor with `TextReader` parameter or static method `FromConsole`.

```csharp
// read from file
using (StreamReader reader = new("file.txt"))
{
    Scanner scanner = new Scanner(reader);
    // ...
}
```

```csharp
// read from console
Scanner scanner = Scanner.FromConsole();
// ...
```

### Read Single Value

If you want to parse your value, you can use `Read<T>` or `TryRead<T>` methods, where `T` must be `IParseble<T>`.

```csharp
Console.WriteLine("Enter the count of numbers: ");
int count = scanner.Read<int>();
```

```csharp
Console.WriteLine($"Enter the count of dates: ");
while(true)
{
    if (scanner.TryRead(out count))
    {
        break;
    }
    Console.WriteLine("Invalid input. Try again");
}
```

You can also use the `ReadBlock`, `ReadChar`, `ReadLine` and `ReadToEnd` methods, if you don't want to parse your input.

`ReadBlock` method returns the first value bounded by spaces. Note, that if input contains only whitespaces `ReadBlock` returns `string.Empty`, and if we have empty input (typicaly, the end of stream) it returns `null`.  

### Read Multiple Values

Use `ReadValues<T>` method to enumerate values:
```csharp
Console.WriteLine("Enter the values: ");
int[] values = scanner.ReadValues<int>().Take(count).ToArray();
```
It is possible to skip every uncorrect input by using the parameter:
```csharp
foreach (DateTime dateTime in scanner.ReadValues<DateTime>(skipOnErrors: true).Take(count))
{
    Console.WriteLine($"Accepted {dateTime}");
}
```

Or you can change wrong inputs by default value (`-1` in example below):
```csharp
Console.WriteLine("Enter the values: ");
int[] values = scanner.ReadValues(defaultValue: -1).Take(count).ToArray();
```

You also can enumerate strings by `ReadBlocks` and `ReadLines` methods.

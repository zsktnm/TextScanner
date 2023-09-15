using Zsktnm.TextScanner;

Scanner scanner = Scanner.FromConsole();

// unsafe input
Console.WriteLine("Enter the count of numbers: ");
int count = scanner.Read<int>();

Console.WriteLine("Enter the values: ");
int[] values = scanner.ReadValues<int>().Take(count).ToArray();

Console.WriteLine($"Sum of values is {values.Sum()}");


// safe input
Console.WriteLine($"Enter the count of dates: ");
while(true)
{
    if (scanner.TryRead(out count))
    {
        break;
    }
    Console.WriteLine("Invalid input. Try again");
}

foreach (DateTime dateTime in scanner.ReadValues<DateTime>(skipOnErrors: true).Take(count))
{
    Console.WriteLine($"Accepted {dateTime}");
}


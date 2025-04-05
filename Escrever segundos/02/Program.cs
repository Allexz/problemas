using System;
using System.Collections.Generic;

abstract class DurationHandler
{
    protected DurationHandler NextHandler;
    protected readonly string TimeUnit;
    protected readonly long Divisor;

    protected DurationHandler(string timeUnit, long divisor, DurationHandler nextHandler)
    {
        TimeUnit = timeUnit;
        Divisor = divisor;
        NextHandler = nextHandler;
    }

    public string Handle(long seconds, List<string> parts)
    {
        if (seconds >= Divisor)
        {
            long value = seconds / Divisor;
            long remainder = seconds % Divisor;

            parts.Add($"{value} {TimeUnit}{(value > 1 ? "s" : "")}");

            if (remainder > 0 && NextHandler != null)
            {
                return NextHandler.Handle(remainder, parts);
            }
        }
        else if (NextHandler != null)
        {
            return NextHandler.Handle(seconds, parts);
        }

        return string.Join(", ", parts);
    }
}

class YearHandler : DurationHandler
{
    public YearHandler(DurationHandler nextHandler)
        : base("year", 31536000, nextHandler) { }
}

class MonthHandler : DurationHandler
{
    public MonthHandler(DurationHandler nextHandler)
        : base("month", 2592000, nextHandler) { }
}

class DayHandler : DurationHandler
{
    public DayHandler(DurationHandler nextHandler)
        : base("day", 86400, nextHandler) { }
}

class HourHandler : DurationHandler
{
    public HourHandler(DurationHandler nextHandler)
        : base("hour", 3600, nextHandler) { }
}

class MinuteHandler : DurationHandler
{
    public MinuteHandler(DurationHandler nextHandler)
        : base("minute", 60, nextHandler) { }
}

class SecondHandler : DurationHandler
{
    public SecondHandler() : base("second", 1, null) { }
}

class Solution
{
    static void Main(string[] args)
    {
        // Construindo a cadeia de responsabilidade
        var handler = new YearHandler(
                        new MonthHandler(
                            new DayHandler(
                                new HourHandler(
                                    new MinuteHandler(
                                        new SecondHandler())))));

        long seconds = Convert.ToInt64(Console.ReadLine().Trim());
        string result = FormatDuration(seconds, handler);
        Console.WriteLine(result);
        Console.ReadKey();
    }

    public static string FormatDuration(long seconds, DurationHandler handler)
    {
        if (seconds == 0) return "now";
        if (seconds < 0) throw new ArgumentException($"Entrada inválida: {seconds}");

        var parts = new List<string>();
        string formatted = handler.Handle(seconds, parts);

        // Melhorando a formatação final (substituindo a última vírgula por " and ")
        int lastComma = formatted.LastIndexOf(',');
        if (lastComma != -1)
        {
            formatted = formatted.Substring(0, lastComma) +
                         " and" +
                         formatted.Substring(lastComma + 1);
        }

        return formatted.Trim();
    }
}
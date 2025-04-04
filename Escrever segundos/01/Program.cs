class Solution
{
    private const int MINUTE = 60;
    private const int HOUR = 3600;
    private const int DAY = 86400;
    private const int MONTH = 2592000;
    private const int YEAR = 31536000;
 
    static void Main(string[] args)
    {
        long seconds = Convert.ToInt64(Console.ReadLine().Trim());
        string result = formatDuration(seconds);
        Console.WriteLine(result);
        Console.ReadKey();
    }

    public static string formatDuration(long seconds)
    {
        string now = "now";
        if (seconds == 0) return now;
        if (seconds < 0) throw new ArgumentException($"Entrada inválida: {seconds}");
 
        if (seconds < 60)
            return $"{seconds} segundo{(seconds > 1 ? "s" : "")}";
        
        string timeFormatted = seconds switch
        {
            < HOUR => formatMinutes(seconds),
            < DAY => formatHours(seconds),
            < MONTH => formatDay(seconds),
            < YEAR => formatMonth(seconds),
            _ => formatYear(seconds)
        };

        return timeFormatted;
    }

    private static string formatMinutes(long seconds)
    {
        string part = string.Empty;
        double result = seconds/MINUTE;
        double mod = seconds%MINUTE;

        if (mod > 0) part = $" {mod} seconds";         

        return $"{result} minute{(result > 1 ? "s" : "")} {part}";
    }

    private static string formatHours(long seconds)
    {
        string part = string.Empty;
        double result = seconds/HOUR;
        double mod = seconds%HOUR;

        if (mod > 0)part = formatMinutes(Convert.ToInt64(mod));

        if (result == 0) return part;

        return  $" {result} hour{(result > 1 ? "s" : "")} {part} ";
    }

    private static string formatDay(long seconds)
    {
        string part = string.Empty;
        double result = seconds / DAY;
        double mod = seconds % DAY;
       
        if (mod > 0) part = formatHours(Convert.ToInt64(mod));

        if (result == 0) return part;

        return $"{result} day{(result > 1 ? "s" : "")} {part}";
    }

    private static string formatMonth(long seconds)
    {
        string part = string.Empty;
        double result = seconds / MONTH;
        double mod = seconds % MONTH;

        if (mod > 0) part = formatDay(Convert.ToInt64(mod));

        if (result == 0) return part;

        return $"{result} month{(result > 1 ? "s" : "")} {part}";
    }

    private static string formatYear(long seconds)
     {
        string part = string.Empty;
        double result = seconds / YEAR;
        double mod = seconds % YEAR;

        if (mod > 0) part = formatMonth(Convert.ToInt64(mod));

        if (result == 0) return part;

        return $"{result} year{(result > 1 ? "s" : "")} {part}";
    }
}

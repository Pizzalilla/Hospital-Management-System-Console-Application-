using System.Text;
using System.Linq;   // for Select/ToList

namespace HospitalManagementSystem.Extensions;

// Common console helpers for consistent UI and input handling
public static class ConsoleExtensions
{
    // Draws a centered box with a title + subtitle and divider line
    public static void HeadingBox(string title, string subtitle, int innerWidth = 70)
    {
        var width = Math.Max(innerWidth, Math.Max(title.Length, subtitle.Length));
        var left = Math.Max(0, (Console.WindowWidth - (width + 4)) / 2);
        var pad = new string(' ', left);

        var top = $"{pad}┌{new string('─', width + 2)}┐";
        var bottom = $"{pad}└{new string('─', width + 2)}┘";

        Console.Clear();
        Console.WriteLine(top);
        Console.WriteLine($"{pad}│ {title.PadRight(width)} │");
        Console.WriteLine($"{pad}│ {new string('─', width)} │");
        Console.WriteLine($"{pad}│ {subtitle.PadRight(width)} │");
        Console.WriteLine(bottom);
        Console.WriteLine();
    }

    // ===== Method overloading example =====
    public static void Pause()                       // no-arg version
    {
        Pause("\nPress any key to return...");
    }
    public static void Pause(string msg)            // overloaded version
    {
        Console.WriteLine(msg);
        Console.ReadKey(true);
    }

    // Reads password input while masking characters with '*'
    public static string ReadPasswordMasked()
    {
        var sb = new StringBuilder();
        while (true)
        {
            var k = Console.ReadKey(intercept: true);
            if (k.Key == ConsoleKey.Enter) { Console.WriteLine(); break; }
            if (k.Key == ConsoleKey.Backspace && sb.Length > 0)
            { sb.Remove(sb.Length - 1, 1); Console.Write("\b \b"); }
            else if (!char.IsControl(k.KeyChar))
            { sb.Append(k.KeyChar); Console.Write('*'); }
        }
        return sb.ToString();
    }

    // Prints a clean table with headers and rows
    public static void PrintTable(string title, string description, string[] headers, IEnumerable<string[]> rows)
    {
        if (!string.IsNullOrWhiteSpace(title)) Console.WriteLine(title);
        if (!string.IsNullOrWhiteSpace(description)) Console.WriteLine(description + "\n");

        var data = rows.ToList();
        var cols = headers.Length;
        var widths = new int[cols];

        // calculate column widths
        for (int c = 0; c < cols; c++)
        {
            widths[c] = headers[c].Length;
            foreach (var r in data)
                if (c < r.Length) widths[c] = Math.Max(widths[c], r[c]?.Length ?? 0);
        }

        string sep = "+-" + string.Join("-+-", widths.Select(w => new string('-', w))) + "-+";
        Console.WriteLine(sep);
        Console.WriteLine("| " + string.Join(" | ", headers.Select((h, i) => h.PadRight(widths[i]))) + " |");
        Console.WriteLine(sep);

        if (data.Count == 0)
        {
            Console.WriteLine("| " + "No records".PadRight(widths.Sum() + (cols - 1) * 3) + " |");
        }
        else
        {
            foreach (var r in data)
                Console.WriteLine("| " + string.Join(" | ", r.Select((c, i) => (c ?? "").PadRight(widths[i]))) + " |");
        }

        Console.WriteLine(sep);
        Console.WriteLine();
    }

    // Lets user press B/b to go back → returns null in that case
    public static string? PromptOrBack(string label, string backHint = "B to go back")
    {
        Console.Write($"{label} (or {backHint}): ");
        var s = Console.ReadLine();
        if (string.Equals(s, "B", StringComparison.OrdinalIgnoreCase)) return null;
        return s ?? "";
    }

    // Integer version of PromptOrBack
    public static int? PromptIntOrBack(string label, string backHint = "B to go back")
    {
        var s = PromptOrBack(label, backHint);
        if (s is null) return null;
        if (int.TryParse(s, out var v)) return v;
        return int.MinValue; // signals invalid
    }
}

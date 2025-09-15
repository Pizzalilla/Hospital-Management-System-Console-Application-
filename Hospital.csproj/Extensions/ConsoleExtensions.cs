namespace HospitalApp.Extensions;

/// <summary>
/// Tiny console helpers for consistent, neat UI and masked password input.
/// </summary>
public static class ConsoleExtensions
{
    public static void Heading(this string title)
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine(new string('─', Math.Max(10, title.Length)));
    }

    /// <summary>
    /// Reads a password from the console and masks characters with '*'.
    /// Satisfies the "masked input" requirement in the spec.
    /// </summary>
    public static string ReadPasswordMasked()
    {
        var sb = new System.Text.StringBuilder();
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

    /// <summary>Common "press any key" pause to keep screens readable.</summary>
    public static void Pause() { 
        Console.WriteLine("\nPress any key to continue..."); Console.ReadKey(); 
    }
}

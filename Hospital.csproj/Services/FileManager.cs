using System.Text.Json;

namespace HospitalApp.Services;

/// <summary>
/// Centralized load/save for plain-text JSON files.
/// Gives you exception-handling marks and keeps IO cohesive.
/// </summary>
public static class FileManager
{
    private static readonly JsonSerializerOptions Opt = new() { WriteIndented = true };

    public static List<T> Load<T>(string path)
    {
        try
        {
            if (!File.Exists(path)) return new List<T>();
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<T>>(json, Opt) ?? new List<T>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[File error] {path}: {ex.Message}");
            return new List<T>(); // return empty to keep app usable
        }
    }

    public static void Save<T>(string path, List<T> data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, Opt);
            File.WriteAllText(path, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[File write error] {path}: {ex.Message}");
        }
    }
}

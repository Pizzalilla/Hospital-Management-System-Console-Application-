using System.Text.Json;

namespace HospitalManagementSystem.Services;

// Centralized JSON load/save for your text files.
// Includes basic exception handling so the app remains usable even if a file is missing/broken.
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
            Console.WriteLine($"[File read error] {path}: {ex.Message}");
            return new List<T>();
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

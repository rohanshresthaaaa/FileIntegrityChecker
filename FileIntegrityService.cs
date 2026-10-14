using System.Security.Cryptography;
using System.Text.Json;

public class FileIntegrityService
{
    private readonly string _folderPath;
    private readonly string _baselinePath;

    public FileIntegrityService(string folderPath, string baselinePath)
    {
        _folderPath = folderPath;
        _baselinePath = baselinePath;
    }

    public void Run()
    {
        // Throw an error so Program.cs can report it
        // and return exit code 1.
        if (!Directory.Exists(_folderPath))
        {
            throw new DirectoryNotFoundException(
                $"Folder not found: {_folderPath}"
            );
        }

        Dictionary<string, string> currentHashes = ScanFiles();

        if (!File.Exists(_baselinePath))
        {
            SaveBaseline(currentHashes);

            Console.WriteLine(
                "Baseline created. Run again to check for changes."
            );

            return;
        }

        Dictionary<string, string> baseline = LoadBaseline();
        CompareFiles(currentHashes, baseline);
    }

    private Dictionary<string, string> ScanFiles()
    {
        // Key = filename; value = SHA-256 hash.
        Dictionary<string, string> hashes = new();

        foreach (string filePath in Directory.GetFiles(_folderPath))
        {
            using FileStream stream = File.OpenRead(filePath);

            string hash = Convert.ToHexString(
                SHA256.HashData(stream)
            );

            string fileName = Path.GetFileName(filePath);
            hashes[fileName] = hash;
        }

        return hashes;
    }

    private void SaveBaseline(Dictionary<string, string> hashes)
    {
        string json = JsonSerializer.Serialize(
            hashes,
            new JsonSerializerOptions
            {
                WriteIndented = true
            }
        );

        File.WriteAllText(_baselinePath, json);
    }

    private Dictionary<string, string> LoadBaseline()
    {
        string json = File.ReadAllText(_baselinePath);

        return JsonSerializer.Deserialize<Dictionary<string, string>>(json)
            ?? throw new InvalidDataException(
                "Baseline contains no file data."
            );
    }

    private void CompareFiles(
        Dictionary<string, string> currentHashes,
        Dictionary<string, string> baseline)
    {
        foreach (var entry in currentHashes)
        {
            string fileName = entry.Key;
            string currentHash = entry.Value;

            if (!baseline.TryGetValue(fileName, out string? originalHash))
            {
                Console.WriteLine($"NEW        {fileName}");
            }
            else if (currentHash != originalHash)
            {
                Console.WriteLine($"MODIFIED   {fileName}");
            }
            else
            {
                Console.WriteLine($"UNCHANGED  {fileName}");
            }
        }

        foreach (string fileName in baseline.Keys)
        {
            if (!currentHashes.ContainsKey(fileName))
            {
                Console.WriteLine($"DELETED    {fileName}");
            }
        }
    }
}
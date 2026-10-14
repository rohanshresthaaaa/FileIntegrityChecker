using System.Text.Json;

try
{
    string folderPath = args.Length > 0 ? args[0] : "samples";
string baselinePath = args.Length > 1 ? args[1] : "baseline.json";

FileIntegrityService checker = new(folderPath, baselinePath);
    checker.Run();
}
catch (UnauthorizedAccessException)
{
    Console.Error.WriteLine(
        "ERROR: Permission denied. Check file and folder permissions."
    );
    Environment.ExitCode = 1;
}
catch (JsonException)
{
    Console.Error.WriteLine(
        "ERROR: The baseline file contains invalid JSON."
    );
    Environment.ExitCode = 1;
}
catch (InvalidDataException ex)
{
    Console.Error.WriteLine($"ERROR: {ex.Message}");
    Environment.ExitCode = 1;
}
catch (IOException ex)
{
    Console.Error.WriteLine($"ERROR: Could not access a file. {ex.Message}");
    Environment.ExitCode = 1;
}
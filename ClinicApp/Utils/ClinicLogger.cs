using System.Text;

namespace ClinicApp.Utils;

public class ClinicLogger
{
    private readonly string _logPath;

    public ClinicLogger(string logPath = "clinic.log")
    {
        _logPath = logPath;
    }

    public void LogInfo(string message)
    {
        WriteEntry("INFO", message);
    }

    public void LogWarning(string message)
    {
        WriteEntry("WARN", message);
    }

    public void LogError(string message)
    {
        WriteEntry("ERROR", message);
    }

    public string[] GetLastLines(int n)
    {
        if (!File.Exists(_logPath) || n <= 0)
            return new string[0];

        string[] allLines = File.ReadAllLines(_logPath, Encoding.UTF8);
        int startIndex = allLines.Length - n;
        if (startIndex < 0)
            startIndex = 0;

        string[] result = new string[allLines.Length - startIndex];
        for (int i = 0; i < result.Length; i++)
            result[i] = allLines[startIndex + i];

        return result;
    }

    public void Clear()
    {
        if (File.Exists(_logPath))
            File.Delete(_logPath);
    }

    public bool Exists()
    {
        return File.Exists(_logPath);
    }

    private void WriteEntry(string level, string message)
    {
        string line = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [{level.PadRight(5)}] {message}";
        File.AppendAllText(_logPath, line + Environment.NewLine, Encoding.UTF8);
    }
}

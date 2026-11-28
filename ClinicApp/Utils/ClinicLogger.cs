using System.Text;
using ClinicApp.Events;

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

    public void OnPatientAdded(object? sender, PatientEventArgs e) =>
        LogInfo($"Новий пацієнт #{e.PatientId}: {e.FullName}");

    public void OnAppointmentBooked(object? sender, AppointmentEventArgs e) =>
        LogInfo($"Запис #{e.AppointmentId} створено: пацієнт #{e.PatientId} → лікар #{e.DoctorId}");

    public void OnUrgentAppointmentBooked(object? sender, AppointmentEventArgs e)
    {
        LogWarning($"ТЕРМІНОВИЙ запис #{e.AppointmentId}: {e.Notes}");

        Directory.CreateDirectory("alerts");
        string alertPath = Path.Combine("alerts", $"urgent_{DateTime.Now:yyyy-MM-dd}.txt");
        string alertLine = $"[{DateTime.Now:HH:mm:ss}] Запис #{e.AppointmentId}: {e.Notes}" + Environment.NewLine;
        File.AppendAllText(alertPath, alertLine, Encoding.UTF8);
    }

    public void OnAppointmentCancelled(object? sender, AppointmentEventArgs e) =>
        LogInfo($"Запис #{e.AppointmentId} скасовано");

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) =>
        LogInfo($"Запис #{e.AppointmentId} завершено");

    public void OnPaymentReceived(object? sender, PaymentEventArgs e) =>
        LogInfo($"Оплата запису #{e.AppointmentId}: {e.Amount:F2} грн");

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) =>
        LogInfo($"План лікування #{e.PlanId} завершено: {e.Diagnosis}");

    private void WriteEntry(string level, string message)
    {
        string line = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] [{level.PadRight(5)}] {message}";
        File.AppendAllText(_logPath, line + Environment.NewLine, Encoding.UTF8);
    }
}

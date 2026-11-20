using System.Text;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class ClinicExporter
{
    private readonly Clinic _clinic;
    private readonly string _baseDir;

    public ClinicExporter(Clinic clinic, string baseDir = "reports")
    {
        _clinic = clinic;
        _baseDir = baseDir;
    }

    public string ExportPatients()
    {
        string reportDirectory = GetReportDirectory();
        string filePath = Path.Combine(reportDirectory, "patients.txt");
        Patient[] patients = _clinic.Patients.GetAll();

        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            WriteHeader(writer, "Пацієнти");
            for (int i = 0; i < patients.Length; i++)
                writer.WriteLine($"{i + 1,3}. {patients[i]}");
            WriteSeparator(writer);
            writer.WriteLine($"Всього: {patients.Length}");
        }

        return filePath;
    }

    public string ExportAppointments()
    {
        string reportDirectory = GetReportDirectory();
        string filePath = Path.Combine(reportDirectory, "appointments.txt");
        Appointment[] appointments = _clinic.Appointments.GetAll();

        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            WriteHeader(writer, "Записи на прийом");
            for (int i = 0; i < appointments.Length; i++)
                writer.WriteLine($"{i + 1,3}. {appointments[i]}");
            WriteSeparator(writer);
            writer.WriteLine($"Всього: {appointments.Length}");
        }

        return filePath;
    }

    public string ExportBilling()
    {
        string reportDirectory = GetReportDirectory();
        string filePath = Path.Combine(reportDirectory, "billing.txt");
        IPayable[] unpaidAppointments = _clinic.Billing.GetAllUnpaid();
        decimal totalDebt = 0m;

        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            WriteHeader(writer, "Рахунки");
            for (int i = 0; i < unpaidAppointments.Length; i++)
            {
                decimal cost = unpaidAppointments[i].GetCost();
                totalDebt += cost;
                writer.WriteLine($"{i + 1,3}. {unpaidAppointments[i]} | Сума: {cost:F2} грн");
            }
            WriteSeparator(writer);
            writer.WriteLine($"Всього: {unpaidAppointments.Length} | Загальний борг: {totalDebt:F2} грн");
        }

        return filePath;
    }

    public string ExportTreatmentPlans()
    {
        string reportDirectory = GetReportDirectory();
        string filePath = Path.Combine(reportDirectory, "treatment_plans.txt");
        TreatmentPlan[] treatmentPlans = _clinic.TreatmentPlans.GetAll();

        using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            WriteHeader(writer, "Плани лікування");
            for (int i = 0; i < treatmentPlans.Length; i++)
            {
                TreatmentPlan plan = treatmentPlans[i];
                writer.WriteLine($"{i + 1,3}. [{plan.Id}] Пацієнт: {plan.PatientId} | Діагноз: {plan.Diagnosis} | Тривалість: {plan.DurationDays} дн. | Статус: {plan.Status}");
            }
            WriteSeparator(writer);
            writer.WriteLine($"Всього: {treatmentPlans.Length}");
        }

        return filePath;
    }

    public string[] ExportAll()
    {
        return new[]
        {
            ExportPatients(),
            ExportAppointments(),
            ExportBilling(),
            ExportTreatmentPlans()
        };
    }

    private string GetReportDirectory()
    {
        string directory = Path.Combine(_baseDir, DateTime.Now.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(directory);
        return directory;
    }

    private void WriteHeader(StreamWriter writer, string title)
    {
        writer.WriteLine($"=== {title} — звіт ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        WriteSeparator(writer);
    }

    private void WriteSeparator(StreamWriter writer)
    {
        writer.WriteLine("----------------------------------------");
    }
}

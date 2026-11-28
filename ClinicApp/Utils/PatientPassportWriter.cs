using System.Text;
using ClinicApp.Events;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class PatientPassportWriter
{
    private readonly Clinic _clinic;
    private readonly string _passportDirectory;

    public PatientPassportWriter(Clinic clinic, string passportDirectory = "patients")
    {
        _clinic = clinic;
        _passportDirectory = passportDirectory;
        Directory.CreateDirectory(_passportDirectory);
    }

    public void OnPatientAdded(object? sender, PatientEventArgs e) => WritePassport(e.PatientId);

    public void OnAppointmentCompleted(object? sender, AppointmentEventArgs e) => WritePassport(e.PatientId);

    public void OnPlanCompleted(object? sender, TreatmentPlanEventArgs e) => WritePassport(e.PatientId);

    private void WritePassport(int patientId)
    {
        Patient? patient = _clinic.Patients.FindById(patientId);
        if (patient == null)
            return;

        Directory.CreateDirectory(_passportDirectory);
        string path = Path.Combine(_passportDirectory, $"passport_{patientId}.txt");
        using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);

        writer.WriteLine($"=== Паспорт пацієнта #{patientId} ===");
        writer.WriteLine($"Згенеровано: {DateTime.Now:dd.MM.yyyy HH:mm}");
        writer.WriteLine();

        writer.WriteLine($"Ім'я: {patient.FullName}");
        writer.WriteLine($"Дата народження: {patient.DateOfBirth:dd.MM.yyyy} ({ClinicFormatter.FormatAge(patient.Age)})");
        writer.WriteLine($"Група крові: {ClinicFormatter.FormatBloodType(patient.BloodType)}   Телефон: {ClinicFormatter.FormatPhone(patient.Phone)}");
        writer.WriteLine();

        MedicalRecord[] records = _clinic.MedicalRecords.GetByPatient(patientId);

        writer.WriteLine("-- Діагнози --");
        int diagnosisCount = 0;
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Diagnosis diagnosis)
            {
                writer.WriteLine(diagnosis.GetSummary());
                diagnosisCount++;
            }
        }
        if (diagnosisCount == 0)
            writer.WriteLine("немає");

        writer.WriteLine("-- Аналізи --");
        int labResultCount = 0;
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is LabResult labResult)
            {
                writer.WriteLine(labResult.GetSummary());
                labResultCount++;
            }
        }
        if (labResultCount == 0)
            writer.WriteLine("немає");

        writer.WriteLine("-- Рецепти --");
        int prescriptionCount = 0;
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i] is Prescription prescription)
            {
                writer.WriteLine(prescription.GetSummary());
                prescriptionCount++;
            }
        }
        if (prescriptionCount == 0)
            writer.WriteLine("немає");
        writer.WriteLine();

        writer.WriteLine("-- Записи --");
        Appointment[] appointments = _clinic.Appointments.GetByPatient(patientId);
        for (int i = 0; i < appointments.Length; i++)
        {
            Appointment appointment = appointments[i];
            writer.WriteLine($"[{appointment.Id}] {appointment.GetDescription()} | {appointment.ScheduledAt:dd.MM.yyyy HH:mm} | {appointment.Status} | {appointment.GetCost():F2} грн");
        }
        if (appointments.Length == 0)
            writer.WriteLine("немає");
        writer.WriteLine();

        writer.WriteLine("-- Плани лікування --");
        TreatmentPlan[] plans = _clinic.TreatmentPlans.GetByPatient(patientId);
        for (int i = 0; i < plans.Length; i++)
        {
            writer.WriteLine($"[{plans[i].Id}] {plans[i].Diagnosis} | {plans[i].DurationDays} днів | {plans[i].Status}");
        }
        if (plans.Length == 0)
            writer.WriteLine("немає");
        writer.WriteLine();

        writer.WriteLine($"Заборгованість: {_clinic.Billing.GetPatientDebt(patientId):F2} грн");
    }
}

using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class SessionManager
{
    private readonly string _sessionPath;

    public SessionManager(string sessionPath = "session.dat")
    {
        _sessionPath = sessionPath;
    }

    public bool Exists()
    {
        return File.Exists(_sessionPath);
    }

    public void Save(Clinic clinic)
    {
        Patient[] patients = clinic.Patients.GetAll();
        using StreamWriter writer = new StreamWriter(_sessionPath, false, Encoding.UTF8);
        writer.WriteLine("[PATIENTS]");
        for (int i = 0; i < patients.Length; i++)
        {
            Patient patient = patients[i];
            writer.WriteLine($"{patient.FirstName},{patient.LastName},{patient.DateOfBirth:dd.MM.yyyy},{patient.BloodType},{patient.Phone}");
        }
        writer.WriteLine("[END]");
    }

    public int Load(Clinic clinic)
    {
        if (!File.Exists(_sessionPath))
            return 0;

        string[] lines = File.ReadAllLines(_sessionPath, Encoding.UTF8);
        string currentSection = "";
        int addedCount = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.StartsWith("["))
            {
                currentSection = line == "[END]" ? "" : line;
                continue;
            }

            if (currentSection != "[PATIENTS]")
                continue;

            try
            {
                Patient patient = ParsePatientLine(line);
                Patient[] existingPatients = clinic.Patients.GetAll();
                bool duplicate = false;
                for (int patientIndex = 0; patientIndex < existingPatients.Length; patientIndex++)
                {
                    Patient existingPatient = existingPatients[patientIndex];
                    if (existingPatient.FirstName == patient.FirstName
                        && existingPatient.LastName == patient.LastName
                        && existingPatient.DateOfBirth.Date == patient.DateOfBirth.Date)
                    {
                        duplicate = true;
                        break;
                    }
                }

                if (duplicate)
                    continue;

                clinic.Patients.Add(patient);
                addedCount++;
            }
            catch (Exception)
            {
            }
        }

        return addedCount;
    }

    private Patient ParsePatientLine(string line)
    {
        string[] parts = line.Split(',');
        if (parts.Length != 5)
            throw new ArgumentException($"Рядок має {parts.Length} полів замість 5.");

        for (int i = 0; i < parts.Length; i++)
            parts[i] = parts[i].Trim();

        DateTime dateOfBirth = DateTime.ParseExact(parts[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
        BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3]);
        if (!char.IsLetter(parts[3][0]) || bloodType.ToString() != parts[3])
            throw new ArgumentException("Невідоме значення групи крові.");

        return new Patient(parts[0], parts[1], dateOfBirth, bloodType, parts[4]);
    }
}

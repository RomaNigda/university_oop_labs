using System.Globalization;
using System.Text;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Utils;

public class CsvImporter
{
    public ImportResult ImportPatients(Clinic clinic, string filePath)
    {
        ImportResult result = new ImportResult();
        if (!File.Exists(filePath))
        {
            result.AddError(0, "Файл не знайдено: " + filePath);
            return result;
        }

        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
        for (int index = 0; index < lines.Length; index++)
        {
            int lineNumber = index + 1;
            string line = lines[index];
            if (index == 0 || string.IsNullOrWhiteSpace(line))
                continue;

            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != 5)
                {
                    result.AddError(lineNumber, $"Рядок має {parts.Length} полів замість 5.");
                    continue;
                }

                for (int i = 0; i < parts.Length; i++)
                    parts[i] = parts[i].Trim();

                DateTime dateOfBirth = DateTime.ParseExact(parts[2], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                BloodType bloodType = (BloodType)Enum.Parse(typeof(BloodType), parts[3]);
                if (!char.IsLetter(parts[3][0]) || bloodType.ToString() != parts[3])
                    throw new ArgumentException("Невідоме значення групи крові.");

                Patient patient = new Patient(parts[0], parts[1], dateOfBirth, bloodType, parts[4]);
                clinic.Patients.Add(patient);
                result.AddSuccess();
            }
            catch (Exception exception)
            {
                result.AddError(lineNumber, exception.Message);
            }
        }

        return result;
    }
}

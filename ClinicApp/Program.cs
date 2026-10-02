using ClinicApp;

Clinic clinic = new Clinic("Медична Клініка");

clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 12, 0, 0, 0), BloodType.APositive, "0501234567"));
clinic.Doctors.Add(new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567"));

Doctor[] found = clinic.Doctors.FindBySpeciality(Speciality.Cardiology);
Console.WriteLine($"Кардіологів: {found.Length}");

if (clinic.Patients.TryFindById(1, out Patient patient))
    Console.WriteLine("Знайдено: " + patient.FullName);

string name = clinic.Patients.FindById(99)?.FullName ?? "не знайдено";
Console.WriteLine(name);
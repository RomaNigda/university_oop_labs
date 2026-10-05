using ClinicApp;

Clinic clinic = new Clinic("Медична Клініка");

clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 12, 0, 0, 0), BloodType.APositive, "0501234567"));
clinic.Doctors.Add(new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567"));

clinic.Appointments.Book(1, 1, new DateTime(2027, 5, 10, 10, 0, 0));

Doctor[] cardiologists = clinic.Doctors.FindBySpeciality(Speciality.Cardiology);
Console.WriteLine($"Кардіологів (enum): {cardiologists.Length}");

Doctor[] partial = clinic.Doctors.FindBySpeciality("кардіо");
Console.WriteLine($"Кардіологів (string): {partial.Length}");

Appointment[] byDate = clinic.Appointments.GetByDate(2027, 5, 10);
Console.WriteLine($"Записів на 10.05.2027: {byDate.Length}");

if (clinic.Patients.TryFindById(1, out Patient patient))
    Console.WriteLine("Знайдено: " + patient.FullName);

string name = clinic.Patients.FindById(99)?.FullName ?? "не знайдено";
Console.WriteLine(name);
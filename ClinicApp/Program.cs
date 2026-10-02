using ClinicApp;

Clinic clinic = new Clinic("Медична Клініка");

clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 12, 0, 0, 0), BloodType.APositive, "0501234567"));
clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 7, 8, 0, 0, 0), BloodType.BNegative, "0672345678"));

clinic.Doctors.Add(new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567"));
clinic.Doctors.Add(new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678"));

Console.WriteLine("--- Індексатори ---");
Console.WriteLine(clinic.Patients[0]);
Console.WriteLine(clinic.Doctors[1]);

Console.WriteLine();
Console.WriteLine("--- ClinicFormatter ---");
Console.WriteLine(ClinicFormatter.FormatBloodType(BloodType.APositive));
Console.WriteLine(ClinicFormatter.FormatSpeciality(Speciality.Cardiology));
Console.WriteLine(ClinicFormatter.FormatAge(1));
Console.WriteLine(ClinicFormatter.FormatAge(3));
Console.WriteLine(ClinicFormatter.FormatAge(11));
Console.WriteLine(ClinicFormatter.FormatAge(21));
Console.WriteLine(ClinicFormatter.FormatPhone("0501234567"));
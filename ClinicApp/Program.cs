using ClinicApp;

Doctor d1 = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
d1.Schedule = new WorkSchedule(8, 16);

Doctor d2 = new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678");
d2.Schedule = new WorkSchedule(9, 18);

Doctor d3 = new Doctor("Андрій", "Власенко", Speciality.Pediatrics, "LIC-003", "0443456789");

Console.WriteLine(d1);
Console.WriteLine(d2);
Console.WriteLine(d3);

WorkSchedule morning = new WorkSchedule(8, 16);
WorkSchedule copy = morning;

Console.WriteLine($"morning: {morning}");
Console.WriteLine($"copy:    {copy}");
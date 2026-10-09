using System;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Models;

Clinic clinic = new Clinic("Медична Клініка");

clinic.Doctors.Add(new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567"));
clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 12, 0, 0, 0), BloodType.APositive, "0501234567"));

Console.WriteLine("=== Додавання пацієнта з некоректним ім'ям ===");
try
{
    Patient bad = new Patient("", "Коваль", new DateTime(1990, 5, 5, 0, 0, 0), BloodType.OPositive, "0672345678");
    clinic.Patients.Add(bad);
}
catch (ArgumentException e)
{
    Console.WriteLine("Помилка: " + e.Message);
}

Console.WriteLine();
Console.WriteLine("=== Додавання пацієнта з датою народження в майбутньому ===");
try
{
    Patient future = new Patient("Марія", "Ткач", new DateTime(2030, 1, 1, 0, 0, 0), BloodType.BNegative, "0933456789");
    clinic.Patients.Add(future);
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine("Помилка: " + e.Message);
}

Console.WriteLine();
Console.WriteLine("=== Додавання лікаря з некоректним графіком (20–6) ===");
try
{
    Doctor badSchedule = new Doctor("Петро", "Іванов", Speciality.Surgery, "LIC-002", "0442345678");
    badSchedule.Schedule = new WorkSchedule(20, 6);
    clinic.Doctors.Add(badSchedule);
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine("Помилка: " + e.Message);
}
catch (ArgumentException e)
{
    Console.WriteLine("Помилка: " + e.Message);
}

Console.WriteLine();
Console.WriteLine("=== Запис на прийом з некоректною тривалістю ===");
try
{
    clinic.Appointments.Book(1, 1, new DateTime(2027, 5, 9, 10, 0, 0), -15);
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine("Помилка: " + e.Message);
}

Console.WriteLine();
Console.WriteLine("=== Коректні дані — усе працює ===");
clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 7, 8, 0, 0, 0), BloodType.BNegative, "0672345678"));
clinic.Doctors.Add(new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-003", "0443456789"));
clinic.Appointments.Book(1, 1, new DateTime(2027, 5, 9, 10, 0, 0), 30);

clinic.Patients.DisplayAll();
clinic.Doctors.DisplayAll();

using ClinicApp;

Console.WriteLine("=== Тест GrowablePatientManager ===");
Console.WriteLine("Додаємо пацієнтів одного за одним...");

GrowablePatientManager manager = new GrowablePatientManager();
PatientManager patientManager = new PatientManager();

for (int i = 1; i <= 20; i++)
{
    manager.Add(new Patient($"Тест", $"Пацієнт{i}", new DateTime(1990, 1, 1, 0, 0, 0), BloodType.APositive, "0000000000"));
}

Console.WriteLine();
Console.WriteLine("Тест пошуку:");
Patient? found = manager.FindById(10);
if (found != null)
    Console.WriteLine($"  FindById(10) → {found.FullName}");
else
    Console.WriteLine("  FindById(10) → не знайдено");

Patient? notFound = manager.FindById(99);
if (notFound != null)
    Console.WriteLine($"  FindById(99) → {notFound.FullName}");
else
    Console.WriteLine("  FindById(99) → не знайдено");

Console.WriteLine();
Console.WriteLine("Порівняння:");
Console.WriteLine($"  PatientManager:         {patientManager.MaxCount} місць (фіксовано)");
Console.WriteLine($"  GrowablePatientManager:  {manager.Capacity} місця (зросте при потребі)");
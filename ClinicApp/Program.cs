using System.Globalization;
using System.Text;
using ClinicApp;
using ClinicApp.Comparators;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
Console.OutputEncoding = Encoding.UTF8;

Clinic clinic = new Clinic("Медична Клініка");

SeedPatientsAndDoctors(clinic);
SeedMedicalRecords(clinic);

UrgentAppointment urgent = new UrgentAppointment(1, 1, DateTime.Today.AddDays(1), "тест");
Appointment asBase = urgent;

Console.WriteLine("\n== new vs override ==");
Console.WriteLine($"Через Appointment:       {asBase.GetDescription()} | пріоритет {asBase.GetPriority()}");
Console.WriteLine($"Через UrgentAppointment: {urgent.GetDescription()} | пріоритет {urgent.GetPriority()}");

// 1. GetDescription() через Appointment дає "Терміновий (тест)", бо він virtual і перевизначений через override у UrgentAppointment — виклик іде за реальним типом об'єкта.
// 2. GetPriority() через Appointment повертає 3, бо базовий метод НЕ virtual, а в UrgentAppointment він прихований через new — виклик іде за типом змінної.
// 3. Щоб GetPriority() теж працював поліморфно, треба зробити його virtual в Appointment і override в UrgentAppointment (замість new).

bool running = true;
while (running)
{
    Console.WriteLine("\n+----------------------------------------------+");
    Console.WriteLine("|           МЕДИЧНА КЛІНІКА                    |");
    Console.WriteLine("+----------------------------------------------+");
    Console.WriteLine("|  1. Пацієнти       — реєстрація, пошук       |");
    Console.WriteLine("|  2. Лікарі         — персонал, розклад       |");
    Console.WriteLine("|  3. Записи         — прийоми, скасування     |");
    Console.WriteLine("|  4. Медична картка — діагнози, рецепти       |");
    Console.WriteLine("|  5. Рахунки        — оплата, борги           |");
    Console.WriteLine("|  6. Черга          — очікування, прийом      |");
    Console.WriteLine("|  7. Звіт           — загальна статистика     |");
    Console.WriteLine("|  8. Аналітика      — рейтинги та витрати     |");
    Console.WriteLine("|  9. Плани лікування — рефлексія, атрибути    |");
    Console.WriteLine("| 10. Файли          — журнал та звіти         |");
    Console.WriteLine("|  0. Вийти                                    |");
    Console.WriteLine("+----------------------------------------------+");
    Console.Write("Оберіть розділ: ");

    switch (Console.ReadLine())
    {
        case "1": PatientsMenu(clinic); break;
        case "2": DoctorsMenu(clinic); break;
        case "3": AppointmentsMenu(clinic); break;
        case "4": MedicalRecordsMenu(clinic); break;
        case "5": BillingMenu(clinic); break;
        case "6": WaitingRoomMenu(clinic); break;
        case "7": clinic.GenerateReport(); break;
        case "8": AnalyticsMenu(clinic); break;
        case "9": TreatmentPlansMenu(clinic); break;
        case "10": FilesMenu(clinic); break;
        case "0": running = false; break;
        default: Console.WriteLine("Невідомий пункт меню."); break;
    }
}

static void AnalyticsMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n── Аналітика ───────────────────");
        Console.WriteLine("1. Лікарі за навантаженням");
        Console.WriteLine("2. Лікарі за виручкою");
        Console.WriteLine("3. Лікарі за іменем");
        Console.WriteLine("4. Пацієнти за кількістю візитів");
        Console.WriteLine("5. Пацієнти за витратами");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть: ");

        switch (Console.ReadLine())
        {
            case "1":
            {
                List<DoctorStats> doctorStats = CollectDoctorStats(clinic);
                doctorStats.Sort();
                for (int i = 0; i < doctorStats.Count; i++)
                    Console.WriteLine(doctorStats[i]);
                break;
            }
            case "2":
            {
                List<DoctorStats> doctorStats = CollectDoctorStats(clinic);
                doctorStats.Sort(new DoctorStatsByRevenue());
                for (int i = 0; i < doctorStats.Count; i++)
                    Console.WriteLine(doctorStats[i]);
                break;
            }
            case "3":
            {
                List<DoctorStats> doctorStats = CollectDoctorStats(clinic);
                doctorStats.Sort(new DoctorStatsByName());
                for (int i = 0; i < doctorStats.Count; i++)
                    Console.WriteLine(doctorStats[i]);
                break;
            }
            case "4":
            {
                List<PatientStats> patientStats = CollectPatientStats(clinic);
                patientStats.Sort();
                for (int i = 0; i < patientStats.Count; i++)
                    Console.WriteLine(patientStats[i]);
                break;
            }
            case "5":
            {
                List<PatientStats> patientStats = CollectPatientStats(clinic);
                patientStats.Sort(new PatientStatsBySpent());
                for (int i = 0; i < patientStats.Count; i++)
                    Console.WriteLine(patientStats[i]);
                break;
            }
            case "0":
                back = true;
                break;
            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}

static List<DoctorStats> CollectDoctorStats(Clinic clinic)
{
    List<DoctorStats> stats = new List<DoctorStats>();
    foreach (DoctorStats stat in clinic.Analytics.ComputeDoctorStats())
        stats.Add(stat);

    return stats;
}

static List<PatientStats> CollectPatientStats(Clinic clinic)
{
    List<PatientStats> stats = new List<PatientStats>();
    foreach (PatientStats stat in clinic.Analytics.ComputePatientStats())
        stats.Add(stat);

    return stats;
}

static void SeedPatientsAndDoctors(Clinic clinic)
{
    clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 15), BloodType.APositive, "0501234567"));
    clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), BloodType.BNegative, "0672345678"));
    clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 3, 10), BloodType.OPositive, "0933456789"));
    clinic.Patients.Add(new Patient("Марія", "Ткач"));

    Doctor cardiologist = new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567");
    Doctor neurologist = new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678");
    Doctor pediatrician = new Doctor("Андрій", "Власенко", Speciality.Pediatrics, "LIC-003", "0443456789");

    cardiologist.Schedule = new WorkSchedule(8, 16);
    neurologist.Schedule = new WorkSchedule(9, 18);
    pediatrician.Schedule = new WorkSchedule(8, 17);

    clinic.Doctors.Add(cardiologist);
    clinic.Doctors.Add(neurologist);
    clinic.Doctors.Add(pediatrician);

    DateTime tomorrow = DateTime.Today.AddDays(1);
    clinic.Appointments.Book(1, 1, tomorrow.AddHours(10), 30);
    clinic.Appointments.BookUrgent(2, 2, tomorrow.AddHours(11), "біль у грудях", 30);
    clinic.Appointments.BookSpecialist(3, 3, tomorrow.AddHours(12), "кардіологія", 60);
}

static void SeedMedicalRecords(Clinic clinic)
{
    MedicalRecordManager records = clinic.MedicalRecords;

    records.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-30), "I10", "Гіпертонічна хвороба", isChronic: true));
    records.Add(new Diagnosis(1, 1, DateTime.Today.AddDays(-5), "J06.9", "Гострий ринофарингіт"));
    records.Add(new LabResult(1, 1, DateTime.Today.AddDays(-10), "Гемоглобін", 145, "г/л", "120–160", isNormal: true));
    records.Add(new LabResult(1, 1, DateTime.Today.AddDays(-3), "Холестерин", 6.2, "ммоль/л", "< 5.2", isNormal: false));
    records.Add(new Prescription(1, 1, DateTime.Today.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));
    records.Add(new Prescription(2, 2, DateTime.Today.AddDays(-40), "Амоксицилін", "500 мг", 10, "3 рази на добу"));
    records.Add(new Diagnosis(2, 2, DateTime.Today.AddDays(-2), "G43.9", "Мігрень"));
    records.Add(new LabResult(3, 3, DateTime.Today.AddDays(-1), "Лейкоцити", 6.5, "10^9/л", "4.0–9.0", isNormal: true));
    records.Add(new Diagnosis(3, 3, DateTime.Today.AddDays(-7), "J20.9", "Гострий бронхіт"));
}



static bool TryReadInt(string prompt, out int value)
{
    Console.Write(prompt);
    if (int.TryParse(Console.ReadLine(), out value))
        return true;

    Console.WriteLine("Некоректне число.");
    return false;
}

static void FilesMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n=== Файли ===");
        Console.WriteLine("1. Останні рядки лога");
        Console.WriteLine("2. Очистити лог");
        Console.WriteLine("3. Експортувати всі звіти");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        switch (Console.ReadLine())
        {
            case "1":
                if (!TryReadInt("Кількість останніх рядків: ", out int count))
                    break;

                string[] lines = clinic.Logger.GetLastLines(count);
                if (lines.Length == 0)
                {
                    Console.WriteLine("Лог порожній або відсутній.");
                    break;
                }

                for (int i = 0; i < lines.Length; i++)
                    Console.WriteLine(lines[i]);
                break;
            case "2":
                clinic.Logger.Clear();
                Console.WriteLine("Лог очищено.");
                break;
            case "3":
            {
                string[] reportPaths = clinic.Exporter.ExportAll();
                for (int i = 0; i < reportPaths.Length; i++)
                    Console.WriteLine(reportPaths[i]);
                clinic.Logger.LogInfo("Експортовано всі звіти.");
                break;
            }
            case "0":
                back = true;
                break;
            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}

static bool TryReadDouble(string prompt, out double value)
{
    Console.Write(prompt);
    if (double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out value))
        return true;

    Console.WriteLine("Некоректне число.");
    return false;
}

static string ReadText(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine() ?? "";
}

static bool TryReadDate(out int year, out int month, out int day)
{
    month = 0;
    day = 0;
    return TryReadInt("Рік: ", out year)
        && TryReadInt("Місяць: ", out month)
        && TryReadInt("День: ", out day);
}

static bool TryReadPatientAndDoctor(Clinic clinic, out int patientId, out int doctorId)
{
    doctorId = 0;

    Console.WriteLine("\nДоступні пацієнти:");
    clinic.Patients.DisplayAll();
    Console.WriteLine("\nДоступні лікарі:");
    clinic.Doctors.DisplayAll();

    if (!TryReadInt("ID пацієнта: ", out patientId)) return false;
    if (!TryReadInt("ID лікаря: ", out doctorId)) return false;

    if (clinic.Patients.FindById(patientId) == null)
    {
        Console.WriteLine("Пацієнта не знайдено.");
        return false;
    }
    if (clinic.Doctors.FindById(doctorId) == null)
    {
        Console.WriteLine("Лікаря не знайдено.");
        return false;
    }
    return true;
}



static void PatientsMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n=== Пацієнти ===");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати пацієнта");
        Console.WriteLine("3. Знайти за ім'ям");
        Console.WriteLine("4. Видалити за ID");
        Console.WriteLine("5. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        switch (Console.ReadLine())
        {
            case "1": clinic.Patients.DisplayAll(); break;
            case "2": AddPatient(clinic); break;
            case "3": FindPatients(clinic); break;
            case "4": RemovePatient(clinic); break;
            case "5": clinic.Patients.DisplayStats(); break;
            case "0": back = true; break;
            default: Console.WriteLine("Невідомий пункт меню."); break;
        }
    }
}

static void WaitingRoomMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n=== Черга ===");
        Console.WriteLine("1. Додати пацієнта до черги");
        Console.WriteLine("2. Прийняти першого");
        Console.WriteLine("3. Хто перший?");
        Console.WriteLine("4. Переглянути всю чергу");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        switch (Console.ReadLine())
        {
            case "1":
                if (!TryReadInt("ID пацієнта: ", out int patientId)) break;

                Patient? patient = clinic.Patients.FindById(patientId);
                if (patient == null)
                {
                    Console.WriteLine("Пацієнта не знайдено.");
                    break;
                }

                clinic.WaitingRoom.Enqueue(patient);
                Console.WriteLine($"Пацієнта {patient.FullName} додано до черги.");
                break;
            case "2":
                try
                {
                    Patient nextPatient = clinic.WaitingRoom.Dequeue();
                    Console.WriteLine($"Прийнято: {nextPatient.FullName}. У черзі лишилось: {clinic.WaitingRoom.Count}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case "3":
                try
                {
                    Patient nextPatient = clinic.WaitingRoom.Peek();
                    Console.WriteLine($"Перший у черзі: {nextPatient.FullName}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                break;
            case "4":
                Patient[] waitingPatients = clinic.WaitingRoom.ToArray();
                if (waitingPatients.Length == 0)
                {
                    Console.WriteLine("Черга порожня.");
                    break;
                }

                for (int i = 0; i < waitingPatients.Length; i++)
                    Console.WriteLine($"{i + 1}. {waitingPatients[i].FullName}");
                break;
            case "0":
                back = true;
                break;
            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}

static void AddPatient(Clinic clinic)
{
    string firstName = ReadText("Ім'я: ");
    string lastName = ReadText("Прізвище: ");

    if (!TryReadInt("Рік народження: ", out int year)) return;
    if (!TryReadInt("Місяць народження: ", out int month)) return;
    if (!TryReadInt("День народження: ", out int day)) return;

    Console.WriteLine("Група крові:");
    Console.WriteLine("0. Unknown      1. APositive   2. ANegative");
    Console.WriteLine("3. BPositive    4. BNegative   5. ABPositive");
    Console.WriteLine("6. ABNegative   7. OPositive   8. ONegative");
    if (!TryReadInt("Ваш вибір: ", out int bloodTypeNumber)) return;

    string phone = ReadText("Телефон: ");

    try
    {
        Patient patient = new Patient(firstName, lastName, new DateTime(year, month, day),
            (BloodType)bloodTypeNumber, phone);
        clinic.Patients.Add(patient);
        clinic.Logger.LogInfo($"Додано пацієнта #{patient.Id}: {patient.FullName}");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
}

static void FindPatients(Clinic clinic)
{
    string name = ReadText("Введіть ім'я або прізвище: ");
    Patient[] found = clinic.Patients.FindByName(name);

    if (found.Length == 0)
    {
        Console.WriteLine("Пацієнтів не знайдено.");
        return;
    }

    for (int i = 0; i < found.Length; i++)
        Console.WriteLine(found[i]);
}

static void RemovePatient(Clinic clinic)
{
    if (!TryReadInt("Введіть ID: ", out int id)) return;

    Console.WriteLine(clinic.Patients.Remove(id)
        ? "Пацієнта видалено."
        : "Пацієнта не знайдено.");
}



static void DoctorsMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n=== Лікарі ===");
        Console.WriteLine("1. Показати всіх");
        Console.WriteLine("2. Додати лікаря");
        Console.WriteLine("3. Знайти за спеціальністю");
        Console.WriteLine("4. Видалити за ID");
        Console.WriteLine("5. Вільні години лікаря");
        Console.WriteLine("6. Статистика");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        switch (Console.ReadLine())
        {
            case "1": clinic.Doctors.DisplayAll(); break;
            case "2": AddDoctor(clinic); break;
            case "3": FindDoctors(clinic); break;
            case "4": RemoveDoctor(clinic); break;
            case "5": ShowDoctorSlots(clinic); break;
            case "6": clinic.Doctors.DisplayStats(); break;
            case "0": back = true; break;
            default: Console.WriteLine("Невідомий пункт меню."); break;
        }
    }
}

static void ShowDoctorSlots(Clinic clinic)
{
    clinic.Doctors.DisplayAll();
    if (!TryReadInt("ID лікаря: ", out int doctorId)) return;

    Doctor? found = clinic.Doctors.FindById(doctorId);
    if (found == null)
    {
        Console.WriteLine("Лікаря не знайдено.");
        return;
    }

    ISchedulable s = found;

    string dateStr = ReadText("Дата (dd.MM.yyyy): ");
    if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
    {
        Console.WriteLine("Некоректна дата. Формат: dd.MM.yyyy");
        return;
    }

    if (!TryReadInt("Скільки слотів: ", out int slotsCount)) return;

    try
    {
        DateTime[] slots = s.GetAvailableSlots(date, slotsCount);
        for (int i = 0; i < slots.Length; i++)
            Console.WriteLine(slots[i].ToString("HH:mm"));
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
}

static void AddDoctor(Clinic clinic)
{
    string firstName = ReadText("Ім'я: ");
    string lastName = ReadText("Прізвище: ");

    Console.WriteLine("Спеціальність:");
    Console.WriteLine("0. General     1. Cardiology   2. Neurology    3. Pediatrics");
    Console.WriteLine("4. Surgery     5. Orthopedics  6. Dermatology  7. Emergency");
    if (!TryReadInt("Ваш вибір: ", out int specialityNumber)) return;

    string licenseNumber = ReadText("Номер ліцензії: ");
    string phone = ReadText("Телефон: ");

    if (!TryReadInt("Початок робочого дня (0-23): ", out int startHour)) return;
    if (!TryReadInt("Кінець робочого дня (1-24): ", out int endHour)) return;

    try
    {
        Doctor doctor = new Doctor(firstName, lastName, (Speciality)specialityNumber,
            licenseNumber, phone);
        doctor.Schedule = new WorkSchedule(startHour, endHour);
        clinic.Doctors.Add(doctor);
        clinic.Logger.LogInfo($"Додано лікаря #{doctor.Id}: {doctor.FullName}");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
}

static void FindDoctors(Clinic clinic)
{
    string speciality = ReadText("Введіть спеціальність: ");
    Doctor[] found = clinic.Doctors.FindBySpeciality(speciality);

    if (found.Length == 0)
    {
        Console.WriteLine("Лікарів не знайдено.");
        return;
    }

    for (int i = 0; i < found.Length; i++)
        Console.WriteLine(found[i]);
}

static void RemoveDoctor(Clinic clinic)
{
    if (!TryReadInt("Введіть ID: ", out int id)) return;

    Console.WriteLine(clinic.Doctors.Remove(id)
        ? "Лікаря видалено."
        : "Лікаря не знайдено.");
}



static void AppointmentsMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n=== Записи на прийом ===");
        Console.WriteLine("1. Майбутні записи");
        Console.WriteLine("2. Створити запис");
        Console.WriteLine("3. Записи пацієнта");
        Console.WriteLine("4. Записи лікаря");
        Console.WriteLine("5. Записи за датою");
        Console.WriteLine("6. Скасувати запис");
        Console.WriteLine("7. Завершити запис");
        Console.WriteLine("8. Скасувати всі записи пацієнта");
        Console.WriteLine("9. За типом прийому");
        Console.WriteLine("0. Назад");
        Console.Write("Ваш вибір: ");

        switch (Console.ReadLine())
        {
            case "1": clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming()); break;
            case "2": BookAppointment(clinic); break;
            case "3": ShowPatientAppointments(clinic); break;
            case "4": ShowDoctorAppointments(clinic); break;
            case "5": ShowAppointmentsByDate(clinic); break;
            case "6": CancelAppointment(clinic); break;
            case "7": CompleteAppointment(clinic); break;
            case "8": CancelAllPatientAppointments(clinic); break;
            case "9": ShowAppointmentsByType(clinic); break;
            case "0": back = true; break;
            default: Console.WriteLine("Невідомий пункт меню."); break;
        }
    }
}

static void CancelAllPatientAppointments(Clinic clinic)
{
    clinic.Patients.DisplayAll();
    if (!TryReadInt("ID пацієнта: ", out int patientId)) return;

    string reason = ReadText("Причина (Enter — без причини): ");
    Appointment[] appointments = clinic.Appointments.GetByPatient(patientId);
    
    int count = AppointmentManager.CancelAll(appointments, reason);
    Console.WriteLine($"Скасовано записів: {count}");
    if (count > 0)
    {
        Patient? patient = clinic.Patients.FindById(patientId);
        clinic.Logger.LogInfo($"Скасовано {count} записів пацієнта #{patientId}: {patient?.FullName ?? "невідомий"}");
    }
}


static void ShowAppointmentsByType(Clinic clinic)
{
    Console.WriteLine("  1. Термінові");
    Console.WriteLine("  2. Консультації спеціаліста");
    Console.WriteLine("  3. Звичайні");
    Console.Write("Оберіть: ");

    string choice = Console.ReadLine() ?? "";

    if (choice == "1")
        clinic.Appointments.DisplayList(clinic.Appointments.GetUrgent());
    else if (choice == "2")
        clinic.Appointments.DisplayList(clinic.Appointments.GetSpecialist());
    else if (choice == "3")
        clinic.Appointments.DisplayList(clinic.Appointments.GetRegular());
    else
        Console.WriteLine("Невідомий пункт меню.");
}

static void BookAppointment(Clinic clinic)
{
    if (!TryReadPatientAndDoctor(clinic, out int patientId, out int doctorId)) return;

    if (!TryReadDate(out int year, out int month, out int day)) return;
    if (!TryReadInt("Година: ", out int hour)) return;
    if (!TryReadInt("Хвилина: ", out int minute)) return;
    if (!TryReadInt("Тривалість у хвилинах: ", out int duration)) return;

    try
    {
        DateTime scheduledAt = new DateTime(year, month, day, hour, minute, 0);
        if (clinic.Appointments.Book(patientId, doctorId, scheduledAt, duration))
        {
            Appointment[] appointments = clinic.Appointments.GetAll();
            Appointment appointment = appointments[appointments.Length - 1];
            Patient? patient = clinic.Patients.FindById(patientId);
            Doctor? doctor = clinic.Doctors.FindById(doctorId);
            clinic.Logger.LogInfo($"Створено запис #{appointment.Id}: {patient?.FullName} → {doctor?.FullName}");
        }
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
}

static void ShowPatientAppointments(Clinic clinic)
{
    clinic.Patients.DisplayAll();
    if (!TryReadInt("ID пацієнта: ", out int patientId)) return;

    clinic.Appointments.DisplayList(clinic.Appointments.GetByPatient(patientId));
}

static void ShowDoctorAppointments(Clinic clinic)
{
    clinic.Doctors.DisplayAll();
    if (!TryReadInt("ID лікаря: ", out int doctorId)) return;

    clinic.Appointments.DisplayList(clinic.Appointments.GetByDoctor(doctorId));
}

static void ShowAppointmentsByDate(Clinic clinic)
{
    if (!TryReadDate(out int year, out int month, out int day)) return;

    try
    {
        clinic.Appointments.DisplayList(clinic.Appointments.GetByDate(year, month, day));
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
}

static void CancelAppointment(Clinic clinic)
{
    if (!TryReadInt("ID запису: ", out int id)) return;
    string reason = ReadText("Причина скасування: ");

    if (clinic.Appointments.Cancel(id, reason))
    {
        Console.WriteLine("Запис скасовано.");
        clinic.Logger.LogInfo(GetAppointmentLogDescription(clinic, id) + " скасовано");
    }
    else
    {
        Console.WriteLine("Запис не знайдено або його вже завершено/скасовано.");
    }
}

static void CompleteAppointment(Clinic clinic)
{
    if (!TryReadInt("ID запису: ", out int id)) return;

    if (clinic.Appointments.Complete(id))
    {
        Console.WriteLine("Запис завершено.");
        clinic.Logger.LogInfo(GetAppointmentLogDescription(clinic, id) + " завершено");
    }
    else
    {
        Console.WriteLine("Запис не знайдено або його вже завершено/скасовано.");
    }
}



static void MedicalRecordsMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n── Медична картка ────────────");
        Console.WriteLine("1. Картка пацієнта");
        Console.WriteLine("2. Усі записи пацієнта");
        Console.WriteLine("3. Додати діагноз");
        Console.WriteLine("4. Додати аналіз");
        Console.WriteLine("5. Додати рецепт");
        Console.WriteLine("6. Записи лікаря");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть: ");

        switch (Console.ReadLine())
        {
            case "1": ShowPatientCard(clinic); break;
            case "2": ShowPatientRecords(clinic); break;
            case "3": AddDiagnosis(clinic); break;
            case "4": AddLabResult(clinic); break;
            case "5": AddPrescription(clinic); break;
            case "6": ShowDoctorRecords(clinic); break;
            case "0": back = true; break;
            default: Console.WriteLine("Невідомий пункт меню."); break;
        }
    }
}

static void ShowPatientCard(Clinic clinic)
{
    if (!TryReadInt("ID пацієнта: ", out int patientId)) return;

    if (clinic.Patients.FindById(patientId) == null)
    {
        Console.WriteLine("Пацієнта не знайдено.");
        return;
    }

    clinic.MedicalRecords.DisplayPatientSummary(patientId);
}


static void ShowPatientRecords(Clinic clinic)
{
    if (!TryReadInt("ID пацієнта: ", out int patientId)) return;

    if (clinic.Patients.FindById(patientId) == null)
    {
        Console.WriteLine("Пацієнта не знайдено.");
        return;
    }

    MedicalRecord[] records = clinic.MedicalRecords.GetByPatient(patientId);
    if (records.Length == 0)
    {
        Console.WriteLine("Записів немає.");
        return;
    }

    Console.WriteLine($"=== Записи пацієнта #{patientId} ===");
    for (int i = 0; i < records.Length; i++)
        Console.WriteLine($"{records[i].GetRecordType()}: {records[i].GetSummary()}");

    Console.WriteLine("\n--- Повний вивід (ToString) ---");
    clinic.MedicalRecords.DisplayList(records);
}

static void ShowDoctorRecords(Clinic clinic)
{
    if (!TryReadInt("ID лікаря: ", out int doctorId)) return;

    if (clinic.Doctors.FindById(doctorId) == null)
    {
        Console.WriteLine("Лікаря не знайдено.");
        return;
    }

    clinic.MedicalRecords.DisplayList(clinic.MedicalRecords.GetByDoctor(doctorId));
}

static void AddDiagnosis(Clinic clinic)
{
    if (!TryReadPatientAndDoctor(clinic, out int patientId, out int doctorId)) return;

    string code = ReadText("Код діагнозу (напр. J06.9): ");
    string description = ReadText("Опис: ");
    if (!TryReadInt("Хронічне? (1=так, 0=ні): ", out int chronic)) return;

    try
    {
        Diagnosis diagnosis = new Diagnosis(patientId, doctorId, DateTime.Today,
            code, description, chronic == 1);
        clinic.MedicalRecords.Add(diagnosis);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
}

static void AddLabResult(Clinic clinic)
{
    if (!TryReadPatientAndDoctor(clinic, out int patientId, out int doctorId)) return;

    string testName = ReadText("Назва аналізу: ");
    if (!TryReadDouble("Значення (число): ", out double value)) return;
    string unit = ReadText("Одиниці виміру: ");
    string referenceRange = ReadText("Норма (напр. 4.0–9.0): ");
    if (!TryReadInt("В нормі? (1=так, 0=ні): ", out int normal)) return;

    try
    {
        LabResult result = new LabResult(patientId, doctorId, DateTime.Today,
            testName, value, unit, referenceRange, normal == 1);
        clinic.MedicalRecords.Add(result);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
}

static void AddPrescription(Clinic clinic)
{
    if (!TryReadPatientAndDoctor(clinic, out int patientId, out int doctorId)) return;

    string medication = ReadText("Препарат: ");
    string dosage = ReadText("Дозування (10 мг): ");
    if (!TryReadInt("Кількість днів: ", out int days)) return;
    string instructions = ReadText("Інструкція (Enter — пропустити): ");

    try
    {
        Prescription prescription = new Prescription(patientId, doctorId, DateTime.Today,
            medication, dosage, days, instructions);
        clinic.MedicalRecords.Add(prescription);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message}");
        LogInputError(clinic, ex);
    }
}



static void BillingMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n--- Рахунки --------------------");
        Console.WriteLine("  1. Борги пацієнта");
        Console.WriteLine("  2. Всі неоплачені записи");
        Console.WriteLine("  3. Оплатити запис");
        Console.WriteLine("  4. Загальний борг клініки");
        Console.WriteLine("  0. Назад");
        Console.Write("Оберіть: ");

        switch (Console.ReadLine())
        {
            case "1": ShowPatientDebt(clinic); break;
            case "2": clinic.Billing.DisplayUnpaid(clinic.Billing.GetAllUnpaid()); break;
            case "3": PayAppointment(clinic); break;
            case "4": Console.WriteLine($"Загальний борг по клініці: {clinic.Billing.GetTotalDebt():F2} грн"); break;
            case "0": back = true; break;
            default: Console.WriteLine("Невідомий пункт меню."); break;
        }
    }
}

static void ShowPatientDebt(Clinic clinic)
{
    clinic.Patients.DisplayAll();
    if (!TryReadInt("ID пацієнта: ", out int patientId)) return;

    IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(patientId);
    if (unpaid.Length == 0)
    {
        Console.WriteLine("Немає неоплачених записів.");
        Console.WriteLine($"Борг: {clinic.Billing.GetPatientDebt(patientId):F2} грн");
        return;
    }

    Console.WriteLine($"Неоплачені записи пацієнта #{patientId}:");
    clinic.Billing.DisplayUnpaid(unpaid);
    Console.WriteLine($"Борг: {clinic.Billing.GetPatientDebt(patientId):F2} грн");
}

static void PayAppointment(Clinic clinic)
{
    if (!TryReadInt("ID запису для оплати: ", out int appointmentId)) return;

    if (clinic.Billing.PayAppointment(appointmentId))
    {
        Console.WriteLine($"Запис [{appointmentId}] оплачено.");
        clinic.Logger.LogInfo(GetAppointmentLogDescription(clinic, appointmentId) + " оплачено");
    }
    else
    {
        Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
    }
}

static void LogInputError(Clinic clinic, Exception exception)
{
    clinic.Logger.LogWarning("Помилка введення: " + exception.Message);
}

static string GetAppointmentLogDescription(Clinic clinic, int appointmentId)
{
    Appointment[] appointments = clinic.Appointments.GetAll();
    for (int i = 0; i < appointments.Length; i++)
    {
        if (appointments[i].Id != appointmentId)
            continue;

        Patient? patient = clinic.Patients.FindById(appointments[i].PatientId);
        return $"Запис #{appointmentId} пацієнта {patient?.FullName ?? "невідомий"}";
    }

    return $"Запис #{appointmentId}";
}

static void TreatmentPlansMenu(Clinic clinic)
{
    bool back = false;
    while (!back)
    {
        Console.WriteLine("\n── Плани лікування ────────────────────");
        Console.WriteLine("  1. Показати всі плани");
        Console.WriteLine("  2. Додати план лікування");
        Console.WriteLine("  3. Плани пацієнта");
        Console.WriteLine("  4. Активувати план");
        Console.WriteLine("  5. Завершити план");
        Console.WriteLine("  6. Скасувати план");
        Console.WriteLine("  7. Інформація про тип TreatmentPlan");
        Console.WriteLine("  0. Назад");
        Console.Write("Оберіть: ");

        switch (Console.ReadLine())
        {
            case "1":
                DisplayTreatmentPlans(clinic.TreatmentPlans.GetAll());
                break;
            case "2":
                TreatmentPlan plan = FormBuilder.Build<TreatmentPlan>();
                Console.WriteLine(clinic.TreatmentPlans.Add(plan)
                    ? "План додано."
                    : "План не додано.");
                break;
            case "3":
                if (TryReadInt("ID пацієнта: ", out int patientId))
                    DisplayTreatmentPlans(clinic.TreatmentPlans.GetByPatient(patientId));
                break;
            case "4":
                ChangeTreatmentPlanStatus(clinic, 1);
                break;
            case "5":
                ChangeTreatmentPlanStatus(clinic, 2);
                break;
            case "6":
                ChangeTreatmentPlanStatus(clinic, 3);
                break;
            case "7":
                ModelValidator.PrintInfo(typeof(TreatmentPlan));
                break;
            case "0":
                back = true;
                break;
            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}

static void DisplayTreatmentPlans(TreatmentPlan[] plans)
{
    if (plans.Length == 0)
    {
        Console.WriteLine("Планів не знайдено.");
        return;
    }

    for (int i = 0; i < plans.Length; i++)
    {
        Console.WriteLine($"[{plans[i].Id}] Пацієнт: {plans[i].PatientId} | Діагноз: {plans[i].Diagnosis} | Тривалість: {plans[i].DurationDays} дн. | Статус: {plans[i].Status}");
    }
}

static void ChangeTreatmentPlanStatus(Clinic clinic, int action)
{
    if (!TryReadInt("ID плану: ", out int id))
        return;

    TreatmentPlan? plan = clinic.TreatmentPlans.GetById(id);
    if (plan == null)
    {
        Console.WriteLine("План не знайдено.");
        return;
    }

    bool changed;
    string successMessage;
    if (action == 1)
    {
        changed = plan.Activate();
        successMessage = "План активовано.";
    }
    else if (action == 2)
    {
        changed = plan.Complete();
        successMessage = "План завершено.";
    }
    else
    {
        changed = plan.Cancel();
        successMessage = "План скасовано.";
    }

    Console.WriteLine(changed ? successMessage : "Перехід неможливий.");
}
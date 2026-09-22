using System;
using ClinicApp;

PatientManager patientManager = new PatientManager();
DoctorManager doctorManager = new DoctorManager();

patientManager.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 12, 0, 0, 0), "A+", "0501234567"));
patientManager.Add(new Patient("Олена", "Коваль", new DateTime(1993, 7, 8, 0, 0, 0), "B-", "0672345678"));
patientManager.Add(new Patient("Максим", "Бойко", new DateTime(2010, 5, 20, 0, 0, 0), "O+", "0933456789"));

doctorManager.Add(new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567"));
doctorManager.Add(new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678"));
doctorManager.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789"));

AppointmentManager appointmentManager = new AppointmentManager(patientManager, doctorManager);

appointmentManager.Book(1, 1, new DateTime(2027, 5, 9, 10, 0, 0));
appointmentManager.Book(2, 2, new DateTime(2027, 5, 9, 11, 0, 0), 45);
appointmentManager.Book(3, 3, new DateTime(2027, 5, 10, 9, 0, 0), 20);
appointmentManager.Book(99, 1, new DateTime(2027, 5, 9, 12, 0, 0));

Console.WriteLine();
Console.WriteLine("Майбутні записи:");
appointmentManager.DisplayList(appointmentManager.GetUpcoming());

Console.WriteLine();
appointmentManager.Cancel(1, "Пацієнт не зміг прийти");

Console.WriteLine();
Console.WriteLine("Записи пацієнта #2:");
appointmentManager.DisplayList(appointmentManager.GetByPatient(2));
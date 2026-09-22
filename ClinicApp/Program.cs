using ClinicApp;

DoctorManager manager = new DoctorManager();

Doctor d1 = new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567");
d1.WorkEndHour = 16;
manager.Add(d1);

Doctor d2 = new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678");
d2.WorkStartHour = 9;
d2.WorkEndHour = 18;
manager.Add(d2);

manager.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789"));

manager.DisplayAll();
manager.DisplayStats();
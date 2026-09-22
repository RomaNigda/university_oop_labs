using ClinicApp;

PatientManager manager = new PatientManager();

manager.Add(new Patient("Іван", "Петренко", new DateTime(1985, 3, 12), "A+", "0501234567"));
manager.Add(new Patient("Олена", "Коваль", new DateTime(1993, 7, 8), "B-", "0672345678"));
manager.Add(new Patient("Максим", "Бойко", new DateTime(2010, 5, 20), "O+", "0933456789"));
manager.Add(new Patient("Марія", "Ткач"));


manager.DisplayAll();
manager.DisplayStats();
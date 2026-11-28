using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp;

public class Clinic
{
    public string Name { get; }
    public PatientManager Patients { get; }
    public WaitingQueue<Patient> WaitingRoom { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }
    public TreatmentPlanManager TreatmentPlans { get; }
    public AnalyticsManager Analytics { get; }
    public MedicalRecordManager MedicalRecords { get; }
    public BillingManager Billing { get; }
    public ClinicLogger Logger { get; }
    public PatientPassportWriter Passport { get; }
    public SessionEventTracker Tracker { get; }
    public ClinicExporter Exporter { get; }
    public SessionManager Session { get; }

    public Clinic(string name)
    {
        Name = name;
        Patients = new PatientManager();
        WaitingRoom = new WaitingQueue<Patient>();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
        TreatmentPlans = new TreatmentPlanManager();
        Analytics = new AnalyticsManager(Appointments, Doctors, Patients);
        MedicalRecords = new MedicalRecordManager();
        Billing = new BillingManager(Appointments);
        Logger = new ClinicLogger();
        Passport = new PatientPassportWriter(this);
        Tracker = new SessionEventTracker(this);
        Exporter = new ClinicExporter(this);
        Session = new SessionManager();

        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        Patients.PatientAdded += Logger.OnPatientAdded;
        Appointments.AppointmentBooked += Logger.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Logger.OnUrgentAppointmentBooked;
        Appointments.AppointmentCancelled += Logger.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Logger.OnAppointmentCompleted;
        Billing.PaymentReceived += Logger.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Logger.OnPlanCompleted;

        Patients.PatientAdded += Passport.OnPatientAdded;
        Appointments.AppointmentCompleted += Passport.OnAppointmentCompleted;
        TreatmentPlans.PlanCompleted += Passport.OnPlanCompleted;

        Patients.PatientAdded += Tracker.OnPatientAdded;
        Appointments.AppointmentBooked += Tracker.OnAppointmentBooked;
        Appointments.UrgentAppointmentBooked += Tracker.OnUrgentAppointmentBooked;
        Appointments.AppointmentCancelled += Tracker.OnAppointmentCancelled;
        Appointments.AppointmentCompleted += Tracker.OnAppointmentCompleted;
        Billing.PaymentReceived += Tracker.OnPaymentReceived;
        TreatmentPlans.PlanCompleted += Tracker.OnPlanCompleted;
    }

    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"=== Розклад на {date:dd.MM.yyyy} ===");
        Appointments.DisplayList(Appointments.GetByDate(date));
    }

    public void GenerateReport()
    {
        Appointment[] upcoming = Appointments.GetUpcoming();
        Doctor[] allDoctors = Doctors.GetAll();

        Console.WriteLine("+----------------------------------------------+");
        Console.WriteLine($"|  Звіт — {Name}");
        Console.WriteLine("+----------------------------------------------+");
        Console.WriteLine($"|  Пацієнтів:          {Patients.Count}");
        Console.WriteLine($"|  Лікарів:            {Doctors.Count}");
        Console.WriteLine($"|  Майбутніх записів:  {upcoming.Length}");
        Console.WriteLine("+----------------------------------------------+");
        Console.WriteLine("|  Навантаження лікарів (майбутні записи):");

        for (int i = 0; i < allDoctors.Length; i++)
        {
            int count = 0;
            for (int j = 0; j < upcoming.Length; j++)
            {
                if (upcoming[j].DoctorId == allDoctors[i].Id) count++;
            }
            Console.WriteLine($"|    {allDoctors[i].FullName} ({allDoctors[i].Speciality}): {count} записів");
        }

        Console.WriteLine("+----------------------------------------------+");
    }
}
namespace ClinicApp;

public class AppointmentManager
{
    private const int MaxAppointments = 500;
    private Appointment[] _appointments;
    private int _count;
    private PatientManager _patients;
    private DoctorManager _doctors;

    public int Count
    {
        get { return _count; }
    }

    public AppointmentManager(PatientManager patients, DoctorManager doctors)
    {
        _appointments = new Appointment[MaxAppointments];
        _count = 0;
        _patients = patients;
        _doctors = doctors;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Patient? patient = _patients.FindById(patientId);
        if (patient == null)
        {
            Console.WriteLine($"Помилка: пацієнта з ID {patientId} не знайдено.");
            return false;
        }

        Doctor? doctor = _doctors.FindById(doctorId);
        if (doctor == null)
        {
            Console.WriteLine($"Помилка: лікаря з ID {doctorId} не знайдено.");
            return false;
        }

        if (_count >= MaxAppointments)
        {
            Console.WriteLine("Досягнуто ліміт записів.");
            return false;
        }

        Appointment appointment = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);
        _appointments[_count] = appointment;
        _count++;

        Console.WriteLine($"Запис [{appointment.Id}] створено: {patient.FullName} → {doctor.FullName} о {scheduledAt:dd.MM.yyyy HH:mm}");
        return true;
    }

    private Appointment? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
                return _appointments[i];
        }
        return null;
    }

    public bool Cancel(int id, string reason = "")
    {
        Appointment? appointment = FindById(id);
        if (appointment == null)
            return false;

        bool result = appointment.Cancel(reason);
        if (result)
            Console.WriteLine($"Запис [{id}] скасовано.");
        return result;
    }

    public bool Complete(int id)
    {
        Appointment? appointment = FindById(id);
        if (appointment == null)
            return false;

        bool result = appointment.Complete();
        if (result)
            Console.WriteLine($"Запис [{id}] завершено.");
        return result;
    }

    public Appointment[] GetByPatient(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId) matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetByDoctor(int doctorId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId) matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetByDate(DateTime date)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date) matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetUpcoming()
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming) matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public void DisplayAppointment(Appointment appointment)
    {
        Patient? patient = _patients.FindById(appointment.PatientId);
        Doctor? doctor = _doctors.FindById(appointment.DoctorId);

        string patientName = (patient != null) ? patient.FullName : $"Пацієнт #{appointment.PatientId}";
        string doctorName = (doctor != null) ? doctor.FullName : $"Лікар #{appointment.DoctorId}";

        string notes = (appointment.Notes.Length > 0) ? $" | {appointment.Notes}" : "";

        Console.WriteLine($"[{appointment.Id}] {patientName} → {doctorName} | {appointment.ScheduledAt:dd.MM.yyyy HH:mm}–{appointment.EndsAt:HH:mm} | {appointment.Status}{notes}");
    }

    public void DisplayList(Appointment[] appointments)
    {
        if (appointments.Length == 0)
        {
            Console.WriteLine("не знайдено");
            return;
        }

        for (int i = 0; i < appointments.Length; i++)
        {
            DisplayAppointment(appointments[i]);
        }
    }
}
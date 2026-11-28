using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class ReportManager
{
    private AppointmentManager _appointments;
    private DoctorManager _doctors;
    private PatientManager _patients;
    private AnalyticsManager _analytics;

    public ReportManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
        _analytics = new AnalyticsManager(appointments, doctors, patients);
    }

    public IEnumerable<SpecialityReport> GetSpecialityStats()
    {
        Appointment[] allAppointments = _appointments.GetAll();

        return _doctors.GetAll()
            .GroupBy(doctor => doctor.Speciality)
            .Select(group =>
            {
                int[] doctorIds = group.Select(doctor => doctor.Id).ToArray();
                Appointment[] specialityAppointments = allAppointments
                    .Where(appointment => doctorIds.Contains(appointment.DoctorId))
                    .ToArray();

                return new SpecialityReport(
                    group.Key,
                    group.Count(),
                    specialityAppointments.Length,
                    specialityAppointments.Sum(appointment => appointment.GetCost()));
            })
            .OrderByDescending(report => report.TotalRevenue)
            .ToArray();
    }

    public string? FindBusiestDoctorName()
    {
        Appointment[] allAppointments = _appointments.GetAll();

        var busiest = _doctors.GetAll()
            .Select(doctor => new
            {
                doctor,
                Count = allAppointments.Count(appointment => appointment.DoctorId == doctor.Id)
            })
            .OrderByDescending(item => item.Count)
            .FirstOrDefault();

        return busiest?.doctor.FullName;
    }

    public IEnumerable<string> GetPatientsWithMultipleVisits(int minVisits)
    {
        return _appointments.GetAll()
            .GroupBy(appointment => appointment.PatientId)
            .Where(group => group.Count() >= minVisits)
            .Join(_patients.GetAll(), group => group.Key, patient => patient.Id, (group, patient) => patient.FullName)
            .ToArray();
    }

    public IEnumerable<DoctorStats> GetTopEarners(int n)
    {
        return _analytics.ComputeDoctorStats()
            .OrderByDescending(stats => stats.TotalRevenue)
            .Take(n)
            .ToArray();
    }

    public bool HasAnyUrgentAppointments()
    {
        return _appointments.GetAll().Any(appointment => appointment is UrgentAppointment);
    }

    public IEnumerable<Speciality> GetActiveSpecialities()
    {
        return _doctors.GetAll()
            .Select(doctor => doctor.Speciality)
            .Distinct()
            .OrderBy(speciality => speciality)
            .ToArray();
    }

    public IEnumerable<(int Year, int Month, decimal Total)> GetMonthlyRevenue()
    {
        return _appointments.GetAll()
            .GroupBy(appointment => new { appointment.ScheduledAt.Year, appointment.ScheduledAt.Month })
            .OrderBy(group => group.Key.Year)
            .ThenBy(group => group.Key.Month)
            .Select(group => (Year: group.Key.Year, Month: group.Key.Month, Total: group.Sum(appointment => appointment.GetCost())))
            .ToArray();
    }
}

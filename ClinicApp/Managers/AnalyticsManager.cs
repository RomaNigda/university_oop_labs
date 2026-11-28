using System.Linq;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager
{
    private AppointmentManager _appointments;
    private DoctorManager _doctors;
    private PatientManager _patients;

    public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Doctor[] doctors = _doctors.GetAll();
        Appointment[] allAppointments = _appointments.GetAll();

        return doctors.Select(doctor =>
        {
            Appointment[] doctorAppointments = allAppointments
                .Where(appointment => appointment.DoctorId == doctor.Id)
                .ToArray();

            DateTime lastAppointmentDate = doctorAppointments.Any()
                ? doctorAppointments.Max(appointment => appointment.ScheduledAt)
                : DateTime.MinValue;

            return new DoctorStats(
                doctor.Id,
                doctor.FullName,
                doctorAppointments.Length,
                doctorAppointments.Sum(appointment => appointment.GetCost()),
                lastAppointmentDate);
        }).ToArray();
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Patient[] patients = _patients.GetAll();
        Appointment[] allAppointments = _appointments.GetAll();

        return patients.Select(patient =>
        {
            Appointment[] patientAppointments = allAppointments
                .Where(appointment => appointment.PatientId == patient.Id)
                .ToArray();

            DateTime lastVisitDate = patientAppointments.Any()
                ? patientAppointments.Max(appointment => appointment.ScheduledAt)
                : DateTime.MinValue;

            return new PatientStats(
                patient.Id,
                patient.FullName,
                patientAppointments.Length,
                patientAppointments.Sum(appointment => appointment.GetCost()),
                lastVisitDate);
        }).ToArray();
    }
}

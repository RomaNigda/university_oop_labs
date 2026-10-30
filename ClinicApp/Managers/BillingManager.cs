using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class BillingManager
{
    private readonly AppointmentManager _appointments;

    public BillingManager(AppointmentManager appointments)
    {
        _appointments = appointments;
    }

    public IPayable[] GetAllUnpaid()
    {
        return FilterUnpaid(_appointments.GetAll());
    }

    public IPayable[] GetUnpaidByPatient(int patientId)
    {
        return FilterUnpaid(_appointments.GetByPatient(patientId));
    }

    private IPayable[] FilterUnpaid(Appointment[] appointments)
    {
        int matches = 0;
        for (int i = 0; i < appointments.Length; i++)
        {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled)
                matches++;
        }

        IPayable[] result = new IPayable[matches];
        int index = 0;
        for (int i = 0; i < appointments.Length; i++)
        {
            if (!appointments[i].IsPaid && !appointments[i].IsCancelled)
            {
                result[index] = appointments[i];
                index++;
            }
        }
        return result;
    }

    public decimal GetTotalDebt()
    {
        IPayable[] unpaid = GetAllUnpaid();
        decimal total = 0m;
        for (int i = 0; i < unpaid.Length; i++)
        {
            total += unpaid[i].GetCost();
        }
        return total;
    }

    public decimal GetPatientDebt(int patientId)
    {
        IPayable[] unpaid = GetUnpaidByPatient(patientId);
        decimal total = 0m;
        for (int i = 0; i < unpaid.Length; i++)
        {
            total += unpaid[i].GetCost();
        }
        return total;
    }

    public bool PayAppointment(int appointmentId)
    {
        Appointment[] all = _appointments.GetAll();
        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].Id == appointmentId)
            {
                if (all[i].IsPaid || all[i].IsCancelled)
                    return false;

                all[i].MarkPaid();
                return true;
            }
        }
        return false;
    }

    public void DisplayUnpaid(IPayable[] items)
    {
        if (items.Length == 0)
        {
            Console.WriteLine("Немає неоплачених записів.");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] is Appointment a)
                Console.WriteLine($"{a} | Сума: {items[i].GetCost():F2} грн");
            else
                Console.WriteLine($"#{i + 1} | Сума: {items[i].GetCost():F2} грн");
        }
    }
}
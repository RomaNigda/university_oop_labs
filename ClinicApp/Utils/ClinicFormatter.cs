using ClinicApp.Enums;

namespace ClinicApp.Utils;

public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bloodType) => bloodType switch
    {
        BloodType.APositive => "A+",
        BloodType.ANegative => "A-",
        BloodType.BPositive => "B+",
        BloodType.BNegative => "B-",
        BloodType.ABPositive => "AB+",
        BloodType.ABNegative => "AB-",
        BloodType.OPositive => "O+",
        BloodType.ONegative => "O-",
        _ => "Невідомо"
    };

    public static string FormatSpeciality(Speciality speciality) => speciality switch
    {
        Speciality.General => "Загальна терапія",
        Speciality.Cardiology => "Кардіологія",
        Speciality.Neurology => "Неврологія",
        Speciality.Pediatrics => "Педіатрія",
        Speciality.Surgery => "Хірургія",
        Speciality.Orthopedics => "Ортопедія",
        Speciality.Dermatology => "Дерматологія",
        Speciality.Emergency => "Швидка допомога",
        _ => "Невідомо"
    };

    public static string FormatAge(int age)
    {
        int lastTwo = age % 100;
        if (lastTwo >= 11 && lastTwo <= 19)
            return age + " років";

        int last = age % 10;
        if (last == 1)
            return age + " рік";
        else if (last >= 2 && last <= 4)
            return age + " роки";
        else
            return age + " років";
    }

    public static string FormatPhone(string phone)
    {
        if (phone.Length != 10)
            return phone;

        for (int i = 0; i < phone.Length; i++)
        {
            if (!char.IsDigit(phone[i]))
                return phone;
        }

        return "(" + phone.Substring(0, 3) + ") " + phone.Substring(3, 3) + "-" + phone.Substring(6, 4);
    }
}
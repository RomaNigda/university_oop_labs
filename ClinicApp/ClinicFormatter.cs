namespace ClinicApp;

public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bloodType)
    {
        switch (bloodType)
        {
            case BloodType.APositive: return "A+";
            case BloodType.ANegative: return "A-";
            case BloodType.BPositive: return "B+";
            case BloodType.BNegative: return "B-";
            case BloodType.ABPositive: return "AB+";
            case BloodType.ABNegative: return "AB-";
            case BloodType.OPositive: return "O+";
            case BloodType.ONegative: return "O-";
            default: return "Невідомо";
        }
    }

    public static string FormatSpeciality(Speciality speciality)
    {
        switch (speciality)
        {
            case Speciality.General: return "Загальна терапія";
            case Speciality.Cardiology: return "Кардіологія";
            case Speciality.Neurology: return "Неврологія";
            case Speciality.Pediatrics: return "Педіатрія";
            case Speciality.Surgery: return "Хірургія";
            case Speciality.Orthopedics: return "Ортопедія";
            case Speciality.Dermatology: return "Дерматологія";
            case Speciality.Emergency: return "Невідкладна допомога";
            default: return "Невідомо";
        }
    }

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
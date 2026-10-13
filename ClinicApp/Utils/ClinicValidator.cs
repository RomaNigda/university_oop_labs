using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    private static readonly Regex PhonePattern = new Regex(@"^(?:\+?38)?([0-9]{10})\z");
    private static readonly Regex EmailPattern = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+\z");

    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Поле не може бути порожнім.", fieldName);
        if (value.Length > 50)
            throw new ArgumentException("Довжина не може перевищувати 50 символів.", fieldName);
    }

    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Телефон не може бути порожнім.", nameof(phone));
        if (!PhonePattern.IsMatch(phone))
            throw new ArgumentException("Телефон має містити 10 цифр (можливо з префіксом +38).", nameof(phone));
    }

    public static string NormalizePhone(string phone)
    {
        if (phone.StartsWith("+38"))
            return phone.Substring(3);
        if (phone.StartsWith("38") && phone.Length == 12)
            return phone.Substring(2);
        return phone;
    }

    public static void ValidateEmail(string email)
    {
        if (email.Length == 0)
            return;
        if (!EmailPattern.IsMatch(email))
            throw new ArgumentException("Некоректний формат email.", nameof(email));
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today)
            throw new ArgumentOutOfRangeException(fieldName, "Дата не може бути в майбутньому.");
        if (value.Year < 1900)
            throw new ArgumentOutOfRangeException(fieldName, "Дата не може бути раніше 1900 року.");
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(fieldName, "Значення має бути більшим за нуль.");
    }
}
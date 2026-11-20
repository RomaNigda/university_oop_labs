using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        ValidationResult result = new ValidationResult();
        PropertyInfo[] properties = obj.GetType().GetProperties();

        foreach (PropertyInfo property in properties)
        {
            object? value = property.GetValue(obj);

            RequiredAttribute? requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttribute != null
                && (value == null || (value is string text && string.IsNullOrWhiteSpace(text))))
            {
                result.AddError(property.Name + ": " + requiredAttribute.ErrorMessage);
            }

            MaxLengthAttribute? maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttribute != null && value is string valueText && valueText.Length > maxLengthAttribute.Length)
            {
                result.AddError(property.Name + ": " + maxLengthAttribute.ErrorMessage);
            }

            MinValueAttribute? minValueAttribute = property.GetCustomAttribute<MinValueAttribute>();
            if (minValueAttribute != null && value != null)
            {
                try
                {
                    if (Convert.ToDouble(value) < minValueAttribute.Min)
                        result.AddError(property.Name + ": " + minValueAttribute.ErrorMessage);
                }
                catch (FormatException)
                {
                }
                catch (InvalidCastException)
                {
                }
                catch (OverflowException)
                {
                }
            }
        }

        return result;
    }

    public static void PrintInfo(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        PropertyInfo[] properties = type.GetProperties();
        int propertyNameWidth = 0;
        foreach (PropertyInfo property in properties)
        {
            if (property.Name.Length > propertyNameWidth)
                propertyNameWidth = property.Name.Length;
        }

        Console.WriteLine(type.Name);
        foreach (PropertyInfo property in properties)
        {
            Console.Write("  " + property.Name.PadRight(propertyNameWidth + 2));
            foreach (Attribute attribute in property.GetCustomAttributes())
                Console.Write(" [" + attribute.GetType().Name.Replace("Attribute", "") + "]");
            Console.WriteLine();
        }
    }
}

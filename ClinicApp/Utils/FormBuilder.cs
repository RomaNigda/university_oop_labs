using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class FormBuilder
{
    public static T Build<T>() where T : new()
    {
        T obj = new T();

        foreach (PropertyInfo property in typeof(T).GetProperties())
        {
            if (property.GetSetMethod() == null)
                continue;

            string prompt = "  " + property.Name;
            List<string> hints = new List<string>();

            RequiredAttribute? requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttribute != null)
                hints.Add("обов'язкове");

            MaxLengthAttribute? maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttribute != null)
                hints.Add("макс. " + maxLengthAttribute.Length);

            MinValueAttribute? minValueAttribute = property.GetCustomAttribute<MinValueAttribute>();
            if (minValueAttribute != null)
                hints.Add("мін. " + minValueAttribute.Min);

            if (hints.Count > 0)
            {
                prompt += " (";
                for (int i = 0; i < hints.Count; i++)
                {
                    if (i > 0)
                        prompt += ", ";
                    prompt += hints[i];
                }
                prompt += ")";
            }

            prompt += ": ";

            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                try
                {
                    object? converted = Convert.ChangeType(input, property.PropertyType);
                    property.SetValue(obj, converted);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
                }
                catch (InvalidCastException)
                {
                    Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
                }
            }
        }

        return obj;
    }
}

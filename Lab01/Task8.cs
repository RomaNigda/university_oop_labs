public static class Task8
{
    public static void Run()
    {
        Console.Write("Enter weight (kg): ");
        double weight = double.Parse(Console.ReadLine());
        Console.Write("Enter height (m): ");
        double height = double.Parse(Console.ReadLine());
        
        Console.Write("Enter price: ");
        double price = double.Parse(Console.ReadLine());
        Console.Write("Enter number of visits: ");
        int visits = int.Parse(Console.ReadLine());
        Console.Write("Enter discount: ");
        int discount = int.Parse(Console.ReadLine());
        
        Console.Write("Enter birth year: ");
        int birthYear = int.Parse(Console.ReadLine());
        
        Console.Write("Enter systolic pressure: ");
        int systolic = int.Parse(Console.ReadLine());
        Console.Write("Enter diastolic pressure: ");
        int diastolic = int.Parse(Console.ReadLine());
        
        double bmi = CalculateBMI(weight, height);
        Console.WriteLine($"ІМТ: {bmi:F2} -> {GetBMICategory(bmi)}");
        
        double total = CalculateCost(price, visits, discount);
        Console.WriteLine($"Сума: {total:F2} грн");
        
        int age = 2026 - birthYear;
        Console.WriteLine($"Вік: {age} р., категорія: {GetAgeCategory(age)}");
        
        Console.WriteLine($"Тиск: {systolic}/{diastolic} — {GetPressureStatus(systolic, diastolic)}");
    }
    
    static double CalculateBMI(double weight, double height)
    {
        return weight / (height * height);
    }
    
    static string GetBMICategory(double bmi)
    {
        if (bmi < 18.5)
            return "недостатня вага";
        else if (bmi < 25)
            return "норма";
        else if (bmi < 30)
            return "надмірна вага";
        else
            return "ожиріння";
    }
    
    static double CalculateCost(double price, int visits, int discount)
    {
        return price * visits * (1 - discount / 100.0);
    }
    
    static string GetAgeCategory(int age)
    {
        if (age >= 0 && age <= 17)
            return "дитина";
        else if (age >= 18 && age <= 59)
            return "дорослий";
        else
            return "пенсіонер";
    }
    
    static string GetPressureStatus(int systolic, int diastolic)
    {
        if (systolic < 120 && diastolic < 80)
            return "норма";
        else if (systolic < 130 && diastolic < 80)
            return "підвищений";
        else if (systolic < 140 || diastolic < 90)
            return "гіпертонія 1 ступеня";
        else
            return "гіпертонія 2 ступеня";
    }
}
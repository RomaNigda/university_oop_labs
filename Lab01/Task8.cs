public class Task8
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
        Console.WriteLine($"BMI: {bmi:F2} -> {GetBMICategory(bmi)}");
        
        double total = CalculateCost(price, visits, discount);
        Console.WriteLine($"Total: {total:F2} грн");
        
        int age = 2026 - birthYear;
        Console.WriteLine($"Age: {age} y.o., category: {GetAgeCategory(age)}");
        
        Console.WriteLine($"Pressure: {systolic}/{diastolic} — {GetPressureStatus(systolic, diastolic)}");
    }
    
    static double CalculateBMI(double weight, double height)
    {
        return weight / (height * height);
    }
    
    static string GetBMICategory(double bmi)
    {
        if (bmi < 18.5)
            return "underweight";
        else if (bmi < 25)
            return "normal";
        else if (bmi < 30)
            return "overweight";
        else
            return "obese";
    }
    
    static double CalculateCost(double price, int visits, int discount)
    {
        return price * visits * (1 - discount / 100.0);
    }
    
    static string GetAgeCategory(int age)
    {
        if (age >= 0 && age <= 17)
            return "child";
        else if (age >= 18 && age <= 59)
            return "adult";
        else
            return "pensioner";
    }
    
    static string GetPressureStatus(int systolic, int diastolic)
    {
        if (systolic < 120 && diastolic < 80)
            return "normal";
        else if (systolic < 130 && diastolic < 80)
            return "elevated";
        else if (systolic < 140 || diastolic < 90)
            return "hypertension stage 1";
        else
            return "hypertension stage 2";
    }
}
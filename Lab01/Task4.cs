public class Task4
{
    public static void Run()
    {
        Console.Write("Enter systolic pressure: ");
        int systolic = int.Parse(Console.ReadLine());
        Console.Write("Enter diastolic pressure: ");
        int diastolic = int.Parse(Console.ReadLine());
        
        string category;
        
        if (systolic < 120 && diastolic < 80)
            category = "normal";
        else if (systolic < 130 && diastolic < 80)
            category = "elevated";
        else if (systolic < 140 || diastolic < 90)
            category = "hypertension stage 1";
        else
            category = "hypertension stage 2";
        
        Console.WriteLine($"Pressure: {systolic}/{diastolic} — {category}");
    }
}
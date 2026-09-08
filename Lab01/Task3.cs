public static class Task3
{
    public static void Run()
    {
        Console.Write("Enter birth year: ");
        int birthYear = int.Parse(Console.ReadLine());
        
        int age = 2026 - birthYear;
        string category;
        
        if (age >= 0 && age <= 17)
            category = "дитина";
        else if (age >= 18 && age <= 59)
            category = "дорослий";
        else
            category = "пенсіонер";
        
        Console.WriteLine($"Вік: {age} р.");
        Console.WriteLine($"Категорія: {category}");
    }
}
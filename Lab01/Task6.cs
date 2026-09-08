public static class Task6
{
    public static void Run()
    {
        Console.Write("Enter card number: ");
        int card = int.Parse(Console.ReadLine());
        
        int lastDigit = card % 10;
        
        string department = lastDigit switch
        {
            0 or 1 => "загальна терапія",
            2 or 3 => "хірургія",
            4 or 5 => "кардіологія",
            6 or 7 => "неврологія",
            8 or 9 => "офтальмологія",
            _ => "невідомо"
        };
        
        string isDiscounted = (card % 2 == 0) ? "так" : "ні";
        string hasCheckup = (card % 3 == 0) ? "так" : "ні";
        
        Console.WriteLine($"Відділення: {department}");
        Console.WriteLine($"Пільгова:   {isDiscounted}");
        Console.WriteLine($"Огляд:      {hasCheckup}");
    }
}
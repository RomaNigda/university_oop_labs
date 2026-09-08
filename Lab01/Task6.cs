public class Task6
{
    public static void Run()
    {
        Console.Write("Enter card number: ");
        int card = int.Parse(Console.ReadLine());
        
        int lastDigit = card % 10;
        
        string department = lastDigit switch
        {
            0 or 1 => "general therapy",
            2 or 3 => "surgery",
            4 or 5 => "cardiology",
            6 or 7 => "neurology",
            8 or 9 => "ophthalmology",
            _ => "unknown"
        };
        
        string isDiscounted = (card % 2 == 0) ? "yes" : "no";
        string hasCheckup = (card % 3 == 0) ? "yes" : "no";
        
        Console.WriteLine($"Department: {department}");
        Console.WriteLine($"Discounted: {isDiscounted}");
        Console.WriteLine($"Checkup:    {hasCheckup}");
    }
}
public class Task2
{
    public static void Run()
    {
        Console.Write("Enter price: ");
        double price = double.Parse(Console.ReadLine());
        Console.Write("Enter number of visits: ");
        int visits = int.Parse(Console.ReadLine());
        Console.Write("Enter discount: ");
        int discount = int.Parse(Console.ReadLine());
    
        double total = price * visits * (1 - discount / 100.0);
        Console.WriteLine($"Sum: {total:F2} hrn");
    }
}
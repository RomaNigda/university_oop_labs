static class Task1
{
    static double weight = 0, height = 0;
   

    public static void Run()
    {
        Console.Write("Enter weight: ");
        weight = double.Parse(Console.ReadLine());
        Console.Write("Enter height: ");
        height = double.Parse(Console.ReadLine());
        
        double imt = weight / (height * height / 10000);
        Console.WriteLine($"Imt: {imt:F2}");
    }
}
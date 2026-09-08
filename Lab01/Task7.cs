public class Task7
{
    public static void Run()
    {
        Console.Write("Enter number of visits: ");
        int n = int.Parse(Console.ReadLine());
        
        decimal[] costs = new decimal[n];
        
        Console.WriteLine("Enter costs: ");
        string[] input = Console.ReadLine().Split(' ');
        
        for (int i = 0; i < n; i++)
        {
            costs[i] = decimal.Parse(input[i]);
        }
        
        decimal total = 0;
        decimal min = decimal.MaxValue;
        decimal max = decimal.MinValue;
        
        foreach (decimal cost in costs)
        {
            total += cost;
            if (cost < min) min = cost;
            if (cost > max) max = cost;
        }
        
        decimal average = total / n;
        
        int aboveAverage = 0;
        for (int i = 0; i < n; i++)
        {
            if (costs[i] > average) aboveAverage++;
        }
        
        int firstExpensiveIndex = -1;
        int index = 0;
        while (index < n)
        {
            if (costs[index] >= 1000)
            {
                firstExpensiveIndex = index;
                break;
            }
            index++;
        }
        
        Console.WriteLine("=== Visit Report ===");
        Console.WriteLine($"Count:          {n}");
        Console.WriteLine($"Total:          {total:F2} грн");
        Console.WriteLine($"Average:        {average:F2} грн");
        Console.WriteLine($"Min / Max:      {min:F2} / {max:F2} грн");
        Console.WriteLine($"Above average:  {aboveAverage} of {n}");
        
        if (firstExpensiveIndex != -1)
        {
            Console.WriteLine($"First > 1000:   #{firstExpensiveIndex + 1} — {costs[firstExpensiveIndex]:F2} грн");
        }
        else
        {
            Console.WriteLine($"First > 1000:   none");
        }
        Console.WriteLine("========================");
    }
}
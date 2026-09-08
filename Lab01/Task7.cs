public static class Task7
{
    public static void Run()
    {
        Console.Write("Enter number of visits: ");
        int n = int.Parse(Console.ReadLine());
        
        decimal[] costs = new decimal[n];
        
        Console.Write("Enter costs: ");
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
            if (costs[index] > 1000)
            {
                firstExpensiveIndex = index;
                break;
            }
            index++;
        }
        
        Console.WriteLine("=== Звіт по прийомах ===");
        Console.WriteLine($"Кількість:        {n}");
        Console.WriteLine($"Загальна сума:    {total:F2} грн");
        Console.WriteLine($"Середня:          {average:F2} грн");
        Console.WriteLine($"Мін / Макс:       {min:F2} / {max:F2} грн");
        Console.WriteLine($"Вище середнього:  {aboveAverage} з {n}");
        
        if (firstExpensiveIndex != -1)
        {
            Console.WriteLine($"Перший > 1000:    #{firstExpensiveIndex + 1} — {costs[firstExpensiveIndex]:F2} грн");
        }
        else
        {
            Console.WriteLine($"Перший > 1000:    немає");
        }
        Console.WriteLine("========================");
    }
}
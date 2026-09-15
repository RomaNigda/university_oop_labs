public static class Task6
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine());
        
        int[][] costs = new int[n][];
        
        for (int i = 0; i < n; i++)
        {
            int k = int.Parse(Console.ReadLine());
            costs[i] = new int[k];
            
            for (int j = 0; j < k; j++)
            {
                costs[i][j] = int.Parse(Console.ReadLine());
            }
        }
        
        int maxSum = 0;
        int maxIdx = 0;
        
        for (int i = 0; i < n; i++)
        {
            int sum = 0;
            for (int j = 0; j < costs[i].Length; j++)
            {
                sum += costs[i][j];
            }
            
            double average = (double)sum / costs[i].Length;
            
            Console.WriteLine($"Лікар {i + 1}: {costs[i].Length} прийоми, сума={sum} грн, середня={average:F2} грн");
            
            if (sum > maxSum)
            {
                maxSum = sum;
                maxIdx = i;
            }
        }
        
        Console.WriteLine($"Найбільший дохід: Лікар {maxIdx + 1} ({maxSum} грн)");
    }
}
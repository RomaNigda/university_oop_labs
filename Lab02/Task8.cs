public static class Task8
{
    public static void Run()
    {
        int d = int.Parse(Console.ReadLine());
        int w = int.Parse(Console.ReadLine());
        
        int[,,] patients = new int[d, w, 2];
        
        for (int i = 0; i < d; i++)
        {
            for (int j = 0; j < w; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    patients[i, j, k] = int.Parse(Console.ReadLine());
                }
            }
        }
        
        int[] totals = new int[d];
        
        for (int i = 0; i < d; i++)
        {
            Console.WriteLine($"Відділення {i + 1}:");
            int departmentTotal = 0;
            
            for (int j = 0; j < w; j++)
            {
                int morning = patients[i, j, 0];
                int evening = patients[i, j, 1];
                int weekTotal = morning + evening;
                
                Console.WriteLine($"  Тиждень {j + 1}: ранок {morning}, вечір {evening} → разом {weekTotal}");
                departmentTotal += weekTotal;
            }
            
            totals[i] = departmentTotal;
            Console.WriteLine($"  Разом: {departmentTotal} пацієнтів");
        }
        
        int maxIdx = 0;
        for (int i = 1; i < d; i++)
        {
            if (totals[i] > totals[maxIdx]) maxIdx = i;
        }
        
        Console.WriteLine($"Найзавантаженіше: Відділення {maxIdx + 1} ({totals[maxIdx]} пацієнтів)");
    }
}
public static class Task2
{
    public static void Run()
    {
        int n = int.Parse(Console.ReadLine());
        
        int[] queue = new int[n];
        
        for (int i = 0; i < n; i++)
        {
            queue[i] = int.Parse(Console.ReadLine());
        }
        
        string before = string.Join(" ", queue);
        
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - 1 - i; j++)
            {
                if (queue[j] > queue[j + 1])
                {
                    (queue[j], queue[j + 1]) = (queue[j + 1], queue[j]);
                }
            }
        }
        
        string after = string.Join(" ", queue);
        
        Console.WriteLine($"Черга (до):    {before}");
        Console.WriteLine($"Черга (після): {after}");
        Console.WriteLine($"Найдешевший:   {queue[0]} грн");
        Console.WriteLine($"Найдорожчий:   {queue[n - 1]} грн");
    }
}
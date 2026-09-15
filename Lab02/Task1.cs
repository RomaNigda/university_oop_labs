using System;
using System.Globalization;

public static class Task1
{
    public static void Run()
    {
        Console.Write("Enter number of patients: ");
        int n = int.Parse(Console.ReadLine());
        
        double[] weights = new double[n];
        
        Console.Write($"Enter weights: ");
        for (int i = 0; i < n; i++)
        {
            weights[i] = double.Parse(Console.ReadLine());
        }
        
        double sum = 0;
        double min = weights[0];
        double max = weights[0];
        
        foreach (double weight in weights)
        {
            sum += weight;
            if (weight < min) min = weight;
            if (weight > max) max = weight;
        }
        
        double average = sum / n;
        
        int aboveAverage = 0;
        for (int i = 0; i < n; i++)
        {
            if (weights[i] > average) aboveAverage++;
        }
        
        Console.WriteLine($"Кількість: {n}");
        Console.WriteLine($"Середня вага: {average:F1} кг");
        Console.WriteLine($"Мін / Макс: {min:F1} / {max:F1} кг");
        Console.WriteLine($"Вище середнього: {aboveAverage} з {n}");
    }
}
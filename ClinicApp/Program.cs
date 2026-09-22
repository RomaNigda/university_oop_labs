using System;
using ClinicApp;

Appointment a1 = new Appointment(1, 1, new DateTime(2027, 5, 9, 10, 0, 0));
Appointment a2 = new Appointment(2, 2, new DateTime(2027, 5, 9, 11, 0, 0), 45);
Appointment a3 = new Appointment(3, 3, new DateTime(2027, 5, 10, 9, 0, 0), 20);

Console.WriteLine(a1);
Console.WriteLine(a2);
Console.WriteLine(a3);

Console.WriteLine();

bool firstCancel = a1.Cancel("Пацієнт не зміг прийти");
Console.WriteLine($"a1.Cancel(...) перший раз → {firstCancel}");

bool secondCancel = a1.Cancel("Ще одна спроба");
Console.WriteLine($"a1.Cancel(...) повторно → {secondCancel}");

bool completeCancelled = a1.Complete();
Console.WriteLine($"a1.Complete() для скасованого → {completeCancelled}");

Console.WriteLine();

bool firstComplete = a2.Complete();
Console.WriteLine($"a2.Complete() перший раз → {firstComplete}");

bool secondComplete = a2.Complete();
Console.WriteLine($"a2.Complete() повторно → {secondComplete}");

bool cancelCompleted = a2.Cancel("Запізно");
Console.WriteLine($"a2.Cancel(...) для завершеного → {cancelCompleted}");

Console.WriteLine();
Console.WriteLine(a1);
Console.WriteLine(a2);
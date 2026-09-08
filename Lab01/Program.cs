// Дробові числа вводимо з КРАПКОЮ незалежно від регіональних налаштувань Windows.
System.Threading.Thread.CurrentThread.CurrentCulture =
    System.Globalization.CultureInfo.InvariantCulture;

Task4.Run();
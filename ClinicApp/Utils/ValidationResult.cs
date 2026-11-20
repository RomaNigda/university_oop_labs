namespace ClinicApp.Utils;

public class ValidationResult
{
    private readonly List<string> _errors = new List<string>();
    private readonly IReadOnlyList<string> _readOnlyErrors;

    public bool IsValid
    {
        get { return _errors.Count == 0; }
    }

    public IReadOnlyList<string> Errors
    {
        get { return _readOnlyErrors; }
    }

    public ValidationResult()
    {
        _readOnlyErrors = _errors.AsReadOnly();
    }

    public void AddError(string error)
    {
        _errors.Add(error);
    }

    public void Print()
    {
        for (int i = 0; i < _errors.Count; i++)
            Console.WriteLine("  [!] " + _errors[i]);
    }
}

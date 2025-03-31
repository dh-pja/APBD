using LegacyApp.Interfaces;

namespace LegacyApp.Utils;

public class EmailValidator : IValidator<string>
{
    public static bool Validate(string input)
    {
        return input.Contains("@") && input.Contains(".");
    }
}
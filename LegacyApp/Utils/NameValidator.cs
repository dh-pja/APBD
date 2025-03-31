using LegacyApp.Interfaces;

namespace LegacyApp.Utils;

public class NameValidator : IValidator<string>
{
    public static bool Validate(string input)
    {
        return !string.IsNullOrEmpty(input);
    }
}
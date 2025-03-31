using System;
using LegacyApp.Interfaces;

namespace LegacyApp.Utils;

public class AgeValidator : IValidator<DateTime>
{
    public static bool Validate(DateTime input)
    {
        var now = DateTime.Now;
        int age = now.Year - input.Year;
        if (now.Month < input.Month || (now.Month == input.Month && now.Day < input.Day)) age--;

        return !(age < 21);
    }
}
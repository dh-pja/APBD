namespace LegacyApp.Interfaces;

public interface IValidator<T>
{
    public static abstract bool Validate(T input);
}
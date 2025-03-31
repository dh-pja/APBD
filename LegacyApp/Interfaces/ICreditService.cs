using System;

namespace LegacyApp.Interfaces;

public interface ICreditService : IDisposable
{
    public void Dispose();
    internal int GetCreditLimit(string lastName, DateTime dateOfBirth);
}
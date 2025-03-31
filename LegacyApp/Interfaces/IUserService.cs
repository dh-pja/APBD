using System;

namespace LegacyApp.Interfaces;

public interface IUserService
{
    public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId);
}
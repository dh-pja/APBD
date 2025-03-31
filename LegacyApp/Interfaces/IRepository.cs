namespace LegacyApp.Interfaces;

public interface IRepository
{
    Client GetById(int id);
}
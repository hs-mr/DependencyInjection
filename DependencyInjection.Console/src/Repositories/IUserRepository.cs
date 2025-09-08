namespace DependencyInjection.Console.Repositories;

using DependencyInjection.Console.Models;

public interface IUserRepository
{
    public User? Get(int id);
    public void Add(User user);
}

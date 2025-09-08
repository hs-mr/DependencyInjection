namespace DependencyInjection.Console.Services;

using DependencyInjection.Console.Models;
using DependencyInjection.Console.Repositories;

public class UserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public User? GetUser(int id) => _repo.Get(id);

    public void AddUser(User user) => _repo.Add(user);
}

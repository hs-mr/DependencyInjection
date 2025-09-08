namespace DependencyInjection.Console.Repositories;

using DependencyInjection.Console.Models;

public class UserRepository : IUserRepository
{
    private readonly Dictionary<int, User> _users = new();

    public User? Get(int id)
    {
        return _users.TryGetValue(id, out var user) ? user : null;
    }

    public void Add(User user)
    {
        _users[user.Id] = user;
    }
}

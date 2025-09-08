namespace DependencyInjection.Console.Services;

using DependencyInjection.Console.Models;

// Klassischer Service ohne DI - "fest verdrahtet"
public class CarService
{
    private readonly Dictionary<int, Car> _cars = new();
    
    public Car? GetCarByUserId(int userId)
    {
        return _cars.Values.FirstOrDefault(car => car.UserId == userId);
    }

    public void AddCar(Car car)
    {
        _cars[car.Id] = car;
    }
}

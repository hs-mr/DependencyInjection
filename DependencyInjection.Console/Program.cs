using Microsoft.Extensions.DependencyInjection;
using DependencyInjection.Console.Models;
using DependencyInjection.Console.Repositories;
using DependencyInjection.Console.Services;

// DI Container einrichten
var services = new ServiceCollection();

// Services registrieren
services.AddSingleton<IUserRepository, UserRepository>();
services.AddScoped<UserService>();


var serviceProvider = services.BuildServiceProvider();

var userService = serviceProvider.GetRequiredService<UserService>();

// Beispiel-Nutzung mit User (mit DI)
var user = new User { Id = 1, Name = "Max Mustermann" };
userService.AddUser(user);

var retrievedUser = userService.GetUser(1);
Console.WriteLine($"Gefundener Benutzer: {retrievedUser?.Name ?? "nicht gefunden"}");

// Beispiel-Nutzung mit Auto (ohne DI)
var carService = new CarService(); 
var car = new Car { Id = 1, Model = "BMW", UserId = user.Id };
carService.AddCar(car);

var usersCar = carService.GetCarByUserId(1);
Console.WriteLine($"Auto des Benutzers: {usersCar?.Model ?? "kein Auto gefunden"}");

// Warten auf Tastendruck
Console.WriteLine("\nDrücken Sie eine Taste zum Beenden...");
Console.ReadKey();
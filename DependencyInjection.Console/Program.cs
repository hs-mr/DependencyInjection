using Microsoft.Extensions.DependencyInjection;
using DependencyInjection.Console.Models;
using DependencyInjection.Console.Repositories;
using DependencyInjection.Console.Services;

// DI Container einrichten
var services = new ServiceCollection();

// Services registrieren
services.AddSingleton<IUserRepository, UserRepository>();
services.AddScoped<UserService>();

// Container bauen
var serviceProvider = services.BuildServiceProvider();

// UserService aus dem Container holen
var userService = serviceProvider.GetRequiredService<UserService>();

// Beispiel-Nutzung
var user = new User { Id = 1, Name = "Max Mustermann" };
userService.AddUser(user);

var retrievedUser = userService.GetUser(1);
Console.WriteLine($"Gefundener Benutzer: {retrievedUser?.Name ?? "nicht gefunden"}");

// Warten auf Tastendruck
Console.WriteLine("\nDrücken Sie eine Taste zum Beenden...");
Console.ReadKey();
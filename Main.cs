using System;

public class Engine
{
    public void Start()
    {
        Console.WriteLine("Engine started!");
    }
}

public class Car
{
    private Engine _engine;

    public Car()
    {
        _engine = new Engine(); // Abhängigkeit wird intern erzeugt
    }

    public void Start()
    {
        _engine.Start();
        Console.WriteLine("Car is running.");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Car car = new Car();
        car.Start();
    }
}

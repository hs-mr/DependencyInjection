# DependencyInjection
Designpattern Dependency Injection<br>
refactoring guru -> Seite für Pattern<br>

Leitfragen:<br>
Framework nutzung?<br>

Pitch:<br>
Dependency Injection in c#<br>
Umsetzung:<br>
Repo öffentlich machen -> Übungsaufgabe stellen<br>

test



## **Was ist Dependency Injection?**

- Statt dass eine Klasse **selbst** ihre Abhängigkeiten erstellt (`new`), bekommt sie diese **von außen übergeben**.
- Vorteil: Weniger Kopplung, besser testbar, flexibler austauschbar.

Beispiel ohne DI:

```csharp
public class UserService
{
    private readonly UserRepository _repo = new UserRepository(); // selbst erzeugt

    public User GetUser(int id) => _repo.Get(id);
}
```

➡ Hier ist `UserService` fest mit `UserRepository` verdrahtet.

Mit DI:

```csharp
public class UserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo; // von außen übergeben
    }

    public User GetUser(int id) => _repo.Get(id);
}
```

➡ Flexibler, weil man z. B. im Test ein Fake-Repo einschleusen kann.


## 🔹 1. Constructor Injection (am häufigsten & empfohlen)

👉 Die Abhängigkeiten werden über den **Konstruktor** übergeben.

**Beispiel:**

```csharp
public class UserService
{
    private readonly IUserRepository _repo;

    // Injection über den Konstruktor
    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public User GetUser(int id) => _repo.Get(id);
}
```

✅ Vorteile:

- Abhängigkeiten sind **zwingend erforderlich** (keine Null-Referenzen).
- Gut testbar.
- Am besten von Frameworks wie ASP.NET Core unterstützt.

❌ Nachteil:

- Wenn zu viele Abhängigkeiten nötig sind → „Constructor Hell“ (zu viele Parameter).

---

## 🔹 2. Property Injection

👉 Die Abhängigkeit wird über eine **öffentliche Property** gesetzt, nicht im Konstruktor.

**Beispiel:**

```csharp
public class UserService
{
    public IUserRepository Repo { get; set; } // von außen setzbar

    public User GetUser(int id) => Repo.Get(id);
}
```

✅ Vorteil:

- Optional, falls die Abhängigkeit nicht immer gebraucht wird.
- Weniger Konstruktor-Parameter.

❌ Nachteile:

- Gefahr von **NullReferenceExceptions**, wenn die Property nicht gesetzt wurde.
- Abhängigkeit ist nicht mehr „immutable“.

---

## 🔹 3. Method Injection

👉 Die Abhängigkeit wird **direkt in die Methode** übergeben, die sie braucht.

**Beispiel:**

```csharp
public class UserService
{
    public User GetUser(int id, IUserRepository repo)
    {
        return repo.Get(id);
    }
}
```

✅ Vorteil:

- Sehr gezielt: Die Abhängigkeit ist nur dort verfügbar, wo sie gebraucht wird.
- Praktisch, wenn nur selten benötigt.

❌ Nachteile:

- Methoden-Signatur kann „aufgebläht“ wirken.
- Weniger elegant, wenn viele Methoden dieselbe Abhängigkeit brauchen.

---

## 🔹 4. Interface Injection (seltener, eher theoretisch)

👉 Ein Service verlangt per Interface, dass ihm eine Abhängigkeit **eingespritzt** werden kann.

**Beispiel:**

```csharp
public interface IInjectable
{
    void Inject(IUserRepository repo);
}

public class UserService : IInjectable
{
    private IUserRepository _repo;

    public void Inject(IUserRepository repo)
    {
        _repo = repo;
    }
}
```

✅ Vorteil:

- Klare „Verpflichtung“ durch das Interface.

❌ Nachteile:

- Komplexer, weniger gebräuchlich.
- In modernen .NET-Projekten fast nie genutzt.

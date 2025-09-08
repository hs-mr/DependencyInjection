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

## Was ist ein Framework?

**Werkzeugkasten** aus fertigen Bibliotheken, Funktionen und Regeln

<aside>
🚧

Beispiel **Entity Framework**:

EF ist ein ORM-Framework (Object-Relational Mapping)

- Es verbindet meine C#-Objekte mit einer Datenbank, ohne, dass ich selber SQL schreiben muss

Ohne EF:

Tabelle `Users` soll in eine Datenbank eingefügt werden

```sql
SELECT * FROM Users WHERE Id = 5;
```

Mit EF:

```csharp
var user = db.Users.First(u ⇒ [u.Id](http://u.Id) == 5);
```

EF weiß, dass die Klasse User zu der Tabelle `Users` gehört

</aside>


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

## Wie hängt das mit Entity Framework zusammen?

EF arbeitet meistens mit einem **DbContext**, z. B.:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
}
```

Früher hat man oft so etwas gemacht:

```csharp
var context = new AppDbContext();
var users = context.Users.ToList();
```

➡ Problem: Der Code erzeugt selbst den `AppDbContext` → schwer zu testen, fest verdrahtet.

## Dependency Injection mit EF Core

In **ASP.NET Core** wird DI automatisch unterstützt.

Man registriert den `DbContext` einmal im **DI-Container** (meist in `Program.cs` oder `Startup.cs`):

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

Was passiert hier?

- Man sagst dem DI-Container: „Wenn jemand `AppDbContext` braucht, erstelle ihn bitte so, mit dieser ConnectionString-Einstellung.“
- Jedes Mal, wenn eine Klasse `AppDbContext` im Konstruktor anfordert, liefert DI automatisch eine passende Instanz.

## Beispiel im Controller

```csharp
public class UserController : Controller
{
    private readonly AppDbContext _context;

    // Der DbContext wird automatisch injiziert
    public UserController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var users = _context.Users.ToList();
        return View(users);
    }
}
```

➡ Man musst **nicht** `new AppDbContext()` aufrufen.

➡ EF + DI kümmern sich darum, dass der Controller den richtigen `DbContext` bekommt.

## Vorteile der DI mit EF

- **Testbarkeit:** Man kann einen Fake/InMemory-DbContext verwenden.
- **Lebensdauersteuerung:** DI regelt, wann ein `DbContext` erstellt und wieder freigegeben wird (typischerweise pro HTTP-Request = *Scoped*).
- **Sauberer Code:** Deine Klassen hängen nicht davon ab, wie der `DbContext` erstellt wird.

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

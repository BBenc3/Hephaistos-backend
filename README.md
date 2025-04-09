# Hephaistos-backend

## Áttekintés
A Hephaistos-backend egy ASP.NET Core-al épített web API, amely számos backend szolgáltatás kezelésére szolgál, beleértve az autentikációt, az email értesítéseket, a fájlátvitelek lebonyolítását és az órarend generálását.

## Főbb jellemzők
- **JWT Autentikáció**: Biztonságos felhasználói azonosítás JSON Web Tokenek (JWT) segítségével.
- **Email Értesítések**: Email-ek küldése az `EmailService` használatával.
- **Egyszer használatos jelszavak (OTP-k)**: OTP-k generálása és validálása az `OtpService` segítségével.
- **Fájlátvitelek**: Fájl feltöltések és letöltések kezelése FTP-n keresztül az `FtpService` használatával.
- **Órarend Generálás**: Órarendek generálása a `TimetableGenerator` segítségével.

## Models Könyvtár
A `Models` könyvtár tartalmazza az alkalmazás adatmodelljeit, amelyek az adatbázis tábláit reprezentálják. Ezek a modellek az Entity Framework segítségével kerülnek leképezésre az adatbázisra.

### Példák a modellekre:

#### University (Egyetem) modell
```csharp
public class University
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Place { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Active { get; set; } = true;
    public string? Note { get; set; }
    // Kapcsolat a Major (szak) entitással - Egy egyetemhez több szak tartozhat
    public virtual ICollection<Major> Majors { get; set; } = new List<Major>();
}
```

#### Major (Szak) modell
```csharp
public class Major
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? UniversityId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Active { get; set; } = true;
    public string? Note { get; set; }
    
    // Kapcsolatok
    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public virtual University? University { get; set; }
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
```

#### User (Felhasználó) modell
```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string? PasswordHash { get; set; }
    public string Role { get; set; } = "User";
    public string Email { get; set; }
    public int StartYear { get; set; } = DateTime.Now.Year;
    public DateTime CreatedAt { get; set; }
    public bool Active { get; set; } = true;
    public string? Note { get; set; }
    public int? MajorId { get; set; }
    public string ProfilePicturepath { get; set; } = "default.png";
    public string? Status { get; set; }
    
    // Kapcsolatok
    public virtual Major? Major { get; set; }
    public virtual ICollection<Auditlog> Auditlogs { get; set; } = new List<Auditlog>();
    public virtual ICollection<Completedsubject> Completedsubjects { get; set; } = new List<Completedsubject>();
    public virtual ICollection<Refreshtoken> Refreshtokens { get; set; } = new List<Refreshtoken>();
    public virtual ICollection<GeneratedTimetable> GeneratedTimetables { get; set; } = new List<GeneratedTimetable>();
}
```

### Kapcsolatok:
- **Egy-a-többhöz (One-to-Many)**: 
  - Egy egyetemhez (`University`) több szak (`Major`) tartozik.
  - Egy szakhoz (`Major`) több felhasználó (`User`) tartozik.
  - Egy felhasználóhoz (`User`) több órarend (`GeneratedTimetable`) tartozhat.
- **Több-a-többhöz (Many-to-Many)**: 
  - Egy felhasználó több tárgyat teljesíthet (`Completedsubject`).
  - Egy szak több tantárgyat tartalmazhat (`Subject`).

## Entity Framework és ASP.NET Core Kapcsolata
Az Entity Framework (EF) egy ORM (Object-Relational Mapper), amely lehetővé teszi az adatbázis műveletek végrehajtását .NET objektumok segítségével.

### Főbb elemek:
1. **DbContext**:
   - A `HephaistosContext` osztály az EF és az adatbázis közötti kapcsolatot biztosítja.
   - `DbSet` tulajdonságokat tartalmaz, például `DbSet<User> Users`, amelyek az adatbázis táblákat reprezentálják.

2. **Konfiguráció**:
   - A `Program.cs` fájlban az `AddDbContext` metódus regisztrálja a `HephaistosContext`-et a dependency injection konténerben.
   - Az adatbázis csatlakozási karakterlánc az `appsettings.json` fájlban található.

3. **Migrációk**:
   - Az EF migrációk segítségével az adatbázis séma szinkronban tartható a modellekkel.
   - Parancsok:
     ```bash
     dotnet ef migrations add <MigrationName>
     dotnet ef database update
     ```

4. **ASP.NET Core Kontrollerek**:
   - A kontrollerek a `HephaistosContext`-et használják az adatbázis műveletekhez, például CRUD (Create, Read, Update, Delete) műveletekhez.

## Controllers Könyvtár
A `Controllers` könyvtár tartalmazza az alkalmazás API végpontjainak kezelőit. Az ASP.NET Core Web API keretrendszerben a kontrollerek felelősek a HTTP kérések feldolgozásáért és a válaszok visszaküldéséért.

### Főbb kontrollerek:

#### AuthController
- **Felelősség**: Felhasználói hitelesítés és jogosultságkezelés
- **Végpontok**:
  - `POST /api/auth/login`: Felhasználói bejelentkezés JWT token generálással
  - `POST /api/auth/register`: Új felhasználó regisztrálása
  - `POST /api/auth/refresh-token`: JWT token frissítése
  - `POST /api/auth/forgot-password`: Jelszó-visszaállítási folyamat indítása
  - `POST /api/auth/reset-password`: Új jelszó beállítása

#### UserController
- **Felelősség**: Felhasználói adatok kezelése
- **Végpontok**:
  - `GET /api/users`: Felhasználók listázása (admin jogosultsággal)
  - `GET /api/users/{id}`: Felhasználói adatok lekérése
  - `PUT /api/users/{id}`: Felhasználói adatok frissítése
  - `DELETE /api/users/{id}`: Felhasználó törlése
  - `GET /api/users/profile`: Bejelentkezett felhasználó profiljának lekérése
  - `PUT /api/users/profile`: Bejelentkezett felhasználó profiljának módosítása

#### UniversityController
- **Felelősség**: Egyetemek és kapcsolódó adatok kezelése
- **Végpontok**:
  - `GET /api/universities`: Egyetemek listázása
  - `GET /api/universities/{id}`: Egyetem részletes adatainak lekérése
  - `POST /api/universities`: Új egyetem létrehozása (admin jogosultsággal)
  - `PUT /api/universities/{id}`: Egyetem adatainak módosítása (admin jogosultsággal)
  - `DELETE /api/universities/{id}`: Egyetem törlése (admin jogosultsággal)

#### MajorController
- **Felelősség**: Szakok kezelése
- **Végpontok**:
  - `GET /api/majors`: Szakok listázása
  - `GET /api/majors/{id}`: Szak részletes adatainak lekérése
  - `GET /api/universities/{universityId}/majors`: Egyetemhez tartozó szakok listázása
  - `POST /api/majors`: Új szak létrehozása (admin jogosultsággal)
  - `PUT /api/majors/{id}`: Szak adatainak módosítása (admin jogosultsággal)
  - `DELETE /api/majors/{id}`: Szak törlése (admin jogosultsággal)

#### TimetableController
- **Felelősség**: Órarendek generálása és kezelése
- **Végpontok**:
  - `POST /api/timetables/generate`: Órarend generálása
  - `GET /api/timetables`: Felhasználóhoz tartozó órarendek listázása
  - `GET /api/timetables/{id}`: Órarend részletes adatainak lekérése
  - `DELETE /api/timetables/{id}`: Órarend törlése

#### FileController
- **Felelősség**: Fájlkezelés FTP szolgáltatáson keresztül
- **Végpontok**:
  - `POST /api/files/upload`: Fájl feltöltése
  - `GET /api/files/{id}/download`: Fájl letöltése
  - `GET /api/files`: Fájlok listázása
  - `DELETE /api/files/{id}`: Fájl törlése

### Controller Implementáció Példa

```csharp
[Route("api/[controller]")]
[ApiController]
public class UniversityController : ControllerBase
{
    private readonly HephaistosContext _context;
    private readonly ILogger<UniversityController> _logger;

    public UniversityController(HephaistosContext context, ILogger<UniversityController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/Universities
    [HttpGet]
    public async Task<ActionResult<IEnumerable<University>>> GetUniversities()
    {
        _logger.LogInformation("Getting all universities");
        return await _context.Universities.Where(u => u.Active).ToListAsync();
    }

    // GET: api/Universities/5
    [HttpGet("{id}")]
    public async Task<ActionResult<University>> GetUniversity(int id)
    {
        _logger.LogInformation("Getting university with ID: {ID}", id);
        
        var university = await _context.Universities.FindAsync(id);

        if (university == null || !university.Active)
        {
            _logger.LogWarning("University with ID {ID} not found", id);
            return NotFound();
        }

        return university;
    }

    // POST, PUT, DELETE methods would follow...
}
```

## ASP.NET Core és REST API

Az ASP.NET Core kiváló támogatást nyújt a REST API fejlesztéséhez a következő jellemzőkkel:

### 1. Routing

Az ASP.NET Core routing rendszere lehetővé teszi az URL-ek és HTTP metódusok egyszerű összekapcsolását a controller action-ökkel:

- **Attribute Routing**: A `[Route]`, `[HttpGet]`, `[HttpPost]` és hasonló attribútumok segítségével egyszerűen definiálhatjuk az API végpontokat.
- **Útvonal paraméterek**: Az útvonal paraméterek (pl. `{id}`) automatikusan kötődnek a metódus paraméterekhez.

### 2. Model Binding

Az ASP.NET Core automatikusan köti a bejövő HTTP kérés adatait a controller action paramétereibe:

- **FromBody**: A kérés törzsét .NET objektummá alakítja.
- **FromQuery**: A query paramétereket köti a metódus paramétereibe.
- **FromRoute**: Az útvonal paramétereket köti a metódus paramétereibe.

### 3. Content Negotiation

Az ASP.NET Core támogatja a tartalom egyeztetést, amely lehetővé teszi, hogy ugyanaz az API végpont különböző formátumokban (JSON, XML, stb.) szolgáljon ki adatokat a kérés `Accept` fejléce alapján.

### 4. Status Kódok és IActionResult

A kontrollerek különböző HTTP státuszkódokat és válaszokat tudnak visszaadni az `IActionResult` interfész implementációin keresztül:

- **Ok()**: 200 OK válasz
- **BadRequest()**: 400 Bad Request válasz
- **NotFound()**: 404 Not Found válasz
- **Created()**: 201 Created válasz új erőforrás létrehozásakor
- **NoContent()**: 204 No Content válasz sikeres művelet után, tartalom nélkül

### 5. Dependency Injection

Az ASP.NET Core beépített függőség befecskendezés támogatással rendelkezik, amely egyszerűsíti a kontrollerek és szolgáltatások közötti kapcsolatot:

```csharp
// A konstruktor automatikusan megkapja a regisztrált szolgáltatásokat
public UserController(HephaistosContext context, EmailService emailService)
{
    _context = context;
    _emailService = emailService;
}
```

### 6. API Documentation

A Swagger/OpenAPI integráció lehetővé teszi az API dokumentáció automatikus generálását:

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title: "Project Hephaistos API",
        Version: "v1"
    });
});
```

Az `AuthorizeCheckOperationFilter` osztály a Swagger dokumentációban vizsgálja az egyes végpontokat, és automatikusan hozzáadja a JWT token követelményeket azokhoz a végpontokhoz, amelyek rendelkeznek `[Authorize]` attribútummal. Ez biztosítja, hogy a Swagger felületen egyértelműen látható legyen, mely végpontok igényelnek autentikációt.

### 7. CORS Támogatás

Az ASP.NET Core egyszerű megoldást kínál a Cross-Origin Resource Sharing (CORS) konfigurálására:

```csharp
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(builder =>
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    )
);
```

### 8. Authentication és Authorization

Az ASP.NET Core támogatja a különféle hitelesítési és jogosultságkezelési mechanizmusokat:

- **JWT Authentication**: JSON Web Tokenek használata a felhasználói hitelesítésre.
- **Policy-based Authorization**: Részletes jogosultságkezelés policies segítségével.

```csharp
[Authorize(Roles = "Admin")]
[HttpPost]
public async Task<IActionResult> CreateUniversity(UniversityDto universityDto)
{
    // Only admins can create universities
}
```

## Első Lépések
1. Klónozd a repót:
   ```bash
   git clone https://github.com/bbenc3/Hephaistos-backend.git
   ```
2. Navigálj a projekt könyvtárába:
   ```bash
   cd Hephaistos-backend
   ```
3. Állítsd be az alkalmazást:
   - Frissítsd az `appsettings.json` fájlt a saját adatbázis csatlakozási karakterláncoddal, JWT beállításokkal, email beállításokkal és FTP konfigurációval.

4. Futtasd az alkalmazást:
   ```bash
   dotnet run
   ```

## API Dokumentáció
Az API dokumentáció elérhető Swagger segítségével. Amint az alkalmazás fut, navigálj a `http://localhost:5000` címre (vagy a konfigurált URL-re), hogy elérd a Swagger UI felületet.

Az API és a Swagger felület online is elérhető a következő címen:
[hephaistos-backend-c6c5ewhraedvgzex.germanywestcentral-01.azurewebsites.net](https://hephaistos-backend-c6c5ewhraedvgzex.germanywestcentral-01.azurewebsites.net)

## Függőségek
- .NET 6.0 vagy újabb
- SQL Server
- SMTP szerver az email funkciókhoz
- FTP szerver a fájlátviteli műveletekhez

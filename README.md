# 🧾 Todo List API - Backend (.NET 9 + SQL Server + JWT)

API RESTful para la **gestión de tareas personales**, con autenticación JWT y conexión a SQL Server.  
Desarrollada en **.NET 9**, incluye autenticación, CRUD de tareas, y pruebas automatizadas.

---

## ⚙️ Tecnologías utilizadas

| Componente | Tecnología | Versión |
|-------------|-------------|---------|
| **Lenguaje** | C# | .NET 9 |
| **Base de datos** | SQL Server | 2022 |
| **ORM** | Entity Framework Core | 9.0 |
| **Autenticación** | JWT (Json Web Token) | - |
| **Testing** | xUnit + Moq | - |
| **Contenedores** | Docker Compose | - |
| **Documentación** | Swagger/OpenAPI | - |

---

## 🧱 Estructura del proyecto

```
todo-list-back/
│
├── Controllers/             
│   ├── AuthController.cs    
│   └── TasksController.cs   
│
├── Data/                     
│   └── AppDbContext.cs     
│
├── Models/                  
│   ├── User.cs             
│   ├── TodoTask.cs          
│   ├── RegisterDto.cs       
│   ├── LoginDto.cs          
│   └── TaskDto.cs           
│
├── Services/                 
│   ├── IAuthService.cs      
│   ├── AuthService.cs       
│   ├── ITaskService.cs      
│   └── TaskService.cs       
│
├── Tests/                    
│   ├── Controllers/
│   │   ├── AuthControllerTests.cs
│   │   └── TasksControllerTests.cs
│   ├── Services/
│   │   ├── AuthServiceTests.cs
│   │   └── TaskServiceTests.cs
│   └── Utils/
│       └── CustomWebApplicationFactory.cs
│
├── appsettings.json          
├── Program.cs                         
└── docker-compose.yml        
```

---

## 🐳 Ejecución con Docker Compose

### 1️⃣ Levantar los servicios

```bash
docker compose up -d
```

**Esto levantará:**
- 🗄️ **SQL Server** en el puerto `1433`

### 2️⃣ Aplicar migraciones

```bash
dotnet ef database update
```

### 3️⃣ Acceder a la aplicación

- 📖 **Swagger UI**: [http://localhost:5114/swagger](http://localhost:5114/swagger)
- 🌐 **API Base URL**: `http://localhost:5114/api`
---

## 🧪 Pruebas automatizadas

### ▶️ Ejecutar todas las pruebas

```bash
dotnet test
```
---

## 🧰 Comandos útiles

```bash
# Compilar el proyecto
dotnet build

# Ejecutar en modo desarrollo
dotnet run

# Ejecutar pruebas
dotnet test

# Aplicar migraciones
dotnet ef database update
```
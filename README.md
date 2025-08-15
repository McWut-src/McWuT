# McWuT

A modern ASP.NET Core web application built with .NET 9.0, featuring user authentication, identity management, and a clean multi-layered architecture.

## 🚀 Features

- **User Authentication & Authorization**: Complete identity management system with ASP.NET Core Identity
- **Two-Factor Authentication**: Enhanced security with 2FA support
- **Responsive Design**: Bootstrap-powered responsive UI
- **Entity Framework Core**: Robust data access layer with SQL Server support
- **Soft Delete Pattern**: Implemented through IEntity interface for data integrity
- **Multi-layered Architecture**: Clean separation of concerns across multiple projects

## 🛠️ Technology Stack

- **.NET 9.0**: Latest .NET framework
- **ASP.NET Core**: Web framework with Razor Pages
- **Entity Framework Core**: Object-relational mapping
- **SQL Server**: Primary database
- **ASP.NET Core Identity**: Authentication and authorization
- **Bootstrap**: Frontend CSS framework
- **jQuery**: JavaScript library for enhanced interactivity

## 📁 Project Structure

```
McWuT/
├── McWuT.Web/              # Main web application (Razor Pages)
│   ├── Areas/              # Identity UI areas
│   ├── Pages/              # Razor pages
│   ├── wwwroot/            # Static assets (CSS, JS, images)
│   └── Program.cs          # Application entry point
├── McWuT.Data/             # Data access layer
│   ├── Contexts/           # Database contexts
│   ├── Migrations/         # EF Core migrations
│   └── Services/           # Data services
├── McWuT.Core/             # Core interfaces and models
│   └── IEntity.cs          # Base entity interface
├── McWuT.Services/         # Business logic layer
├── McWuT.Common/           # Shared utilities and helpers
└── McWuT.sln              # Solution file
```

## 🔧 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (LocalDB, Express, or full version)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) (recommended)

## ⚡ Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/McWut-src/McWuT.git
cd McWuT
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Database Setup

The application is configured to use SQL Server with the following default connection string:
```
Server=localhost;Database=MyDatabase243;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

Update the connection string in `McWuT/Program.cs` if needed, then run:

```bash
dotnet ef database update --project McWuT.Data --startup-project McWuT
```

### 4. Run the Application

```bash
dotnet run --project McWuT
```

The application will start at `https://localhost:5001` (HTTPS) or `http://localhost:5000` (HTTP).

## 🔐 Authentication

The application includes a complete authentication system with:

- User registration and login
- Email confirmation (configurable)
- Password reset functionality
- Two-factor authentication
- Account management pages
- Role-based authorization support

## 🗄️ Database

The application uses Entity Framework Core with:

- **Code-First approach**: Models define the database schema
- **Migrations**: Version control for database changes
- **Soft Delete**: Entities implementing `IEntity` support soft deletion
- **Identity Tables**: Standard ASP.NET Core Identity schema

### Entity Interface

All domain entities should implement the `IEntity` interface:

```csharp
public interface IEntity
{
    int Id { get; set; }
    DateTime CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
    DateTime? DeletedDate { get; set; }
    Guid UniqueId { get; set; }
}
```

## 🔧 Configuration

### Connection String

Update the connection string in `McWuT/Program.cs`:

```csharp
builder.Configuration["ConnectionStrings:DefaultConnection"] = "Your_Connection_String_Here";
```

### Identity Settings

Identity options can be configured in `Program.cs`:

```csharp
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = true;
    // Add other options as needed
})
```

## 🚀 Deployment

### Development

```bash
dotnet run --project McWuT --environment Development
```

### Production

1. Update connection strings for production database
2. Configure HTTPS certificates
3. Set environment-specific configurations
4. Deploy using your preferred hosting platform (Azure, IIS, etc.)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Development Guidelines

- Follow C# coding conventions
- Write unit tests for new features
- Update documentation as needed
- Ensure migrations are properly created for database changes

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

If you encounter any issues or have questions:

1. Check the [Issues](../../issues) section
2. Create a new issue with detailed information
3. Provide steps to reproduce any bugs

## 📞 Contact

For questions or support, please open an issue in this repository.

---

**Happy Coding!** 🎉
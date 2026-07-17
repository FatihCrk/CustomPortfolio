# Portfolio CMS - Enterprise Level Content Management System

[![Build Status](https://github.com/FatihCrk/CustomPortfolio/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/FatihCrk/CustomPortfolio/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-blue)](https://www.docker.com/)

## 🚀 Features

### Core Features
- ✅ **Fully Dynamic CMS** - All content manageable from admin panel
- ✅ **Multi-Database Support** - PostgreSQL & SQL Server
- ✅ **Setup Wizard** - One-time initial configuration
- ✅ **RBAC System** - Super Admin, Admin, Editor, Viewer roles
- ✅ **JWT Authentication** - With refresh tokens & rotation
- ✅ **Enterprise Security** - OWASP Top 10 compliant
- ✅ **Audit Logging** - Complete activity tracking
- ✅ **Multi-language Ready** - i18n support (TR/EN)
- ✅ **SEO Optimized** - Meta tags, sitemap, schema.org
- ✅ **Performance Focused** - Caching, compression, optimization

### Admin Panel Capabilities
- Dashboard with analytics
- Profile & user management
- Content management (Projects, Blog, Skills, Experience, etc.)
- Media library with file management
- Contact form management
- Site settings & theme customization
- Menu builder
- Widget system
- Email template management
- Webhook configuration
- Feature flags
- Backup & restore
- Revision history

### Security Features
- BCrypt password hashing
- CSRF, XSS, SQL Injection protection
- Rate limiting & brute force protection
- Account lockout mechanism
- Token blacklist & revocation
- Secure headers (CSP, HSTS, etc.)
- Session management
- API key management
- Activity logging

## 🏗️ Architecture

```
Portfolio.Api              # Web API Layer
Portfolio.Application      # Application Logic & DTOs
Portfolio.Domain          # Domain Entities & Interfaces
Portfolio.Infrastructure  # External Services
Portfolio.Persistence     # Data Access Layer
Portfolio.Shared          # Shared Utilities
```

### Design Patterns & Principles
- Clean Architecture
- SOLID Principles
- Repository Pattern
- Unit of Work
- Dependency Injection
- CQRS (optional with MediatR)

## 🛠️ Tech Stack

### Backend
- ASP.NET Core 8 Web API
- Entity Framework Core 8
- PostgreSQL / SQL Server
- JWT Authentication
- Serilog Logging
- FluentValidation
- AutoMapper
- Hangfire (background jobs)

### Frontend (Coming Soon)
- Angular 20+
- Standalone Components
- Signals
- Angular Material
- SCSS

## 📦 Getting Started

### Prerequisites
- .NET 8 SDK
- Docker & Docker Compose (optional)
- PostgreSQL 15+ or SQL Server 2022+
- Node.js 18+ (for frontend)

### Quick Start with Docker

```bash
# Clone the repository
git clone https://github.com/FatihCrk/CustomPortfolio.git
cd CustomPortfolio

# Copy environment file
cp .env.example .env

# Update .env with your settings

# Start all services
docker-compose up --build

# Access the API
# http://localhost:5000/swagger
```

### Manual Setup

#### 1. Database Configuration

**PostgreSQL:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=PortfolioCMS;Username=postgres;Password=YourPassword"
  }
}
```

**SQL Server:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PortfolioCMS;User Id=sa;Password=YourPassword;TrustServerCertificate=true"
  }
}
```

#### 2. Run Migrations

```bash
cd src/Portfolio.Api

# Create migration
dotnet ef migrations add InitialCreate --project ../Portfolio.Persistence

# Update database
dotnet ef database update --project ../Portfolio.Persistence
```

#### 3. Run the Application

```bash
# From the API directory
dotnet run

# Or from root
dotnet run --project src/Portfolio.Api
```

Access Swagger UI at: `https://localhost:7000/swagger`

### First-Time Setup

1. Navigate to `/setup` endpoint
2. Fill in Super Admin details:
   - First Name & Last Name
   - Username
   - Email
   - Password
3. Complete setup wizard
4. Login with your credentials

## 📁 Project Structure

```
/workspace
├── src/
│   ├── Portfolio.Api/           # API endpoints, controllers
│   ├── Portfolio.Application/   # Business logic, DTOs, validators
│   ├── Portfolio.Domain/        # Entities, enums, interfaces
│   ├── Portfolio.Infrastructure/# Services (Auth, Email, Cache, etc.)
│   ├── Portfolio.Persistence/   # DbContext, repositories, migrations
│   └── Portfolio.Shared/        # Common utilities, exceptions
├── docker-compose.yml           # Docker orchestration
├── Dockerfile                   # Container build instructions
├── .env.example                 # Environment variables template
├── .github/workflows/           # CI/CD pipelines
└── README.md                    # This file
```

## 🔐 Security Best Practices Implemented

- ✅ Password hashing with BCrypt
- ✅ JWT with secure cookie storage
- ✅ Refresh token rotation
- ✅ Rate limiting per IP
- ✅ Input validation & output encoding
- ✅ Parameterized queries (EF Core)
- ✅ Security headers (CSP, X-XSS, HSTS, etc.)
- ✅ Audit logging for all operations
- ✅ Brute force protection
- ✅ Account lockout after failed attempts
- ✅ Token blacklist support

## 📊 Monitoring & Logging

- **Serilog** for structured logging
- Console & File sinks configured
- Activity logs stored in database
- Health check endpoint: `/health`
- Metrics ready for Prometheus/Grafana

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true

# Run specific test project
dotnet test src/Portfolio.Tests
```

## 🚀 Deployment

### Docker Production Deployment

```bash
# Build production image
docker build -t portfolio-cms:latest .

# Run with production settings
docker run -d \
  -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__DefaultConnection="your-connection-string" \
  portfolio-cms:latest
```

### Environment Variables

See `.env.example` for all available configuration options:
- Database connection
- JWT settings
- SMTP configuration
- File upload limits
- Security settings
- Logging levels

## 📝 API Documentation

Once running, access:
- **Swagger UI**: `http://localhost:5000/swagger`
- **OpenAPI Spec**: `http://localhost:5000/swagger/v1/swagger.json`
- **Health Check**: `http://localhost:5000/health`

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Fatih Crk**

- GitHub: [@FatihCrk](https://github.com/FatihCrk)
- Project: [CustomPortfolio](https://github.com/FatihCrk/CustomPortfolio)

## 🙏 Acknowledgments

- .NET Team for ASP.NET Core
- EF Core Team
- All open-source contributors

---

**Made with ❤️ using .NET 8**

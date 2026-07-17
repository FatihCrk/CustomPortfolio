# Portfolio CMS - Enterprise Level Content Management System

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-19+-red.svg)](https://angular.io/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## 🎯 Project Overview

Enterprise-level, fully dynamic Portfolio CMS with complete admin panel control. Built with Clean Architecture, SOLID principles, and enterprise security standards.

## ✨ Features

### Backend (ASP.NET Core 8)
- ✅ **Clean Architecture** with Domain-Driven Design
- ✅ **CQRS Pattern** with MediatR
- ✅ **Repository & Unit of Work** patterns
- ✅ **JWT Authentication** with Refresh Token rotation
- ✅ **RBAC** (Role-Based Access Control)
- ✅ **BCrypt Password Hashing**
- ✅ **OWASP Top 10** Protection
- ✅ **Rate Limiting** & Brute Force Protection
- ✅ **Audit Logging** with IP/Device tracking
- ✅ **Global Exception Handling**
- ✅ **FluentValidation**
- ✅ **AutoMapper**
- ✅ **Serilog** structured logging
- ✅ **Swagger/OpenAPI** documentation
- ✅ **Health Checks**
- ✅ **MSSQL & PostgreSQL** support

### Frontend (Angular 19+)
- ✅ **Standalone Components**
- ✅ **Angular Signals** for reactive state
- ✅ **Lazy Loading** routes
- ✅ **Route Guards** for authentication
- ✅ **Angular Material** UI components
- ✅ **Responsive Design**
- ✅ **Dark/Light Theme** support
- ✅ **SCSS** styling
- ✅ **HTTP Interceptors**
- ✅ **Token Management**

### Admin Panel Modules
- 🔐 **Authentication** (Login, Setup Wizard)
- 📊 **Dashboard** with statistics
- 👥 **User Management** (CRUD operations)
- 📁 **Project Management**
- 📝 **Blog Management** (Coming soon)
- 🎯 **Skill Management** (Coming soon)
- 🏆 **Certificate Management** (Coming soon)
- 📧 **Message Management** (Coming soon)
- ⚙️ **Settings** (Coming soon)

## 🏗️ Architecture

```
Portfolio CMS
├── Backend (ASP.NET Core 8)
│   ├── Portfolio.Api              # API Layer
│   ├── Portfolio.Application      # Application Logic
│   ├── Portfolio.Domain          # Domain Entities
│   ├── Portfolio.Infrastructure  # External Services
│   ├── Portfolio.Persistence     # Data Access
│   └── Portfolio.Shared          # Common Utilities
│
└── Frontend (Angular 19+)
    └── portfolio-admin           # Admin Panel
        ├── src/app/core          # Core modules
        ├── src/app/features      # Feature modules
        └── src/app/shared        # Shared components
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- MSSQL Server or PostgreSQL
- Docker (optional)

### Backend Setup

1. **Clone the repository**
```bash
git clone https://github.com/FatihCrk/CustomPortfolio.git
cd CustomPortfolio
```

2. **Configure Database**
Edit `src/Portfolio.Api/appsettings.json`:

**For MSSQL:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PortfolioCMS;User Id=sa;Password=YourPassword;TrustServerCertificate=true;"
  }
}
```

**For PostgreSQL:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=PortfolioCMS;Username=postgres;Password=YourPassword;"
  }
}
```

3. **Run Migrations**
```bash
cd src/Portfolio.Api
dotnet ef database update --project ../Portfolio.Persistence
```

4. **Start Backend**
```bash
dotnet run
```

Backend will be available at: `https://localhost:7000/swagger`

### Frontend Setup

1. **Navigate to Admin Panel**
```bash
cd portfolio-admin
```

2. **Install Dependencies**
```bash
npm install
```

3. **Start Development Server**
```bash
npm start
```

Frontend will be available at: `http://localhost:4200`

### Docker Setup (Recommended)

```bash
# Build and run all services
docker-compose up --build

# Or run in background
docker-compose up -d
```

Access points:
- API: http://localhost:5000/swagger
- Admin Panel: http://localhost:4200

## 🔐 Security Features

- **Password Policy**: Minimum 8 characters, uppercase, lowercase, number, special character
- **Account Lockout**: 5 failed attempts = 15 minute lockout
- **Token Rotation**: Automatic refresh token rotation
- **Secure Cookies**: HttpOnly, Secure, SameSite=Strict
- **Security Headers**: CSP, XSS Protection, HSTS
- **Input Validation**: Server-side validation on all inputs
- **SQL Injection Protection**: Parameterized queries
- **Rate Limiting**: Prevents brute force attacks
- **Audit Logging**: All actions logged with IP and device info

## 📊 Default Roles

| Role | Permissions |
|------|-------------|
| **Super Admin** | Full system access |
| **Admin** | Manage users, content, settings |
| **Editor** | Create, edit, delete content |
| **Viewer** | Read-only access |

## 🛠️ Development

### Backend Commands

```bash
# Add migration
dotnet ef migrations add MigrationName --project src/Portfolio.Persistence --startup-project src/Portfolio.Api

# Update database
dotnet ef database update --project src/Portfolio.Persistence --startup-project src/Portfolio.Api

# Run tests
dotnet test

# Build for production
dotnet build --configuration Release
```

### Frontend Commands

```bash
# Install dependencies
npm install

# Start development server
npm start

# Build for production
npm run build

# Run tests
npm test

# Lint code
npm run lint
```

## 📦 API Endpoints

### Authentication
- `POST /api/auth/setup` - Initial setup (one-time)
- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout
- `POST /api/auth/logout-all` - Logout from all devices
- `POST /api/auth/refresh-token` - Refresh access token
- `GET /api/auth/setup-status` - Check setup status

### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user

### Projects
- `GET /api/projects` - Get all projects (public)
- `GET /api/projects/{id}` - Get project by ID (public)
- `POST /api/projects` - Create project (Editor+)
- `PUT /api/projects/{id}` - Update project (Editor+)
- `DELETE /api/projects/{id}` - Delete project (Admin+)

## 🌍 Environment Variables

Create `.env` file in root directory:

```env
# Database
DB_PROVIDER=mssql
CONNECTION_STRING=Server=localhost;Database=PortfolioCMS;...

# JWT Settings
JWT_SECRET=your-super-secret-key-min-32-chars
JWT_ISSUER=PortfolioCMS
JWT_AUDIENCE=PortfolioCMSUsers
ACCESS_TOKEN_EXPIRY_MINUTES=60
REFRESH_TOKEN_EXPIRY_DAYS=7

# Email (Optional)
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USER=your-email@gmail.com
SMTP_PASS=your-password

# Redis (Optional)
REDIS_CONNECTION=localhost:6379
```

## 📈 Performance Optimization

- Server-side caching with Redis
- Response compression
- Image optimization (WebP)
- Lazy loading in Angular
- Bundle minification
- CDN ready architecture
- Virtual scrolling for large lists
- Pagination support

## 🧪 Testing

```bash
# Backend tests
cd src/Portfolio.Tests
dotnet test

# Frontend tests
cd portfolio-admin
npm test
```

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Fatih Crk**
- GitHub: [@FatihCrk](https://github.com/FatihCrk)
- Repository: [CustomPortfolio](https://github.com/FatihCrk/CustomPortfolio)

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📞 Support

For issues and questions:
- Create an issue on GitHub
- Check existing documentation
- Review API Swagger documentation

---

**Built with ❤️ using ASP.NET Core 8 & Angular 19+**

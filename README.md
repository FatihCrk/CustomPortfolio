# Portfolio CMS - Enterprise Level Content Management System

## 🎯 Proje Özeti

Tamamen admin panelinden yönetilebilen, yüksek güvenlikli, production seviyesinde, modern, responsive, SEO uyumlu ve performans odaklı bir Portfolio CMS.

## 🏗️ Mimari

### Clean Architecture Katmanları

```
src/
├── Portfolio.Domain          # Domain Entities, Enums, Interfaces
├── Portfolio.Application     # Business Logic, DTOs, Validators
├── Portfolio.Infrastructure  # External Services (Auth, Email, Storage)
├── Portfolio.Persistence     # EF Core, Repositories, Migrations
├── Portfolio.Api            # Web API, Controllers, Middleware
└── Portfolio.Shared         # Common Utilities, Constants

frontend/
├── portfolio-ui             # Public Website (Angular)
└── portfolio-admin          # Admin Panel (Angular)
```

## 🛠️ Teknolojiler

### Backend
- ASP.NET Core 8 Web API
- Entity Framework Core 8
- PostgreSQL
- JWT Authentication + Refresh Token
- BCrypt Password Hash
- FluentValidation
- AutoMapper
- MediatR (CQRS)
- Serilog
- Redis Cache

### Frontend
- Angular 20+
- Standalone Components
- Angular Signals
- Angular Material
- SCSS

## 🔐 Güvenlik Özellikleri

- BCrypt Password Hashing
- JWT + Refresh Token Rotation
- Secure Cookies (HttpOnly, SameSite)
- CSRF Protection
- XSS Prevention
- Rate Limiting
- Brute Force Protection
- Account Lockout
- Token Blacklist
- 2FA Support (TOTP)
- SQL Injection Prevention
- Input Validation & Output Encoding
- Security Headers (HSTS, CSP)
- Audit Logging

## 📋 Özellikler

### Kimlik Yönetimi
- Setup Wizard (İlk Kurulum)
- RBAC (Super Admin, Admin, Editor, Viewer)
- Detaylı İzin Yönetimi
- Session Management
- API Key Management
- Login History

### CMS Modülleri
- Profil Yönetimi
- Hero Bölümü
- Yetenekler
- Deneyim
- Eğitim
- Projeler (Kategorili, Etiketli)
- Sertifikalar
- Referanslar
- Blog (Kategori, Etiket, Yorum)
- Hizmetler
- İletişim Bilgileri
- Sosyal Medya
- Menü Yönetimi
- Sayfa Oluşturucu (Page Builder)
- Widget Sistemi

### Medya Yönetimi
- Media Library
- Dosya Tipi Kontrolü
- Boyut Kontrolü
- Thumbnail Oluşturma
- WebP Dönüşümü
- Image Optimization

### İletişim Formu
- Spam Koruması
- Captcha
- Rate Limit
- CSV/Excel Export
- E-posta Bildirimi

### SEO
- Meta Title/Description
- Keywords
- Canonical URL
- OpenGraph
- Twitter Card
- Schema.org
- Sitemap
- Robots.txt
- Slug Yönetimi

### Performans
- Server Side Caching (Redis)
- Response Compression
- Pagination
- Virtual Scroll Ready

### Ekstra Özellikler
- Çoklu Dil (i18n) Hazır Altyapı
- Çoklu Tema Sistemi
- Form Builder
- Revizyon Geçmişi (Versioning)
- Yedekleme/Geri Yükleme
- Ziyaretçi Analitikleri
- Webhook Desteği
- Feature Flag Sistemi
- Health Checks
- Maintenance Mode
- E-posta Şablon Yönetimi

## 🚀 Kurulum

### Gereksinimler
- .NET 8 SDK
- Node.js 20+
- PostgreSQL 15+
- Redis (Opsiyonel)

### Backend Kurulum

```bash
cd src

# Veritabanı bağlantı string'i ayarla
# appsettings.json veya environment variables

# Migration oluştur
dotnet ef migrations add InitialCreate --project Portfolio.Persistence --startup-project Portfolio.Api

# Migration uygula
dotnet ef database update --project Portfolio.Persistence --startup-project Portfolio.Api

# API'yi çalıştır
cd Portfolio.Api
dotnet run
```

### Frontend Kurulum

```bash
# Admin Panel
cd frontend/portfolio-admin
npm install
ng serve

# Public UI
cd frontend/portfolio-ui
npm install
ng serve
```

## 📁 Proje Yapısı

```
/workspace
├── src/
│   ├── Portfolio.sln
│   ├── Portfolio.Domain/
│   │   ├── Common/
│   │   │   ├── BaseEntity.cs
│   │   │   └── AuditableEntity.cs
│   │   ├── Entities/
│   │   │   ├── Identity.cs
│   │   │   └── Cms.cs
│   │   ├── Enums/
│   │   │   └── Enums.cs
│   │   ├── Interfaces/
│   │   │   └── IRepository.cs
│   │   └── Portfolio.Domain.csproj
│   ├── Portfolio.Application/
│   │   ├── Interfaces/
│   │   │   ├── IAuthService.cs
│   │   │   ├── IUnitOfWork.cs
│   │   │   └── IServices.cs
│   │   └── Portfolio.Application.csproj
│   ├── Portfolio.Infrastructure/
│   │   ├── Services/
│   │   └── Portfolio.Infrastructure.csproj
│   ├── Portfolio.Persistence/
│   │   ├── Contexts/
│   │   │   └── ApplicationDbContext.cs
│   │   ├── Configurations/
│   │   │   └── IdentityConfigurations.cs
│   │   ├── Repositories/
│   │   │   ├── Repository.cs
│   │   │   └── UnitOfWork.cs
│   │   └── Portfolio.Persistence.csproj
│   ├── Portfolio.Api/
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   ├── Filters/
│   │   ├── Extensions/
│   │   └── Portfolio.Api.csproj
│   └── Portfolio.Shared/
│       └── Portfolio.Shared.csproj
└── frontend/
    ├── portfolio-ui/
    └── portfolio-admin/
```

## ✅ Tamamlanan İşlemler

- [x] Proje mimarisi ve klasör yapısı
- [x] Domain Entities (Identity + CMS)
- [x] Enums (RoleType, MessageStatus, PublishStatus, etc.)
- [x] Base Entity ve Auditable Entity sınıfları
- [x] IRepository Interface
- [x] IUnitOfWork Interface
- [x] IAuthService Interface
- [x] IServices Interfaces (Email, Storage, Cache, Log, etc.)
- [x] ApplicationDbContext
- [x] Entity Configurations (Identity)
- [x] Generic Repository Implementation
- [x] Unit of Work Implementation
- [x] Solution ve Project dosyaları

## 🔄 Devam Eden/Sıradaki İşlemler

1. **Infrastructure Services**
   - AuthService (JWT, Refresh Token, 2FA)
   - EmailService
   - FileStorageService
   - CacheService
   - LogService
   - RateLimitService
   - SlugService

2. **API Katmanı**
   - Controllers (Auth, Users, Roles, CMS modules)
   - Middleware (Exception, Logging, RateLimit)
   - Filters (Authorization, Validation)
   - DTOs ve Mapping Profiles
   - Validators (FluentValidation)

3. **Setup Wizard**
   - İlk kullanıcı oluşturma
   - Rol ve izin seed
   - Site ayarları initializasyonu

4. **Frontend (Angular)**
   - Admin Panel
   - Public UI
   - Authentication Guards
   - HTTP Interceptors
   - Responsive Design

## 📝 Notlar

- Tüm içerikler veritabanından yönetilir, hardcoded içerik yoktur
- Soft delete tüm entity'lerde aktiftir
- Audit logging (CreatedBy, UpdatedAt, vb.) otomatiktir
- Tüm endpoint'ler validation ve authorization ile korunacaktır
- API versioning aktif olacaktır

## 📄 Lisans

Enterprise License - Tüm hakları saklıdır.

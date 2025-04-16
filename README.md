# GeeCommerce
A config-driven .NET microservices core (Auth/DB/Caching) to accelerate multi-tenant SaaS development.

# 🧩 Modular Microservices Core Framework

**A domain-agnostic, plug-and-play architecture for rapid enterprise microservices development**  
[![.NET](https://img.shields.io/badge/.NET-8.0-%23512bd4)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-15+-%23dd0031)](https://angular.io/)

## 🚀 Key Features
| Feature | Description |
|---------|-------------|
| **⚡ Zero-Code Integration** | Configure Auth, DB, and Caching via settings - no boilerplate |
| **🔐 Auth Pipeline** | JWT/OAuth2.0 with RBAC/ABAC policies |
| **🗃️ Database Agnostic** | SQL (PostgreSQL/SQL Server) or NoSQL (MongoDB) via `IDatabaseProvider` |
| **🧠 Smart Caching** | Switch between Redis/in-memory with `ICacheStrategy` DI |
| **📦 Domain Isolation** | Event-driven (Kafka/RabbitMQ) microservices with independent databases |

## 🛠️ Quick Start
```csharp
// Program.cs
//Configurare App setting and ConnectionStrings
builder.ConfigureDefaultApplicationServices<CatalogDbContext>();
builder.AddDefaultSwaggerGen();
builder.Services.RunDefaultDatabaseServices<CatalogDbContext>();
var certificate = DataSettingsManager.LoadSettings().CertificateDetail;
builder.AddDefaultAuthentication(certificate?.CertificatePath, certificate?.CertificatePassword);

var app = builder.Build();
app.UseSharedPolicies();
app.Run();

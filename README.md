# RPG Inventory - Full-Stack RPG Management System

A full-stack RPG character and inventory management system built with **ASP.NET Core 9** (Clean Architecture) and **React 19 + TypeScript** (Vite).

---

## Table of Contents

- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
  - [1. Clone the repository](#1-clone-the-repository)
  - [2. Configure the database](#2-configure-the-database)
  - [3. Apply database migrations](#3-apply-database-migrations)
  - [4. Start the API](#4-start-the-api)
  - [5. Start the frontend](#5-start-the-frontend)
- [Running Both Services](#running-both-services)
- [API Reference](#api-reference)
- [Environment Variables](#environment-variables)
- [Project Structure](#project-structure)
- [Tech Stack](#tech-stack)

---

## Architecture

```
RPGInventory/
+-- src/
|   +-- RpgApi.Domain          # Entities, interfaces, domain logic (no dependencies)
|   +-- RpgApi.Application     # Use cases, CQRS commands/queries via MediatR
|   +-- RpgApi.Infrastructure  # EF Core, SQL Server, JWT, BCrypt implementations
|   +-- RpgApi.Api             # ASP.NET Core Web API, controllers, middleware
+-- frontend/
    +-- rpg-client             # React 19 + TypeScript + Vite SPA
```

The backend follows **Clean Architecture** - dependencies point inward. The API layer knows nothing about the database; the Domain knows nothing about ASP.NET.

---

## Prerequisites

| Tool | Version | Download |
|---|---|---|
| .NET SDK | 9.0+ | https://dotnet.microsoft.com/download |
| SQL Server | 2019+ / Express | https://www.microsoft.com/en-us/sql-server/sql-server-downloads |
| Node.js | 20+ | https://nodejs.org |
| EF Core CLI | latest | `dotnet tool install -g dotnet-ef` |

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/CMHZMAN/RPGInventory.git
cd RPGInventory
```

### 2. Configure the database

Edit `src/RpgApi.Api/appsettings.json` and update the connection string to match your SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=RpgDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

> **Windows Auth** (Trusted_Connection) is the default. Change to `User Id=...;Password=...;` for SQL authentication.

### 3. Apply database migrations

Run this **once** to create all tables and seed initial item data:

```powershell
dotnet ef database update `
  --project src\RpgApi.Infrastructure\RpgApi.Infrastructure.csproj `
  --startup-project src\RpgApi.Api\RpgApi.Api.csproj
```

### 4. Start the API

```powershell
dotnet run --project src\RpgApi.Api\RpgApi.Api.csproj
```

The API starts on **`http://localhost:5115`**.

| URL | Description |
|---|---|
| `http://localhost:5115/swagger` | Swagger UI - interactive API explorer |
| `http://localhost:5115/api/auth/register` | Register a new user (public) |
| `http://localhost:5115/api/auth/login` | Login and receive a JWT (public) |
| `http://localhost:5115/api/characters` | Character endpoints (requires JWT) |
| `http://localhost:5115/api/items` | Item endpoints (requires JWT) |

### 5. Start the frontend

```powershell
cd frontend\rpg-client
npm install      # first time only
npm run dev
```

The frontend starts on **`http://localhost:5173`**.

---

## Running Both Services

Open **two terminal windows** and run them simultaneously:

**Terminal 1 - API:**
```powershell
dotnet run --project src\RpgApi.Api\RpgApi.Api.csproj
```

**Terminal 2 - Frontend:**
```powershell
cd frontend\rpg-client
npm run dev
```

Then open **`http://localhost:5173`** in your browser. You will be redirected to the login page automatically if you are not authenticated.

> **Tip:** Use `dotnet watch` instead of `dotnet run` to get hot reload on the API during development:
> ```powershell
> dotnet watch --project src\RpgApi.Api\RpgApi.Api.csproj
> ```

---

## API Reference

### Authentication (public - no token required)

| Method | Endpoint | Body |
|---|---|---|
| `POST` | `/api/auth/register` | `{ "username", "email", "password" }` |
| `POST` | `/api/auth/login` | `{ "username", "password" }` |

Both return:
```json
{ "token": "eyJ...", "username": "...", "expiresAt": "..." }
```

### Using the JWT token

Add the token to every protected request:
```
Authorization: Bearer eyJhbGci...
```

In **Swagger UI**, click **Authorize** (top right) and paste the token - no `Bearer` prefix needed.

### Characters (JWT required)

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/characters` | Get all characters |
| `GET` | `/api/characters/{id}` | Get character with inventory |
| `POST` | `/api/characters` | Create character |
| `PUT` | `/api/characters/{id}/name` | Rename character |
| `POST` | `/api/characters/{id}/levelup` | Level up (+1 level) |
| `POST` | `/api/characters/{id}/inventory/{itemId}` | Add item to inventory |
| `DELETE` | `/api/characters/{id}/inventory/{itemId}` | Remove item from inventory |
| `DELETE` | `/api/characters/{id}` | Delete character |

### Items (JWT required)

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/items` | Get all items |
| `GET` | `/api/items/{id}` | Get item by id |
| `POST` | `/api/items` | Create item |
| `PUT` | `/api/items/{id}` | Update item |
| `DELETE` | `/api/items/{id}` | Delete item |

---

## Environment Variables

The following settings can be overridden in `appsettings.json` or via environment variables:

| Key | Default | Description |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Express / RpgDb | SQL Server connection string |
| `Jwt:Secret` | *(must be set)* | Signing key - **change in production** |
| `Jwt:Issuer` | `RpgApi` | JWT issuer claim |
| `Jwt:Audience` | `RpgClient` | JWT audience claim |
| `Jwt:ExpiryMinutes` | `60` | Token lifetime in minutes |

> Warning: Never commit a real `Jwt:Secret` to source control.
> Use `dotnet user-secrets` locally and environment variables / secrets manager in production.

**Setting a secret locally:**
```powershell
dotnet user-secrets set "Jwt:Secret" "your-super-secret-key-min-32-chars" `
  --project src\RpgApi.Api\RpgApi.Api.csproj
```

---

## Project Structure

```
src/
+-- RpgApi.Domain/
|   +-- Entities/          # Character, Item, CharacterItem, User
|   +-- Enums/             # CharacterClass, ItemType
|   +-- Interfaces/        # IRepository, IUnitOfWork, IUserRepository, ...
|
+-- RpgApi.Application/
|   +-- Auth/Commands/     # RegisterCommand, LoginCommand + handlers
|   +-- Characters/        # Commands, Queries, DTOs
|   +-- Items/             # Commands, Queries, DTOs
|   +-- Common/            # Interfaces (IJwtService, IPasswordHasher), Exceptions
|
+-- RpgApi.Infrastructure/
|   +-- Persistence/       # RpgDbContext, EF configurations, Migrations
|   +-- Repositories/      # UnitOfWork, CharacterRepository, ItemRepository, ...
|   +-- Services/          # JwtService, BcryptPasswordHasher
|
+-- RpgApi.Api/
    +-- Controllers/       # AuthController, CharactersController, ItemsController
    +-- Middleware/        # ExceptionHandlingMiddleware
    +-- Program.cs         # App composition root

frontend/rpg-client/
+-- src/
|   +-- api/               # apiClient.ts (Axios + JWT interceptors), rpgApi.ts
|   +-- components/        # CharacterCard, CreateCharacterModal, ...
|   +-- hooks/             # useRpgApi
|   +-- pages/             # CharactersPage, CharacterDetailPage, ItemsPage, LoginPage
|   +-- types/             # api.types.ts
+-- vite.config.ts
```

---

## Tech Stack

### Backend

| | Technology |
|---|---|
| Framework | ASP.NET Core 9 |
| ORM | Entity Framework Core 9 (SQL Server) |
| Auth | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Password hashing | BCrypt.Net |
| CQRS / Mediator | MediatR 12 |
| API docs | Swashbuckle / Swagger UI |
| Architecture | Clean Architecture |

### Frontend

| | Technology |
|---|---|
| Framework | React 19 + TypeScript |
| Build tool | Vite 8 |
| HTTP client | Axios (with JWT interceptors) |
| Routing | React Router v7 |
| Data fetching | TanStack Query v5 |
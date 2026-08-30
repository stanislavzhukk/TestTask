# Conference Hall Booking API
API for managing conference hall bookings and rentals: searching for available halls, creating bookings, and calculating rental cost based on time and selected services.

Full project documentation (business logic, architecture, design decisions) is provided separately as documentation.pdf.

## Tech Stack

- .NET 8 (ASP.NET Core)
- PostgreSQL 16
- PGAdmin 4
- Docker & Docker Compose
- Entity Framework Core
- Redis(not used here, only possibility)
- Swagger / OpenAPI


## Architecture
 
```
Domain          — entities, repository interfaces, domain rules
Application     — services, DTOs, service interfaces
Infrastructure  — EF Core, repositories, migrations, Identity
API             — controllers, Swagger, middleware
```

---

## Getting Started

### 1 Prerequisites

Make sure you have installed:

- Docker  
- Docker Compose  

---

### 2 Environment variables

Create a `.env` file from the example:

```bash
cp .env.example .env
```

### 3 Build and start all services:

```bash
docker compose up --build
```

Database migrations are applied and seed data (3 halls, 3 base
amenities) is loaded automatically on startup.

---

### 4 Services & URLs

- PGAdmin (PostgreSQL UI)
http://localhost:5050/

- Swagger (API docs & testing)
http://localhost:8080/swagger/index.html
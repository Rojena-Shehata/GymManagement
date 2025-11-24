# 🏋️‍♂️ Gym Management System (MVC, N-Tier Architecture)

## 📖 Description
A web-based application for managing gym operations including **member management**, **trainer scheduling**, **session booking**, and **membership plans**.  
The system is built with **ASP.NET Core MVC**, uses **Entity Framework Core** for data access, and stores data in a **SQL Server** database.

---
📌 Features

✔️ Supports Identity Roles (Admin, SuperAdmin)
✔️ Register new Super Admins & Admins
✔️ Server-side Model Validation
✔️ Automatic Localization for Errors & UI Text based on browser language and coyld be changed (En,Ar)
✔️ AutoMapper support for mapping large ViewModels
✔️ Clean Architecture separation (BLL / DAL / Controllers)

---

## 🎯 Goals
- Centralize **Members** and **Plans** management  
- Manage **Trainers** and **Session schedules**  
- Improve **data consistency** and **workflow automation**  
- Provide a scalable and maintainable architecture using **N-Tier design**
- Enable a SuperAdmin role with full system control including Admin management
- Support multi-language UI (English + Arabic)

---
┌──────────────────────────┐
│      Presentation Layer   │  → MVC (Controllers, Views, Localization)
└───────────────▲──────────┘
                │
┌───────────────┴──────────┐
│  Business Logic Layer     │  → Services, Validation, AutoMapper
└───────────────▲──────────┘
                │
┌───────────────┴──────────┐
│   Data Access Layer       │  → EF Core, Repositories, Unit of Work
└───────────────────────────┘
------

## 🏗️ High-Level Architecture (Three-Layer Architecture)

### 🔹 Presentation Layer
- **ASP.NET MVC Controllers** and **Razor Views**
- Handles all user interactions
- Uses **Bootstrap** and **Custom CSS** for responsive UI

### 🔹 Business Logic Layer
- Contains **Service classes** such as `TrainerService`, `SessionService`, etc.
- Enforces core business rules and validation
- Acts as a bridge between UI and Data Access layers

### 🔹 Data Access Layer
- Implements **Repository Pattern** wrapping **Entity Framework Core DbContext**
- Provides CRUD operations and query abstraction
- Uses **Unit of Work** to coordinate multiple repositories within a single transaction

---

## ⚙️ Technology Stack

| Category                   | Technology                                             |
| -------------------------- | ------------------------------------------------------ |
| **Backend**                | ASP.NET Core MVC                                       |
| **ORM**                    | Entity Framework Core                                  |
| **Database**               | Microsoft SQL Server                                   |
| **Frontend**               | Razor Views, Bootstrap, Custom CSS                     |
| **Localization**           | JSON resource files (`en-US.json`, `ar-EG.json`)       |
| **Authentication / Roles** | ASP.NET Core Identity ( Admin, SuperAdmin)             |
| **Patterns**               | Repository Pattern, Unit of Work, Dependency Injection |
| **Mapping**                | AutoMapper                                             |

---

## 🎨 Design Patterns Used

### 🧱 Repository Pattern
Provides abstraction between business logic and data access layers for easier maintenance and testing.

### 🧩 Unit of Work Pattern
Coordinates the work of multiple repositories by managing a **single database context**.  
Ensures all related operations succeed or fail together, improving **data integrity** and **transaction management**.

### 💉 Dependency Injection (DI)
Enhances modularity and testability by injecting dependencies like repositories and services instead of hardcoding them.

---

## 🧠 Technologies Used
- **C#**
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **Bootstrap / HTML / CSS**
- Localization
- ASP.NET Identity roles

---

## ⚙️ Key Features
- Manage **Members**, **Trainers**, **Sessions**, and **Membership Plans**
- CRUD operations with **validation and business rules**
- Repository + Unit of Work for clean data access
- Separation of concerns for maintainability
- Simple and intuitive web interface
- Localization (EN/AR): Automatic culture switching + JSON-based translations.
- Identity Roles: (SuperAdmin / Admin)

---

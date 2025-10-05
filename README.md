# 🏋️‍♂️ Gym Management System (MVC, N-Tier Architecture)

A web-based application built using **ASP.NET MVC** that helps manage gym members, trainers, subscriptions, and payments efficiently.  
The project follows a **3-Tier (N-Tier) architecture** for better separation of concerns and maintainability.

---

## 🏗️ Architecture Overview

### 🔹 N-Tier Architecture
The system is divided into three main layers:

1. **Presentation Layer (MVC Project)**
   - Built using **ASP.NET MVC** and **Razor Views**
   - Handles user interaction (UI)
   - Sends user input to the Business Layer

2. **Business Logic Layer (Class Library)**
   - Contains all business rules and validation logic
   - Acts as a bridge between Presentation and Data layers
   - Ensures data consistency and workflow logic

3. **Data Access Layer (Class Library)**
   - Uses **Entity Framework Core** to interact with the database
   - Handles CRUD operations with SQL Server
   - Provides repository methods used by the Business Layer

---

## 🎨 Design Patterns Used

### 🧱 Repository Pattern
Provides abstraction between the business logic and data access layers for easier maintenance and testing.

### 💉 Dependency Injection (DI)
Improves modularity and testability by injecting dependencies like repositories and services instead of hardcoding them.

---

## 🧠 Technologies Used
- **C#**
- **ASP.NET MVC**
- **Entity Framework Core**
- **SQL Server**
- **Bootstrap / HTML / CSS**

---

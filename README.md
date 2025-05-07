# TaskManagement System

The TaskManagement System is a modular, scalable web application developed using **ASP.NET Core MVC**. It promotes **clean separation of concerns**, maintainability, and the use of **modern architectural practices**. The application supports **multi-role (Admin/User)** task workflows with **secure JWT authentication**, **role-based authorization**, and **transactional integrity**.

---

## 🏗 Architecture Overview

The system is structured using **Clean Layered Architecture** with strong boundaries between layers:

- **Presentation Layer (`Web`)**: UI rendering via Razor Views, partial views, static resources, and middleware.
- **Services Layer**: Bridges Web and API via typed `Flurl`-based HTTP clients using authenticated cookie-based communication.
- **API Layer (`TaskManagement.API`)**: Exposes all business functionalities via RESTful endpoints with validation and error handling.
- **Business Layer**: Implements domain logic with `IUnitOfWork`, AutoMapper profiles, and service interfaces.
- **Data Layer**: Includes repository patterns, seed data, SQL procedures, and `DbContext` with EF Core.
- **Core Layer**: Defines DTOs, enums, route constants, configurations, and common utilities.

---

## ⚙ Key Enhancements and Architectural Patterns

### ✅ Modular Dependency Injection
- All services registered using `IDependencyInjection` and resolved via `ITypeFinder`.
- Each layer declares its dependencies independently with predictable `Order`.

### ✅ Unit of Work Pattern
- Coordinates multiple repository operations inside a single database transaction.
- Enforces rollback safety on failure using `IUnitOfWork`.

### ✅ AutoMapper Configuration
- ViewModel ↔ Entity mapping handled via modular profile classes.
- Auto-registered during startup using `AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())`.

### ✅ Global Exception Middleware
- Handles and logs unhandled exceptions.
- Returns structured `ApiResponse` with error details and proper HTTP status codes.

### ✅ Service Layer via Flurl
- Clean, testable, and typed API consumers built using `Flurl.Http`.
- Base service encapsulates authentication headers and cookie parsing.

### ✅ Stored Procedure-Based Filtering
- Task filtering (searching/sorting/pagination) handled using `sp_GetFilteredTasks`.
- Procedure is added through **EF Core migration** and maintained in `/migrations/scripts/StoredProcedures`.
- Ensures **predictable cross-environment behavior** without manual intervention.

### ✅ Configured and Seeded DbContext
- Fluent-based configurations in `Data.Configurations.*`.
- Data seeded (users, tasks, etc.) via `Data.Seed.*`.
- SP result models mapped using `.HasNoKey().ToView(null)`.

### ✅ JWT Authentication with Cookies
- `AccessToken` and `RefreshToken` stored in `HttpOnly`, `Secure` cookies.
- Claims contain `role`, `userId`, and `ipAddress` for validation logic.

---

## 🔐 Role-Based API Authorization

The system enforces strict separation of capabilities between `Admin` and `User` roles using **shared endpoints**:

### 👤 User Role
- Can **view and update status** of:
  - Personal tasks (created with `CreatorId == AssigneeId`).
  - Tasks assigned by Admin.
- Can **create personal tasks** (self-assigned).
- Cannot view or access **other users’ personal tasks**.
- Cannot manage users.

### 🛠 Admin Role
- Can create:
  - Tasks assigned to others.
  - Tasks for themselves.
- Can update **any task** except **users' personal tasks**.
- Full **CRUD on users**.
- Can **filter/sort/search tasks**, excluding user personal tasks.
- Use **empty filter model** to retrieve all admin-accessible tasks.

### 🔁 Common Filtering Rules
- Filters are applied dynamically based on non-null fields in the request.
- SP supports flexible filtering using columns like `status`, `priority`, `searchTerm`, etc.
- Sorting and pagination are handled within the same stored procedure.

---

## 🧪 Testing & Validation

- All endpoints tested through both Web UI and Postman.
- Role-based access verified through JWT decoding and controlled API response logic.
- Transactions tested via `IUnitOfWork` orchestration.
- Manual regression testing done across multiple environments.

---

## 🔐 Security Considerations

- JWTs are stored using secure cookie settings (`HttpOnly`, `Secure`, `SameSite`).
- Role checks and ownership checks are implemented at the API level.
- No sensitive fields (e.g., password hashes) are exposed in any model or view.


## 📦 Complition with Respect to Requirements

- All mandatory and optional requirements are met as per the specifications till API level.

### Functionalities Implpemented 
- CRUD of User by Admin
- CRUD of Task by Admin
- Personal Task Implementation
- Task Filtering
- Task Sorting
- Task Pagination
- Task Searching
- Task Assignment
- Task Status Update
- Task Priority Update
- User Task Status Update
- User Personal Task Management
- Well Integrated Frontend with Backend till Admin's Task Management & User Listing & User's Task Listings
- Global Exception Handling
- JWT Authentication
- Auto Mapper 
- Unit of Work
- Stored Procedure for Task Filtering
- Flurl for API Communication
- User Response Handler 


### Room of Improvements
- Can be Integrated completely with Frontend (i.e. User Panel, Admin Panel's User Create operation)
- Massage's to user can be more precise 
- Url.js can be used for ajax calls 
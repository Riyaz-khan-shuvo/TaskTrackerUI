Here’s a **professional README.md** for your **TaskTrackerUI (ASP.NET Core MVC)** project. You can directly paste this into your repository:

---

# 🧾 TaskTrackerUI – ASP.NET Core MVC Frontend

## 📌 Overview

TaskTrackerUI is the frontend MVC application for the Task Tracker system. It is built using **ASP.NET Core MVC (.NET 8)** with **Razor Views**, **jQuery AJAX and Kendo UI**, and **Bootstrap** for UI styling.

The application allows authenticated users to manage tasks efficiently, including creating, editing, searching, filtering, and updating task status without full page reloads.

---

## 🚀 Features

### 🔐 Authentication

* ASP.NET Core Identity-based authentication
* User Registration / Login / Logout
* Secure access with `[Authorize]`
* Displays logged-in username in layout

### 📋 Task Management

* Create new tasks
* Edit existing tasks
* Delete tasks with confirmation
* Mark tasks as completed (AJAX toggle)

### 🔍 Search & Filter

* Search tasks by title (AJAX-based)
* Filter by:

  * Completed
  * Pending
* Sort by due date

### ⚡ AJAX Enhancements

* Search without page reload
* Status update (complete/incomplete) dynamically
* Smooth user experience with jQuery

### 🎨 UI/UX

* Clean Razor Views
* Responsive layout using Bootstrap
* Interactive task table
* Confirmation dialogs for delete actions


## 🗄️ Database

The application uses **MS SQL Server** with Entity Framework Core.

### Tasks Table Schema

```sql
CREATE TABLE Tasks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    DueDate DATETIME NULL,
    Priority INT NOT NULL,
    IsCompleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
)
```

### Priority Mapping

* 1 = Low
* 2 = Medium
* 3 = High

---

## ⚙️ Tech Stack

* ASP.NET Core MVC (.NET 8)
* ASP.NET Core Identity
* Entity Framework Core
* MS SQL Server
* jQuery & AJAX
* Bootstrap 5

---

## 🔐 Authentication Setup

* Identity is configured using EF Core
* Default login required for task access
* `[Authorize]` applied on TaskController
* Secure password hashing handled by Identity framework

---

## 📡 API Integration

This UI project communicates with the TaskTracker API:

* Task CRUD operations
* Search & filtering
* Status updates via AJAX

---

## 🧠 Key Highlights

* Clean MVC architecture
* Separation of concerns (Controller / View / Model)
* AJAX-driven UI for better UX
* Secure authentication system
* Scalable structure for future enhancements

* Pagination for task list
* Export to CSV
* Toast notifications (SweetAlert)
* Role-based access (Admin/User)


---

## 🛠️ Setup Instructions

### 1. Clone Repository

```bash
git clone https://github.com/Riyaz-khan-shuvo/TaskTrackerUI.git
```

### 2. Run Application

```bash
dotnet run
```

---


---

## 👨‍💻 Author

**Riyaz Hossain**
.NET Developer

---



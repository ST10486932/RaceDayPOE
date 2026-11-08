# RaceDay - Event Management System (PROG6212 POE)

## Project Overview
RaceDay is a comprehensive, full-stack web-based event management platform designed specifically for the South African road running, walking, and cycling community. This project was built progressively across three parts as part of the Portfolio of Evidence (PROG6212), culminating in a fully functional, containerised MVC application with cloud storage integration.

## Features
- **User authentication:** Secure login and registration with ASP.NET Core Identity (Admin, Organiser, Participant roles).
- **Event management:** Organisers can create, edit, and delete events.
- **Category management:** Organisers can define categories (e.g., 10km, Elite) for each event.
- **Participant enrolments:** Users can browse events and enrol in specific categories.
- **Race results:** Organisers can capture and publish race results.
- **Azure Blob Storage:** Event banner images are uploaded securely to the cloud.
- **Role-Specific Dashboards:** Distinct interfaces and navigation menus for all three roles.
- **Dockerised:** Fully containerised for easy deployment.

## User Roles
- **Admin:** Oversees the entire system, monitors activities, and manages user roles.
- **Organiser:** Creates and manages events, defines categories, views enrolments, and captures results.
- **Participant:** Browses events, enrols in categories, and views their personal race history and results.

---

## 📚 Project Breakdown

### Part 1: System Planning and Database
- **ERD:** Designed a relational database with 7 entities (User, Organiser, Participant, Admin, Event, Category, Enrolment).
- **API Plan:** Mapped out all required RESTful API endpoints.
- **SQL Script:** Wrote a complete SQL Server script to create tables, constraints, and seed sample data.

### Part 2: API & Database Implementation
- Built the backend using **ASP.NET Core MVC (.NET 8)** and **Entity Framework Core**.
- Configured a SQL Server database (`RaceDay2DB`) using EF Core Code-First Migrations.
- Implemented **ASP.NET Core Identity** for secure authentication and role management.
- Wrote integration tests using xUnit and an in-memory database to verify controller logic.

### Part 3: UI, Azure Blob Storage, & Docker (Final Part)
- **Role-Specific Interfaces:** Beautifully designed dashboards for Admin, Organiser, and Participant with colour-coded statuses.
- **Azure Blob Storage:** Integrated the `Azure.Storage.Blobs` SDK. Event banners are uploaded to an Azure container and displayed on the event page.
- **Docker Containerisation:** Created a multi-stage `Dockerfile` and `.dockerignore`.

---

## Technology Stack
- **Framework:** ASP.NET Core MVC (.NET 8)
- **Database ORM:** Entity Framework Core
- **Database:** SQL Server (LabVM2049939\SQLEXPRESS)
- **Authentication:** ASP.NET Core Identity
- **Cloud Storage:** Azure Blob Storage
- **Containerisation:** Docker
- **Frontend:** HTML5, CSS3, Bootstrap 5, Razor Pages

---

## 🚀 How to Run This Project (Local Setup)
1. Clone this repository.
2. Open `appsettings.json` and update the `DefaultConnection` string to point to your SQL Server.
3. Ensure your Azure `ConnectionString` and `ContainerName` are populated in `appsettings.json`.
4. Open the **Package Manager Console** in Visual Studio and run:
   ```powershell
   Update-Database

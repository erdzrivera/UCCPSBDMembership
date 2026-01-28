# UCCP SBD Membership Application

## 📝 Overview
The **UCCP SBD Membership App** is a modern, full-stack web application designed to streamline the management of church membership records and recognized organizations. Built on the robust **ABP Framework**, it combines a high-performance **.NET 9.0** backend with a responsive **Angular 19** frontend, all styled with a custom "Modern Rustic" aesthetic.

## 🚀 Technology Stack

### Frontend
- **Framework**: Angular 19.1.0
- **UI Theme**: ABP LeptonX Theme (v4.2.3) with custom overrides
- **Styling**: SCSS, Bootstrap 5, FontAwesome
- **State Management**: RxJS

### Backend
- **Framework**: .NET 9.0
- **Platform**: ABP Framework 9.2.3
- **ORM**: Entity Framework Core
- **Database**: SQL Server (Default configuration)
- **Caching**: StackExchange.Redis

## ✨ Key Features

### Core Modules
- **Member Management**:
  - Comprehensive CRUD operations for member profiles.
  - Advanced filtering (Name, Birthdays).
  - Default sorting by Last Name for quick lookup.
- **Organization Management**:
  - Track Church-Recognized Organizations (CROs).
  - Manage organization details including Name and Abbreviations.
  - Default sorting by Name.
- **Membership Types**:
  - Configurable categorization system for members.

### User Experience Enhancements
- **"Modern Rustic" Theme**: A custom-tailored design system using a Stone & Sage color palette (`#52796f`, `#b08968`) for a warm, professional feel.
- **Global Loading Overlay**: A centralized, non-blocking loading spinner providing consistent feedback across the application.
- **Enhanced Navigation**: A polished sidebar with a custom logo implementation, refined transitions, and optimized spacing.

## 🛠️ Prerequisites
Ensure you have the following installed on your development machine:
- [Node.js](https://nodejs.org/) (v18 or higher)
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or LocalDB)
- [Redis](https://redis.io/) (Optional but recommended for ABP caching)

## 📦 Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd UCCP_SBD_Membership_App
```

### 2. Backend Setup
The backend consists of two main runnable projects: the **AuthServer** and the **API Host**.

**Run the AuthServer:**
```bash
cd aspnet-core/src/UCCP.SBD.Membership.AuthServer
dotnet run
```

**Run the API Host:**
```bash
cd aspnet-core/src/UCCP.SBD.Membership.HttpApi.Host
dotnet run
```
*Wait for both services to be fully up and running before starting the frontend.*

### 3. Frontend Setup
Navigate to the Angular project directory to install dependencies and start the development server.

```bash
cd angular
npm install
npm start
```
The application will be available at `http://localhost:4200/`.

## 📂 Project Structure Overview

- **`angular/`**: The client-side Single Page Application (SPA).
  - `src/app/members`: Member management logic.
  - `src/app/organizations`: Organization management logic.
  - `src/styles.scss`: Global styles and theme overrides.
- **`aspnet-core/`**: The server-side solution following Domain-Driven Design (DDD).
  - `src/*.Domain`: Enterprise domain logic and entities.
  - `src/*.Application`: Application services and DTOs.
  - `src/*.EntityFrameworkCore`: Database context and migrations.
  - `src/*.HttpApi.Host`: Main API entry point.

## 🎨 Theme Customization
The application defines a set of CSS variables for consistent branding. You can find and modify these in `angular/src/styles.scss`:

```scss
:root {
  /* Modern Rustic Palette */
  --lpx-primary: #52796f;   /* Sage Green */
  --lpx-secondary: #b08968; /* Sand/Tan */
  --lpx-brand: #44403c;     /* Stone */
}
```

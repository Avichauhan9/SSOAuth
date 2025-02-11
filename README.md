# Azure AD SSO Backend Template with SQL Server and EF Core

![Azure AD](https://img.shields.io/badge/Azure%20AD-0089D6?logo=microsoft-azure&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoft-sql-server&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-512BD4?logo=.net&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?logo=.net&logoColor=white)

The **Azure AD Backend Template** is a Visual Studio project template designed to help you quickly set up a secure, scalable backend application with **Azure Active Directory (AD)** authentication, **SQL Server** database, and **Entity Framework (EF) Core** for database management. This template provides a robust foundation for building modern web applications with integrated Single Sign-On (SSO), role-based access control, and automated database migrations.

---

## Features

- **Azure AD Integration**: Pre-configured Azure AD authentication with SSO support.
- **SQL Server Database**: Pre-configured SQL Server connection with EF Core migrations.
- **Role-Based Access Control (RBAC)**: Manage users, roles, and permissions easily.
- **Automatic Database Setup**: Database and tables are created automatically on first run.
- **Prebuilt RESTful APIs**: Endpoints for user management, role assignment, and permission control.
- **Customizable and Extensible**: Easily extend the template to fit your needs.

---

## Prerequisites

Before you begin, ensure you have the following installed:

- **[Visual Studio 2022](https://visualstudio.microsoft.com/vs/)** (Community, Professional, or Enterprise).
- **[.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)** or later.
- **[SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)** (local or cloud-based).
- **Azure AD Tenant**: Required for authentication.

---

## Installation

### Step 1: Clone the repository

1. Clone this repository to your local machine:

   ```bash
   git clone https://github.com/Avichauhan9/SSOAuth.git
   ```

   or

   ```bash
   git clone git@github.com:Avichauhan9/SSOAuth.git
   ```

2. navigate to cloned repository

   ```bash
   cd SSOAuth.Net
   ```

---

## Configuration

### Step 1: Configure Azure AD

1. Go to the [Azure Portal](https://portal.azure.com/).
2. Register a new application and note the following:

   - **Instance**
   - **Tenant ID**
   - **Client ID**
   - **Client Secret**
   - **Domain**
   - **Client App Name**
   - **Redirect URL**
   - **Master Client ID**
   - **Master Client Secret**

3. Update the `appsettings.json` file with your Azure AD credentials:
   ```json
   "AzureAd": {
     "Instance": "https://login.microsoftonline.com/",
     "TenantId": "YOUR_TENANT_ID",
     "ClientId": "YOUR_CLIENT_ID",
     "ClientSecret": "YOUR_CLIENT_SECRET",
     "Domain": "YOUR_DOMAIN",
     "ClientAppName": "YOUR_APP_NAME",
     "InviteRedirectUrl": "YOUR_REDIRECT_URL",
     "MasterClientId": "YOUR_MASTER_CLIENTID",
     "MasterClientSecret": "YOUR_MASTER_CLIENT_SECRET"
   }
   ```

---

### Step 2: Configure SQL Server

1. Update the connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=YourDatabase;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
2. Replace `YourDatabase` with your desired database name.

---

### Step 3: Configure SMTP Secrets for sending email on fresh user registration.

1. Update SMTP Settings in `appsettings.json`:
   ```json
   "SmtpSettings": {
    "Sender": "",
    "SmtpServer": "",
    "Receiver": "",
    "Port": 587,
    "Username": "",
    "Password": "",
    "SSL": false
   }
   ```

### Step 4: Run the Application

1. Build and run the project:
   ```bash
   dotnet build
   dotnet run
   ```
2. The application will:
   - Create the database and tables (if they don’t exist).
   - Seed initial data with one user(admin@example.com), replace this email with one of the existing user of your tenant so that azure token of that user can be used for accesing swagger endpoints.

---

## Included Components

- **Models**: `User`, `Role`, `Permission`, `UserRole`, `RolePermission`, `NotificationTemplate`.
- **Context**: `AppDBContext` with preconfigured relationships and seed data.
- **Endpoints**: RESTful APIs for user management, role assignment, and permission control.
- **Middlewares**: Authentication, authorization, and error handling.
- **Utilities**: Helper classes for configuration and extensions.

---

### API Endpoints

- **User Management**:

  - `GET /users`: Get all users.
  - `POST /users/create`: Create a new user.
  - `GET /users/{id}`: Get user by id.
  - `GET /users/me`: Get info of logged in user.
  - `PUT /users/update`: Update a user.
  - `PATCH /users/update-status`: Update user status.

- **Role Management**:

  - `GET /roles`: Get all roles.
  - `POST /roles/create`: Create a new role.
  - `GET /roles/{id}`: Get role by id.
  - `GET /roles/users/{roleId}`: Get all users given specific role.
  - `PUT /roles/update`: Update a user.

- **Permission Management**:
  - `GET /permissions`: Get all permissions.
  - `POST /permissions/create`: Create a new permission.
  - `GET /permissions/{id}`: Get permission by id.
  - `GET /permissions/roles/{permissionId}`: Get all roles associated with permission.

---

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/YourFeature`).
3. Commit your changes (`git commit -m 'Add some feature'`).
4. Push to the branch (`git push origin feature/YourFeature`).
5. Open a pull request.

---

## Acknowledgments

- [Microsoft Azure](https://azure.microsoft.com/) for Azure AD integration.
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) for database management.
- [Visual Studio](https://visualstudio.microsoft.com/) for the development environment.

---

Enjoy building secure and scalable backend applications with the **Azure AD Backend Template**! 🚀

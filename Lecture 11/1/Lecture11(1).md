# Lecture 8(I): Adding Identity for User Management (Login, Logout & Account Management)

## 🎯 Lecture Goal:
In this lecture, we will:
- Integrate ASP.NET Core Identity for user authentication and management.
- Use Scaffolded Items to quickly set up login, logout, registration, and account management pages.
- Understand the default structure and configuration of Identity.

---

## 1️⃣ What is ASP.NET Core Identity?
### 📌 ASP.NET Core Identity is a membership system that allows us to:
- Manage user authentication (sign-up, login, logout).
- Implement role-based authorization (Admin, Customer, etc.).
- Handle password hashing, email confirmation, and token-based authentication.

### 📌 Key Components of Identity:
- **IdentityUser**: Represents a user in the system.
- **IdentityRole**: Represents roles (e.g., Admin, Customer).
- **IdentityDbContext**: Manages Identity tables in the database.

---

## 2️⃣ Installing ASP.NET Core Identity Packages
Run the following commands in **Package Manager Console**:
```powershell
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
Install-Package Microsoft.AspNetCore.Identity.UI
```

### 🔹 Explanation:
- `Identity.EntityFrameworkCore` adds user management capabilities.
- `Identity.UI` provides pre-built Razor pages for login, registration, and account management.

> **Note:** If you have already created an Individual Account Project during project setup, Identity is automatically configured. You can skip to Step 4️⃣.

---

## 3️⃣ Adding Identity to the Project

### 📌 Step 1: Configure Identity in `Program.cs`
Update `Program.cs` to register Identity services:

```csharp
using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity Services
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Enable Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();  // Required for Identity scaffolded pages
app.Run();
```

### 🔹 Explanation:
- `AddDefaultIdentity<IdentityUser>()` registers default identity services.
- Configures password policies (e.g., digits required, no uppercase required).
- `app.MapRazorPages()` enables scaffolded Identity pages.

### 📌 Step 2: Update `appsettings.json` with a Connection String
Modify `appsettings.json`:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ECommerceDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 🔹 Explanation:
- Specifies SQL Server as the database with a trusted connection.

> **Note:** If you used the Individual Account template during project setup, these configurations are already included.

---

## 4️⃣ Scaffolding Identity in ASP.NET Core MVC

### 📌 Step 1: Add Scaffolded Item for Identity
1. Right-click on the Project → **Add → New Scaffolded Item**.
2. Select **Identity** → Click **Add**.
3. In the “Add Identity” dialog:
   - Choose **Account/Login, Register, Logout, Manage/Index**.
   - Select **+** to create a new Data context class (e.g., `ApplicationDbContext`).
   - Click **Add**.

### 📌 Identity Pages Generated:
```
📂 Areas/Identity/Pages/Account/ → Login, Register, Logout
📂 Areas/Identity/Pages/Account/Manage/ → Profile management, Change Password
```

### 📌 Step 2: Update Navigation Links for Login & Register
Modify `Views/Shared/_Layout.cshtml`:

```html
<ul class="navbar-nav ml-auto">
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="Identity" asp-page="/Account/Login">Login</a>
    </li>
    <li class="nav-item">
        <a class="nav-link text-dark" asp-area="Identity" asp-page="/Account/Register">Register</a>
    </li>
    <li class="nav-item">
        <form asp-area="Identity" asp-page="/Account/Logout" method="post">
            <button class="btn btn-link text-dark">Logout</button>
        </form>
    </li>
</ul>
```

---

## 5️⃣ Testing User Registration and Login

### 📌 Steps to Test:
1. Run the application (`F5`).
2. Click **Register** → Create a new user.
3. Log in with the newly created user credentials.
4. Test logout and login again.

### 📌 Testing in SQL Server:
1. Open **SQL Server Management Studio (SSMS)**.
2. Check the `AspNetUsers` table in the **ECommerceDB** database for new users.
3. Verify roles in `AspNetRoles` (if any).

---

## 6️⃣ Customizing Identity Pages

### ➡ To Customize a Scaffolded Page:
Edit `Areas/Identity/Pages/Account/Register.cshtml`.

### ➡ Example: Adding Full Name Field in Register.cshtml:
```html
<div class="form-group">
    <label asp-for="Input.FullName"></label>
    <input asp-for="Input.FullName" class="form-control" />
    <span asp-validation-for="Input.FullName" class="text-danger"></span>
</div>
```

---

## 7️⃣ Setting Up Password Policies and User Settings

Update `Program.cs`:

```csharp
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.SignIn.RequireConfirmedEmail = false;
});
```

---

## 8️⃣ Applying Migrations for Identity Tables

Run the following in **Package Manager Console**:
```powershell
Add-Migration AddIdentityTables
Update-Database
```

### 🔹 Explanation:
- `Add-Migration` creates tables like `AspNetUsers`, `AspNetRoles`, `AspNetUserRoles`.
- `Update-Database` applies the changes to SQL Server.

> **Note:** If you selected **Individual Account** when creating the project, migrations are already applied. You only need to run `Update-Database`.

---

## 9️⃣ Testing Identity Integration
### 📌 Steps to Verify:
1. Run the application.
2. Register, login, and log out as a new user.
3. Check `AspNetUsers` table for new entries.
4. Test password policies during registration.

---

## 🔜 Next Steps: Implementing Role-Based Authorization
### 📌 In the next lecture, we will:
✔ Implement **role-based access control (RBAC)**.
✔ Create **Admin and Customer roles**.
✔ Manage access to specific pages based on roles.

---

## ✨ Summary & Key Takeaways:
✅ Integrated **ASP.NET Core Identity** with scaffolded items.
✅ Configured **login, logout, registration, and account management pages**.
✅ Applied migrations to **create Identity tables** in the database.
✅ Tested **user registration and login functionality**.

🚀 **Next, we’ll implement Role-Based Authorization to secure our e-commerce application!**

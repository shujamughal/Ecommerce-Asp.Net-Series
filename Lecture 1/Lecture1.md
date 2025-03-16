# Lecture 1: Introduction to ASP.NET Core MVC

## 🎯 Lecture Goal
Introduce students to ASP.NET Core MVC, covering core concepts, project structure, and MVC architecture, to build a strong foundation before starting the e-commerce application in the next lecture.

## 1️⃣ What is ASP.NET Core MVC?
ASP.NET Core MVC is a framework for building web applications based on the Model-View-Controller (MVC) pattern. It provides a structured way to separate concerns in web development, making applications more scalable, maintainable, and testable.

### 📌 Why Use ASP.NET Core MVC?
✔ **Cross-Platform** – Runs on Windows, Linux, and macOS.  
✔ **Modular & Lightweight** – Uses middleware for flexibility.  
✔ **Built-in Features** – Includes authentication, authorization, dependency injection, and API support.  
✔ **Separation of Concerns** – Follows the MVC pattern for cleaner code.  

## 2️⃣ Understanding the MVC Architecture
MVC (Model-View-Controller) is a design pattern used to organize application logic into three main components:

- **🔹 Model (M):** Represents the data & business logic of the application. It interacts with the database and contains validation rules.
- **🔹 View (V):** Defines the user interface. It displays data from the Model and interacts with users.
- **🔹 Controller (C):** Handles user requests, processes data, and decides what response to return (View or JSON data).

### 📌 How MVC Works in a Web Application
1️⃣ A user makes a request (e.g., visiting `/Products`).  
2️⃣ The Controller processes the request and retrieves data from the Model.  
3️⃣ The View displays the data as an HTML page.  
This approach improves maintainability by keeping concerns separate.

## 3️⃣ Setting Up the Development Environment
To start building ASP.NET Core MVC applications, install the following tools:

✔ **.NET SDK (latest version)** – [Download](https://dotnet.microsoft.com/en-us/download)  
✔ **Visual Studio 2022** (or **VS Code** with C# extension)  
✔ **SQL Server & SSMS** (or SQLite/PostgreSQL for database management)  

## 4️⃣ Creating an ASP.NET Core MVC Project

### 📌 Steps to Create a New Project
1️⃣ Open **Visual Studio** → Click **Create a new project**.  
2️⃣ Select **ASP.NET Core Web App (Model-View-Controller)**.  
3️⃣ Choose **.NET 8** (or latest) and name the project (e.g., `MyWebApp`).  
4️⃣ Select **Individual Authentication** (for login/signup support).  
5️⃣ Click **Create** – Visual Studio generates a ready-to-use MVC structure.  

## 5️⃣ Exploring the Default Project Structure
Once the project is created, it contains several folders and files:

📂 **Controllers/** → Handles user requests (e.g., `HomeController.cs`).  
📂 **Models/** → Represents database entities (e.g., `Product.cs`).  
📂 **Views/** → Defines the UI using Razor syntax (`.cshtml` files).  
📂 **wwwroot/** → Stores static files like CSS, JavaScript, and images.  
📂 **appsettings.json** → Holds configurations for database, logging, etc.  

This default structure ensures a clean separation of concerns, making development more organized.

## 6️⃣ Running the Default ASP.NET Core MVC Application
1️⃣ Click **Run (F5)** – The browser opens the default MVC app.  
2️⃣ Explore the **Home, Privacy, and Account** pages.  
3️⃣ Open `HomeController.cs` and observe how actions handle requests.  

## 7️⃣ Next Steps: Building the E-Commerce Application
### 📌 In the next lecture, we will:
✔ Begin working on our **E-Commerce Application**.  
✔ Set up the **Product Model, Controller, and View**.  
✔ Start integrating **Bootstrap** for frontend styling.  

## ✨ Summary & Key Takeaways:
✅ **ASP.NET Core MVC** follows the **Model-View-Controller** pattern for a structured application.  
✅ We set up the **development environment** and created a new **MVC project**.  
✅ We explored the **default project structure** and understood the **request lifecycle**.  
🚀 **Next, we start building our real-world e-commerce application!**

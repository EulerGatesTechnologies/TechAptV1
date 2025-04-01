# Setup Guide for Technical Aptitude Assessment Project

## Introduction

Welcome to the setup guide for the Technical Aptitude Assessment project. This document provides step-by-step instructions to set up and run the project on your local machine. The project is a Blazor Server application built with .NET 8, utilizing SQLite for data storage. It demonstrates key programming principles, threading, data persistence, and file downloads.

## Prerequisites

To set up and run this project, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) - Required for building and running the application.
- [Visual Studio 2022 Community Edition](https://visualstudio.microsoft.com/vs/community/) - Optional but recommended for development.
- Git - Necessary for cloning the repository.

**Note:** SQLite is used for data storage and is included within the project dependencies, requiring no separate installation.

Additionally, ensure you have a modern browser (e.g., Chrome, Firefox, Edge) to access the Blazor application.

## Installation Steps

Follow these steps to prepare the project:

1. **Clone the Repository**

   Clone the repository to your local machine:

   ```bash
   git clone https://github.com/EulerGatesTechnologies/TechAptV1.git
   cd TechAptV1
   ```
2. **Restore NuGet Packages

    Restore the project dependencies:
     ```bash
       dotnet ef database update
     ```
     This command creates the Number table and generates the SQLite database file in the project directory.

   Configuration
   The SQLite database is automatically configured when you run dotnet ef database update. No additional configuration is required.

   Running the Project
To run the application, use one of the following methods:

Using the Command Line:
```bash
dotnet run --project TechAptV1.Client
```

Below is the complete content for your `setup.md` file, which you can download and add to your GitHub project as a covering page. This guide provides clear, step-by-step instructions to set up and run your Technical Aptitude Assessment project, built with .NET 8, Blazor Server, and SQLite. Simply copy the text below into a file named `setup.md` and place it in the root of your GitHub repository.

```markdown
# Setup Guide for Technical Aptitude Assessment Project

## Introduction

Welcome to the setup guide for the Technical Aptitude Assessment project. This document provides step-by-step instructions to set up and run the project on your local machine. The project is a Blazor Server application built with .NET 8, utilizing SQLite for data storage. It demonstrates key programming principles, threading, data persistence, and file downloads.

## Prerequisites

To set up and run this project, ensure you have the following installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) - Required for building and running the application.
- [Visual Studio 2022 Community Edition](https://visualstudio.microsoft.com/vs/community/) - Optional but recommended for development.
- Git - Necessary for cloning the repository.

**Note:** SQLite is used for data storage and is included within the project dependencies, requiring no separate installation.

Additionally, ensure you have a modern browser (e.g., Chrome, Firefox, Edge) to access the Blazor application.

## Installation Steps

Follow these steps to prepare the project:

1. **Clone the Repository**

   Clone the repository to your local machine:

   ```bash
   git clone https://github.com/your-username/your-repo-name.git
   cd your-repo-name
   ```

   Replace `your-username` and `your-repo-name` with your GitHub username and repository name.

2. **Restore NuGet Packages**

   Restore the project dependencies:

   ```bash
   dotnet restore
   ```

3. **Update the Database**

   Apply the database migrations to create the SQLite database:

   ```bash
   dotnet ef database update
   ```

   This command creates the `Number` table and generates the SQLite database file in the project directory.

## Configuration

The SQLite database is automatically configured when you run `dotnet ef database update`. No additional configuration is required.

## Running the Project

To run the application, use one of the following methods:

- **Using the Command Line:**

  ```bash
  dotnet run --project TechAptV1.Client
  ```

- **Using Visual Studio:**

  Open the solution in Visual Studio, set `TechAptV1.Client` as the startup project, and press `F5`.

The application will be accessible at `https://localhost:5001` by default.

## Testing

The project includes unit tests and integration tests. To run the tests:

- **Using the Command Line:**

  ```bash
  dotnet test
  ```

- **Using Visual Studio:**

  Navigate to `Test > Test Explorer`, then run all tests from the Test Explorer window.

## Notes

- Asynchronous methods have been renamed to append 'Async' to their names (e.g., `Save` becomes `SaveAsync`), following .NET conventions.
- Unit and integration tests are included within the main project, organized by namespaces, for simplicity and logical structuring.

## Troubleshooting

If you encounter issues:

- Ensure the .NET 8 SDK is installed correctly by running `dotnet --version`.
- Verify that the database migrations have been applied using `dotnet ef database update`.
- In Visual Studio, confirm that `TechAptV1.Client` is set as the startup project.
- Check that your browser is compatible with Blazor Server applications.
```

To add this to your GitHub project:
1. Copy the text above.
2. Create a new file in your repository named `setup.md` (either via GitHub's web interface or locally).
3. Paste the content into the file and save or commit it.

This `setup.md` will serve as a comprehensive covering page for your project, guiding users through the setup process while highlighting key nuances like the async method naming and test organization. Let me know if you need any adjustments!

Nuances and Notes
The following details highlight specific changes and considerations for this project:

Async Method Naming Convention: Methods utilizing asynchronous programming have been renamed to append "Async" to their names, aligning with .NET conventions. For example, Save is now SaveAsync. This enhances clarity and distinguishes async operations from their synchronous counterparts.
Database Location: The SQLite database file is automatically created in the project directory upon running dotnet ef database update. No manual configuration of the database path is required.
Threading Implementation: Task 1 incorporates multi-threading to compute and manage a shared global variable. The setup requires no additional configuration beyond the standard .NET runtime, which supports multi-threading natively.
Test Organization: For simplicity, unit and integration tests reside within the main project, separated by namespaces rather than distinct test projects. This design choice reduces complexity while maintaining logical structure.
File Downloads: The Results page allows downloading data as XML or binary files. When triggered, these downloads prompt the browser to save the files locally, requiring no additional setup.
Out-of-the-Box Compatibility: The project is designed to compile and run without modifications on a vanilla install of Visual Studio 2022 Community Edition, adhering to the evaluation criteria specified in the original readme.md.
   

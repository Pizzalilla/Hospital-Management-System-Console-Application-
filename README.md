# Hospital Management System (Console Application)

## Overview
This project implements a simplified **Hospital Management System** as a C# console application. It was developed as part of an academic assignment to demonstrate the application of **object-oriented programming (OOP) principles** in a structured and maintainable way.  

The system provides role-based access for **Admins**, **Doctors**, and **Patients**, with JSON-based persistence for all core entities. While simplified compared to a real hospital system, it models essential interactions such as maintaining patient and doctor records, managing appointments, and supporting administrative oversight.

---

## Objectives
- Apply and demonstrate **OOP principles** including inheritance, constructors, method overriding, method overloading, extension methods, and garbage collection.  
- Design a clear **modular architecture** with separation of concerns.  
- Provide a consistent and user-friendly interface in a **console environment**.  
- Ensure persistent storage of data using lightweight JSON files.  

---

## System Design

### Architecture
The project follows a layered structure:  

- **Models** → Core domain entities (`User`, `Doctor`, `Patient`, `Admin`, `Appointment`).  
- **Services** → Business logic, including authentication (`AuthService`) and file operations (`FileManager`).  
- **Menus** → Role-specific menu handlers for Admin, Doctor, and Patient interactions.  
- **Extensions** → Console helpers (`ConsoleExtensions`) and string utilities (`StringExtensions`) for improved user interface and code reuse.  

### Data Management
Data is stored in JSON format to provide persistence without a database. This ensures the system can be executed and tested easily in a lightweight environment.  

---

## Demonstration of OOP Principles
The assignment rubric requires evidence of specific OOP features. These are explicitly demonstrated in the project:

- **Inheritance**:  
  `Doctor`, `Patient`, and `Admin` all inherit common attributes and methods from the abstract base class `User`.

- **Constructors**:  
  Each model defines constructors for flexible initialization, including parameterized versions for loading from persistent data.

- **Method Overriding**:  
  `ToString()` is overridden in multiple model classes to provide clear and role-specific string representations.

- **Method Overloading**:  
  The `Pause()` method in `ConsoleExtensions` is overloaded to allow both a default pause and a custom message.

- **Extension Methods**:  
  The `ToTitleCase()` extension method extends the built-in `string` class, improving readability and demonstrating language-level extensibility.

- **Garbage Collection**:  
  An Admin-accessible demonstration explicitly triggers `GC.Collect()` and reports memory usage before and after, providing observable evidence of garbage collection.

---

## Usage
1. Launch the application via `dotnet run`.  
2. Log in with credentials from the JSON data files (Admins, Doctors, Patients).  
3. The system will present a role-specific menu with options tailored to that role.  
4. All changes (e.g., new appointments, patient-doctor assignments) are persisted back into JSON for subsequent runs.

---

## Conclusion
This project provides a compact yet comprehensive demonstration of object-oriented programming in C#. Through role-based workflows and persistent data management, it balances practical functionality with academic objectives.  

The modular design, clean console interface, and explicit mapping to OOP principles ensure that the assignment requirements are fully satisfied while maintaining professional coding standards.

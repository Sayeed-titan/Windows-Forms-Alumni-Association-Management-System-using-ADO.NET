# Alumni Association Management System

![C#](https://img.shields.io/badge/Language-C%23-blue)
![Windows Forms](https://img.shields.io/badge/Framework-Windows%20Forms-green)
![ADO.NET](https://img.shields.io/badge/Database-ADO.NET-orange)

---

## Table of Contents
- [Project Overview](#project-overview)
- [Features](#features)
- [Screenshots / Demo](#screenshots--demo)
- [Usage](#usage)
- [Future Improvements](#future-improvements)
- [License](#license)

---

## Project Overview

The **Alumni Association Management System** is a Windows Forms desktop application designed to help alumni associations manage their members efficiently.  

It provides **full CRUD functionality**, **image management**, **form validation**, and **user-friendly notifications**. Administrators can add, update, delete, and view alumni records with photos, graduation year, email, and phone number while ensuring data integrity and a smooth user experience.  

---

## Features

### Core Features
- **Alumni Management**: Add, update, delete alumni records with photo support.  
- **Form Validation**: Ensures required fields are filled before saving.  
- **Dynamic UI**:
  - Add button switches to Update mode when a row is selected.  
  - Cancel button resets the form during updates.  
  - Clear button appears only when textboxes or photo are populated.  
  - Delete button visible only when a row is selected.
- **Notifications**: Snackbar-style messages for Add, Update, and Delete actions.  
- **DataGridView Enhancements**:
  - Photos displayed directly in the grid.  
  - Auto-fit columns and increased row height for better readability.  
- **File Management**: Photos stored in local `Images` folder within the project.  

### Technical Details
- Built with **C# Windows Forms**.  
- Database operations handled using **ADO.NET**.  
- Images stored in a local folder, with safe path management.  
- Proper handling of selection events and empty rows to prevent errors.

---

## Screenshots / Demo

### Form Layout

### DataGridView with Photos

### Snackbar Notifications



---

## Usage

1. **Clone the repository:**
   ```bash
   git clone https://github.com/YourUsername/AlumniAssociationApp.git

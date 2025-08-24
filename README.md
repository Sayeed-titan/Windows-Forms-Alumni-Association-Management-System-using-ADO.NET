# Alumni Association Management System

**Technologies:**  
C# Windows Forms, ADO.NET, SQL Server, DataGridView, PictureBox  

---

## Project Overview

Alumni Association Management System is a desktop application designed to help alumni associations manage their members efficiently. It provides full **CRUD functionality** with **image support**, **form validation**, and **user-friendly notifications**.  

The system allows administrators to add, update, delete, and view alumni information, including photos, graduation year, email, and phone number. It ensures data integrity with proper validations and prevents errors on blank rows in the grid.  

---

## Features

### Core Features
- **Alumni Management:** Add, update, delete alumni records with photo support.
- **Form Validation:** Required fields must be filled before saving.
- **Dynamic UI:** 
  - Add button switches to Update mode on row selection.  
  - Cancel button resets the form during updates.  
  - Clear button appears only when textboxes or photo are populated.  
  - Delete button visible only when a row is selected.
- **Notifications:** Snackbar-style messages for Add, Update, and Delete actions.
- **DataGridView:** 
  - Photos displayed directly in the grid.  
  - Auto-fit columns and increased row height for images.
- **File Management:** Photos saved in local `Images` folder inside the project.

### Technical Details
- Uses **ADO.NET** for database connectivity.  
- Stores images in local folder with proper path management.  
- Handles selection and updates safely to prevent errors.  

---

## Future Improvements
- **Search & Filter:** Filter alumni by name, year, or email.  
- **Export & Reporting:** Export alumni list to PDF or Excel; add printable reports.  
- **Role-Based Access:** Admin vs. Staff privileges.  
- **Database Enhancements:** Store images as BLOBs; add audit logs.  
- **UI Enhancements:** Modern, responsive interface.  
- **Logging & Notifications:** Persistent logs of actions and improved notifications.

---

## Screenshots
*(Add your screenshots here to showcase the form and grid)*

---

## Usage
1. Clone the repository:  
   ```bash
   git clone https://github.com/YourUsername/AlumniAssociationApp.git

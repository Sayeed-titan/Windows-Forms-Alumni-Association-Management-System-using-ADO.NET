CREATE DATABASE AlumniDB;
GO
USE AlumniDB;
GO

CREATE TABLE Alumni (
    AlumniID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100),
    GraduationYear INT,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
	PhotoPath NVARCHAR(200) NULL;

);

CREATE TABLE Events (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(100),
    EventDate DATE,
    Description NVARCHAR(500)
);

CREATE TABLE Attendance (
    AttendanceID INT IDENTITY(1,1) PRIMARY KEY,
    AlumniID INT FOREIGN KEY REFERENCES Alumni(AlumniID),
    EventID INT FOREIGN KEY REFERENCES Events(EventID)
);

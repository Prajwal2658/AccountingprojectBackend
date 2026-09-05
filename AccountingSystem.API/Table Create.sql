CREATE TABLE Users
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    FullName NVARCHAR(200) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT UQ_Users_Username UNIQUE (Username)
);
GO

CREATE TABLE Roles
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,

    CONSTRAINT UQ_Roles_Name UNIQUE (Name)
);
GO

CREATE TABLE UserRoles
(
    UserId INT NOT NULL,
    RoleId INT NOT NULL,

    CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId),

    CONSTRAINT FK_UserRoles_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id),

    CONSTRAINT FK_UserRoles_Roles
        FOREIGN KEY (RoleId)
        REFERENCES Roles(Id)
);
GO

---default user
INSERT INTO Roles (Name)
VALUES ('Admin');
INSERT INTO Roles (Name)
VALUES 
    ('Manager'),
    ('Accountant'),
    ('Sales'),
    ('Purchase'),
    ('User');


INSERT INTO Users
(
    Username,
    PasswordHash,
    FullName,
    IsActive
)
VALUES
(
    'admin',
    '$2a$11$JENtwlgz1sXI0tCRWfnRqOgGOq3zINlSJEQyFAlV5yVctJfCEUvFO',
    'System Administrator',
    1
);


INSERT INTO UserRoles (UserId, RoleId)
SELECT
    u.Id,
    r.Id
FROM Users u
CROSS JOIN Roles r
WHERE u.Username = 'admin'
  AND r.Name = 'Admin';

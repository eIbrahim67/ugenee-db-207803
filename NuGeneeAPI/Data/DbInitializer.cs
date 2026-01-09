using Microsoft.EntityFrameworkCore;
using NuGeneeAPI.Models.Entities;

namespace NuGeneeAPI.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            // 1. Ensure DB exists
            await context.Database.EnsureCreatedAsync();

            // 2. Force create missing tables (Safety check for EnsureCreated limitations)
            var createTablesSql = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CourseEnrollmentRequests')
                BEGIN
                    CREATE TABLE CourseEnrollmentRequests (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        FirstName NVARCHAR(MAX) NOT NULL,
                        LastName NVARCHAR(MAX) NOT NULL,
                        Email NVARCHAR(MAX) NULL,
                        Phone NVARCHAR(MAX) NOT NULL,
                        DateOfBirth NVARCHAR(MAX) NULL,
                        Gender NVARCHAR(MAX) NOT NULL,
                        EducationLevel NVARCHAR(MAX) NOT NULL,
                        TermsAccepted BIT NOT NULL,
                        IsContacted BIT NOT NULL DEFAULT 0,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                        CourseId INT NOT NULL,
                        CONSTRAINT FK_CourseEnrollmentRequests_Courses FOREIGN KEY (CourseId) REFERENCES Courses(Id) ON DELETE CASCADE
                    );
                END
                ELSE
                BEGIN
                    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CourseEnrollmentRequests') AND name = 'Email')
                        ALTER TABLE CourseEnrollmentRequests ALTER COLUMN Email NVARCHAR(MAX) NULL;
                    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('CourseEnrollmentRequests') AND name = 'DateOfBirth')
                        ALTER TABLE CourseEnrollmentRequests ALTER COLUMN DateOfBirth NVARCHAR(MAX) NULL;
                END

                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'LearningPathEnrollmentRequests')
                BEGIN
                    CREATE TABLE LearningPathEnrollmentRequests (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        FirstName NVARCHAR(MAX) NOT NULL,
                        LastName NVARCHAR(MAX) NOT NULL,
                        Email NVARCHAR(MAX) NULL,
                        Phone NVARCHAR(MAX) NOT NULL,
                        DateOfBirth NVARCHAR(MAX) NULL,
                        Gender NVARCHAR(MAX) NOT NULL,
                        EducationLevel NVARCHAR(MAX) NOT NULL,
                        TermsAccepted BIT NOT NULL,
                        IsContacted BIT NOT NULL DEFAULT 0,
                        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                        PathId INT NOT NULL,
                        CONSTRAINT FK_LearningPathEnrollmentRequests_LearningPaths FOREIGN KEY (PathId) REFERENCES LearningPaths(Id) ON DELETE CASCADE
                    );
                END
                ELSE
                BEGIN
                    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LearningPathEnrollmentRequests') AND name = 'Email')
                        ALTER TABLE LearningPathEnrollmentRequests ALTER COLUMN Email NVARCHAR(MAX) NULL;
                    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LearningPathEnrollmentRequests') AND name = 'DateOfBirth')
                        ALTER TABLE LearningPathEnrollmentRequests ALTER COLUMN DateOfBirth NVARCHAR(MAX) NULL;
                END
            ";

            await context.Database.ExecuteSqlRawAsync(createTablesSql);
        }
    }
}

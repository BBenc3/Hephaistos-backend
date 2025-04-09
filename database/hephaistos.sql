DROP DATABASE IF EXISTS `Hephaistos-database`;
CREATE DATABASE `Hephaistos-database`;
USE `Hephaistos-database`;

SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS auditlog;
DROP TABLE IF EXISTS classschedules;
DROP TABLE IF EXISTS completedsubjects;
DROP TABLE IF EXISTS refreshtokens;
DROP TABLE IF EXISTS SubjectPrerequisites;
DROP TABLE IF EXISTS subjects;
DROP TABLE IF EXISTS generatedtimetables;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS majors;
DROP TABLE IF EXISTS universities;

SET FOREIGN_KEY_CHECKS = 1;

CREATE TABLE universities (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NULL,
    Place VARCHAR(255) NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    Active TINYINT(1) NOT NULL DEFAULT 1,
    Note TEXT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE majors (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL DEFAULT '',
    UniversityId INT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    Active TINYINT(1) NOT NULL DEFAULT 1,
    Note TEXT NULL,
    CONSTRAINT fk_majors_universities FOREIGN KEY (UniversityId) REFERENCES universities(Id)
        ON UPDATE CASCADE
        ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(255) NOT NULL DEFAULT '',
    PasswordHash VARCHAR(255) NULL,
    Role VARCHAR(255) NOT NULL DEFAULT '',
    Email VARCHAR(255) NOT NULL DEFAULT '',
    StartYear INT NOT NULL DEFAULT 0,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    Active TINYINT(1) NOT NULL DEFAULT 1,
    Note TEXT NULL,
    MajorId INT NULL,
    ProfilePicturepath VARCHAR(255) NOT NULL DEFAULT '',
    Status VARCHAR(50) NULL,
    CONSTRAINT fk_users_major FOREIGN KEY (MajorId) REFERENCES majors(Id)
         ON UPDATE CASCADE
         ON DELETE SET NULL,
    KEY idx_users_major (MajorId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE generatedtimetables (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NOT NULL,
    Name VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    Active TINYINT(1) NOT NULL DEFAULT 1,
    CONSTRAINT fk_generatedtimetables_users FOREIGN KEY (UserId) REFERENCES users(Id)
         ON UPDATE CASCADE
         ON DELETE CASCADE,
    KEY idx_generatedtimetables_user (UserId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE subjects (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NULL,
    Code VARCHAR(255) NULL,
    CreditValue INT NULL,
    MajorId INT NULL,
    IsElective TINYINT(1) NOT NULL DEFAULT 0,
    IsEvenSemester TINYINT(1) NOT NULL DEFAULT 0,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    Active TINYINT(1) NOT NULL DEFAULT 1,
    Note TEXT NULL,
    GeneratedTimetableId INT NULL,
    CONSTRAINT fk_subjects_majors FOREIGN KEY (MajorId) REFERENCES majors(Id)
         ON UPDATE CASCADE
         ON DELETE SET NULL,
    CONSTRAINT fk_subjects_generatedtimetables FOREIGN KEY (GeneratedTimetableId) REFERENCES generatedtimetables(Id)
         ON UPDATE CASCADE
         ON DELETE SET NULL,
    KEY idx_subjects_GeneratedTimetableId (GeneratedTimetableId),
    KEY idx_subjects_MajorId (MajorId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE SubjectPrerequisites (
    PrerequisiteId INT NOT NULL,
    SubjectId INT NOT NULL,
    PRIMARY KEY (PrerequisiteId, SubjectId),
    CONSTRAINT fk_sp_SubjectId FOREIGN KEY (SubjectId) REFERENCES subjects(Id)
         ON UPDATE CASCADE
         ON DELETE NO ACTION,
    CONSTRAINT fk_sp_PrerequisiteId FOREIGN KEY (PrerequisiteId) REFERENCES subjects(Id)
         ON UPDATE CASCADE
         ON DELETE CASCADE,
    KEY idx_sp_SubjectId (SubjectId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE completedsubjects (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    UserId INT NULL,
    SubjectId INT NULL,
    Active TINYINT(1) NOT NULL DEFAULT 1,
    CONSTRAINT fk_completedsubjects_users FOREIGN KEY (UserId) REFERENCES users(Id)
         ON UPDATE CASCADE
         ON DELETE SET NULL,
    CONSTRAINT fk_completedsubjects_subjects FOREIGN KEY (SubjectId) REFERENCES subjects(Id)
         ON UPDATE CASCADE
         ON DELETE SET NULL,
    KEY idx_completedsubjects_UserId (UserId),
    KEY idx_completedsubjects_SubjectId (SubjectId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE classschedules (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    SubjectId INT NULL,
    Year INT NULL,
    DayOfWeek VARCHAR(20) NULL,
    StartTime TIME NULL,
    EndTime TIME NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    active TINYINT(1) NOT NULL DEFAULT 1,
    GeneratedTimetableId INT NULL,
    CONSTRAINT fk_classschedules_subjects FOREIGN KEY (SubjectId) REFERENCES subjects(Id)
         ON UPDATE CASCADE
         ON DELETE CASCADE,
    CONSTRAINT fk_classschedules_generatedtimetables FOREIGN KEY (GeneratedTimetableId) REFERENCES generatedtimetables(Id)
         ON UPDATE CASCADE
         ON DELETE SET NULL,
    KEY idx_classschedules_SubjectId (SubjectId),
    KEY idx_classschedules_GeneratedTimetableId (GeneratedTimetableId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE refreshtokens (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Token TEXT NULL,
    Expires DATETIME NULL,
    Created DATETIME NOT NULL DEFAULT '1000-01-01 00:00:00',
    Revoked DATETIME NULL,
    UserId INT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    Active TINYINT(1) NOT NULL DEFAULT 1,
    CONSTRAINT fk_refreshtokens_users FOREIGN KEY (UserId) REFERENCES users(Id)
         ON UPDATE CASCADE
         ON DELETE SET NULL,
    KEY idx_refreshtokens_UserId (UserId)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE auditlog (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    TableName VARCHAR(255) NULL,
    RecordId INT NULL,
    OperationType VARCHAR(50) NULL,
    ChangedData JSON NULL,
    ChangedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    ChangedBy INT NULL,
    CONSTRAINT fk_auditlog_users FOREIGN KEY (ChangedBy) REFERENCES users(Id)
         ON UPDATE CASCADE
         ON DELETE SET NULL,
    KEY idx_auditlog_ChangedBy (ChangedBy)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

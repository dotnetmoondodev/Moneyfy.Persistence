IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Expenses] (
    [Id] uniqueidentifier NOT NULL,
    [Description] nvarchar(128) NOT NULL,
    [CreationDate] datetimeoffset NOT NULL,
    [Value] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Expenses] PRIMARY KEY ([Id])
);

CREATE TABLE [Incomes] (
    [Id] uniqueidentifier NOT NULL,
    [Source] nvarchar(128) NOT NULL,
    [Withholding] decimal(18,2) NOT NULL,
    [Description] nvarchar(128) NOT NULL,
    [CreationDate] datetimeoffset NOT NULL,
    [Value] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Incomes] PRIMARY KEY ([Id])
);

CREATE TABLE [Notifications] (
    [Id] uniqueidentifier NOT NULL,
    [DateToSend] datetimeoffset NOT NULL,
    [HourToSend] int NOT NULL,
    [Frequency] int NOT NULL,
    [Method] int NOT NULL,
    [PaymentId] uniqueidentifier NULL,
    [Repeatable] bit NOT NULL,
    [Enable] bit NOT NULL,
    [Email] nvarchar(128) NULL,
    [PhoneNumber] nvarchar(32) NULL,
    [Description] nvarchar(128) NOT NULL,
    [CreationDate] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id])
);

CREATE TABLE [Payments] (
    [Id] uniqueidentifier NOT NULL,
    [Currency] int NOT NULL,
    [IsAutoDebit] bit NOT NULL,
    [PaymentMediaReference] nvarchar(128) NOT NULL,
    [Description] nvarchar(128) NOT NULL,
    [CreationDate] datetimeoffset NOT NULL,
    [Value] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250609011035_CreateTables', N'9.0.5');

COMMIT;
GO


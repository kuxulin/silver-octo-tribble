CREATE TABLE [dbo].[Managers]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY CLUSTERED ([Id] ASC), 
    [FullName] NVARCHAR(max) NOT NULL, 
    [PhoneNumber] NVARCHAR(max) NOT NULL, 
    [CreationDate] DATETIME NULL
)

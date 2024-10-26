CREATE TABLE [dbo].[TaskStatuses]
(
	[Id] INT NOT NULL IDENTITY,
	[Name] NVARCHAR(50) NOT NULL, 

    [CreationDate] DATETIME2 NULL, 
    CONSTRAINT [PK_TaskStatuses] PRIMARY KEY CLUSTERED ([Id] ASC), 
)

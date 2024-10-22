CREATE TABLE [dbo].[TodoTaskStatuses]
(
	[Id] INT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL, 

    [CreationDate] DATETIME2 NULL, 
    CONSTRAINT [PK_TodoTaskStatuses] PRIMARY KEY CLUSTERED ([Id] ASC), 
)

CREATE TABLE [dbo].[Changes]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [CreatorId] INT NULL, 
    [TaskId] UNIQUEIDENTIFIER NULL, 
    [ActionType] NVARCHAR(50) NOT NULL, 
    [CreationDate] DATETIME2 NOT NULL,
    [TaskTitle] NVARCHAR(50) NULL, 
    [ProjectId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_Changes_Users] FOREIGN KEY ([CreatorId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Changes_Tasks] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[TodoTasks] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Changes_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([Id])  
)

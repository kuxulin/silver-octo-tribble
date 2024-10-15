CREATE TABLE [dbo].[ToDoTasks] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_TodoTasks_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Title]        NVARCHAR (30)    NOT NULL,
    [Text]         NVARCHAR (MAX)   NOT NULL,
    [ProjectId]    UNIQUEIDENTIFIER NOT NULL,
    [CreationDate] DATETIME2(7) NOT NULL,
    CONSTRAINT [PK_TodoTasks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TodoTasks_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([Id]) ON DELETE CASCADE
);


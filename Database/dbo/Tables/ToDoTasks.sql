CREATE TABLE [dbo].[TodoTasks] (
    [Id]           UNIQUEIDENTIFIER CONSTRAINT [DF_TodoTasks_Id] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Title]        NVARCHAR (30)    NOT NULL,
    [Text]         NVARCHAR (MAX)   NOT NULL,
    [ProjectId]    UNIQUEIDENTIFIER NOT NULL,
    [EmployeeId]   UNIQUEIDENTIFIER NULL,
    [StatusId]     INT              NOT NULL,
    [CreationDate] DATETIME2(7)     NOT NULL,
    CONSTRAINT [PK_TodoTasks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TodoTasks_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [dbo].[Projects] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TodoTasks_Employees] FOREIGN KEY ([EmployeeId]) REFERENCES [dbo].[Employees] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_TodoTasks_TaskStatuses] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[TaskStatuses] ([Id])
);


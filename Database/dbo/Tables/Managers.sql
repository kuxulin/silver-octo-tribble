CREATE TABLE [dbo].[Managers]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [CreationDate] DATETIME2(7) NOT NULL,
    [UserId]       int not null
    CONSTRAINT [PK_Managers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Managers_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
)

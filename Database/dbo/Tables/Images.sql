CREATE TABLE [dbo].[Images]
(
	[Id] UNIQUEIDENTIFIER NOT NULL, 
    [Name] NVARCHAR(50) NULL, 
    [Content] NVARCHAR(MAX) NULL, 
    [UserId] INT NOT NULL, 
    [CreationDate] DATETIME2(7) NOT NULL,
    CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Images_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,  
)

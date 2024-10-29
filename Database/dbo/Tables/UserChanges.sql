CREATE TABLE [dbo].[UserChanges]
(
	[UserId] int NOT NULL , 
    [ChangeId] UNIQUEIDENTIFIER NOT NULL, 
    [IsRead] BIT NOT NULL,
    CONSTRAINT [PK_UserChanges] PRIMARY KEY ([UserId], [ChangeId]),
    CONSTRAINT FK_UserChangeId FOREIGN KEY (UserId) 
    REFERENCES [dbo].[AspNetUsers](Id) on delete cascade,
    CONSTRAINT FK_ChangeUserId FOREIGN KEY (ChangeId) 
    REFERENCES [dbo].[Changes](Id) on delete cascade
)

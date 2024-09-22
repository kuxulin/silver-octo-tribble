CREATE TABLE [dbo].[ProjectsManagers]
(
	[ProjectId] UNIQUEIDENTIFIER NOT NULL , 
    [ManagerId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [PK_ProjectsManagers] PRIMARY KEY ([ManagerId], [ProjectId]),
    CONSTRAINT FK_ProjectManagerId FOREIGN KEY (ProjectId) 
    REFERENCES [dbo].[Projects](Id),
    CONSTRAINT FK_ManagerProjectId FOREIGN KEY (ManagerId) 
    REFERENCES [dbo].[Managers](Id)
)

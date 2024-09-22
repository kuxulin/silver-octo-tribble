CREATE TABLE [dbo].[ProjectsEmployees]
(
	[ProjectId] UNIQUEIDENTIFIER NOT NULL , 
    [EmployeeId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [PK_ProjectsEmployees] PRIMARY KEY ([EmployeeId], [ProjectId]),
    CONSTRAINT FK_ProjectEmployeeId FOREIGN KEY (ProjectId) 
    REFERENCES [dbo].[Projects](Id),
    CONSTRAINT FK_EmployeeProjectId FOREIGN KEY (EmployeeId) 
    REFERENCES [dbo].[Employees](Id)
)

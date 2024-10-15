﻿CREATE TABLE [dbo].[Managers]
(
	[Id] UNIQUEIDENTIFIER NOT NULL, 
    [FullName] NVARCHAR(100) NOT NULL, 
    [PhoneNumber] NVARCHAR(15) NOT NULL, 
    [CreationDate] DATETIME NULL,
    CONSTRAINT [PK_Managers] PRIMARY KEY CLUSTERED ([Id] ASC),
)

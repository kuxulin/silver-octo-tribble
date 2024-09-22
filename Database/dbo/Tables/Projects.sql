﻿CREATE TABLE [dbo].[Projects] (
    [Id]   UNIQUEIDENTIFIER NOT NULL,
    [Name] NVARCHAR (50)    NOT NULL,
    [CreationDate] DATETIME NOT NULL, 
    CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED ([Id] ASC)
);


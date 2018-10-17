CREATE TABLE [dbo].[Peoms] (
     [Id]       UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL
    ,[PeopleId] UNIQUEIDENTIFIER NOT NULL
    ,[Title]    NVARCHAR (50)    NULL
    ,[Content]  NVARCHAR (MAX)   NULL
    ,[Distance] FLOAT (53)       NULL
    ,CONSTRAINT [PK_Peoms] PRIMARY KEY CLUSTERED ([Id] ASC)
    ,CONSTRAINT FK_Peoms_Peoples FOREIGN KEY (PeopleId) REFERENCES dbo.Peoples (Id)
);


CREATE TABLE [dbo].[Peoples] (
     Id             UNIQUEIDENTIFIER DEFAULT (newsequentialid())  NOT NULL
    ,FirstName      NVARCHAR (50)    NOT NULL
    ,LastName       NVARCHAR (50)    NULL
    ,Gender         NVARCHAR (50)    NULL
    ,Qoute          NVARCHAR (MAX)   NULL
    ,City           NVARCHAR (50)    NULL
    ,Street         NVARCHAR (50)    NULL
    ,Email          NVARCHAR (50)    NULL
    ,PictureMedium  NVARCHAR (MAX)   NULL
    ,CONSTRAINT [PK_Peoples] PRIMARY KEY CLUSTERED ([Id] ASC)
);


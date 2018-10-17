CREATE PROCEDURE dbo.spSetPeom
(
   @UserId        UNIQUEIDENTIFIER
  ,@Title         NVARCHAR(50)  = NULL
  ,@Content       NVARCHAR(MAX) = NULL
  ,@Distance      FLOAT         = NULL
)
AS
BEGIN
  SET NOCOUNT ON;

  INSERT INTO Peoms (PeopleId, Title, Content, Distance)
  VALUES (@UserId, @Title, @Content, @Distance);

END

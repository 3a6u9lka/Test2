CREATE PROCEDURE [dbo].[spSetPeople]
(
   @FirstName     NVARCHAR(50)
  ,@LastName      NVARCHAR(50)  = NULL
  ,@Gender        NVARCHAR(50)  = NULL
  ,@Qoute         NVARCHAR(MAX) = NULL
  ,@City          NVARCHAR(50)  = NULL
  ,@Street        NVARCHAR(50)  = NULL
  ,@Email         NVARCHAR(50)  = NULL
  ,@PictureMedium NVARCHAR(MAX) = NULL
)
AS
BEGIN
  SET NOCOUNT ON;

  CREATE TABLE #tt (Id UNIQUEIDENTIFIER)

  INSERT INTO Peoples (FirstName, LastName, Gender, Qoute, City, Street, Email, PictureMedium)
  OUTPUT INSERTED.Id INTO #tt
  VALUES (@FirstName, @LastName, @Gender, @Qoute, @City, @Street, @Email, @PictureMedium);

  SELECT t.Id FROM #tt t

  IF OBJECT_ID('tempdb.dbo.#tt') IS NOT NULL
    DROP TABLE #tt 
  
END

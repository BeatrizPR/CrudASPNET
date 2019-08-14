USE [ASPCRUD]
GO

CREATE PROC ContactDeleteByID
@contactID int
AS 
BEGIN
	DELETE
	FROM Contact
	WHERE ContactID=@contactID
END

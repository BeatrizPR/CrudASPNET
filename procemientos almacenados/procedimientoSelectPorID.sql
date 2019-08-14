USE [ASPCRUD]
GO

CREATE PROC ContactViewByID
@contactID int
AS 
BEGIN
	SELECT *
	FROM Contact
	WHERE ContactID=@contactID
END

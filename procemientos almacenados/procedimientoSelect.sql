USE [ASPCRUD]
GO

CREATE PROC ContactViewAll
AS 
BEGIN
	SELECT *
	FROM Contact
END

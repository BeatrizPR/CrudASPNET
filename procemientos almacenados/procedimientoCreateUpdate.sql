USE [ASPCRUD]
GO

CREATE PROC ContactCreateOrUpdate
@ContactID int,
@Name nvarchar(50),
@Mobile nvarchar(50),
@Address nvarchar(250)

AS 

BEGIN

IF(@ContactID=0)
	BEGIN
	INSERT INTO Contact (Name,Mobile,Address)
	VALUES (@Name, @Mobile, @Address)
	END
ELSE
	BEGIN
	UPDATE Contact
	SET
		Name=@Name,
		Mobile = @Mobile,
		Address=@Address
	WHERE ContactID= @ContactID
	END
END





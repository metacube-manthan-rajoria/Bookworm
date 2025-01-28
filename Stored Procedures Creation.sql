-- Update Procedure
CREATE PROCEDURE updateCategories @id INT, @name VARCHAR, @displayOrder INT
AS
BEGIN
	UPDATE Categories SET Name = @name, DisplayOrder = @displayOrder WHERE Id = @id
END

-- Select Procedure
CREATE PROCEDURE selectCategories
AS
BEGIN
	SELECT * FROM Categories
END

-- Delete Procedure
CREATE PROCEDURE deleteCategories @id INT
AS
BEGIN
	DELETE FROM Categories WHERE Id = @id
END

-- Insert Procedure
CREATE PROCEDURE insertCategories @id INT, @name VARCHAR, @displayOrder INT
AS
BEGIN
	INSERT INTO Categories(Name, DisplayOrder) VALUES(@name, @displayOrder)
END


-- Using Procedure
EXEC updateBookReturnDate @id = 15, @date = '2025-01-20';
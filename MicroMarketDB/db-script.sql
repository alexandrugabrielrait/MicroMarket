CREATE DATABASE MicroMarket;
GO

USE MicroMarket;
GO

CREATE TABLE ProductTypes
(
	ProductTypeId INT NOT NULL,
	Name NVARCHAR(300) NOT NULL,
    UNIQUE(ProductTypeId)
)

INSERT INTO ProductTypes
VALUES
(0, 'Development Board'),
(1, 'Wires'),
(2, 'Resistor')
GO

CREATE TABLE Products
(
    ProductId INT NOT NULL,
	Name NVARCHAR(300) NOT NULL,
	Price DECIMAL(18, 5) NOT NULL,
	ProductTypeId INT NOT NULL,
	Description NVARCHAR(MAX),
	Quantity INT NOT NULL,
    UNIQUE(ProductId),
	FOREIGN KEY (ProductTypeId) REFERENCES ProductTypes(ProductTypeId),
	CHECK (Quantity >= 0)
)

INSERT INTO Products
VALUES
(1, 'Board', 32.3, 0, 'Some board', 100),
(5, 'Resistor', 44.3, 2, NULL, 50),
(6, 'Wire', 1, 1, 'Some wires', 5),
(7, 'Easy Board', 5, 0, 'Easier board than some board', 3),
(9, 'Male-Female Wires', 1, 0, 'Multi wires', 1000),
(11, 'Male-Male Wires', 1, 0, 'Multiple wires', 1000)
GO

CREATE PROCEDURE DecreaseStock (@ProductId INT, @Quantity INT)
AS
	UPDATE Products SET Quantity = Quantity - @Quantity WHERE ProductId = @ProductId
GO

CREATE FUNCTION GetProductType (@ProductId INT) RETURNS TABLE
AS
RETURN
	SELECT * FROM ProductTypes WHERE ProductTypeId = (SELECT ProductTypeId from Products WHERE ProductId = @ProductId);
GO
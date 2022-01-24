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
GO

INSERT INTO ProductTypes
VALUES
(0, 'Development Board'),
(1, 'Wires'),
(2, 'Circuit Component'),
(3, 'Buttons'),
(4, 'Displays')
GO

CREATE TABLE Products
(
    ProductId INT NOT NULL,
	Name NVARCHAR(300) NOT NULL,
	Price DECIMAL(18, 5) NOT NULL,
	ProductTypeId INT NOT NULL,
	Stock INT NOT NULL,
	ImageSource VARCHAR(MAX),
	Description NVARCHAR(MAX),
    UNIQUE(ProductId),
	FOREIGN KEY (ProductTypeId) REFERENCES ProductTypes(ProductTypeId),
	CHECK (Stock >= 0)
)
GO

INSERT INTO Products
VALUES
(1, 'PLACĂ DEZVOLTARE ALTERA DEO-NANO P0082', 32.3, 0, 100, 'https://cleste.ro/6794-superlarge_default/placa-dezvoltare-altera-deo-nano-p0082.jpg', 'P0082 (Terasic) este o placă de dezvoltare DE0-Nano este o platformă de dezvoltare FPGA de dimensiuni compacte, potrivită pentru proiectarea circuitelor de prototipare, cum ar fi roboți și proiecte „portabile”.'),
(2, 'Resistor', 44.3, 2, 50, NULL, NULL),
(3, 'Wire', 1, 1, 5, NULL, 'Some wires'),
(4, 'PLACĂ DEZVOLTARE NUCLEO-L010RB', 5, 0, 3, 'https://cleste.ro/7934-superlarge_default/placa-dezvoltare-nucleo-l010rb.jpg', 'Placa de dezvoltare STM32 Nucleo-64 cu STM32L010RB MCU acceptă conectivitatea morpho Arduino și ST.'),
(5, '10 X FIRE DUPONT MAMA-TATA 10CM', 1, 0, 1000, 'https://cleste.ro/1875-superlarge_default/10-x-fire-dupont-mama-tata-10cm.jpg', '10 x fire mamă-tată de 10 cm, ideale pentru a conecta rapid module la plăcile de dezvoltare.'),
(6, '10 X FIRE DUPONT TATA-TATA 30CM', 1, 0, 1000, 'https://cleste.ro/381-superlarge_default/10xfire-dupont-tata-tata-30cm.jpg', '10 x fire tata-tata de 30 cm, ideale pentru a conecta rapid module la plăcile de dezvoltare.')
GO

CREATE TABLE Transactions
(
	TransactionId INT NOT NULL IDENTITY(1,1),
    UserId INT NOT NULL,
	TransactionTime DATETIME NOT NULL,
    PRIMARY KEY (TransactionId)
)
GO

CREATE TABLE TransactionProduct
(
	TransactionId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    FOREIGN KEY (TransactionId) REFERENCES Transactions(TransactionId),
	FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
	CHECK (Quantity >= 1)
)
GO

CREATE PROCEDURE DecreaseStock (@ProductId INT, @Quantity INT)
AS
	UPDATE Products SET Stock = Stock - @Quantity WHERE ProductId = @ProductId
GO

CREATE FUNCTION GetProductType (@ProductId INT) RETURNS TABLE
AS
RETURN
	SELECT * FROM ProductTypes WHERE ProductTypeId = (SELECT ProductTypeId from Products WHERE ProductId = @ProductId);
GO
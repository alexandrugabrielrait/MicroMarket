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
(4, 'Displays'),
(5, 'Breadboard')
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
    UNIQUE (ProductId),
	FOREIGN KEY (ProductTypeId) REFERENCES ProductTypes(ProductTypeId),
	CHECK (Stock >= 0)
)
GO

INSERT INTO Products
VALUES
(1, 'PLACĂ DEZVOLTARE ALTERA DEO-NANO P0082', 32.3, 0, 1, 'https://cleste.ro/6794-superlarge_default/placa-dezvoltare-altera-deo-nano-p0082.jpg', N'P0082 (Terasic) este o placă de dezvoltare DE0-Nano este o platformă de dezvoltare FPGA de dimensiuni compacte, potrivită pentru proiectarea circuitelor de prototipare, cum ar fi roboți și proiecte „portabile”.'),
(2, 'BREADBOARD 400 PUNCTE', 9, 5, 2, 'https://cleste.ro/160-superlarge_default/breadboard-400-puncte.jpg', N'Breadboard-ul este folosit în general pentru realizarea rapidă a montajelor fără a fi nevoie de lipirea firelor, pentru testarea proiectelor.'),
(3, 'DISPLAY WTVGA MAXTOUCH DE 4,3 \"', 600, 4, 3, 'https://cleste.ro/7910-superlarge_default/display-wtvga-maxtouch-de-4-3.jpg', N'Producător de siliciu :Microchip'),
(4, 'PLACĂ DEZVOLTARE NUCLEO-L010RB', 5, 0, 4, 'https://cleste.ro/7934-superlarge_default/placa-dezvoltare-nucleo-l010rb.jpg', N'Placa de dezvoltare STM32 Nucleo-64 cu STM32L010RB MCU acceptă conectivitatea morpho Arduino și ST.'),
(5, '10 X FIRE DUPONT MAMA-TATA 10CM', 1, 1, 5, 'https://cleste.ro/1875-superlarge_default/10-x-fire-dupont-mama-tata-10cm.jpg', N'10 x fire mamă-tată de 10 cm, ideale pentru a conecta rapid module la plăcile de dezvoltare.'),
(6, '10 X FIRE DUPONT TATA-TATA 30CM', 1, 1, 6, 'https://cleste.ro/381-superlarge_default/10xfire-dupont-tata-tata-30cm.jpg', N'10 x fire tata-tata de 30 cm, ideale pentru a conecta rapid module la plăcile de dezvoltare.'),
(7, 'BREADBOARD MINI 170 PUNCTE', 3, 5, 7, 'https://cleste.ro/11859-superlarge_default/breadboard-mini-170-puncte.jpg', N'Breadboard-ul este folosit în general pentru realizarea rapidă a montajelor fără a fi nevoie de lipirea firelor, pentru testarea proiectelor.')
GO

CREATE FUNCTION IsValidEmail(@Email varchar(255)) RETURNS BIT
AS
BEGIN
RETURN
CASE
	WHEN @Email = '' THEN 0
	WHEN @Email like '% %' THEN 0
	WHEN @Email like ('%["(),:;<>\]%') THEN 0
	WHEN substring(@Email,charindex('@',@Email),len(@Email)) like ('%[!#$%&*+/=?^`_{|]%') THEN 0
	WHEN (left(@Email,1) like ('[-_.+]') or right(@Email,1) like ('[-_.+]')) THEN 0                                                                                    
	WHEN (@Email like '%[%' or @Email like '%]%') THEN 0
	WHEN @Email LIKE '%@%@%' THEN 0
	WHEN @Email NOT LIKE '_%@_%._%' THEN 0
	ELSE 1
END
END 
GO

CREATE TABLE Users
(
    UserId UNIQUEIDENTIFIER  NOT NULL,
	Email VARCHAR(255),
    PRIMARY KEY (UserId),
	CHECK (dbo.IsValidEmail(Email) = 1)
)
GO

CREATE TABLE Transactions
(
	TransactionId UNIQUEIDENTIFIER NOT NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,
	TransactionTime DATETIME NOT NULL,
    PRIMARY KEY (TransactionId),
	FOREIGN KEY (UserId) REFERENCES Users(UserId)
)
GO

CREATE TABLE TransactionProducts
(
	TransactionProductId UNIQUEIDENTIFIER NOT NULL,
	TransactionId UNIQUEIDENTIFIER NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    PRIMARY KEY (TransactionProductId),
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

CREATE PROCEDURE PrintAssignment (@UserId UNIQUEIDENTIFIER, @Email VARCHAR(255))
AS
	IF dbo.IsValidEmail(@Email) = 1
		PRINT 'User ' + @Email + ' assigned unique ID: ' + CAST(@UserId AS VARCHAR(255));
	ELSE
		PRINT 'User with an invalid email (' + @Email + ') assigned unique ID: ' + CAST(@UserId AS VARCHAR(255));
GO

CREATE TRIGGER NewUser ON Users
FOR INSERT
AS
	DECLARE
		@UserId UNIQUEIDENTIFIER,
		@Email VARCHAR(255);
	DECLARE insert_cursor CURSOR FOR SELECT * FROM inserted;
	OPEN insert_cursor;
	FETCH NEXT FROM insert_cursor into @UserId, @Email;
	EXECUTE PrintAssignment @UserId, @Email;
	CLOSE insert_cursor;
	DEALLOCATE insert_cursor;
GO

CREATE or ALTER TRIGGER NewTransactionProduct ON TransactionProducts
AFTER INSERT
AS
	DECLARE
		@ProductId INT,
		@Name NVARCHAR(300),
		@Quantity INT,
		@TransactionId UNIQUEIDENTIFIER,
		@UserId UNIQUEIDENTIFIER,
		@TransactionTime DATETIME,
		@Email VARCHAR(255);
	DECLARE insert_cursor CURSOR FOR SELECT p.ProductId, p.Name, Quantity, t.TransactionId, u.UserId, TransactionTime, Email FROM inserted i
	join Transactions t on i.TransactionId = t.TransactionId
	join Users u on t.UserId = u.UserId
	join Products p on i.ProductId = p.ProductId;
	OPEN insert_cursor;
	FETCH NEXT FROM insert_cursor into @ProductId, @Name, @Quantity, @TransactionId, @UserId, @TransactionTime, @Email;
	PRINT 'User ' + @Email + ' bought the following ' + CAST(@Quantity AS VARCHAR(255)) + ' items of type ' + @Name + ' on ' + CAST(@TransactionTime AS VARCHAR(255));
	CLOSE insert_cursor;
	EXECUTE DecreaseStock @ProductId, @Quantity;
GO
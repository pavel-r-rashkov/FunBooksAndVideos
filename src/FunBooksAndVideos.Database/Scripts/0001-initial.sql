CREATE TABLE ProductTypes
(
    Id INT IDENTITY PRIMARY KEY,
    [Name] NVARCHAR(512) NOT NULL,
    IsPhysical BIT NOT NULL,
    RequiresActivation BIT NOT NULL,
)

CREATE TABLE Products
(
    Id INT IDENTITY PRIMARY KEY,
    [Name] NVARCHAR(512) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    ProductTypeId INT NOT NULL,
    FOREIGN KEY (ProductTypeId) REFERENCES ProductTypes(Id) ON DELETE CASCADE,
)

CREATE TABLE Customers
(
    Id INT IDENTITY PRIMARY KEY,
    [FirstName] NVARCHAR(512) NOT NULL,
    [LastName] NVARCHAR(512) NOT NULL,
)

CREATE TABLE Memberships
(
    Id INT IDENTITY PRIMARY KEY,
    CustomerId INT NOT NULL,
    ProductId INT NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
)

CREATE SEQUENCE OrderNumbers
    START WITH 1000000  
    INCREMENT BY 1;

CREATE TABLE Orders
(
    Id INT IDENTITY PRIMARY KEY,
    OrderNumber INT CONSTRAINT DF_OrderNumber DEFAULT NEXT VALUE FOR OrderNumbers,
    CustomerId INT NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id) ON DELETE CASCADE,
    Total DECIMAL(18, 2) NOT NULL,
)

CREATE TABLE OrderLineItems
(
    Id INT IDENTITY PRIMARY KEY,
    ProductId INT NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
    OrderId INT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    Quantity INT NOT NULL,
    Price DECIMAL NOT NULL,
)

CREATE TABLE ShippingSlips
(
    Id INT IDENTITY PRIMARY KEY,
    OrderId INT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
)

CREATE TABLE DeliveryItems
(
    Id INT IDENTITY PRIMARY KEY,
    ShippingSlipId INT NOT NULL,
    FOREIGN KEY (ShippingSlipId) REFERENCES ShippingSlips(Id) ON DELETE CASCADE,
    ProductId INT NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
)

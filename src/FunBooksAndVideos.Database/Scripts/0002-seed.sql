-- Seed data

-- Customers
SET IDENTITY_INSERT Customers ON;

INSERT INTO Customers(Id, FirstName, LastName)
VALUES
	(1, 'Jonnie', 'Webb'),
	(2, 'Woodie', 'Holder'),
	(3, 'Winnie', 'Gordon'),
	(4, 'Avery', 'Craig'),
	(5, 'Oswald', 'Lynn'),
	(6, 'Elder', 'Bryan'),
	(7, 'Rae', 'Owen'),
	(8, 'Bradford', 'Hodges'),
	(9, 'Wallace', 'Foley'),
	(10, 'Jack', 'Wade'),
	(11, 'Mariah', 'Sanford'),
	(12, 'Hayden', 'Gillespie');

SET IDENTITY_INSERT Customers OFF;

-- Product types
SET IDENTITY_INSERT ProductTypes ON;

INSERT INTO ProductTypes(Id, [Name], IsPhysical, RequiresActivation)
VALUES
	(1, 'Books', 1, 0),
	(2, 'Videos', 0, 0),
	(3, 'Memberships', 0, 1);

SET IDENTITY_INSERT ProductTypes OFF;

-- Products
SET IDENTITY_INSERT Products ON;

INSERT INTO Products(Id, [Name], Price, ProductTypeId)
VALUES
	(1, 'And Then There Were None', 10.0, 1),
	(2, 'Anna Karenina', 22.0, 1),
	(3, 'The Bell Jar', 3.73, 1),
	(4, 'Beloved', 26.0, 1),
	(5, 'The Bloody Chamber and Other Stories', 35.0, 1),
	(6, 'A Brief History of Time', 83.0, 1),
	(7, 'The Call of the Wild', 25.0, 1),
	(8, 'The Catcher in the Rye', 67.5, 1),
	(9, 'Comprehensive First Aid Training', 20.0, 2),
	(10, 'The Godfather', 13.0, 2),
	(11, 'Gone with the Wind', 18.0, 2),
	(12, 'Schindler''s List', 10.3, 2),
	(13, 'Raging Bull ', 17.0, 2),
	(14, 'Book Club', 10.0, 3),
	(15, 'Video Club', 20.0, 3);

SET IDENTITY_INSERT Products OFF;

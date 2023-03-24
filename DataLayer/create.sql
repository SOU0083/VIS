CREATE TABLE Objekt (
	[Id] INT IDENTITY(1, 1) PRIMARY KEY,
	[HierarchieId] HIERARCHYID NOT NULL UNIQUE,
	[Nazev] VARCHAR (50) NOT NULL , 
	[SmazanoOd] DATE NULL
)

CREATE TABLE KategorieInstituce (
	[Id] INT IDENTITY(1, 1) PRIMARY KEY,
	[Nazev] VARCHAR (50) NOT NULL
)

CREATE TABLE Instituce (
	[Id] INT PRIMARY KEY FOREIGN KEY REFERENCES Objekt([Id]),
	[Email] VARCHAR (254) NOT NULL , 
	[Telefon] BIGINT NULL , 
	[Ulice] VARCHAR (50) NULL , 
    [Cislo_popisne] VARCHAR (10) NOT NULL ,
	[Mesto] VARCHAR (50) NOT NULL , 
    [PSC] INT NOT NULL
)

CREATE TABLE Instituce_KategorieInstituce (
	[InstituceId] INT FOREIGN KEY REFERENCES Instituce([Id]),
	[KategorieInstituceId] INT FOREIGN KEY REFERENCES KategorieInstituce([Id]),
	PRIMARY KEY ([InstituceId],[KategorieInstituceId])
)

CREATE TABLE Zakaznik (
	[Id] INT IDENTITY(1, 1) PRIMARY KEY,
	[Jmeno] VARCHAR (50) NOT NULL , 
	[Prijmeni] VARCHAR (50) NOT NULL , 
	[Email] VARCHAR (254) NOT NULL , 
	[Telefon] BIGINT NULL , 
	[Ulice] VARCHAR (50) NULL , 
    [Cislo_popisne] VARCHAR (10) NOT NULL ,
	[Mesto] VARCHAR (50) NOT NULL , 
    [PSC] INT NOT NULL
)

CREATE TABLE RezervacniObjekt (
	[Id] INT PRIMARY KEY FOREIGN KEY REFERENCES Objekt([Id]),
	[Cena] INT NOT NULL ,
	[Pocet] INT NOT NULL ,
	[TypRezervace] TINYINT NOT NULL
)

CREATE TABLE Udalost (
	[Id] INT IDENTITY(1, 1) PRIMARY KEY,
	[Nazev] VARCHAR (50) NOT NULL , 
	[ObjektId] INT FOREIGN KEY REFERENCES Objekt([Id]),
	[Start] SMALLDATETIME NOT NULL,
	[Konec] SMALLDATETIME NOT NULL,
	[RezervaceOd] SMALLDATETIME NOT NULL,
	[RezervaceDo] SMALLDATETIME NOT NULL,
	[SmazanoOd] DATE NULL
)

CREATE TABLE Rezervace (
	[Id] INT IDENTITY(1, 1) PRIMARY KEY,
	[ZakaznikId] INT FOREIGN KEY REFERENCES Zakaznik([Id]),
	[RezervacniObjektId] INT NULL FOREIGN KEY REFERENCES RezervacniObjekt([Id]),
	[UdalostId] INT NULL FOREIGN KEY REFERENCES Udalost([Id]),
	[Od] SMALLDATETIME NULL,
	[Do] SMALLDATETIME NULL,
	[SmazanoOd] DATE NULL
)
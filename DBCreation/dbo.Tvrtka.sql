CREATE TABLE [dbo].[Tvrtka]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Naziv] VARCHAR(50) NOT NULL, 
    [OIB] INT NULL, 
    [IBAN] VARCHAR(50) NULL, 
    [Adresa] VARCHAR(100) NULL, 
    [Vlasnik] VARCHAR(50) NULL
)

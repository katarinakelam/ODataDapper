CREATE TABLE [dbo].[Stavka]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Naziv] VARCHAR(50) NOT NULL, 
    [Opis] VARCHAR(50) NULL, 
    [Cijena] FLOAT NOT NULL
)

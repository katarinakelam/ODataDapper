CREATE TABLE [dbo].[Racun_Stavka]
(
	[Racun_Id] INT NOT NULL , 
    [Stavka_Id] INT NOT NULL, 
    [Kolicina] INT NOT NULL DEFAULT 1, 
    PRIMARY KEY ([Racun_Id], [Stavka_Id]), 
    CONSTRAINT [FK_Racun_Stavka_Racun] FOREIGN KEY ([Racun_Id]) REFERENCES [Racun]([Id]),
    CONSTRAINT [FK_Racun_Stavka_Stavka] FOREIGN KEY ([Stavka_Id]) REFERENCES [Stavka]([Id])
)

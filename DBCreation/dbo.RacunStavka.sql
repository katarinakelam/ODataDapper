CREATE TABLE [dbo].[Racun_Stavka] (
    [Racun_Id]  INT NOT NULL,
    [Stavka_Id] INT NOT NULL,
    [Kolicina]  INT DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Racun_Id] ASC, [Stavka_Id] ASC),
    CONSTRAINT [FK_Racun_Stavka_Racun] FOREIGN KEY ([Racun_Id]) REFERENCES [dbo].[Racun] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Racun_Stavka_Stavka] FOREIGN KEY ([Stavka_Id]) REFERENCES [dbo].[Stavka] ([Id]) ON DELETE SET NULL
);


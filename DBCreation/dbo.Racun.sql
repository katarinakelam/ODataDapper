CREATE TABLE [dbo].[Racun] (
    [Id]             INT           NOT NULL,
    [Zaposlenik_Id]  INT           NULL,
    [DatumIzdavanja] DATETIME      NULL,
    [JIR]            VARCHAR (MAX) NULL,
    [UkupanIznos]    FLOAT (53)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Racun_Zaposlenik] FOREIGN KEY ([Zaposlenik_Id]) REFERENCES [dbo].[Zaposlenik] ([Id]) ON DELETE SET NULL
);


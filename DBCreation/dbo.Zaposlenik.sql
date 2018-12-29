CREATE TABLE [dbo].[Zaposlenik] (
    [Id]            INT           NOT NULL,
    [Ime]           VARCHAR (50)  NOT NULL,
    [Prezime]       VARCHAR (50)  NOT NULL,
    [DatumRodjenja] DATETIME          NULL,
    [Adresa]        VARCHAR (100) NULL,
    [Dopustenje]    BIT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


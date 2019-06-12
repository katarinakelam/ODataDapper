SELECT DISTINCT s.Naziv as stavkaNaziv,
s.Cijena as stavkaCijena,
z.Ime as zaposlenikIme,
r.DatumIzdavanja as datumRacuna,
z.Dopustenje as zaposlenikImaDopustenje
FROM Zaposlenik z, Racun r, Stavka s 
LEFT OUTER join Racun_Stavka rs on s.Id = rs.Stavka_Id
LEFT join Racun on rs.Racun_Id = Racun.Id
RIGHT join Zaposlenik on Racun.Zaposlenik_Id = Zaposlenik.Id
where s.Cijena > 6 and s.Naziv like '%al%' and z.Ime like '%ate'
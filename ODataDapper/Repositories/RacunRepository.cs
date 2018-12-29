using ODataDapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataDapper.Repositories
{
    /// <summary>
    /// The racun repository.
    /// </summary>
    /// <seealso cref="BaseRepository" />
    public class RacunRepository : BaseRepository
    {
        /// <summary>
        /// Gets the stavka by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns matched racun.</returns>
        public Racun GetById(int id)
        {
            //Get basic racun info
            var racun =  QueryFirstOrDefault<Racun>("SELECT * FROM Racun WHERE Id = @Id", new { id });
            racun.Stavke = new List<Stavka>();

            //Get all stavke from this racun
            racun.Stavke = Query<Stavka>("SELECT Stavka.Id, Stavka.Naziv, Stavka.Cijena, Stavka.Opis FROM Stavka JOIN Racun_Stavka ON Stavka.Id = Racun_Stavka.Stavka_Id join Racun on Racun_Stavka.Racun_Id = Racun.Id where Racun.Id = @Id", new { id });

            //Get the matching zaposlenik from this racun
            racun.Zaposlenik = QueryFirstOrDefault<Zaposlenik>("select Zaposlenik.Id, Zaposlenik.Ime, Zaposlenik.Prezime, Zaposlenik.Adresa, Zaposlenik.DatumRodjenja, Zaposlenik.Dopustenje from Zaposlenik, Racun where Zaposlenik.Id = Racun.Zaposlenik_Id and Racun.Id = @Id", new { id });

            return racun;
        }
    }
}
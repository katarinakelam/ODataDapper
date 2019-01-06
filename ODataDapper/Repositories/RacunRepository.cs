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
        /// Gets the racun by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns matched racun.</returns>
        public Racun GetById(int id)
        {
            //Get basic racun info
            var racun = QueryFirstOrDefault<Racun>("SELECT * FROM Racun WHERE Racun.Id = @Id ", new { id });
            racun.Stavke = new List<Stavka>();

            //Get all stavke from this racun
            racun.Stavke = Query<Stavka>("SELECT Stavka.Id, Stavka.Naziv, Stavka.Cijena, Stavka.Opis FROM Stavka JOIN Racun_Stavka ON Stavka.Id = Racun_Stavka.Stavka_Id join Racun on Racun_Stavka.Racun_Id = Racun.Id where Racun.Id = @Id", new { id });

            racun.Zaposlenik = new Zaposlenik();
            //Get the matching zaposlenik from this racun
            racun.Zaposlenik = QueryFirstOrDefault<Zaposlenik>("SELECT Zaposlenik.Id, Zaposlenik.Ime, Zaposlenik.Prezime, Zaposlenik.Adresa, Zaposlenik.DatumRodjenja, Zaposlenik.Dopustenje from Zaposlenik, Racun where Zaposlenik.Id = Racun.Zaposlenik_Id and Racun.Id = @Id", new { id });

            return racun;
        }

        /// <summary>
        /// Gets all racuni.
        /// </summary>
        /// <returns>
        /// Returns all racuni in the database.
        /// </returns>
        public IEnumerable<Racun> GetAll()
        {
            //Get all racuni from the database
            var racuni = Query<Racun>("SELECT * FROM Racun");

            //Create an empty new list of Racuni
            var newRacuni = new List<Racun>();

            foreach (var racun in racuni)
            {
                //Create a new racun
                var newRacun = new Racun();

                //Copy data from existing racun to a new one
                newRacun = racun;
                newRacun.Stavke = new List<Stavka>();
                newRacun.Zaposlenik = new Zaposlenik();

                //Get all stavke from this racun
                newRacun.Stavke = Query<Stavka>("SELECT Stavka.Id, Stavka.Naziv, Stavka.Cijena, Stavka.Opis FROM Stavka JOIN Racun_Stavka ON Stavka.Id = Racun_Stavka.Stavka_Id join Racun on Racun_Stavka.Racun_Id = Racun.Id where Racun.Id = @Id", new { racun.Id });

                //Get the matching zaposlenik from this racun
                newRacun.Zaposlenik = QueryFirstOrDefault<Zaposlenik>("SELECT Zaposlenik.Id, Zaposlenik.Ime, Zaposlenik.Prezime, Zaposlenik.Adresa, Zaposlenik.DatumRodjenja, Zaposlenik.Dopustenje from Zaposlenik, Racun where Zaposlenik.Id = Racun.Zaposlenik_Id and Racun.Id = @Id", new { racun.Id });

                //Add the expanded racun to list of racuni
                newRacuni.Add(newRacun);
            }

            return newRacuni;
        }

        /// <summary>
        /// Updates the racun.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="racun">The racun.</param>
        /// <exception cref="Exception">Entity in the database not updated</exception>
        public void Update(int id, Racun racun)
        {
            //Gets the number of rows affected by the database command
            var numberOfRowsAffected = Execute("UPDATE Racun SET DatumIzdavanja = @DatumIzdavanja, JIR = @JIR, UkupanIznos = @UkupanIznos, Zaposlenik_Id = @Zaposlenik_Id WHERE Id = @Id;", new
            {
                DatumIzdavanja = racun.DatumIzdavanja,
                JIR = racun.JIR,
                UkupanIznos = racun.UkupanIznos,
                Zaposlenik_Id = racun.Zaposlenik_Id,
                Id = id
            });

            if (numberOfRowsAffected == 0)
                throw new Exception("Entity in the database not updated");
        }

        /// <summary>
        /// Deletes the specified item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="Exception">Entity in the database not deleted</exception>
        public void Delete(int id)
        {
            //Gets the number of rows affected by the database command
            var numberOfRowsAffected = Execute("DELETE FROM Racun WHERE Id = @Id", new { id });

            if (numberOfRowsAffected == 0)
                throw new Exception("Entity in the database not deleted");
        }

        /// <summary>
        /// Creates the specified racun.
        /// </summary>
        /// <param name="racun">The racun.</param>
        /// <returns>
        /// Returns the created racun.
        /// </returns>
        public Racun Create(Racun racun)
        {
            //Gets the number of rows affected by the database command
            var numberOfRowsAffected = Execute("INSERT INTO Racun (JIR, UkupanIznos, Zaposlenik_Id, DatumIzdavanja) VALUES (@JIR, @UkupanIznos, @Zaposlenik_Id, @DatumIzdavanja)", new
            {
                JIR = racun.JIR,
                UkupanIznos = racun.UkupanIznos,
                Zaposlenik_Id = racun.Zaposlenik_Id,
                DatumIzdavanja = racun.DatumIzdavanja
            });

            if (numberOfRowsAffected == 0)
                throw new Exception("Entity in the database not created");

            //Get last added item to the database
            var racunFromDb = QueryFirstOrDefault<Racun>("SELECT * from Racun ORDER BY Id DESC LIMIT 1");
            return racunFromDb;
        }
    }
}
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
        /// <returns>
        /// Returns matched racun.
        /// </returns>
        public Racun GetById(int id)
        {
            //Get basic racun info
            var racun = QueryFirstOrDefault<Racun>("SELECT * FROM Racun WHERE Racun.Id = @Id ", new { id });

            if (racun == null)
                return racun;

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
        /// <param name="filterSQL">The filter SQL.</param>
        /// <returns>
        /// Returns all racuni in the database.
        /// </returns>
        public IEnumerable<Racun> GetAll(string filterSQL)
        {
            //Get all racuni from the database
            var racuni = Query<Racun>("SELECT * FROM Racun" + filterSQL);

            //Create an empty new list of Racuni
            var newRacuni = new List<Racun>();

            if (racuni != null)
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

        /// <summary>
        /// Adds the stavka to racun.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="stavka">The stavka.</param>
        /// <returns>
        /// Returns the updated racun.
        /// </returns>
        /// <exception cref="Exception">The given stavka already exists in the database, please check then add by id</exception>
        public Racun AddStavkaToRacun(int id, StavkaDTO stavka)
        {
            //Get basic racun info
            var racun = QueryFirstOrDefault<Racun>("SELECT * FROM Racun WHERE Racun.Id = @Id ", new { id });

            if (stavka == null)
                throw new ArgumentNullException(nameof(stavka));

            //Check whether the stavka already exists in the database
            var stavkaFromDb = QueryFirstOrDefault<Stavka>("SELECT * FROM Stavka WHERE Naziv = @Naziv, Opis = @Opis, Cijena = @Cijena", new
            {
                Naziv = stavka.Naziv,
                Opis = stavka.Opis,
                Cijena = stavka.Cijena
            });

            if (stavkaFromDb != null)
                throw new Exception("The given stavka already exists in the database, please check then add by id");

            //Get stavka repository for easier operations
            StavkaRepository stavkaRepository = new StavkaRepository();

            //Create a stavka model out of stavka data transfer object
            var stavkaToCreate = new Stavka()
            {
                Naziv = stavka.Naziv,
                Opis = stavka.Opis,
                Cijena = stavka.Cijena
            };

            //Save the new stavka to the database
            var newStavka = stavkaRepository.Create(stavkaToCreate);

            //Add the connection between new stavka and the racun to the database
            Execute("INSERT INTO Racun_Stavka (Racun_Id, Stavka_Id) VALUES (@Racun_Id, @Stavka_Id)", new
            {
                Racun_Id = id,
                Stavka_Id = newStavka.Id
            });

            return racun;
        }

        /// <summary>
        /// Adds the stavka to racun by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="stavkaId">The stavka identifier.</param>
        /// <returns>
        /// Returns the updated racun.
        /// </returns>
        /// <exception cref="Exception">The given stavka does not exist in the database, please create the stavka first</exception>
        public Racun AddStavkaToRacunById(int id, int? stavkaId)
        {
            //Get basic racun info
            var racun = QueryFirstOrDefault<Racun>("SELECT * FROM Racun WHERE Racun.Id = @Id ", new { id });

            if (stavkaId == null)
                throw new ArgumentNullException(nameof(stavkaId));

            //Check whether the stavka exists in the database
            var stavkaFromDb = QueryFirstOrDefault<Stavka>("SELECT * FROM Stavka WHERE Id = @Id", new { Id = stavkaId.Value });

            if (stavkaFromDb == null)
                throw new Exception("The given stavka does not exist in the database, please create the stavka first");

            //Add the connection between new stavka and the racun to the database
            Execute("INSERT INTO Racun_Stavka (Racun_Id, Stavka_Id) VALUES (@Racun_Id, @Stavka_Id)", new
            {
                Racun_Id = id,
                Stavka_Id = stavkaId.Value
            });

            return racun;
        }
    }
}
using ODataDapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataDapper.Repositories
{
    /// <summary>
    /// The zaposlenik repository
    /// </summary>
    /// <seealso cref="BaseRepository" />
    public class ZaposlenikRepository : BaseRepository
    {
        /// <summary>
        /// Gets the zaposlenik by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns matched zaposlenik.</returns>
        public Zaposlenik GetById(int id)
        {
            return QueryFirstOrDefault<Zaposlenik>("SELECT * FROM Zaposlenik WHERE Id = @Id", new { id });
        }

        /// <summary>
        /// Gets all zaposlenici.
        /// </summary>
        /// <returns>
        /// Returns all zaposlenici in the database.
        /// </returns>
        public IEnumerable<Zaposlenik> GetAll()
        {
            return Query<Zaposlenik>("SELECT * FROM Zaposlenik ORDER BY Prezime");
        }

        /// <summary>
        /// Updates the zaposlenik.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="zaposlenik">The zaposlenik.</param>
        /// <returns>
        /// Returns updated zaposlenik
        /// </returns>
        public void Update(int id, Zaposlenik zaposlenik)
        {
            var numberOfRowsAffected = Execute("UPDATE Zaposlenik SET Ime = @Ime, Prezime = @Prezime, DatumRodjenja = @DatumRodjenja, Dopustenje = @Dopustenje WHERE Id = @Id;", new
            {
                Ime = zaposlenik.Ime,
                Prezime = zaposlenik.Prezime,
                DatumRodjenja = zaposlenik.DatumRodjenja,
                Dopustenje = zaposlenik.Dopustenje,
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
            var numberOfRowsAffected = Execute("DELETE FROM Zaposlenik WHERE Id = @Id", new { id });

            if (numberOfRowsAffected == 0)
                throw new Exception("Entity in the database not deleted");
        }
    }
}
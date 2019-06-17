using ODataDapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// <param name="filterSql">The filter SQL.</param>
        /// <returns>
        /// Returns all zaposlenici in the database.
        /// </returns>
        public async Task<IEnumerable<Zaposlenik>> GetAll(string filterSql)
        {
            return await QueryAsync<Zaposlenik>("SELECT * FROM Zaposlenik" + filterSql);
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="sqlClause">The composite sql clause.</param>
        /// <returns>
        /// Returns the count of items in collection
        /// </returns>
        public async Task<int> GetCount(KeyValuePair<string, string> sqlClause)
        {
            var sql = sqlClause.Key + "Zaposlenik " + sqlClause.Value;
            return (await QueryAsync<int>(sql)).Single();
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

        /// <summary>
        /// Creates the specified zaposlenik.
        /// </summary>
        /// <param name="zaposlenik">The zaposlenik.</param>
        /// <returns>
        /// Returns the created zaposlenik.
        /// </returns>
        public Zaposlenik Create(Zaposlenik zaposlenik)
        {
            var lastZaposlenikFromDb = QueryFirstOrDefault<Racun>("SELECT TOP 1 * FROM Zaposlenik ORDER BY Id DESC ");
            zaposlenik.Id = lastZaposlenikFromDb.Id + 1;

            //Gets the number of rows affected by the database command
            var numberOfRowsAffected = Execute("INSERT INTO Zaposlenik (Id, Ime, Prezime, DatumRodjenja, Dopustenje) VALUES (@Id, @Ime, @Prezime, @DatumRodjenja, @Dopustenje )", new
            {
                Id = zaposlenik.Id,
                Ime = zaposlenik.Ime,
                Prezime = zaposlenik.Prezime,
                DatumRodjenja = zaposlenik.DatumRodjenja,
                Dopustenje = zaposlenik.Dopustenje
            });

            if (numberOfRowsAffected == 0)
                throw new Exception("Entity in the database not created");

            //Get last added item to the database
            var zaposlenikFromDb = QueryFirstOrDefault<Zaposlenik>("SELECT TOP 1 * FROM Zaposlenik ORDER BY Id DESC");
            return zaposlenikFromDb;
        }
    }
}
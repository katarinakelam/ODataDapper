using ODataDapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ODataDapper.Repositories
{
    /// <summary>
    /// The Stavka repository.
    /// </summary>
    /// <seealso cref="BaseRepository" />
    public class StavkaRepository : BaseRepository
    {
        /// <summary>
        /// Gets the stavka by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns matched stavka.</returns>
        public Stavka GetById(int id)
        {
            return QueryFirstOrDefault<Stavka>("SELECT * FROM Stavka WHERE Id = @Id", new { id });
        }

        /// <summary>
        /// Gets all stavke.
        /// </summary>
        /// <param name="filterSql">The filter SQL.</param>
        /// <returns>
        /// Returns all stavke in the database.
        /// </returns>
        public async Task<IEnumerable<Stavka>> GetAll(string filterSql)
        {
            return (await QueryAsync<Stavka>("SELECT * FROM Stavka" + filterSql));
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
            var sql = sqlClause.Key + "Stavka" + sqlClause.Value;
            return (await QueryAsync<int>(sql)).Single();
        }


        /// <summary>
        /// Updates the stavka.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="stavka">The stavka.</param>
        /// <exception cref="Exception">Entity in the database not updated</exception>
        public void Update(int id, Stavka stavka)
        {
            //Gets the number of rows affected by the database command
            var numberOfRowsAffected = Execute("UPDATE Stavka SET Naziv = @Naziv, Opis = @Opis, Cijena = @Cijena WHERE Id = @Id;", new
            {
                Naziv = stavka.Naziv,
                Opis = stavka.Opis,
                Cijena = stavka.Cijena,
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
            var numberOfRowsAffected = Execute("DELETE FROM Stavka WHERE Id = @Id", new { id });

            if (numberOfRowsAffected == 0)
                throw new Exception("Entity in the database not deleted");
        }

        /// <summary>
        /// Creates the specified stavka.
        /// </summary>
        /// <param name="stavka">The stavka.</param>
        /// <returns>
        /// Returns the created stavka.
        /// </returns>
        public Stavka Create(Stavka stavka)
        {
            var lastStavkaFromDb = QueryFirstOrDefault<Racun>("SELECT TOP 1 * FROM Stavka ORDER BY Id DESC ");
            stavka.Id = lastStavkaFromDb.Id + 1;

            //Gets the number of rows affected by the database command
            var numberOfRowsAffected = Execute("INSERT INTO Stavka (Id, Naziv, Opis, Cijena) VALUES (@Id, @Naziv, @Opis, @Cijena )", new
            {
                Id = stavka.Id,
                Naziv = stavka.Naziv,
                Opis = stavka.Opis,
                Cijena = stavka.Cijena
            });

            if (numberOfRowsAffected == 0)
                throw new Exception("Entity in the database not created");

            //Get last added item to the database
            var stavkaFromDb = QueryFirstOrDefault<Stavka>("SELECT TOP 1 * FROM Stavka ORDER BY Id DESC ");
            return stavkaFromDb;
        }
    }
}
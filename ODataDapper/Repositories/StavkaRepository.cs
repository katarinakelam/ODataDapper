using ODataDapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <returns>
        /// Returns all stavke in the database.
        /// </returns>
        public IEnumerable<Stavka> GetAll()
        {
            return Query<Stavka>("SELECT * FROM Stavka ORDER BY Naziv");
        }
    }
}
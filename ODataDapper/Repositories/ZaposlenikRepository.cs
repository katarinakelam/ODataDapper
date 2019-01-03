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
    }
}
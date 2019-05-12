using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using ODataDapper.Models;
using Microsoft.Data.OData;
using ODataDapper.Repositories;
using ODataDapper.Helpers;

namespace ODataDapper.Controllers
{
    public class ZaposleniciController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();
        private ZaposlenikRepository zaposlenikRepository = new ZaposlenikRepository();

        // GET: odata/Zaposlenici
        /// <summary>
        /// Gets the zaposlenici.
        /// </summary>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>
        /// Returns all zaposlenici from the database.
        /// </returns>
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IHttpActionResult GetZaposlenici(ODataQueryOptions<Zaposlenik> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            var sqlBuilder = new SQLQueryBuilder(queryOptions);

            // make $count works
            Request.ODataProperties().TotalCount = zaposlenikRepository.GetCount(sqlBuilder.ToCountSql());

            return Ok(zaposlenikRepository.GetAll(sqlBuilder.ToSql()));
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>
        /// Returns counted items
        /// </returns>
        public IHttpActionResult GetCount(ODataQueryOptions<Racun> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            var sqlBuilder = new SQLQueryBuilder(queryOptions);  
            return Ok(zaposlenikRepository.GetCount(sqlBuilder.ToCountSql()));
        }

        // GET: odata/Zaposlenici(5)
        /// <summary>
        /// Gets the zaposlenik.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>
        /// Returns the matching zaposlenik
        /// </returns>
        public IHttpActionResult GetZaposlenik([FromODataUri] int key, ODataQueryOptions<Zaposlenik> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(zaposlenikRepository.GetById(key));
        }

        // PUT: odata/Zaposlenici(5)
        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="delta">The delta.</param>
        /// <returns>
        /// Returns the updated zaposlenik
        /// </returns>
        public IHttpActionResult Put([FromODataUri] int key, Delta<Zaposlenik> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the existing zaposlenik from the database
            var zaposlenikToUpdate = zaposlenikRepository.GetById(key);

            //Overwrite existing data with the new one
            delta.Put(zaposlenikToUpdate);

            //Update the entity with new data
            zaposlenikRepository.Update(key, zaposlenikToUpdate);

            return Updated(zaposlenikToUpdate);
        }

        // POST: odata/Zaposlenici
        /// <summary>
        /// Posts the specified zaposlenik.
        /// </summary>
        /// <param name="zaposlenik">The zaposlenik.</param>
        /// <returns>
        /// Returns the created zaposlenik
        /// </returns>
        public IHttpActionResult Post(Zaposlenik zaposlenik)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Insert the new zaposlenik into the database
            var createdZaposlenik = zaposlenikRepository.Create(zaposlenik);
            return Created(createdZaposlenik);
        }

        // DELETE: odata/Zaposlenici(5)
        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// Returns the no content status code
        /// </returns>
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            zaposlenikRepository.Delete(key);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

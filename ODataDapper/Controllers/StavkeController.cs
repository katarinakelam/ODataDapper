using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
    public class StavkeController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();
        private StavkaRepository stavkaRepository = new StavkaRepository();

        // GET: odata/Stavke
        // GET: odata/Stavke?$filter=Cijena+lt+6
        /// <summary>
        /// Gets the stavke.
        /// </summary>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>
        /// Returns all stavke from the database
        /// </returns>
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public async Task<IHttpActionResult> GetStavke(ODataQueryOptions<Stavka> queryOptions)
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
            Request.ODataProperties().TotalCount = await stavkaRepository.GetCount(sqlBuilder.ToCountSql());

            return Ok(await stavkaRepository.GetAll(sqlBuilder.ToSql()));
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>
        /// Returns counted items
        /// </returns>
        public async Task<IHttpActionResult> GetCount(ODataQueryOptions<Racun> queryOptions)
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
            return Ok(await stavkaRepository.GetCount(sqlBuilder.ToCountSql()));
        }

        // GET: odata/Stavke(5)
        /// <summary>
        /// Gets the stavka.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>
        /// Returns requested stavka
        /// </returns>
        public IHttpActionResult GetStavka([FromODataUri] int key, ODataQueryOptions<Stavka> queryOptions)
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

            return Ok(stavkaRepository.GetById(key));
        }

        // PUT: odata/Stavke(5)
        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="delta">The delta.</param>
        /// <returns>
        /// Returns updated stavka
        /// </returns>
        public IHttpActionResult Put([FromODataUri] int key, Delta<Stavka> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the existing stavka from the database
            var stavkaToUpdate = stavkaRepository.GetById(key);

            //Overwrite existing data with the new one
            delta.Put(stavkaToUpdate);

            //Update the entity with new data
            stavkaRepository.Update(key, stavkaToUpdate);

            return Updated(stavkaToUpdate);
        }

        // POST: odata/Stavke
        /// <summary>
        /// Posts the specified stavka.
        /// </summary>
        /// <param name="stavka">The stavka.</param>
        /// <returns>
        /// Returns created stavka
        /// </returns>
        public IHttpActionResult Post(Stavka stavka)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Insert the stavka to the database
            var createdStavka = stavkaRepository.Create(stavka);
            return Created(createdStavka);
        }

        // DELETE: odata/Stavke(5)
        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// Returns the no content status code
        /// </returns>
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            stavkaRepository.Delete(key);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

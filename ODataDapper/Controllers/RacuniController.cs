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
    public class RacuniController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();
        private RacunRepository racunRepository = new RacunRepository();

        // GET: odata/Racuni
        // GET: odata/Racuni?$filter=Zaposlenik_Id+eq+1
        // GET: odata/Racuni?expand=Stavke
        /// <summary>
        /// Gets the racuni.
        /// </summary>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>
        /// Returns all racuni from the database.
        /// </returns>
        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public async Task<IHttpActionResult> GetRacuni(ODataQueryOptions<Racun> queryOptions)
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
            Request.ODataProperties().TotalCount = await racunRepository.GetCount(sqlBuilder.ToCountSql());

            return Ok(await racunRepository.GetAll(sqlBuilder.ToSql()));
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
            return Ok(await racunRepository.GetCount(sqlBuilder.ToCountSql()));
        }

        // GET: odata/Racuni(5)
        /// <summary>
        /// Gets the racun.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="queryOptions">The query options.</param>
        /// <returns>
        /// Returns the requested racun.
        /// </returns>
        public IHttpActionResult GetRacun([FromODataUri] int key, ODataQueryOptions<Racun> queryOptions)
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

            return Ok(racunRepository.GetById(key));
        }

        // PUT: odata/Racuni(5)
        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="delta">The delta.</param>
        /// <returns>
        ///  Returns the updated racun.
        /// </returns>
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<Racun> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the existing racun from the database
            var racunToUpdate = await racunRepository.GetById(key);

            //Overwrite existing data with the new one
            delta.Put(racunToUpdate);

            //Update the entity with new data
            racunRepository.Update(key, racunToUpdate);

            return Updated(racunToUpdate);
        }

        // POST: odata/Racuni
        /// <summary>
        /// Posts the specified racun.
        /// </summary>
        /// <param name="racun">The racun.</param>
        /// <returns>
        ///  Returns the created racun.
        /// </returns>
        public IHttpActionResult Post(Racun racun)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Insert the new racun into the database
            var createdRacun = racunRepository.Create(racun);
            return Created(createdRacun);
        }

        // DELETE: odata/Racuni(5)
        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// Returns the No Content status code
        /// </returns>
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            racunRepository.Delete(key);
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds the stavka.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///  Returns the updated racun.
        /// </returns>
        [HttpPost]
        public IHttpActionResult AddStavka([FromODataUri] int key, ODataActionParameters parameters)
        {
            //read parameter from ODataActionParameters 
            var stavkaToAdd = parameters["Value"] as StavkaDTO;

            //Updates the racun with the given new stavka
            var updatedRacun = racunRepository.AddStavkaToRacun(key, stavkaToAdd);

            //Returns updated data
            return Updated(updatedRacun);
        }

        /// <summary>
        /// Adds the stavka by the identifier..
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// Returns the updated racun.
        /// </returns>
        [HttpPost]
        public IHttpActionResult AddStavkaById([FromODataUri] int key, ODataActionParameters parameters)
        {
            //read parameter from ODataActionParameters 
            var stavkaId = parameters["Value"] as int?;

            //Updates the racun with the given new stavka
            var updatedRacun = racunRepository.AddStavkaToRacunById(key, stavkaId);

            //Returns updated data
            return Updated(updatedRacun);
        }

    }
}

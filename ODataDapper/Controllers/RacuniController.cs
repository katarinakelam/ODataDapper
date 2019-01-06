using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using ODataDapper.Models;
using Microsoft.Data.OData;
using ODataDapper.Repositories;

namespace ODataDapper.Controllers
{
    public class RacuniController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();
        private RacunRepository racunRepository = new RacunRepository();

        // GET: odata/Racuni
        [EnableQuery]
        public IHttpActionResult GetRacuni(ODataQueryOptions<Racun> queryOptions)
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

            return Ok(racunRepository.GetAll());
        }

        // GET: odata/Racuni(5)
        [EnableQuery]
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
        public IHttpActionResult Put([FromODataUri] int key, Delta<Racun> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Get the existing racun from the database
            var racunToUpdate = racunRepository.GetById(key);

            //Overwrite existing data with the new one
            delta.Put(racunToUpdate);

            //Update the entity with new data
            racunRepository.Update(key, racunToUpdate);

            return Updated(racunToUpdate);
        }

        // POST: odata/Racuni
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
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            racunRepository.Delete(key);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

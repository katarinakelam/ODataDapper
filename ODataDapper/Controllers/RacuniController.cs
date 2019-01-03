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

        // PUT: odata/Racuns(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Racun> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(racun);

            // TODO: Save the patched entity.

            // return Updated(racun);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/Racuns
        public IHttpActionResult Post(Racun racun)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(racun);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/Racuns(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Racun> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(racun);

            // TODO: Save the patched entity.

            // return Updated(racun);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/Racuns(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}

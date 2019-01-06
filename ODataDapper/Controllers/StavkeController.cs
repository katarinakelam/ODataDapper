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
    public class StavkeController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();
        private StavkaRepository stavkaRepository = new StavkaRepository();

        // GET: odata/Stavke
        public IHttpActionResult GetStavke(ODataQueryOptions<Stavka> queryOptions)
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

            return Ok(stavkaRepository.GetAll());
        }

        // GET: odata/Stavke(5)
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
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            stavkaRepository.Delete(key);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

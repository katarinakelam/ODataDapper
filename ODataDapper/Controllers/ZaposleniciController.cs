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
    public class ZaposleniciController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();
        private ZaposlenikRepository zaposlenikRepository = new ZaposlenikRepository();

        // GET: odata/Zaposlenici
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

            return Ok(zaposlenikRepository.GetAll());
        }

        // GET: odata/Zaposlenici(5)
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
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            zaposlenikRepository.Delete(key);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}

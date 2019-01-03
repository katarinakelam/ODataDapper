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

        // PUT: odata/Zaposleniks(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Zaposlenik> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(zaposlenik);

            // TODO: Save the patched entity.

            // return Updated(zaposlenik);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/Zaposleniks
        public IHttpActionResult Post(Zaposlenik zaposlenik)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(zaposlenik);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/Zaposleniks(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Zaposlenik> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(zaposlenik);

            // TODO: Save the patched entity.

            // return Updated(zaposlenik);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/Zaposleniks(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}

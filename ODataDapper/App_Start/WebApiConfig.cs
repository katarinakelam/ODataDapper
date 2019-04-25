using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using ODataDapper.Models;

namespace ODataDapper
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Stavka>("Stavke");
            builder.EntitySet<Zaposlenik>("Zaposlenici");
            builder.EntitySet<Racun>("Racuni");

            //Configure action of adding stavka to racun on Post: ~odata/Racuni(1)/AddStavkaById
            var addStavkaByIdToRacunAction = builder.Entity<Racun>().Action("AddStavkaById");
            addStavkaByIdToRacunAction.Parameter<int>("Value");

            //Configure action of adding stavka to racun on Post: ~odata/Racuni(1)/AddStavka
            var addStavkaToRacunAction = builder.Entity<Racun>().Action("AddStavka");
            addStavkaToRacunAction.Parameter<StavkaDTO>("Value");

            config.Routes.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: builder.GetEdmModel()
             );
        }
    }
}

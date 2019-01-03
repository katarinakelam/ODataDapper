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
            config.Routes.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: null,
                model: builder.GetEdmModel()
             );
        }
    }
}

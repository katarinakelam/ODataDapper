using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.OData.Routing;
using System.Web.Http.OData.Routing.Conventions;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace ODataDapper.Helpers
{
    /// <summary>
    /// The count option odata routing convention.
    /// </summary>
    /// <seealso cref="EntitySetRoutingConvention" />
    public class CountODataRoutingConvention : EntitySetRoutingConvention
    {
        /// <summary>
        /// Selects the action for OData requests.
        /// </summary>
        /// <param name="odataPath">The OData path.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="actionMap">The action map.</param>
        /// <returns>
        /// null if the request isn't handled by this convention; otherwise, the name of the selected action
        /// </returns>
        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            if (controllerContext.Request.Method == HttpMethod.Get && odataPath.PathTemplate == "~/entityset/$count")
            {
                if (actionMap.Contains("GetCount"))
                {
                    return "GetCount";
                }
            }
            return null;
        }
    }

    /// <summary>
    /// The count option odata path handler
    /// </summary>
    /// <seealso cref="DefaultODataPathHandler" />
    public class CountODataPathHandler : DefaultODataPathHandler
    {
        /// <summary>
        /// Parses the next OData path segment following an entity collection.
        /// </summary>
        /// <param name="model">The model to use for path parsing.</param>
        /// <param name="previous">The previous path segment.</param>
        /// <param name="previousEdmType">The EDM type of the OData path up to the previous segment.</param>
        /// <param name="segment">The value of the segment to parse.</param>
        /// <returns>
        /// A parsed representation of the segment.
        /// </returns>
        protected override ODataPathSegment ParseAtEntityCollection(IEdmModel model, ODataPathSegment previous, IEdmType previousEdmType, string segment)
        {
            if (segment == "$count")
            {
                return new CountPathSegment();
            }
            return base.ParseAtEntityCollection(model, previous, previousEdmType, segment);
        }
    }

    /// <summary>
    /// The count option odata path segment
    /// </summary>
    /// <seealso cref="ODataPathSegment" />
    public class CountPathSegment : ODataPathSegment
    {
        /// <summary>
        /// Gets the segment kind for the current segment.
        /// </summary>
        public override string SegmentKind
        {
            get
            {
                return "$count";
            }
        }

        /// <summary>
        /// Gets the EDM type for this segment.
        /// </summary>
        /// <param name="previousEdmType">The EDM type of the previous path segment.</param>
        /// <returns>
        /// The EDM type for this segment.
        /// </returns>
        public override IEdmType GetEdmType(IEdmType previousEdmType)
        {
            return EdmCoreModel.Instance.FindDeclaredType("Edm.Int32");
        }

        /// <summary>
        /// Gets the entity set for this segment.
        /// </summary>
        /// <param name="previousEntitySet">The entity set of the previous path segment.</param>
        /// <returns>
        /// The entity set for this segment.
        /// </returns>
        public override IEdmEntitySet GetEntitySet(IEdmEntitySet previousEntitySet)
        {
            return previousEntitySet;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "$count";
        }
    }
}
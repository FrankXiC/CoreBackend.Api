using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CoreBackend.Api.Helper
{
    public abstract class CreateUrl
    {
        private IUrlHelper _urlHelper;

        public CreateUrl(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public string CreateResouceUrl(PaginationBase paremeters,PaginationResourceUriType uriType, string EntityName)
        {
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParameters = paremeters;
                    previousParameters.PageIndex--;
                    return _urlHelper.Link(EntityName, previousParameters);
                case PaginationResourceUriType.NextPage:
                    var nextParameters = paremeters;
                    nextParameters.PageIndex++;
                    return _urlHelper.Link(EntityName, nextParameters);
                default:
                    return _urlHelper.Link(EntityName, paremeters);
            }
        }
    }
}

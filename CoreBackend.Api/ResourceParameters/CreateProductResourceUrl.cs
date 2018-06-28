using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBackend.Api.Helper;
using Microsoft.AspNetCore.Mvc;

namespace CoreBackend.Api.ResourceParameters
{
    public class CreateProductResourceUrl
    {
        private IUrlHelper _urlHelper;
        public CreateProductResourceUrl(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public  string CreateResouceUrl(ResourceParameter paremeters, PaginationResourceUriType uriType, string EntityName) {
            switch (uriType) {
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

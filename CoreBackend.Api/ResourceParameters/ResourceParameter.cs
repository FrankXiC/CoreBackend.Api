using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBackend.Api.Helper;

namespace CoreBackend.Api.ResourceParameters
{
    public class ResourceParameter:PaginationBase
    {
        public string Name { get; set; }

        public int Price { get; set; }
        public string OrderBy { get; set; }
    }
}

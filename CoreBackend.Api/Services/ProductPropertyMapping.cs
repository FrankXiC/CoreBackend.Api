using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBackend.Api.Models;
using CoreBackend.Api.ResourceParameters;

namespace CoreBackend.Api.Services
{
    public class ProductPropertyMapping:PropertyMapping<ResourceParameter,Product>, IProductPropertyMapping
    {
        public ProductPropertyMapping() : base(
            new Dictionary<string, List<MappedProperty>>(StringComparer.OrdinalIgnoreCase)
            {
                [nameof(ResourceParameter.Name)] = new List<MappedProperty>
                {
                    new MappedProperty {Name = nameof(Product.Name), Revert = false}
                },
                [nameof(ResourceParameter.Price)] = new List<MappedProperty>
                {
                    new MappedProperty {Name = nameof(Product.Price), Revert = false}
                }
            })
        {

        }
    }
}

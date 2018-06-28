using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace CoreBackend.Api.Services
{
    public abstract class PropertyMapping<Tsource,TDestination> : IPropertyMapping
    {
        public Dictionary<string,List<MappedProperty>> MappingDictionary { get; }

        protected PropertyMapping(Dictionary<string,List<MappedProperty>> mappingDictionary)
        {
            MappingDictionary = mappingDictionary;
            MappingDictionary[nameof(IEntity.Id)]=new List<MappedProperty>
            {
                new MappedProperty{Name = nameof(IEntity.Id),Revert = false},
            };
           
        }
    }
}

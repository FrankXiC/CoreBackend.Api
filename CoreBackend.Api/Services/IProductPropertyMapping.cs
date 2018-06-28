using System.Collections.Generic;

namespace CoreBackend.Api.Services
{
    public interface IProductPropertyMapping
    {
        Dictionary<string, List<MappedProperty>> MappingDictionary { get; }
    }
}
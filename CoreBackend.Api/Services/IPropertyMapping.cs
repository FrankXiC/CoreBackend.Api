using System.Collections.Generic;

namespace CoreBackend.Api.Services
{
    public interface IPropertyMapping
    {
        Dictionary<string, List<MappedProperty>> MappingDictionary { get; }
    }
}
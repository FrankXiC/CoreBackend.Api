using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBackend.Api.Services;
using System.Linq.Dynamic.Core;

namespace CoreBackend.Api.Helper
{
    public static class QuerableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            IPropertyMapping propertyMapping)
        {
            if (source==null)
            {
                throw new ArgumentException(nameof(source));
            }

            var mappingDictonary = propertyMapping.MappingDictionary;
            if (mappingDictonary == null)
            {
                throw new ArgumentException(nameof(mappingDictonary));
            }

            if (string.IsNullOrEmpty(orderBy))
            {
                return source;
            }

            var orderByAfterSplit = orderBy.Split(',');
            foreach (var orderByClause in orderByAfterSplit)
            {
                var trimmedOrderByClause = orderByClause.Trim();
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedOrderByClause
                    : trimmedOrderByClause.Remove(indexOfFirstSpace);
                if (!mappingDictonary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                var mappedProperties = mappingDictonary[propertyName];
                if (mappedProperties == null)
                {
                    throw new ArgumentException(propertyName);
                }

                mappedProperties.Reverse();
                foreach (var destinationProperty in mappedProperties)
                {
                    if (destinationProperty.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    source = source.OrderBy(destinationProperty.Name +
                                            (orderDescending ? " descending" : " ascending"));
                }
            }

            return source;
        }
    }
}

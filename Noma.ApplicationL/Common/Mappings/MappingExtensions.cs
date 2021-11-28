using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noma.ApplicationL.Common.Mappings
{
    public static class MappingExtensions
    {
        public static List<TDestination> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration)
          => queryable.ProjectTo<TDestination>(configuration).ToList();


    }
}

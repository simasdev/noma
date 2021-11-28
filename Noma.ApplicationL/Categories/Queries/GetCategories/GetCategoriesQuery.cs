using AutoMapper;
using MediatR;
using Noma.ApplicationL.Common.Repositories;
using Noma.ApplicationL.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using System.Linq;

namespace Noma.ApplicationL.Categories.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<List<CategoryDTO>>
    {

    }

    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDTO>>
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }
        public async Task<List<CategoryDTO>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await categoryRepository.GetCategories();

            return mapper.Map<IEnumerable<CategoryDTO>>(categories).ToList();
        }
    }
}

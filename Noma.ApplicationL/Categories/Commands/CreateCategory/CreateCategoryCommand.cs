using MediatR;
using Noma.Domain.Entities;
using Noma.ApplicationL.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Noma.ApplicationL.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Title { get; set; }
        public int Color { get; set; }
        public bool IsDefault { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly ICategoryRepository categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.InsertCategory(new Category()
            {
                Title = request.Title,
                Color = request.Color,
                IsDefault = request.IsDefault
            });

            return category.Id;
        }
    }
}

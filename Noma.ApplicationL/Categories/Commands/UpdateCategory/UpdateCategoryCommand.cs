using MediatR;
using Noma.ApplicationL.Common.Repositories;
using Noma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Noma.ApplicationL.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Color { get; set; }
        public bool IsDefault { get; set; }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await categoryRepository.GetCategoryById(request.Id) is Category category)
            {
                category.Title = request.Title;
                category.Color = request.Color;
                category.IsDefault = request.IsDefault;

                await categoryRepository.UpdateCategory(category);
            }

            return Unit.Value;
        }
    }
}

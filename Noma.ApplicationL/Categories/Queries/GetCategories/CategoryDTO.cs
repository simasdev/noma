using Noma.ApplicationL.Common.Mappings;
using Noma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noma.ApplicationL.Categories.Queries.GetCategories
{
    public class CategoryDTO: IMapFrom<Category>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Color { get; set; }
        public bool IsDefault { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}

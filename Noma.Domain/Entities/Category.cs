using Noma.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noma.Domain.Entities
{
    public class Category: AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Color { get; set; }

        public bool IsDefault { get; set; }
    }
}

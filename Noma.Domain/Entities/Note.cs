using Noma.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noma.Domain.Entities
{
    public class Note: AuditableEntity
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public bool InTrash { get; set; }

        public int? CategoryId { get; set; }

        public int BackgroundColor { get; set; }
    }
}

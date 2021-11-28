using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Noma.ApplicationL.Common.Mappings;
using Noma.Domain.Entities;

namespace Noma.ApplicationL.Notes.Queries.GetNotes
{
    public class NoteDTO: IMapFrom<Note>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public bool InTrash { get; set; }

        public int? CategoryId { get; set; }

        public int BackgroundColor { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}

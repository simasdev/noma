using Noma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noma.ApplicationL.Common.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetNotes();
        Task<Note> GetNoteById(int id);
        Task<Note> InsertNote(Note note);
        Task<Note> UpdateNote(Note note);
        Task DeleteNote(int id);
    }
}

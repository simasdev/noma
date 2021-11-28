using Microsoft.EntityFrameworkCore;
using Noma.ApplicationL.Common.Repositories;
using Noma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Noma.Infrastructure.Persistence.Repositories.NoteRepositories
{
    public class SQLiteNoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext context;
        public SQLiteNoteRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Note>> GetNotes()
        {
            return await context.Notes.ToListAsync();
        }

        public async Task<Note> GetNoteById(int id)
        {
            return await context.Notes.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<Note> InsertNote(Note note)
        {
            await context.Notes.AddAsync(note);
            await context.SaveChangesAsync();

            return note;
        }

        public async Task<Note> UpdateNote(Note note)
        {
            var updatedNote = context.Notes.Attach(note);
            updatedNote.State = EntityState.Modified;
            await context.SaveChangesAsync();

            return note;
        }

        public async Task DeleteNote(int id)
        {
            var note = await context.Notes.FirstOrDefaultAsync(n => n.Id == id);

            if (note != null)
            {
                context.Notes.Remove(note);
                await context.SaveChangesAsync();
            }
        }
    }
}

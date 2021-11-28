using MediatR;
using Noma.ApplicationL.Common.Repositories;
using Noma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Noma.ApplicationL.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommand: IRequest
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool InTrash { get; set; }
        public int? CategoryId { get; set; }
        public int BackgroundColor { get; set; }
    }

    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand>
    {
        private readonly INoteRepository noteRepository;

        public UpdateNoteCommandHandler(INoteRepository noteRepository)
        {
            this.noteRepository = noteRepository;
        }

        public async Task<Unit> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            if(await noteRepository.GetNoteById(request.Id) is Note note)
            {
                note.Content = request.Content;
                note.InTrash = request.InTrash;
                note.CategoryId = request.CategoryId;
                note.BackgroundColor = request.BackgroundColor;

                await noteRepository.UpdateNote(note);
            }

            return Unit.Value;
        }
    }

}

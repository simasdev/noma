using MediatR;
using Noma.ApplicationL.Common.Repositories;
using Noma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Noma.ApplicationL.Notes.Commands.CreateNote
{
    public class CreateNoteCommand: IRequest<int>
    {
        public string Content { get; set; }
        public bool InTrash { get; set; }
        public int? CategoryId { get; set; }
        public int BackgroundColor { get; set; }
    }

    public class CreateNoteCommandHandler: IRequestHandler<CreateNoteCommand, int>
    {
        private readonly INoteRepository noteRepository;

        public CreateNoteCommandHandler(INoteRepository noteRepository)
        {
            this.noteRepository = noteRepository;
        }

        public async Task<int> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            var note = await noteRepository.InsertNote(new Note()
            {
                Content = request.Content,
                InTrash = request.InTrash,
                CategoryId = request.CategoryId,
                BackgroundColor = request.BackgroundColor
            });

            return note.Id;
        }
    }
}

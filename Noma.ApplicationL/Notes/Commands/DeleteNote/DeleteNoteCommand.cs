using MediatR;
using Noma.ApplicationL.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Noma.ApplicationL.Notes.Commands.DeleteNote
{
    public class DeleteNoteCommand: IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteNoteCommandHandler: IRequestHandler<DeleteNoteCommand>
    {
        private readonly INoteRepository noteRepository;

        public DeleteNoteCommandHandler(INoteRepository noteRepository)
        {
            this.noteRepository = noteRepository;
        }

        public async Task<Unit> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            await noteRepository.DeleteNote(request.Id);

            return Unit.Value;
        }
    }
}

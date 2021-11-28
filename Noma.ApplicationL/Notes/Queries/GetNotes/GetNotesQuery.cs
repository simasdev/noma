using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Noma.ApplicationL.Common.Repositories;
using Noma.Domain.Entities;
using System.Linq;

namespace Noma.ApplicationL.Notes.Queries.GetNotes
{
    public class GetNotesQuery: IRequest<List<NoteDTO>>
    {
    }


    public class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, List<NoteDTO>>
    {
        private readonly INoteRepository noteRepository;
        private readonly IMapper mapper;

        public GetNotesQueryHandler(INoteRepository noteRepository, IMapper mapper)
        {
            this.noteRepository = noteRepository;
            this.mapper = mapper;
        }

        public async Task<List<NoteDTO>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
        {
            var notes = await noteRepository.GetNotes();

            return mapper.Map<IEnumerable<NoteDTO>>(notes).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Noma.ApplicationL.Common.CustomEventArguments
{
    public class NoteCreationEventArgs: EventArgs
    {
        public int CategoryId { get; set; }
        public string Content { get; set; }
    }
}

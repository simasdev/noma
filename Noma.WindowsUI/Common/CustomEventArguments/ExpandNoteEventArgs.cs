using Noma.WindowsUI.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noma.WindowsUI.Common.CustomEventArguments
{
    public class ExpandNoteEventArgs: EventArgs
    {
        private readonly Note note;
        public ExpandNoteEventArgs(Note note)
        {
            this.note = note;
        }

        public Note Note { get => note; }
    }
}

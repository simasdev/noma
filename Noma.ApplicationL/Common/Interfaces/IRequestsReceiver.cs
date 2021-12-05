using System;
using System.Collections.Generic;
using System.Text;
using Noma.ApplicationL.Common.CustomEventArguments;
namespace Noma.ApplicationL.Common.Interfaces
{
    public interface IRequestsReceiver
    {
       event EventHandler<NoteCreationEventArgs> NoteCreationRequested;

        void StartReceiving();
        void StopReceiving();
    }
}

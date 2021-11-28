using System;
using System.Collections.Generic;
using System.Text;

namespace Noma.WindowsUI.Common.CustomEventArguments
{
    public class FilterCriteriaChangedEventArgs: EventArgs
    {
        public string SearchInput { get; set; }
    }
}

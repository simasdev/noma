using Noma.WindowsUI.Enums;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noma.WindowsUI.Common.Base
{
    public abstract class ModelBase: BindableBase
    {
        public ModelState State { get; set; }

        protected virtual void ChangeState()
        {
            if (this.State == ModelState.Unmodified)
                this.State = ModelState.Modified;
        }
    }
}

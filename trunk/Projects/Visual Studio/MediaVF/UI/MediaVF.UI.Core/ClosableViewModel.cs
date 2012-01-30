using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaVF.UI.Core
{
    public class ClosableViewModel : ViewModelBase
    {
        #region Events

        public event Action Close;

        protected void RaiseClose()
        {
            if (Close != null)
                Close();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomFileCopier.Helpers
{
    class DispatcherWrapper 
        : IDispatcherWrapper
    {
        public void Invoke(Action action)
        {
            App.Current.Dispatcher.Invoke(action);
        }
    }
}

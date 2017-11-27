using System;

namespace RandomFileCopier.Helpers
{
    interface IDispatcherWrapper
    {
        void Invoke(Action action);
    }
}
using System;
using System.Diagnostics;

namespace iDecryptIt;

public class ReactiveUIExceptionHandler : IObserver<Exception>
{
    public void OnNext(Exception value)
    {
        if (Debugger.IsAttached)
            Debugger.Break();

        Program.FatalException(value);
    }

    public void OnError(Exception error)
    {
        if (Debugger.IsAttached)
            Debugger.Break();

        Program.FatalException(error);
    }

    public void OnCompleted()
    {
        if (Debugger.IsAttached)
            Debugger.Break();

        Program.FatalException(new NotSupportedException());
    }
}

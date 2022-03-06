using System;
using System.IO;

namespace iDecryptIt.IO;

public class DmgReader : IDisposable
{
    private readonly Stream _input;

    private DmgReader(Stream input)
    {
        if (!input.CanSeek)
            throw new ArgumentException("Input must be seekable.", nameof(input));

        _input = input;
        _input.Position = 0;

    }

    public static DmgReader Parse(Stream input) =>
        new(input);

#region IDisposable

    public void Dispose()
    {
        _input.Dispose();
        GC.SuppressFinalize(this);
    }

#endregion
}

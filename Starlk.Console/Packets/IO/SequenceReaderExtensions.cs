using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Starlk.Console.Packets.IO;

internal static class SequenceReaderExtensions
{
    public static bool TryReadVariableInteger(ref this SequenceReader<byte> reader, out int value)
    {
        value = default;

        var numbersRead = 0;
        var result = 0;

        byte read;

        do
        {
            if (!reader.TryRead(out read))
            {
                return false;
            }

            var temporaryValue = read & 0b01111111;
            result |= temporaryValue << (7 * numbersRead);

            numbersRead++;

            if (numbersRead > 5)
            {
                return false;
            }
        } while ((read & 0b10000000) != 0);

        value = result;
        return true;
    }

    public static bool TryReadVariableString(
        ref this SequenceReader<byte> reader,
        [NotNullWhen(true)] out string? value)
    {
        value = default;

        if (!reader.TryReadVariableInteger(out var length)
            || !reader.TryReadExact(length, out var buffer))
        {
            return false;
        }

        value = Encoding.UTF8.GetString(buffer);
        return true;
    }

    public static bool TryReadUnsignedShort(ref this SequenceReader<byte> reader, out ushort value)
    {
        // if (!reader.TryReadBigEndian(out short signedValue))
        // {
        //     value = default;
        //     return false;
        // }
        // 
        // value = (ushort) signedValue;
        // return true;

        Unsafe.SkipInit(out value);
        return reader.TryReadBigEndian(out Unsafe.As<ushort, short>(ref value));
    }
}
using System.Buffers.Binary;
using System.Text;

namespace Starlk.Console.Packets.IO;

internal ref struct SpanWriter
{
    public int Position { get; private set; }

    private readonly Span<byte> destination;

    public SpanWriter(Span<byte> destination)
    {
        this.destination = destination;
    }

    public void WriteVariableInteger(int value)
    {
        var unsigned = (uint) value;

        do
        {
            var current = (byte) (unsigned & 127);
            unsigned >>= 7;

            if (unsigned != 0)
            {
                current |= 128;
            }

            destination[Position++] = current;
        } while (unsigned != 0);
    }

    public void WriteVariableString(string value)
    {
        WriteVariableInteger(Encoding.UTF8.GetByteCount(value));
        Position += Encoding.UTF8.GetBytes(value, destination[Position..]);
    }

    public void WriteUnsignedShort(ushort value)
    {
        BinaryPrimitives.WriteUInt16BigEndian(destination[Position..], value);
        Position += sizeof(ushort);
    }

    public void WriteLong(long value)
    {
        BinaryPrimitives.WriteInt64BigEndian(destination[Position..], value);
        Position += sizeof(long);
    }
}
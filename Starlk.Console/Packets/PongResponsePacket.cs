using Starlk.Console.Packets.IO;

namespace Starlk.Console.Packets;

internal readonly struct PongResponsePacket : IOutgoingPacket
{
    public int Type => 0x01;

    public required long Payload { get; init; }

    public int CalculateLength()
    {
        return VariableIntegerHelper.GetBytesCount(Type) + sizeof(long);
    }

    public int Write(SpanWriter writer)
    {
        writer.WriteLong(Payload);
        return writer.Position;
    }
}
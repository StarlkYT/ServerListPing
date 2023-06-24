using System.Text;
using Starlk.Console.Packets.IO;

namespace Starlk.Console.Packets.StatusResponse;

internal readonly struct StatusResponsePacket : IOutgoingPacket
{
    public int Type => 0x00;

    public required string Payload { get; init; }

    public int CalculateLength()
    {
        var payloadLength = Encoding.UTF8.GetByteCount(Payload);

        return VariableIntegerHelper.GetBytesCount(Type)
               + VariableIntegerHelper.GetBytesCount(payloadLength)
               + Payload.Length;
    }

    public int Write(SpanWriter writer)
    {
        writer.WriteVariableString(Payload);
        return writer.Position;
    }
}
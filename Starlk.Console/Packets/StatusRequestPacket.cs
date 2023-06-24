using System.Buffers;

namespace Starlk.Console.Packets;

internal readonly struct StatusRequestPacket : IIngoingPacket<StatusRequestPacket>
{
    public int Type => 0x00;

    public static bool TryRead(in ReadOnlySequence<byte> sequence, out StatusRequestPacket packet)
    {
        packet = default;
        return true;
    }
}
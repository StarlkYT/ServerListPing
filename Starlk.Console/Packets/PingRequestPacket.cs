using System.Buffers;

namespace Starlk.Console.Packets;

internal readonly struct PingRequestPacket : IIngoingPacket<PingRequestPacket>
{
    public int Type => 0x00;

    public required long Payload { get; init; }

    public static bool TryRead(in ReadOnlySequence<byte> sequence, out PingRequestPacket packet)
    {
        var reader = new SequenceReader<byte>(sequence);

        if (!reader.TryReadBigEndian(out long payload))
        {
            packet = default;
            return false;
        }

        packet = new PingRequestPacket()
        {
            Payload = payload
        };

        return true;
    }
}
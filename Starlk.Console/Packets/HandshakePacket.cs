using System.Buffers;
using Starlk.Console.Packets.IO;

namespace Starlk.Console.Packets;

internal readonly struct HandshakePacket : IIngoingPacket<HandshakePacket>
{
    public int Type => 0x00;

    public required int ProtocolVersion { get; init; }

    public required string Address { get; init; }

    public required ushort Port { get; init; }

    public required int NextState { get; init; }

    public static bool TryRead(in ReadOnlySequence<byte> sequence, out HandshakePacket packet)
    {
        var reader = new SequenceReader<byte>(sequence);

        if (!reader.TryReadVariableInteger(out var protocolVersion)
            || !reader.TryReadVariableString(out var address)
            || !reader.TryReadUnsignedShort(out var port)
            || !reader.TryReadVariableInteger(out var nextState))
        {
            packet = default;
            return false;
        }

        packet = new HandshakePacket()
        {
            ProtocolVersion = protocolVersion,
            Address = address,
            Port = port,
            NextState = nextState
        };

        return true;
    }
}
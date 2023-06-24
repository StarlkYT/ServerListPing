using System.Buffers;
using Starlk.Console.Packets.IO;

namespace Starlk.Console.Packets;

internal interface IPacket
{
    public int Type { get; }
}

internal interface IIngoingPacket<TSelf> : IPacket where TSelf : IIngoingPacket<TSelf>
{
    public static abstract bool TryRead(in ReadOnlySequence<byte> sequence, out TSelf packet);
}

internal interface IOutgoingPacket : IPacket
{
    public int CalculateLength();

    public int Write(SpanWriter writer);
}
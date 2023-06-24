using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using Bedrock.Framework.Protocols;

namespace Starlk.Console.Packets.IO;

internal record struct Packet(int Type, ReadOnlySequence<byte> Payload);

internal sealed class PacketMessage : IMessageReader<Packet?>, IMessageWriter<IOutgoingPacket>
{
    public static PacketMessage Instance => new PacketMessage();

    public bool TryParseMessage(in ReadOnlySequence<byte> input, ref SequencePosition consumed,
        ref SequencePosition examined,
        [UnscopedRef] out Packet? message)
    {
        message = default;
        var reader = new SequenceReader<byte>(input);

        if (!reader.TryReadVariableInteger(out var length)
            || !reader.TryReadVariableInteger(out var type))
        {
            return false;
        }

        length -= VariableIntegerHelper.GetBytesCount(type);

        if (reader.Remaining < length)
        {
            return false;
        }

        var payload = input.Slice(reader.Position, length);
        message = new Packet(type, payload);

        consumed = payload.End;
        examined = consumed;
        return true;
    }

    public void WriteMessage(IOutgoingPacket message, IBufferWriter<byte> output)
    {
        var length = message.CalculateLength();
        var span = output.GetSpan(length);

        var writer = new SpanWriter(span);
        writer.WriteVariableInteger(length);
        writer.WriteVariableInteger(message.Type);

        var offset = message.Write(writer);
        output.Advance(offset);
    }
}
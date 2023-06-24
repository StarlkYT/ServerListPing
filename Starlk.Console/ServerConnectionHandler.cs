using Bedrock.Framework.Protocols;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Starlk.Console.Packets;
using Starlk.Console.Packets.IO;
using Starlk.Console.Packets.StatusResponse;

namespace Starlk.Console;

internal sealed class ServerConnectionHandler : ConnectionHandler
{
    private readonly ILogger<ServerConnectionHandler> logger;

    public ServerConnectionHandler(ILogger<ServerConnectionHandler> logger)
    {
        this.logger = logger;
    }

    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        var reader = connection.CreateReader();
        var result = await reader.ReadAsync(PacketMessage.Instance);

        if (result.Message is not { Type: 0x00 })
        {
            logger.LogWarning("Failed to read handshake packet");
            return;
        }

        if (!HandshakePacket.TryRead(result.Message.Value.Payload, out var handshakePacket)
            || handshakePacket.NextState != 1)
        {
            logger.LogWarning("Received invalid handshake packet");
            return;
        }

        logger.LogInformation("Read handshake packet");

        while (!connection.ConnectionClosed.IsCancellationRequested)
        {
            reader.Advance();
            result = await reader.ReadAsync(PacketMessage.Instance);

            if (result.Message is not { Type: 0x00 or 0x01 })
            {
                logger.LogWarning("Failed to read packet (expected status/ping request)");
                return;
            }

            logger.LogInformation("Read packet type {Type}", result.Message.Value.Type);

            var writer = connection.CreateWriter();

            switch (result.Message.Value.Type)
            {
                case 0x00:
                    if (!StatusRequestPacket.TryRead(result.Message.Value.Payload, out _))
                    {
                        logger.LogWarning("Received invalid status request packet");
                        break;
                    }

                    await writer.WriteAsync(PacketMessage.Instance, new StatusResponsePacket()
                    {
                        Payload = ServerStatus
                            .Create("Starlk", 763, 0, 0, Chat.Create("C#", color: "blue"))
                            .Serialize()
                    });
                    break;

                case 0x01:
                    if (!PingRequestPacket.TryRead(result.Message.Value.Payload, out var pingRequestPacket))
                    {
                        logger.LogWarning("Received invalid ping request packet");
                        break;
                    }

                    await writer.WriteAsync(PacketMessage.Instance, new PongResponsePacket()
                    {
                        Payload = pingRequestPacket.Payload
                    });

                    logger.LogInformation("Responded to ping request packet");
                    return;
            }
        }
    }
}
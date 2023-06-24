using System.Text.Json.Serialization;

namespace Starlk.Console.Packets.StatusResponse;

internal sealed class ServerStatus
{
    public required ServerVersion Version { get; init; }

    [JsonPropertyName("players")]
    public required PlayerInformation PlayerInformation { get; init; }

    public required Chat Description { get; init; }

    public string Favicon { get; init; } = string.Empty;

    public static ServerStatus Create(
        string name,
        int protocol,
        int max,
        int online,
        Chat description,
        string favicon = "")
    {
        return new ServerStatus()
        {
            Version = new ServerVersion()
            {
                Name = name,
                Protocol = protocol
            },
            PlayerInformation = new PlayerInformation()
            {
                Max = max,
                Online = online
            },
            Description = description,
            Favicon = favicon
        };
    }
}

public sealed class ServerVersion
{
    public required string Name { get; init; }

    public required int Protocol { get; init; }
}

public sealed class PlayerInformation
{
    public required int Max { get; init; }

    public required int Online { get; init; }
}
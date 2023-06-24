namespace Starlk.Console.Packets.StatusResponse;

internal sealed class Chat
{
    public string? Text { get; init; }

    public bool Bold { get; init; }

    public bool Italic { get; init; }

    public bool Underlined { get; init; }

    public bool StrikeThrough { get; init; }

    public bool Obfuscated { get; init; }

    public string Color { get; init; } = "white";

    public Chat[]? Extra { get; init; }

    public static Chat Create(string text, bool bold = false, bool italic = false, bool underlined = false,
        bool strikeThrough = false, bool obfuscated = false, string color = "white")
    {
        return new Chat()
        {
            Text = text,
            Bold = bold,
            Italic = italic,
            Underlined = underlined,
            StrikeThrough = strikeThrough,
            Obfuscated = obfuscated,
            Color = color
        };
    }
}
﻿using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Starlk.Console.Packets.StatusResponse;

internal static class MinecraftJsonSerializerOptions
{
    public static JsonSerializerOptions Default => new JsonSerializerOptions()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}

public static class MinecraftJsonSerializerExtensions
{
    public static string Serialize<T>(this T instance)
    {
        return JsonSerializer.Serialize(instance, MinecraftJsonSerializerOptions.Default);
    }
}
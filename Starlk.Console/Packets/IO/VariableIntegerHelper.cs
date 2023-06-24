﻿using System.Numerics;

namespace Starlk.Console.Packets.IO;

internal static class VariableIntegerHelper
{
    public static int GetBytesCount(int value)
    {
        return (BitOperations.LeadingZeroCount((uint) value | 1) - 38) * -1171 >> 13;
    }
}
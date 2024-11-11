using System;

namespace InfluxDB.Flux.Builder.Options
{
    [Flags]
    public enum ParameterizedTypes : ushort
    {
        None       = 0b0000000000, // 0
        Identifier = 0b0000000001, // 1
        String     = 0b0000000010, // 2
        Boolean    = 0b0000000100, // 4
        Integer    = 0b0000001000, // 8
        UInteger   = 0b0000010000, // 16
        Float      = 0b0000100000, // 32
        Time       = 0b0001000000, // 64
        Duration   = 0b0010000000, // 128
        Array      = 0b0100000000, // 256
        Object     = 0b1000000000, // 512

        Numeric = Integer | UInteger | Float,
        All = Identifier | String | Boolean | Numeric | Time | Duration | Array | Object
    }

    public static class ParameterizedTypesExtensions
    {
        /// <summary>
        /// Determines whether one or more bit fields are set in the current instance.
        /// </summary>
        /// <remarks>
        /// Similar to <see cref="Enum.HasFlag(Enum)"/> but avoids .NET Framework and .NET Core &lt; 2.1 performance issues.
        /// See <see href="https://github.com/dotnet/runtime/issues/6080">GitHub issue</see> and
        /// <see href="https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-2-1/#gist89028118">blog post</see>.
        /// </remarks>
        public static bool IsSet(this ParameterizedTypes self, ParameterizedTypes flag) => (self & flag) == flag;
    }
}

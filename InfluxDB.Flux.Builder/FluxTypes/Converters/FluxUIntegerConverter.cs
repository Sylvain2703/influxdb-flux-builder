using InfluxDB.Client.Api.Domain;
using System.Globalization;

namespace InfluxDB.Flux.Builder.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert unsigned integer to Flux notation and Flux AST representation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/uint/">Unsigned integer - InfluxDB documentation</seealso>
    public static class FluxUIntegerConverter
    {
        public static string ToFluxNotation(this byte value) => WithUIntConversionAsFluxNotation(value.ToString(CultureInfo.InvariantCulture));
        public static string ToFluxNotation(this ushort value) => WithUIntConversionAsFluxNotation(value.ToString(CultureInfo.InvariantCulture));
        public static string ToFluxNotation(this uint value) => WithUIntConversionAsFluxNotation(value.ToString(CultureInfo.InvariantCulture));
        public static string ToFluxNotation(this ulong value) => WithUIntConversionAsFluxNotation(value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Ensures support for unsigned integers.
        /// </summary>
        private static string WithUIntConversionAsFluxNotation(string value) => $"uint(v: \"{value}\")";

        public static UnsignedIntegerLiteral ToFluxAstNode(this byte value) => new(nameof(UnsignedIntegerLiteral), value.ToString(CultureInfo.InvariantCulture));
        public static UnsignedIntegerLiteral ToFluxAstNode(this ushort value) => new(nameof(UnsignedIntegerLiteral), value.ToString(CultureInfo.InvariantCulture));
        public static UnsignedIntegerLiteral ToFluxAstNode(this uint value) => new(nameof(UnsignedIntegerLiteral), value.ToString(CultureInfo.InvariantCulture));
        public static UnsignedIntegerLiteral ToFluxAstNode(this ulong value) => new(nameof(UnsignedIntegerLiteral), value.ToString(CultureInfo.InvariantCulture));
    }
}

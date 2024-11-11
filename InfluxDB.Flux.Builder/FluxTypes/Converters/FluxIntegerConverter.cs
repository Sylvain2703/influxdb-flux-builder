using InfluxDB.Client.Api.Domain;
using System.Globalization;

namespace InfluxDB.Flux.Builder.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert integer to Flux notation and Flux AST representation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/int/">Integer - InfluxDB documentation</seealso>
    public static class FluxIntegerConverter
    {
        public static string ToFluxNotation(this sbyte value) => value.ToString(CultureInfo.InvariantCulture);
        public static string ToFluxNotation(this short value) => value.ToString(CultureInfo.InvariantCulture);
        public static string ToFluxNotation(this int value) => value.ToString(CultureInfo.InvariantCulture);
        public static string ToFluxNotation(this long value) => value.ToString(CultureInfo.InvariantCulture);

        public static IntegerLiteral ToFluxAstNode(this sbyte value) => new(nameof(IntegerLiteral), value.ToString(CultureInfo.InvariantCulture));
        public static IntegerLiteral ToFluxAstNode(this short value) => new(nameof(IntegerLiteral), value.ToString(CultureInfo.InvariantCulture));
        public static IntegerLiteral ToFluxAstNode(this int value) => new(nameof(IntegerLiteral), value.ToString(CultureInfo.InvariantCulture));
        public static IntegerLiteral ToFluxAstNode(this long value) => new(nameof(IntegerLiteral), value.ToString(CultureInfo.InvariantCulture));
    }
}

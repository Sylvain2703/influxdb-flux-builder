using InfluxDB.Client.Api.Domain;
using System;
using System.Text.Encodings.Web;

namespace InfluxDB.Flux.Builder.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert string to Flux notation and Flux AST representation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/string/">String - InfluxDB documentation</seealso>
    public static class FluxStringConverter
    {
        public static string ToFluxNotation(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), FluxAnyTypeConverter.NullValueMessage);

            // Encode string to escape quotes.
            // There is no guarantee that this completely protects against Flux injection attacks!
            // Passing parameters separately as a Flux AST is therefore recommended.
            return $"\"{JavaScriptEncoder.UnsafeRelaxedJsonEscaping.Encode(value)}\"";
        }

        public static StringLiteral ToFluxAstNode(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), FluxAnyTypeConverter.NullValueMessage);

            return new(nameof(StringLiteral), value);
        }
    }
}

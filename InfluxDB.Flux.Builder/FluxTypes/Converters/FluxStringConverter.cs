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

            // Encode the string to escape specific characters according to https://docs.influxdata.com/flux/latest/spec/lexical-elements/#string-literals.
            // This should protect against Flux injection attacks, but passing parameters separately as a Flux AST is still recommended.
            return '"' + JavaScriptEncoder.UnsafeRelaxedJsonEscaping.Encode(value).Replace("${", "\\${") + '"';
        }

        public static StringLiteral ToFluxAstNode(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), FluxAnyTypeConverter.NullValueMessage);

            return new(nameof(StringLiteral), value);
        }
    }
}

using InfluxDB.Client.Api.Domain;
using NodaTime;
using System;
using Duration = NodaTime.Duration;

namespace InfluxDB.Flux.Builder.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert duration to Flux notation and Flux AST representation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/duration/">Duration - InfluxDB documentation</seealso>
    public static class FluxDurationConverter
    {
        public static FluxDuration AsFluxDuration(this TimeSpan value) => value;
        public static FluxDuration AsFluxDuration(this Duration value) => value;
        public static FluxDuration AsFluxDuration(this Period value) => value ?? throw new ArgumentNullException(nameof(value), FluxAnyTypeConverter.NullValueMessage);

        public static string ToFluxNotation(this TimeSpan value) => value.AsFluxDuration().ToFluxNotation();
        public static string ToFluxNotation(this Duration value) => value.AsFluxDuration().ToFluxNotation();
        public static string ToFluxNotation(this Period value) => value.AsFluxDuration().ToFluxNotation();

        public static Expression ToFluxAstNode(this TimeSpan value) => value.AsFluxDuration().ToFluxAstNode();
        public static Expression ToFluxAstNode(this Duration value) => value.AsFluxDuration().ToFluxAstNode();
        public static Expression ToFluxAstNode(this Period value) => value.AsFluxDuration().ToFluxAstNode();
    }
}

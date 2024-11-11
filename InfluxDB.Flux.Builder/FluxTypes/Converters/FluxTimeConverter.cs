using InfluxDB.Client.Api.Domain;
using NodaTime;
using System;

namespace InfluxDB.Flux.Builder.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert time to Flux notation and Flux AST representation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/time/">Time - InfluxDB documentation</seealso>
    public static class FluxTimeConverter
    {
        public static FluxTime AsFluxTime(this DateTime value) => value;
        public static FluxTime AsFluxTime(this DateTimeOffset value) => value;
#if NET6_0_OR_GREATER
        public static FluxTime AsFluxTime(this DateOnly value) => value;
#endif
        public static FluxTime AsFluxTime(this Instant value) => value;
        public static FluxTime AsFluxTime(this ZonedDateTime value) => value;
        public static FluxTime AsFluxTime(this OffsetDateTime value) => value;
        public static FluxTime AsFluxTime(this OffsetDate value) => value;
        public static FluxTime AsFluxTime(this LocalDateTime value) => value;
        public static FluxTime AsFluxTime(this LocalDate value) => value;

        public static string ToFluxNotation(this DateTime value) => value.AsFluxTime().ToFluxNotation();
        public static string ToFluxNotation(this DateTimeOffset value) => value.AsFluxTime().ToFluxNotation();
#if NET6_0_OR_GREATER
        public static string ToFluxNotation(this DateOnly value) => value.AsFluxTime().ToFluxNotation();
#endif
        public static string ToFluxNotation(this Instant value) => value.AsFluxTime().ToFluxNotation();
        public static string ToFluxNotation(this ZonedDateTime value) => value.AsFluxTime().ToFluxNotation();
        public static string ToFluxNotation(this OffsetDateTime value) => value.AsFluxTime().ToFluxNotation();
        public static string ToFluxNotation(this OffsetDate value) => value.AsFluxTime().ToFluxNotation();
        public static string ToFluxNotation(this LocalDateTime value) => value.AsFluxTime().ToFluxNotation();
        public static string ToFluxNotation(this LocalDate value) => value.AsFluxTime().ToFluxNotation();

        public static Expression ToFluxAstNode(this DateTime value) => value.AsFluxTime().ToFluxAstNode();
        public static Expression ToFluxAstNode(this DateTimeOffset value) => value.AsFluxTime().ToFluxAstNode();
#if NET6_0_OR_GREATER
        public static Expression ToFluxAstNode(this DateOnly value) => value.AsFluxTime().ToFluxAstNode();
#endif
        public static Expression ToFluxAstNode(this Instant value) => value.AsFluxTime().ToFluxAstNode();
        public static Expression ToFluxAstNode(this ZonedDateTime value) => value.AsFluxTime().ToFluxAstNode();
        public static Expression ToFluxAstNode(this OffsetDateTime value) => value.AsFluxTime().ToFluxAstNode();
        public static Expression ToFluxAstNode(this OffsetDate value) => value.AsFluxTime().ToFluxAstNode();
        public static Expression ToFluxAstNode(this LocalDateTime value) => value.AsFluxTime().ToFluxAstNode();
        public static Expression ToFluxAstNode(this LocalDate value) => value.AsFluxTime().ToFluxAstNode();
    }
}

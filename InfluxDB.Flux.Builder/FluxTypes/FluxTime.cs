using InfluxDB.Client.Api.Domain;
using NodaTime;
using NodaTime.Text;
using System;
using DateTimeLiteral = InfluxDB.Flux.Builder.Parameterization.DateTimeLiteral;

namespace InfluxDB.Flux.Builder.FluxTypes
{
    /// <summary>
    /// Represents a single point in time in Flux with nanosecond precision.
    /// </summary>
    /// <remarks>
    /// This can only be obtained by the implicit conversion of a <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="Instant"/>,
    /// <see cref="ZonedDateTime"/>, <see cref="OffsetDateTime"/>, <see cref="OffsetDate"/>, <see cref="LocalDateTime"/> or <see cref="LocalDate"/>.
    /// </remarks>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/time/">Time - InfluxDB documentation</seealso>
    public sealed class FluxTime : FluxTimeable
    {
        private readonly Instant _value;

        private FluxTime(Instant value) => _value = value;

        public static implicit operator FluxTime(DateTime value) => new(Instant.FromDateTimeUtc(value.ToUniversalTime()));
        public static implicit operator FluxTime(DateTimeOffset value) => new(Instant.FromDateTimeOffset(value));
#if NET6_0_OR_GREATER
        public static implicit operator FluxTime(DateOnly value) => new(Instant.FromDateTimeUtc(value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)));
#endif
        public static implicit operator FluxTime(Instant value) => new(value);
        public static implicit operator FluxTime(ZonedDateTime value) => new(value.ToInstant());
        public static implicit operator FluxTime(OffsetDateTime value) => new(value.ToInstant());
        public static implicit operator FluxTime(OffsetDate value) => new(value.At(LocalTime.Midnight).ToInstant());
        public static implicit operator FluxTime(LocalDateTime value) => new(value.WithOffset(Offset.Zero).ToInstant());
        public static implicit operator FluxTime(LocalDate value) => new(value.At(LocalTime.Midnight).WithOffset(Offset.Zero).ToInstant());

        public override string ToString() => ToFluxNotation();

        public override string ToFluxNotation() => InstantPattern.ExtendedIso.Format(_value);

        public override Expression ToFluxAstNode() => new DateTimeLiteral(nameof(DateTimeLiteral), _value);
    }
}

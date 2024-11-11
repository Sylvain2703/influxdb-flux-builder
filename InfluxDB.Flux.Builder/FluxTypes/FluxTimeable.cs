using InfluxDB.Client.Api.Domain;
using InfluxDB.Flux.Builder.FluxTypes.Converters;
using NodaTime;
using System;
using System.Collections.Generic;
using Duration = NodaTime.Duration;

namespace InfluxDB.Flux.Builder.FluxTypes
{
    /// <summary>
    /// Represents a single point in time in Flux with nanosecond precision.
    /// It can be an absolute date and time, or a duration relative to <c>now()</c>.
    /// </summary>
    /// <remarks>
    /// This can only be obtained by the implicit conversion of a <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="Instant"/>,
    /// <see cref="ZonedDateTime"/>, <see cref="OffsetDateTime"/>, <see cref="OffsetDate"/>, <see cref="LocalDateTime"/>, <see cref="LocalDate"/>
    /// or a duration relative to <c>now()</c>, such as <see cref="TimeSpan"/>, <see cref="Duration"/> or <see cref="Period"/>.
    /// </remarks>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/time/">Time - InfluxDB documentation</seealso>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/duration/">Duration - InfluxDB documentation</seealso>
    /// <seealso href="https://docs.influxdata.com/flux/latest/spec/types/#timeable-constraint">Timeable - InfluxDB documentation</seealso>
    public abstract class FluxTimeable : IFluxType
    {
        public abstract string ToFluxNotation();

        public abstract Expression ToFluxAstNode();

        public virtual bool CanConvertToFluxAstNode => true;

        public virtual IEnumerable<FluxPackageImport>? DependentPackageImports => null;


        public static implicit operator FluxTimeable(DateTime value) => value.AsFluxTime();
        public static implicit operator FluxTimeable(DateTimeOffset value) => value.AsFluxTime();
#if NET6_0_OR_GREATER
        public static implicit operator FluxTimeable(DateOnly value) => value.AsFluxTime();
#endif
        public static implicit operator FluxTimeable(Instant value) => value.AsFluxTime();
        public static implicit operator FluxTimeable(ZonedDateTime value) => value.AsFluxTime();
        public static implicit operator FluxTimeable(OffsetDateTime value) => value.AsFluxTime();
        public static implicit operator FluxTimeable(OffsetDate value) => value.AsFluxTime();
        public static implicit operator FluxTimeable(LocalDateTime value) => value.AsFluxTime();
        public static implicit operator FluxTimeable(LocalDate value) => value.AsFluxTime();

        public static implicit operator FluxTimeable(TimeSpan value) => value.AsFluxDuration();
        public static implicit operator FluxTimeable(Duration value) => value.AsFluxDuration();
        public static implicit operator FluxTimeable(Period value) => value.AsFluxDuration();
    }
}

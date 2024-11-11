using InfluxDB.Client.Api.Domain;
using System;
using System.Collections.Generic;

namespace InfluxDB.Flux.Builder.FluxTypes
{
    /// <summary>
    /// Represents a Flux function that returns a single point in time.
    /// </summary>
    public sealed class FluxTimeFunction : FluxTimeable
    {
        private readonly string _value;
        private readonly IEnumerable<FluxPackageImport>? _imports;

        private FluxTimeFunction(string value, IEnumerable<FluxPackageImport>? imports = null)
        {
            _value = value;
            _imports = imports;
        }

        public override string ToString() => ToFluxNotation();

        public override string ToFluxNotation() => _value;

        public override Expression ToFluxAstNode() => throw new NotSupportedException("Generating a Flux AST for time functions is not yet supported and not urgent since there is no risk of Flux injection (as functions are not user-defined)"); // TODO

        public override bool CanConvertToFluxAstNode => false;

        public override IEnumerable<FluxPackageImport>? DependentPackageImports => _imports;


        /// <summary>
        /// Returns the current system time at which the Flux script is executed.
        /// </summary>
        /// <remarks>It is cached at runtime, so all executions of this function in a Flux script return the same time value.</remarks>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/now/">now() function - InfluxDB documentation</seealso>
        public static readonly FluxTimeFunction Now = new("now()");

        /// <summary>
        /// Returns the <see cref="Now"/> timestamp truncated to the day unit.
        /// </summary>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/today/">today() function - InfluxDB documentation</seealso>
        public static readonly FluxTimeFunction Today = new("today()");

        /// <summary>
        /// Returns the current system time at which this function is executed.
        /// </summary>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/system/time/">system.time() function - InfluxDB documentation</seealso>
        public static readonly FluxTimeFunction SystemTime = new("system.time()", [FluxPackages.System]);

        /// <summary>
        /// Returns the earliest time InfluxDB can store a value.
        /// </summary>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/internal/influxql/#constants">Constants - InfluxDB documentation</seealso>
        public static readonly FluxTimeFunction MinValue = new("influxql.minTime", [FluxPackages.Internal_InfluxQL]);

        /// <summary>
        /// Returns the latest time InfluxDB can store a value.
        /// </summary>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/internal/influxql/#constants">Constants - InfluxDB documentation</seealso>
        public static readonly FluxTimeFunction MaxStorageValue = new("influxql.maxTime", [FluxPackages.Internal_InfluxQL]);

        /// <summary>
        /// <para>Returns the time immediately after <see cref="MaxStorageValue"/>, which is the latest time InfluxDB can store a value.</para>
        /// <para>This is useful to cover the complete time range for functions with an exclusive stop/end time.</para>
        /// </summary>
        public static readonly FluxTimeFunction MaxRangeValue = new("date.add(d: 1ns, to: influxql.maxTime)", [FluxPackages.Internal_InfluxQL, FluxPackages.Date]);
    }
}

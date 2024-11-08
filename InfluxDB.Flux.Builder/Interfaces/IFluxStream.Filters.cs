using InfluxDB.Flux.Builder.FluxTypes;
using NodaTime;
using System;

namespace InfluxDB.Flux.Builder
{
    public partial interface IFluxStream
    {
        /// <summary>
        /// Filters rows based on time bounds.
        /// </summary>
        /// <remarks>
        /// <para>Input data must have a <c>_time</c> column of type time. Rows with a <see langword="null"/> value in the <c>_time</c> are filtered.</para>
        /// <para>It adds a <c>_start</c> column with the value of start and a <c>_stop</c> column with the value of stop. These columns are added to the group key.</para>
        /// <para>Each input table's group key value is modified to fit within the time bounds. Tables with all rows outside the time bounds are filtered entirely.</para>
        /// </remarks>
        /// <param name="start">
        /// <para>Earliest time to include in results. Results include rows with <c>_time</c> values that match the specified start time.</para>
        /// <para>Use a duration relative to <c>now()</c> (eg. <see cref="TimeSpan"/>, <see cref="Duration"/> or <see cref="Period"/>) or an absolute time
        /// (eg. <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="Instant"/>, <see cref="OffsetDateTime"/>, etc.).</para>
        /// </param>
        /// <param name="stop">
        /// <para>Latest time to include in results. Results exclude rows with <c>_time</c> values that match the specified stop time. Default is <c>now()</c>.</para>
        /// <para>Use a duration relative to <c>now()</c> (eg. <see cref="TimeSpan"/>, <see cref="Duration"/> or <see cref="Period"/>) or an absolute time
        /// (eg. <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="Instant"/>, <see cref="OffsetDateTime"/>, etc.).</para>
        /// </param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/range/">range() function - InfluxDB documentation</seealso>
        IFluxStream Range(FluxTimeable start, FluxTimeable? stop = null);

        /// <summary>
        /// Filters data based on conditions defined in a predicate function.
        /// </summary>
        /// <param name="buildFilter">A function to build a filter predicate that evaluates to <see langword="true"/> or <see langword="false"/>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/filter/">filter() function - InfluxDB documentation</seealso>
        IFluxStream Filter(Func<FluxFilterBuilder, FluxCondition> buildFilter);
    }
}

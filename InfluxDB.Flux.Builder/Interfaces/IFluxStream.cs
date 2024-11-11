using InfluxDB.Flux.Builder.Parameterization;
using System;

namespace InfluxDB.Flux.Builder
{
    public partial interface IFluxStream : IFluxConfigurable, IFluxBuilder
    {
        /// <summary>
        /// Performs an operation specified in the <paramref name="rawFlux"/> interpolated string, on the piped data stream.
        /// </summary>
        /// <remarks>
        /// This method provides a built-in mechanism to protect against Flux injection attacks.
        /// Interpolated values in the <paramref name="rawFlux"/> query string will be parameterized automatically.
        /// </remarks>
        /// <param name="rawFlux">An interpolated string representing raw Flux (to operate on and return a data stream).</param>
        IFluxStream PipeCustomFlux(FormattableString rawFlux);

        /// <summary>
        /// Performs an operation returned by the <paramref name="rawFluxBuilder"/> function, on the piped data stream
        /// (without built-in protection against Flux injection attacks).
        /// </summary>
        /// <remarks>
        /// To prevent Flux injection attacks, <b>never pass a concatenated or interpolated string</b> (<c>$""</c>) with
        /// non-validated user-provided values into this method.<br/>Instead, use the <see cref="ParametersManager"/>
        /// argument provided by <paramref name="rawFluxBuilder"/> to parameterize the values, as below:
        /// <code>
        /// PipeCustomFluxUnsafe(p => "stddev(mode: " + p.Parameterize("stddevMode", mode) + ")")
        /// </code>
        /// </remarks>
        /// <param name="rawFluxBuilder">A function that builds a string representing raw Flux (to operate on and return a data stream).</param>
        IFluxStream PipeCustomFluxUnsafe(Func<ParametersManager, string> rawFluxBuilder);

        /// <summary>
        /// <para>Delivers input data as a result of the query.</para>
        /// <para>A query may have multiple yields, each identified by unique name specified in the name parameter.</para>
        /// </summary>
        /// <remarks>
        /// This function is implicit for queries that output a single stream of tables and is only necessary when yielding multiple results from a query.
        /// </remarks>
        /// <param name="name">Unique name for the yielded results. Default is <c>_results</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/yield/">yield() function - InfluxDB documentation</seealso>
        IFluxStream Yield(string? name = null);
    }
}

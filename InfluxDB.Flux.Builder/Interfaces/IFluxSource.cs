using InfluxDB.Flux.Builder.Parameterization;
using System;

namespace InfluxDB.Flux.Builder
{
    public interface IFluxSource
    {
        /// <summary>
        /// Queries data from an InfluxDB data source.
        /// </summary>
        /// <param name="bucket">Name of the bucket to query.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/from/">from() function - InfluxDB documentation</seealso>
        IFluxStream From(string bucket);

        /// <summary>
        /// Queries data from any supported data source specified in the <paramref name="rawFlux"/> interpolated string.
        /// </summary>
        /// <remarks>
        /// This method provides a built-in mechanism to protect against Flux injection attacks.
        /// Interpolated values in the <paramref name="rawFlux"/> string will be parameterized automatically.
        /// </remarks>
        /// <param name="rawFlux">An interpolated string representing raw Flux (to stream data from a source).</param>
        IFluxStream FromCustomFlux(FormattableString rawFlux);

        /// <summary>
        /// Queries data from any supported data source returned by the <paramref name="buildRawFlux"/> function
        /// (without built-in protection against Flux injection attacks).
        /// </summary>
        /// <remarks>
        /// To prevent Flux injection attacks, <b>never pass a concatenated or interpolated string</b> (<c>$""</c>) with
        /// non-validated user-provided values into this method.<br/>Instead, use the <see cref="ParametersManager"/>
        /// argument provided by <paramref name="buildRawFlux"/> to parameterize the values, as below:
        /// <code>
        /// FromCustomFluxUnsafe(p => "requests.do(method: " + p.Parameterize("httpMethod", method) + $", url: {p.Parameterize("serviceUrl", url)})")
        /// </code>
        /// </remarks>
        /// <param name="buildRawFlux">A function that builds a string representing raw Flux (to stream data from a source).</param>
        IFluxStream FromCustomFluxUnsafe(Func<ParametersManager, string> buildRawFlux);
    }
}

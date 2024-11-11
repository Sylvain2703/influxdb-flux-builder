using InfluxDB.Client.Api.Domain;

namespace InfluxDB.Flux.Builder
{
    public interface IFluxBuilder
    {
        /// <summary>
        /// Returns a <see cref="Query"/> that contains the generated Flux query, its parameters, imports and the value of <c>now()</c>.
        /// </summary>
        /// <param name="dialect">Options for tabular data output. Default output is <see href="https://docs.influxdata.com/influxdb/latest/reference/syntax/annotated-csv/">annotated CSV</see> with headers.</param>
        Query ToQuery(Dialect? dialect = null);

        /// <summary>
        /// Returns a string representation of the generated Flux query, its parameters, imports and the value of <c>now()</c>.
        /// </summary>
        /// <remarks>
        /// This string is <b>not suitable for direct execution</b> and is intended <b>only for use in debugging</b>.
        /// </remarks>
        string ToDebugQueryString();
    }
}

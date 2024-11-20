using InfluxDB.Flux.Builder.Options;
using System;

namespace InfluxDB.Flux.Builder
{
    public interface IFluxConfigurable
    {
        /// <summary>
        /// Adjusts <see cref="FluxBuilderOptions"/> configuration while building the Flux query.
        /// </summary>
        IFluxStream Configure(Action<FluxBuilderOptions> configureOptions);
    }
}

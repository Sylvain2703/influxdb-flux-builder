using InfluxDB.Flux.Builder.Extensions;
using InfluxDB.Flux.Builder.Parameterization;
using System;

namespace InfluxDB.Flux.Builder
{
    public partial class FluxQueryBuilder
    {
        /// <inheritdoc/>
        public IFluxStream From(string bucket)
        {
            _stringBuilder.Append("from(bucket: ").Append(_parameters.Parameterize("from_bucket", bucket)).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream FromCustomFlux(FormattableString rawFlux)
        {
            _stringBuilder.Append(_parameters.Parameterize(rawFlux, "fromCustomFlux"));
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream FromCustomFluxUnsafe(Func<ParametersManager, string> buildRawFlux)
        {
            _stringBuilder.Append(buildRawFlux(_parameters));
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Yield(string? name = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("yield(");

            if (!string.IsNullOrWhiteSpace(name))
                _stringBuilder.Append("name: ").Append(_parameters.Parameterize("yield_name", name));

            _stringBuilder.Append(')');
            return this;
        }
    }
}

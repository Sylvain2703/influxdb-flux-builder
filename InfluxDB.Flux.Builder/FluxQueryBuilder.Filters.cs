using InfluxDB.Flux.Builder.Extensions;
using InfluxDB.Flux.Builder.FluxTypes;
using System;

namespace InfluxDB.Flux.Builder
{
    public partial class FluxQueryBuilder
    {
        /// <inheritdoc/>
        public IFluxStream Range(FluxTimeable start, FluxTimeable? stop = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("range(start: ").Append(_parameters.Parameterize("range_start", start));

            if (stop != null)
                _stringBuilder.Append(", stop: ").Append(_parameters.Parameterize("range_stop", stop));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Filter(Action<FluxFilter> filterAction)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("filter(fn: (r) => ");

            var filter = new FluxFilter(_stringBuilder, _options, _parameters);
            filterAction.Invoke(filter);

            _stringBuilder.Append(')');
            return this;
        }
    }
}

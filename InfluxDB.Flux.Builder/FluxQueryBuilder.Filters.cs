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
        public IFluxStream Filter(Func<FluxFilterBuilder, FluxCondition> buildFilter)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("filter(fn: (r) => ");

            var filterBuilder = new FluxFilterBuilder(_stringBuilder, _options, _parameters);
            buildFilter.Invoke(filterBuilder).Invoke();

            _stringBuilder.Append(')');
            return this;
        }
    }
}

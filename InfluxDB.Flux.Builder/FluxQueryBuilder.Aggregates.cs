using InfluxDB.Flux.Builder.Extensions;
using InfluxDB.Flux.Builder.FluxTypes;

namespace InfluxDB.Flux.Builder
{
    public partial class FluxQueryBuilder
    {
        /// <inheritdoc/>
        public IFluxStream Aggregate(FluxIdentifier function, string? column = null) =>
            AggregateCore("aggregate", column, _parameters.Parameterize("aggregate_fn", function));

        private IFluxStream AggregateCore(string trustedFunctionName, string? column, string? parameterizedFunction = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append(parameterizedFunction ?? trustedFunctionName).Append('(');

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append("column: ").Append(_parameters.Parameterize(trustedFunctionName + "_column", column));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Sum(string? column = null) => AggregateCore("sum", column);

        /// <inheritdoc/>
        public IFluxStream Count(string? column = null) => AggregateCore("count", column);

        /// <inheritdoc/>
        public IFluxStream Mean(string? column = null) => AggregateCore("mean", column);

        /// <inheritdoc/>
        public IFluxStream MovingAverage(int n)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("movingAverage(n: ").Append(_parameters.Parameterize("movingAverage_n", n)).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream TimedMovingAverage(FluxDuration every, FluxDuration period, string? column = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("timedMovingAverage(every: ").Append(_parameters.Parameterize("timedMovingAverage_every", every))
                .Append(", period: ").Append(_parameters.Parameterize("timedMovingAverage_period", period));

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: ").Append(_parameters.Parameterize("timedMovingAverage_column", column));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Mode(string? column = null) => AggregateCore("mode", column);

        /// <inheritdoc/>
        public IFluxStream Spread(string? column = null) => AggregateCore("spread", column);


        /// <inheritdoc/>
        public IFluxStream Window(FluxDuration every, FluxDuration? period = null, FluxDuration? offset = null,
            FluxLocation? location = null, string? timeColumn = null, string? startColumn = null, string? stopColumn = null, bool createEmpty = false)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("window(every: ").Append(_parameters.Parameterize("window_every", every));

            if (period != null)
                _stringBuilder.Append(", period: ").Append(_parameters.Parameterize("window_period", period));

            if (offset != null)
                _stringBuilder.Append(", offset: ").Append(_parameters.Parameterize("window_offset", offset));

            if (location != null)
                _stringBuilder.Append(", location: ").Append(_parameters.Parameterize("window_location", location));

            if (!string.IsNullOrWhiteSpace(timeColumn))
                _stringBuilder.Append(", timeColumn: ").Append(_parameters.Parameterize("window_timeColumn", timeColumn));

            if (!string.IsNullOrWhiteSpace(startColumn))
                _stringBuilder.Append(", startColumn: ").Append(_parameters.Parameterize("window_startColumn", startColumn));

            if (!string.IsNullOrWhiteSpace(stopColumn))
                _stringBuilder.Append(", stopColumn: ").Append(_parameters.Parameterize("window_stopColumn", stopColumn));

            _stringBuilder.Append(", createEmpty: ").Append(_parameters.Parameterize("window_createEmpty", createEmpty)).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream AggregateWindow(FluxIdentifier aggregateFunction, FluxDuration every, FluxDuration? period = null, FluxDuration? offset = null,
            FluxLocation? location = null, string? column = null, string? timeSrcColumn = null, string? timeDstColumn = null, bool createEmpty = true)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("aggregateWindow(fn: ").Append(_parameters.Parameterize("aggregateWindow_fn", aggregateFunction))
                .Append(", every: ").Append(_parameters.Parameterize("aggregateWindow_every", every));

            if (period != null)
                _stringBuilder.Append(", period: ").Append(_parameters.Parameterize("aggregateWindow_period", period));

            if (offset != null)
                _stringBuilder.Append(", offset: ").Append(_parameters.Parameterize("aggregateWindow_offset", offset));

            if (location != null)
                _stringBuilder.Append(", location: ").Append(_parameters.Parameterize("aggregateWindow_location", location));

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: ").Append(_parameters.Parameterize("aggregateWindow_column", column));

            if (!string.IsNullOrWhiteSpace(timeSrcColumn))
                _stringBuilder.Append(", timeSrc: ").Append(_parameters.Parameterize("aggregateWindow_timeSrc", timeSrcColumn));

            if (!string.IsNullOrWhiteSpace(timeDstColumn))
                _stringBuilder.Append(", timeDst: ").Append(_parameters.Parameterize("aggregateWindow_timeDst", timeDstColumn));

            _stringBuilder.Append(", createEmpty: ").Append(_parameters.Parameterize("aggregateWindow_createEmpty", createEmpty)).Append(')');
            return this;
        }
    }
}

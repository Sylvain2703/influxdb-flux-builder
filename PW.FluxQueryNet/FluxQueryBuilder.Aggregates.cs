﻿using PW.FluxQueryNet.Extensions;

namespace PW.FluxQueryNet
{
    public partial class FluxQueryBuilder
    {
        /// <summary>
        /// Returns the result of the Flux function specified by <paramref name="methodName"/>.
        /// </summary>
        /// <param name="methodName">Name of the Flux function.</param>
        /// <param name="column">Column to operate on.</param>
        public FluxQueryBuilder Aggregate(string methodName, string? column = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> ").Append(methodName).Append('(');

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append("column: \"").Append(column).Append('"');

            _stringBuilder.Append(')');
            return this;
        }

        /// <summary>
        /// Returns the sum of non-<see langword="null"/> values in a specified <paramref name="column"/>.
        /// </summary>
        /// <param name="column">Column to operate on. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/sum/">sum() function - InfluxDB documentation</seealso>
        public FluxQueryBuilder Sum(string? column = null) => Aggregate("sum", column);

        /// <summary>
        /// <para>Returns the number of records in each input table.</para>
        /// <para>The function counts both <see langword="null"/> and non-<see langword="null"/> records.</para>
        /// </summary>
        /// <remarks>This function returns <c>0</c> for empty tables.</remarks>
        /// <param name="column">Column to count values in and store the total count.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/count/">count() function - InfluxDB documentation</seealso>
        public FluxQueryBuilder Count(string? column = null) => Aggregate("count", column);

        /// <summary>
        /// Returns the average of non-<see langword="null"/> values in a specified <paramref name="column"/> from each input table.
        /// </summary>
        /// <param name="column">Column to use to compute means. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/mean/">mean() function - InfluxDB documentation</seealso>
        public FluxQueryBuilder Mean(string? column = null) => Aggregate("mean", column);

        /// <summary>
        /// Calculates the mean of non-<see langword="null"/> values using the current value and <c>n - 1</c> previous values in the <c>_value</c> column.
        /// </summary>
        /// <remarks>
        /// Moving average rules:
        /// <list type="bullet">
        ///     <item>The average over a period populated by <paramref name="n"/> values is equal to their algebraic mean.</item>
        ///     <item>The average over a period populated by only <see langword="null"/> values is <see langword="null"/>.</item>
        ///     <item>Moving averages skip <see langword="null"/> values.</item>
        ///     <item>If <paramref name="n"/> is less than the number of records in a table, it returns the average of the available values.</item>
        /// </list>
        /// </remarks>
        /// <param name="n">Number of values to average.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/movingaverage/">movingAverage() function - InfluxDB documentation</seealso>
        public FluxQueryBuilder MovingAverage(int n)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> movingAverage(n: ").Append(n.ToFlux()).Append(')');
            return this;
        }

        /// <summary>
        /// Calculates the mean of values in a defined time range at a specified frequency.
        /// </summary>
        /// <remarks>
        /// <paramref name="every"/> and <paramref name="period"/> parameters support all valid duration units (eg. <c>1s</c>, <c>1m</c>, <c>1h</c>, <c>1d</c>),
        /// including calendar months (<c>1mo</c>) and years (<c>1y</c>).<br/> When aggregating by week (<c>1w</c>), all calculated weeks begin on Thursday
        /// since weeks are determined using the Unix epoch (1970-01-01 00:00:00 UTC) which was a Thursday.
        /// </remarks>
        /// <param name="every">Frequency of time window.</param>
        /// <param name="period">Length of each averaged time window. A negative duration indicates start and stop boundaries are reversed.</param>
        /// <param name="column">Column to operate on. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/timedmovingaverage/">timedMovingAverage() function - InfluxDB documentation</seealso>
        public FluxQueryBuilder TimedMovingAverage(FluxDuration every, FluxDuration period, string? column = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> timedMovingAverage(every: ").Append(every).Append(", period: ").Append(period);

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: \"").Append(column).Append('"');

            _stringBuilder.Append(')');
            return this;
        }

        /// <summary>
        /// <para>Returns the non-<see langword="null"/> value or values that occur most often in a specified <paramref name="column"/> in each input table.</para>
        /// <para>If there are multiple modes, it returns all mode values in a sorted table. If there is no mode, it returns <see langword="null"/>.</para>
        /// </summary>
        /// <remarks>This function drops empty tables.</remarks>
        /// <param name="column">Column to return the mode from. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/mode/">mode() function - InfluxDB documentation</seealso>
        public FluxQueryBuilder Mode(string? column = null) => Aggregate("mode", column);

        /// <summary>
        /// Returns the difference between the minimum and maximum values in a specified <paramref name="column"/>.
        /// </summary>
        /// <param name="column">Column to operate on. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/spread/">spread() function - InfluxDB documentation</seealso>
        public FluxQueryBuilder Spread(string? column = null) => Aggregate("spread", column);


        /// <summary>
        /// Groups records using regular time intervals.
        /// </summary>
        /// <param name="every">Duration of time between windows.</param>
        /// <param name="period">Duration of windows. A negative duration indicates start and stop boundaries are reversed. Default is the <paramref name="every"/> value.</param>
        /// <param name="offset">Duration to shift the window boundaries by. A negative duration indicates the offset goes backwards in time. Default is <c>0s</c>.</param>
        /// <param name="location">Location used to determine timezone.</param>
        /// <param name="timeColumn">Column that contains time values. Default is <c>_time</c>.</param>
        /// <param name="startColumn">Column to store the window start time in. Default is <c>_start</c>.</param>
        /// <param name="stopColumn">Column to store the window stop time in. Default is <c>_stop</c>.</param>
        /// <param name="createEmpty">Create empty tables for empty window. Default is <see langword="false"/>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/window/">window() function - InfluxDB documentation</seealso>
        public FluxQueryBuilder Window(FluxDuration every, FluxDuration? period = null, FluxDuration? offset = null,
            string? location = null, string? timeColumn = null, string? startColumn = null, string? stopColumn = null, bool createEmpty = false)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> window(every: ").Append(every);

            if (period != null)
                _stringBuilder.Append(", period: ").Append(period);

            if (offset != null)
                _stringBuilder.Append(", offset: ").Append(offset);

            if (!string.IsNullOrWhiteSpace(location))
                _stringBuilder.Append(", location: ").Append(location);

            if (!string.IsNullOrWhiteSpace(timeColumn))
                _stringBuilder.Append(", timeColumn: \"").Append(timeColumn).Append('"');

            if (!string.IsNullOrWhiteSpace(startColumn))
                _stringBuilder.Append(", startColumn: \"").Append(startColumn).Append('"');

            if (!string.IsNullOrWhiteSpace(stopColumn))
                _stringBuilder.Append(", stopColumn: \"").Append(stopColumn).Append('"');

            _stringBuilder.Append(", createEmpty: ").Append(createEmpty.ToFlux()).Append(')');
            return this;
        }

        /// <summary>
        /// Downsamples data by grouping data into fixed windows of time and applying an aggregate or selector function to each window.
        /// </summary>
        /// <param name="aggregateFunction">Aggregate or selector function to apply to each time window.</param>
        /// <param name="every">Duration of time between windows.</param>
        /// <param name="period">Duration of windows. A negative duration indicates start and stop boundaries are reversed. Default is the <paramref name="every"/> value.</param>
        /// <param name="offset">Duration to shift the window boundaries by. A negative duration indicates the offset goes backwards in time. Default is <c>0s</c>.</param>
        /// <param name="location">Location used to determine timezone.</param>
        /// <param name="column">Column to operate on.</param>
        /// <param name="timeSrcColumn">Column to use as the source of the new time value for aggregate values. Default is <c>_stop</c>.</param>
        /// <param name="timeDstColumn">Column to store time values for aggregate values in. Default is <c>_time</c>.</param>
        /// <param name="createEmpty">
        /// Create empty tables for empty window. Default is <see langword="true"/>.<br/>
        /// When <see langword="true"/>, aggregate functions return empty tables, but selector functions do not. By design, selectors drop empty tables.
        /// </param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/aggregatewindow/">aggregateWindow() function - InfluxDB documentation</seealso>
        public FluxQueryBuilder AggregateWindow(string aggregateFunction, FluxDuration every, FluxDuration? period = null, FluxDuration? offset = null,
            string? location = null, string? column = null, string? timeSrcColumn = null, string? timeDstColumn = null, bool createEmpty = true)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.Append("|> aggregateWindow(fn: ").Append(aggregateFunction).Append(", every: ").Append(every);

            if (period != null)
                _stringBuilder.Append(", period: ").Append(period);

            if (offset != null)
                _stringBuilder.Append(", offset: ").Append(offset);

            if (!string.IsNullOrWhiteSpace(location))
                _stringBuilder.Append(", location: ").Append(location);

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: \"").Append(column).Append('"');

            if (!string.IsNullOrWhiteSpace(timeSrcColumn))
                _stringBuilder.Append(", timeSrc: \"").Append(timeSrcColumn).Append('"');

            if (!string.IsNullOrWhiteSpace(timeDstColumn))
                _stringBuilder.Append(", timeDst: \"").Append(timeDstColumn).Append('"');

            _stringBuilder.Append(", createEmpty: ").Append(createEmpty.ToFlux()).Append(')');
            return this;
        }
    }
}

using InfluxDB.Client.Api.Domain;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;
using static NodaTime.NodaConstants;
using Duration = NodaTime.Duration;

namespace InfluxDB.Flux.Builder.FluxTypes
{
    /// <summary>
    /// Represents a length of time in Flux with nanosecond precision.
    /// </summary>
    /// <remarks>
    /// This can only be obtained by the implicit conversion of a <see cref="TimeSpan"/>, <see cref="Duration"/> or <see cref="Period"/>.
    /// </remarks>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/duration/">Duration - InfluxDB documentation</seealso>
    public sealed class FluxDuration : FluxTimeable
    {
        private const long MonthsPerYear = 12;
        private const long MicrosecondsPerMillisecond = 1000;

        // Use the same representation as in the Flux specification and source code:
        // https://github.com/influxdata/flux/blob/v0.194.5/docs/SPEC.md?plain=1#L171
        // https://github.com/influxdata/flux/blob/v0.194.5/values/time.go#L18-L29

        private readonly bool _isNegative;
        private readonly long _months; // Represents years and months.
        private readonly long _nanoseconds; // Represents days, hours, minutes, ..., and nanoseconds.

        private FluxDuration(long months, long nanoseconds)
        {
            if (months >= 0 && nanoseconds >= 0)
            {
                _isNegative = false;
                _months = months;
                _nanoseconds = nanoseconds;
            }
            else if (months <= 0 && nanoseconds <= 0)
            {
                _isNegative = true;
                _months = Math.Abs(months);
                _nanoseconds = Math.Abs(nanoseconds);
            }
            else
            {
                throw new ArgumentException("All values of a Flux duration must have the same sign.");
            }
        }

        public static implicit operator FluxDuration(TimeSpan value) => new(0, value.Ticks * NanosecondsPerTick);
        public static implicit operator FluxDuration(Duration value) => new(0, value.ToInt64Nanoseconds());
        public static implicit operator FluxDuration(Period value)
        {
            // Months and years are separated because the number of days in a month varies.
            var months =
                value.Months +
                value.Years * MonthsPerYear;

            var nanoseconds =
                value.Nanoseconds +
                value.Ticks        * NanosecondsPerTick +
                value.Milliseconds * NanosecondsPerMillisecond +
                value.Seconds      * NanosecondsPerSecond +
                value.Minutes      * NanosecondsPerMinute +
                value.Hours        * NanosecondsPerHour +
                value.Days         * NanosecondsPerDay +
                value.Weeks        * NanosecondsPerWeek;

            return new FluxDuration(months, nanoseconds);
        }

        public (bool isNegative, long years, long months, long weeks, long days, long hours, long minutes, long seconds, long milliseconds, long microseconds, long nanoseconds)
            GetNormalizedValues() =>
            (
                isNegative:   _isNegative,
                years:        _months / MonthsPerYear,
                months:       _months % MonthsPerYear,
                weeks:        _nanoseconds / NanosecondsPerWeek,
                days:         _nanoseconds / NanosecondsPerDay         % DaysPerWeek,
                hours:        _nanoseconds / NanosecondsPerHour        % HoursPerDay,
                minutes:      _nanoseconds / NanosecondsPerMinute      % MinutesPerHour,
                seconds:      _nanoseconds / NanosecondsPerSecond      % SecondsPerMinute,
                milliseconds: _nanoseconds / NanosecondsPerMillisecond % MillisecondsPerSecond,
                microseconds: _nanoseconds / NanosecondsPerMicrosecond % MicrosecondsPerMillisecond,
                nanoseconds:  _nanoseconds                             % NanosecondsPerMicrosecond
            );

        public override string ToString() => ToFluxNotation();

        public override string ToFluxNotation()
        {
            var (isNegative, years, months, weeks, days, hours, minutes, seconds, milliseconds, microseconds, nanoseconds) = GetNormalizedValues();

            var stringBuilder = new StringBuilder();

            if (isNegative)
                stringBuilder.Append('-');
            if (years != 0)
                stringBuilder.Append(years).Append('y');
            if (months != 0)
                stringBuilder.Append(months).Append("mo");
            if (weeks != 0)
                stringBuilder.Append(weeks).Append('w');
            if (days != 0)
                stringBuilder.Append(days).Append('d');
            if (hours != 0)
                stringBuilder.Append(hours).Append('h');
            if (minutes != 0)
                stringBuilder.Append(minutes).Append('m');
            if (seconds != 0)
                stringBuilder.Append(seconds).Append('s');
            if (milliseconds != 0)
                stringBuilder.Append(milliseconds).Append("ms");
            if (microseconds != 0)
                stringBuilder.Append(microseconds).Append("us");
            if (nanoseconds != 0)
                stringBuilder.Append(nanoseconds).Append("ns");

            if (stringBuilder.Length == 0 || (isNegative && stringBuilder.Length == 1))
                stringBuilder.Append("0ns");

            return stringBuilder.ToString();
        }

        public override Expression ToFluxAstNode()
        {
            var (isNegative, years, months, weeks, days, hours, minutes, seconds, milliseconds, microseconds, nanoseconds) = GetNormalizedValues();

            var values = new List<InfluxDB.Client.Api.Domain.Duration>();
            Expression expression = new DurationLiteral(nameof(DurationLiteral), values);

            if (isNegative)
                expression = new UnaryExpression(nameof(UnaryExpression), "-", expression);
            if (years != 0)
                values.Add(new(nameof(Duration), years, "y"));
            if (months != 0)
                values.Add(new(nameof(Duration), months, "mo"));
            if (weeks != 0)
                values.Add(new(nameof(Duration), weeks, "w"));
            if (days != 0)
                values.Add(new(nameof(Duration), days, "d"));
            if (hours != 0)
                values.Add(new(nameof(Duration), hours, "h"));
            if (minutes != 0)
                values.Add(new(nameof(Duration), minutes, "m"));
            if (seconds != 0)
                values.Add(new(nameof(Duration), seconds, "s"));
            if (milliseconds != 0)
                values.Add(new(nameof(Duration), milliseconds, "ms"));
            if (microseconds != 0)
                values.Add(new(nameof(Duration), microseconds, "us"));
            if (nanoseconds != 0)
                values.Add(new(nameof(Duration), nanoseconds, "ns"));

            if (values.Count == 0)
                values.Add(new(nameof(Duration), 0, "ns"));

            return expression;
        }
    }
}

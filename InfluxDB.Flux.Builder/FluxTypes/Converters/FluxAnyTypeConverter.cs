using InfluxDB.Client.Api.Domain;
using NodaTime;
using System;
using System.Collections;
using System.Globalization;
using Duration = NodaTime.Duration;

namespace InfluxDB.Flux.Builder.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert any <see cref="object"/> to Flux notation and Flux AST representation.
    /// </summary>
    public static class FluxAnyTypeConverter
    {
        internal const string NullValueMessage = "Cannot represent a null value in Flux.";

        public static string ToFluxNotation(this object value) => value switch
        {
            null => throw new ArgumentNullException(nameof(value), NullValueMessage),

            IFluxType x => x.ToFluxNotation(),

            // Boolean
            bool b => b.ToFluxNotation(),

            // Integers
            sbyte i => i.ToFluxNotation(),
            short i => i.ToFluxNotation(),
            int i => i.ToFluxNotation(),
            long i => i.ToFluxNotation(),

            // Unsigned integers
            byte u => u.ToFluxNotation(),
            ushort u => u.ToFluxNotation(),
            uint u => u.ToFluxNotation(),
            ulong u => u.ToFluxNotation(),

            // Floating-point numbers
            float f => f.ToFluxNotation(),
            double f => f.ToFluxNotation(),
            decimal f => f.ToFluxNotation(),

            // Dates and times
            DateTime d => d.ToFluxNotation(),
            DateTimeOffset d => d.ToFluxNotation(),
#if NET6_0_OR_GREATER
            DateOnly d => d.ToFluxNotation(),
#endif
            Instant d => d.ToFluxNotation(),
            ZonedDateTime d => d.ToFluxNotation(),
            OffsetDateTime d => d.ToFluxNotation(),
            OffsetDate d => d.ToFluxNotation(),
            LocalDateTime d => d.ToFluxNotation(),
            LocalDate d => d.ToFluxNotation(),

            // Durations
            TimeSpan d => d.ToFluxNotation(),
            Duration d => d.ToFluxNotation(),
            Period d => d.ToFluxNotation(),

            // String
            string s => s.ToFluxNotation(),

            // Array
            IEnumerable e => e.ToFluxNotation(),

            _ => Convert.ToString(value, CultureInfo.InvariantCulture)!.ToFluxNotation()
        };

        public static Expression ToFluxAstNode(this object value) => value switch
        {
            null => throw new ArgumentNullException(nameof(value), NullValueMessage),

            IFluxType x => x.ToFluxAstNode(),

            // Boolean
            bool b => b.ToFluxAstNode(),

            // Integers
            sbyte i => i.ToFluxAstNode(),
            short i => i.ToFluxAstNode(),
            int i => i.ToFluxAstNode(),
            long i => i.ToFluxAstNode(),

            // Unsigned integers
            byte u => u.ToFluxAstNode(),
            ushort u => u.ToFluxAstNode(),
            uint u => u.ToFluxAstNode(),
            ulong u => u.ToFluxAstNode(),

            // Floating-point numbers
            float f => f.ToFluxAstNode(),
            double f => f.ToFluxAstNode(),
            decimal f => f.ToFluxAstNode(),

            // Dates and times
            DateTime d => d.ToFluxAstNode(),
            DateTimeOffset d => d.ToFluxAstNode(),
#if NET6_0_OR_GREATER
            DateOnly d => d.ToFluxAstNode(),
#endif
            Instant d => d.ToFluxAstNode(),
            ZonedDateTime d => d.ToFluxAstNode(),
            OffsetDateTime d => d.ToFluxAstNode(),
            OffsetDate d => d.ToFluxAstNode(),
            LocalDateTime d => d.ToFluxAstNode(),
            LocalDate d => d.ToFluxAstNode(),

            // Durations
            TimeSpan d => d.ToFluxAstNode(),
            Duration d => d.ToFluxAstNode(),
            Period d => d.ToFluxAstNode(),

            // String
            string s => s.ToFluxAstNode(),

            // Array
            IEnumerable e => e.ToFluxAstNode(),

            _ => Convert.ToString(value, CultureInfo.InvariantCulture)!.ToFluxAstNode()
        };
    }
}

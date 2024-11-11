using InfluxDB.Client.Api.Domain;
using InfluxDB.Flux.Builder.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfluxDB.Flux.Builder.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert array to Flux notation and Flux AST representation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/composite/array/">Array - InfluxDB documentation</seealso>
    public static class FluxArrayConverter
    {
        public static string ToFluxNotation(this IEnumerable values) => values.Cast<object>().ToFluxNotation();
        public static string ToFluxNotation<T>(this IEnumerable<T> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), FluxAnyTypeConverter.NullValueMessage);

            return new StringBuilder("[")
                .AppendJoin(", ", values.Select(v => v!.ToFluxNotation()))
                .Append(']')
                .ToString();
        }

        public static ArrayExpression ToFluxAstNode(this IEnumerable values) => values.Cast<object>().ToFluxAstNode();
        public static ArrayExpression ToFluxAstNode<T>(this IEnumerable<T> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values), FluxAnyTypeConverter.NullValueMessage);

            return new(nameof(ArrayExpression), values.Select(v => v!.ToFluxAstNode()).ToList());
        }
    }
}

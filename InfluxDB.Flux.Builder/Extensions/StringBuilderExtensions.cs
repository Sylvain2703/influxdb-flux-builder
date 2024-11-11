using System;
using System.Collections.Generic;
using System.Text;

namespace InfluxDB.Flux.Builder.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendPipe(this StringBuilder stringBuilder) => stringBuilder.Append("|> ");

#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP2_0_OR_GREATER
        public static StringBuilder AppendJoin<T>(this StringBuilder stringBuilder, string separator, IEnumerable<T> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            using IEnumerator<T> enumerator = values.GetEnumerator();

            if (!enumerator.MoveNext())
                return stringBuilder;

            T value = enumerator.Current;
            if (value != null)
                stringBuilder.Append(value.ToString());

            while (enumerator.MoveNext())
            {
                stringBuilder.Append(separator);

                value = enumerator.Current;
                if (value != null)
                    stringBuilder.Append(value.ToString());
            }

            return stringBuilder;
        }
#endif
    }
}

using InfluxDB.Flux.Builder.Options;
using InfluxDB.Flux.Builder.Parameterization;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfluxDB.Flux.Builder
{
    public class FluxFilter
    {
        private readonly StringBuilder _stringBuilder;
        private readonly FluxBuilderOptions _options;
        private readonly ParametersManager _parameters;
        private bool _firstCondition = true;

        internal FluxFilter(StringBuilder stringBuilder, FluxBuilderOptions options, ParametersManager parameters)
        {
            _stringBuilder = stringBuilder;
            _options = options;
            _parameters = parameters;
        }

        private void AppendAndIfNeeded()
        {
            if (_firstCondition)
                _firstCondition = false;
            else
                _stringBuilder.Append(" and ");
        }

        /// <summary>
        /// <para>Add conditions to the filter predicate with raw Flux specified in the <paramref name="rawFluxFilters"/> interpolated string.</para>
        /// <para>Records representing each row are passed as <c>r</c>.</para>
        /// </summary>
        /// <remarks>
        /// This method provides a built-in mechanism to protect against Flux injection attacks.
        /// Interpolated values in the <paramref name="rawFluxFilters"/> query string will be parameterized automatically.
        /// </remarks>
        /// <param name="rawFluxFilters">An interpolated string representing a raw Flux predicate (eg. <c>"r._value &gt; 0 and r._value &lt; 50"</c>).</param>
        public FluxFilter Where(FormattableString rawFluxFilters)
        {
            AppendAndIfNeeded();
            _stringBuilder.Append(_parameters.Parameterize(rawFluxFilters, "filter_where"));

            return this;
        }

        /// <summary>
        /// <para>
        /// Add conditions to the filter predicate with raw Flux returned by the <paramref name="rawFluxFiltersBuilder"/> function
        /// (without built-in protection against Flux injection attacks).
        /// </para>
        /// <para>Records representing each row are passed as <c>r</c>.</para>
        /// </summary>
        /// <remarks>
        /// To prevent Flux injection attacks, <b>never pass a concatenated or interpolated string</b> (<c>$""</c>) with
        /// non-validated user-provided values into this method.<br/>Instead, use the <see cref="ParametersManager"/>
        /// argument provided by <paramref name="rawFluxFiltersBuilder"/> to parameterize the values, as below:
        /// <code>
        /// WhereUnsafe(p => $"r._value == {p.Parameterize("val1", expectedValue)} or r._value == " + p.Parameterize("val2", fallbackValue))
        /// </code>
        /// </remarks>
        /// <param name="rawFluxFiltersBuilder">A function that builds a string representing a raw Flux predicate (eg. <c>"r._value &gt; 0 and r._value &lt; 50"</c>).</param>
        public FluxFilter WhereUnsafe(Func<ParametersManager, string> rawFluxFiltersBuilder)
        {
            AppendAndIfNeeded();
            _stringBuilder.Append(rawFluxFiltersBuilder(_parameters));

            return this;
        }

        /// <summary>
        /// Add a condition to the filter predicate to keep only records with the specified <paramref name="measurement"/>.
        /// </summary>
        /// <param name="measurement">Name of the measurement to filter records.</param>
        public FluxFilter Measurement(string measurement)
        {
            AppendAndIfNeeded();
            _stringBuilder.Append("r._measurement == ").Append(_parameters.Parameterize("filter_measurement", measurement));

            return this;
        }

        /// <summary>
        /// Add a condition to the filter predicate to keep only records with the specified <paramref name="tagKey"/> and <paramref name="tagValue"/>.
        /// </summary>
        /// <param name="tagKey">Key of the tag to filter.</param>
        /// <param name="tagValue">Value of the tag to filter.</param>
        public FluxFilter Tag(string tagKey, string tagValue)
        {
            AppendAndIfNeeded();

            // The Flux function "record.get()" is currently the only way to get a value from a record using a key specified with a variable.
            // Unfortunately, "r[myVariable]" does not work. See https://github.com/influxdata/flux/issues/2510.
            _stringBuilder.Append("record.get(r: r, key: ").Append(_parameters.Parameterize("filter_tagKey", tagKey)).Append(", default: \"\") == ")
                .Append(_parameters.Parameterize("filter_tagValue", tagValue));
            _options.ImportPackage(FluxPackages.Experimental_Record);

            return this;
        }

        /// <summary>
        /// Add conditions to the filter predicate to keep only records with the specified <paramref name="tags"/>.
        /// </summary>
        /// <param name="tags">A dictionary of tag keys and values to filter records.</param>
        public FluxFilter Tags(IDictionary<string, string> tags)
        {
            if (tags == null)
                return this;

            foreach (var tag in tags)
                Tag(tag.Key, tag.Value);

            return this;
        }

        /// <summary>
        /// Add conditions to the filter predicate to keep only records with any specified <paramref name="fields"/>.
        /// </summary>
        /// <param name="fields">An array of field keys to filter records.</param>
        public FluxFilter Fields(params string[] fields)
        {
            if (fields == null || fields.Length == 0)
                return this;

            AppendAndIfNeeded();

            _stringBuilder.Append('(');
            for (int i = 0; i < fields.Length; i++)
            {
                if (i > 0)
                    _stringBuilder.Append(" or ");

                _stringBuilder.Append("r._field == ").Append(_parameters.Parameterize("filter_field", fields[i]));
            }
            _stringBuilder.Append(')');

            return this;
        }
    }
}

using InfluxDB.Flux.Builder.Options;
using InfluxDB.Flux.Builder.Parameterization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfluxDB.Flux.Builder
{
    public delegate void FluxCondition();

    public class FluxFilterBuilder
    {
        private const string AndOperator = " and ";
        private const string OrOperator = " or ";

        private readonly StringBuilder _stringBuilder;
        private readonly FluxBuilderOptions _options;
        private readonly ParametersManager _parameters;

        internal FluxFilterBuilder(StringBuilder stringBuilder, FluxBuilderOptions options, ParametersManager parameters)
        {
            _stringBuilder = stringBuilder;
            _options = options;
            _parameters = parameters;
        }

        /// <summary>
        /// Adds conditions separated by the <c>and</c> logical operator to the filter predicate.
        /// </summary>
        /// <param name="conditions">Conditions to be separated by the <c>and</c> logical operator.</param>
        public FluxCondition And(IEnumerable<FluxCondition> conditions) => AppendGroup(conditions, AndOperator);

        /// <summary>
        /// Adds conditions separated by the <c>and</c> logical operator to the filter predicate.
        /// </summary>
        /// <param name="conditions">Conditions to be separated by the <c>and</c> logical operator.</param>
        public FluxCondition And(params FluxCondition[] conditions) => AppendGroup(conditions, AndOperator);

        /// <summary>
        /// Adds conditions separated by the <c>or</c> logical operator to the filter predicate.
        /// </summary>
        /// <param name="conditions">Conditions to be separated by the <c>or</c> logical operator.</param>
        public FluxCondition Or(IEnumerable<FluxCondition> conditions) => AppendGroup(conditions, OrOperator);

        /// <summary>
        /// Adds conditions separated by the <c>or</c> logical operator to the filter predicate.
        /// </summary>
        /// <param name="conditions">Conditions to be separated by the <c>or</c> logical operator.</param>
        public FluxCondition Or(params FluxCondition[] conditions) => AppendGroup(conditions, OrOperator);

        private FluxCondition AppendGroup(IEnumerable<FluxCondition> conditions, string logicalOperator) => () =>
        {
            if (conditions == null || !conditions.Any())
                throw new ArgumentException($"No filtering condition has been provided.");

            bool firstCondition = true;

            _stringBuilder.Append('(');
            foreach (var condition in conditions)
            {
                if (firstCondition)
                    firstCondition = false;
                else
                    _stringBuilder.Append(logicalOperator);

                condition();
            }
            _stringBuilder.Append(')');
        };


        /// <summary>
        /// <para>Adds conditions to the filter predicate with raw Flux specified in the <paramref name="rawFluxFilters"/> interpolated string.</para>
        /// <para>Records representing each row are passed as <c>r</c>.</para>
        /// </summary>
        /// <remarks>
        /// This method provides a built-in mechanism to protect against Flux injection attacks.
        /// Interpolated values in the <paramref name="rawFluxFilters"/> query string will be parameterized automatically.
        /// </remarks>
        /// <param name="rawFluxFilters">An interpolated string representing a raw Flux predicate (eg. <c>"r._value &gt; 0 and r._value &lt; 50"</c>).</param>
        public FluxCondition Where(FormattableString rawFluxFilters) => () =>
            _stringBuilder.Append(_parameters.Parameterize(rawFluxFilters, "filter_where"));

        /// <summary>
        /// <para>
        /// Adds conditions to the filter predicate with raw Flux returned by the <paramref name="rawFluxFiltersBuilder"/> function
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
        public FluxCondition WhereUnsafe(Func<ParametersManager, string> rawFluxFiltersBuilder) => () =>
            _stringBuilder.Append(rawFluxFiltersBuilder(_parameters));


        /// <summary>
        /// Adds a condition to the filter predicate to keep only records with the specified <paramref name="measurement"/>.
        /// </summary>
        /// <param name="measurement">Name of the measurement to filter records.</param>
        public FluxCondition Measurement(string measurement) => () =>
            _stringBuilder.Append("r._measurement == ").Append(_parameters.Parameterize("filter_measurement", measurement));


        /// <summary>
        /// Adds a condition to the filter predicate to keep only records with the specified <paramref name="tagKey"/> and <paramref name="tagValue"/>.
        /// </summary>
        /// <param name="tagKey">Key of the tag to filter.</param>
        /// <param name="tagValue">Value of the tag to filter.</param>
        public FluxCondition Tag(string tagKey, string tagValue) => () =>
        {
            // The Flux function "record.get()" is currently the only way to get a value from a record using a key specified with a variable.
            // Unfortunately, "r[myVariable]" does not work. See https://github.com/influxdata/flux/issues/2510.
            _stringBuilder.Append("record.get(r: r, key: ").Append(_parameters.Parameterize("filter_tagKey", tagKey)).Append(", default: \"\") == ")
                .Append(_parameters.Parameterize("filter_tagValue", tagValue));
            _options.ImportPackage(FluxPackages.Experimental_Record);
        };

        /// <summary>
        /// Adds conditions to the filter predicate to keep only records with the specified <paramref name="tags"/>.
        /// </summary>
        /// <param name="tags">A dictionary of tag keys and values to filter records.</param>
        public FluxCondition Tags(IDictionary<string, string> tags) => And(tags.Select(t => Tag(t.Key, t.Value)));


        /// <summary>
        /// Adds a condition to the filter predicate to keep only records with the specified <paramref name="field"/>.
        /// </summary>
        /// <param name="field">Key of the field to filter.</param>
        public FluxCondition Field(string field) => () =>
            _stringBuilder.Append("r._field == ").Append(_parameters.Parameterize("filter_field", field));

        /// <summary>
        /// Adds conditions to the filter predicate to keep only records with any specified <paramref name="fields"/>.
        /// </summary>
        /// <param name="fields">An array of field keys to filter records.</param>
        public FluxCondition Fields(IEnumerable<string> fields) => Or(fields.Select(Field));

        /// <summary>
        /// Adds conditions to the filter predicate to keep only records with any specified <paramref name="fields"/>.
        /// </summary>
        /// <param name="fields">An array of field keys to filter records.</param>
        public FluxCondition Fields(params string[] fields) => Or(fields.Select(Field));
    }
}

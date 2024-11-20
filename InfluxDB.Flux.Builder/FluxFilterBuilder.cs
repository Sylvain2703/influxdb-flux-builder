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
        private const string EqualOperator = " == ";
        private const string NotEqualOperator = " != ";
        private const string LessOperator = " < ";
        private const string LessOrEqualOperator = " <= ";
        private const string GreaterOperator = " > ";
        private const string GreaterOrEqualOperator = " >= ";
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

        #region Create record keys

        /// <summary>
        /// Creates a custom record key, that can be used as a condition operand.
        /// </summary>
        public FluxRecordKey this[string recordKey] => new(this, recordKey);

        /// <summary>
        /// Creates a record key, that can be used as a condition operand, to represent the measurement.
        /// </summary>
        public FluxRecordKey Measurement => new(this, "_measurement", trustedKey: true);

        /// <summary>
        /// Creates a record key, that can be used as a condition operand, to represent the field.
        /// </summary>
        public FluxRecordKey Field => new(this, "_field", trustedKey: true);

        /// <summary>
        /// Creates a record key, that can be used as a condition operand, to represent the value.
        /// </summary>
        public FluxRecordKey Value => new(this, "_value", trustedKey: true);

        /// <summary>
        /// Creates a record key, that can be used as a condition operand, to represent the timestamp.
        /// </summary>
        public FluxRecordKey Time => new(this, "_time", trustedKey: true);

        /// <summary>
        /// Creates a record key, that can be used as a condition operand, to represent the start time.
        /// </summary>
        public FluxRecordKey Start => new(this, "_start", trustedKey: true);

        /// <summary>
        /// Creates a record key, that can be used as a condition operand, to represent the stop time.
        /// </summary>
        public FluxRecordKey Stop => new(this, "_stop", trustedKey: true);

        #endregion

        #region Comparison operators

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the <paramref name="left"/> value is equal to the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The left operand of the comparison.</param>
        /// <param name="right">The right operand of the comparison.</param>
        public FluxCondition Equal(object left, object right) => AppendComparison(left, right, EqualOperator);

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the <paramref name="left"/> value is not equal to the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The left operand of the comparison.</param>
        /// <param name="right">The right operand of the comparison.</param>
        public FluxCondition NotEqual(object left, object right) => AppendComparison(left, right, NotEqualOperator);

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the <paramref name="left"/> value is less than the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The left operand of the comparison.</param>
        /// <param name="right">The right operand of the comparison.</param>
        public FluxCondition Less(object left, object right) => AppendComparison(left, right, LessOperator);

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the <paramref name="left"/> value is less than or equal to the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The left operand of the comparison.</param>
        /// <param name="right">The right operand of the comparison.</param>
        public FluxCondition LessOrEqual(object left, object right) => AppendComparison(left, right, LessOrEqualOperator);

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the <paramref name="left"/> value is greater than the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The left operand of the comparison.</param>
        /// <param name="right">The right operand of the comparison.</param>
        public FluxCondition Greater(object left, object right) => AppendComparison(left, right, GreaterOperator);

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the <paramref name="left"/> value is greater than or equal to the <paramref name="right"/> value.
        /// </summary>
        /// <param name="left">The left operand of the comparison.</param>
        /// <param name="right">The right operand of the comparison.</param>
        public FluxCondition GreaterOrEqual(object left, object right) => AppendComparison(left, right, GreaterOrEqualOperator);

        private FluxCondition AppendComparison(object left, object right, string comparisonOperator) => () =>
        {
            AppendOperand(left);
            _stringBuilder.Append(comparisonOperator);
            AppendOperand(right);
        };

        private void AppendOperand(object operand)
        {
            if (operand is FluxRecordKey recordKey)
                recordKey.Append(_stringBuilder, _options, _parameters, "filter_key");
            else
                _stringBuilder.Append(_parameters.Parameterize("filter_value", operand));
        }

        #endregion

        #region Logical operators

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

        #endregion

        #region Custom Flux

        /// <summary>
        /// <para>Adds conditions to the filter predicate with raw Flux specified in the <paramref name="rawFlux"/> interpolated string.</para>
        /// <para>Records representing each row are passed as <c>r</c>.</para>
        /// </summary>
        /// <remarks>
        /// This method provides a built-in mechanism to protect against Flux injection attacks.
        /// Interpolated values in the <paramref name="rawFlux"/> string will be parameterized automatically.
        /// </remarks>
        /// <param name="rawFlux">An interpolated string representing a raw Flux predicate (eg. <c>"r._value &gt; 0 and r._value &lt; 50"</c>).</param>
        public FluxCondition WithCustomFlux(FormattableString rawFlux) => () =>
            _stringBuilder.Append(_parameters.Parameterize(rawFlux, "filter_withCustomFlux"));

        /// <summary>
        /// <para>
        /// Adds conditions to the filter predicate with raw Flux returned by the <paramref name="buildRawFlux"/> function
        /// (without built-in protection against Flux injection attacks).
        /// </para>
        /// <para>Records representing each row are passed as <c>r</c>.</para>
        /// </summary>
        /// <remarks>
        /// To prevent Flux injection attacks, <b>never pass a concatenated or interpolated string</b> (<c>$""</c>) with
        /// non-validated user-provided values into this method.<br/>Instead, use the <see cref="ParametersManager"/>
        /// argument provided by <paramref name="buildRawFlux"/> to parameterize the values, as below:
        /// <code>
        /// WithCustomFluxUnsafe(p => $"r._value == {p.Parameterize("val1", expectedValue)} or r._value == " + p.Parameterize("val2", fallbackValue))
        /// </code>
        /// </remarks>
        /// <param name="buildRawFlux">A function that builds a string representing a raw Flux predicate (eg. <c>"r._value &gt; 0 and r._value &lt; 50"</c>).</param>
        public FluxCondition WithCustomFluxUnsafe(Func<ParametersManager, string> buildRawFlux) => () =>
            _stringBuilder.Append(buildRawFlux(_parameters));

        #endregion

        #region Shorthand methods

        /// <summary>
        /// Adds a condition to the filter predicate to keep only records that match the specified <paramref name="measurement"/>.
        /// </summary>
        /// <param name="measurement">Name of the measurement to filter records.</param>
        public FluxCondition MatchMeasurement(string measurement) => () =>
            _stringBuilder.Append("r._measurement == ").Append(_parameters.Parameterize("filter_measurement", measurement));

        /// <summary>
        /// Adds conditions to the filter predicate to keep only records that match any specified <paramref name="measurements"/>.
        /// </summary>
        /// <param name="measurements">Name of the measurements to filter records.</param>
        public FluxCondition MatchAnyMeasurements(IEnumerable<string> measurements) => Or(measurements.Select(MatchMeasurement));

        /// <summary>
        /// Adds conditions to the filter predicate to keep only records that match any specified <paramref name="measurements"/>.
        /// </summary>
        /// <param name="measurements">Name of the measurements to filter records.</param>
        public FluxCondition MatchAnyMeasurements(params string[] measurements) => Or(measurements.Select(MatchMeasurement));


        /// <summary>
        /// Adds a condition to the filter predicate to keep only records that match the specified <paramref name="tagKey"/> and <paramref name="tagValue"/>.
        /// </summary>
        /// <param name="tagKey">Key of the tag to filter.</param>
        /// <param name="tagValue">Value of the tag to filter.</param>
        public FluxCondition MatchTag(string tagKey, string tagValue) => Equal(this[tagKey], tagValue);

        /// <summary>
        /// Adds conditions to the filter predicate to keep only records that match any specified <paramref name="tags"/>.
        /// </summary>
        /// <param name="tags">A dictionary of tag keys and values to filter records.</param>
        public FluxCondition MatchAnyTags(IDictionary<string, string> tags) => Or(tags.Select(t => MatchTag(t.Key, t.Value)));

        /// <summary>
        /// Adds conditions to the filter predicate to keep only records that match all specified <paramref name="tags"/>.
        /// </summary>
        /// <param name="tags">A dictionary of tag keys and values to filter records.</param>
        public FluxCondition MatchAllTags(IDictionary<string, string> tags) => And(tags.Select(t => MatchTag(t.Key, t.Value)));


        /// <summary>
        /// Adds a condition to the filter predicate to keep only records that match the specified <paramref name="field"/>.
        /// </summary>
        /// <param name="field">Key of the field to filter records.</param>
        public FluxCondition MatchField(string field) => () =>
            _stringBuilder.Append("r._field == ").Append(_parameters.Parameterize("filter_field", field));

        /// <summary>
        /// Adds conditions to the filter predicate to keep only records that match any specified <paramref name="fields"/>.
        /// </summary>
        /// <param name="fields">Key of the fields to filter records.</param>
        public FluxCondition MatchAnyFields(IEnumerable<string> fields) => Or(fields.Select(MatchField));

        /// <summary>
        /// Adds conditions to the filter predicate to keep only records that match any specified <paramref name="fields"/>.
        /// </summary>
        /// <param name="fields">Key of the fields to filter records.</param>
        public FluxCondition MatchAnyFields(params string[] fields) => Or(fields.Select(MatchField));

        #endregion
    }
}

using InfluxDB.Flux.Builder.Options;
using InfluxDB.Flux.Builder.Parameterization;
using System.Text;

namespace InfluxDB.Flux.Builder
{
    public class FluxRecordKey
    {
        private readonly FluxFilterBuilder _filterBuilder;
        private readonly string _key;
        private readonly bool _trustedKey;

        internal FluxRecordKey(FluxFilterBuilder filterBuilder, string key, bool trustedKey = false)
        {
            _filterBuilder = filterBuilder;
            _key = key;
            _trustedKey = trustedKey;
        }

        internal void Append(StringBuilder stringBuilder, FluxBuilderOptions options, ParametersManager parameters, string paramName)
        {
            if (_trustedKey)
            {
                stringBuilder.Append("r.").Append(_key);
            }
            else
            {
                // The "record.get()" Flux function is currently the only way to get a value from a record using a key specified with a variable.
                // Unfortunately, "r[myVariable]" does not work. See https://github.com/influxdata/flux/issues/2510.
                options.ImportPackage(FluxPackages.Experimental_Record);
                stringBuilder.Append("record.get(r: r, key: ").Append(parameters.Parameterize(paramName, _key)).Append(", default: \"\")");
            }
        }

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the value at <see langword="this"/> record key is equal to <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The right operand of the comparison.</param>
        public FluxCondition Equal(object value) => _filterBuilder.Equal(this, value);

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the value at <see langword="this"/> record key is not equal to <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The right operand of the comparison.</param>
        public FluxCondition NotEqual(object value) => _filterBuilder.NotEqual(this, value);

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the value at <see langword="this"/> record key is less than <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The right operand of the comparison.</param>
        public FluxCondition Less(object value) => _filterBuilder.Less(this, value);

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the value at <see langword="this"/> record key is less than or equal to <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The right operand of the comparison.</param>
        public FluxCondition LessOrEqual(object value) => _filterBuilder.LessOrEqual(this, value);

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the value at <see langword="this"/> record key is greater than <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The right operand of the comparison.</param>
        public FluxCondition Greater(object value) => _filterBuilder.Greater(this, value);

        /// <summary>
        /// Adds a condition to the filter predicate that evaluates if the value at <see langword="this"/> record key is greater than or equal to <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The right operand of the comparison.</param>
        public FluxCondition GreaterOrEqual(object value) => _filterBuilder.GreaterOrEqual(this, value);
    }
}

using InfluxDB.Client.Api.Domain;
using System.Globalization;
using FloatLiteral = InfluxDB.Flux.Builder.Parameterization.FloatLiteral;

namespace InfluxDB.Flux.Builder.FluxTypes.Converters
{
    /// <summary>
    /// Extension methods to convert float to Flux notation and Flux AST representation.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/data-types/basic/float/">Float - InfluxDB documentation</seealso>
    public static class FluxFloatConverter
    {
        public static string ToFluxNotation(this float value) => WithFloatConversionAsFluxNotation(value switch
        {
            float.PositiveInfinity => "+Inf",
            float.NegativeInfinity => "-Inf",
            _ => value.ToString(CultureInfo.InvariantCulture)
        });
        public static string ToFluxNotation(this double value) => WithFloatConversionAsFluxNotation(value switch
        {
            double.PositiveInfinity => "+Inf",
            double.NegativeInfinity => "-Inf",
            _ => value.ToString(CultureInfo.InvariantCulture)
        });
        public static string ToFluxNotation(this decimal value) => WithFloatConversionAsFluxNotation(value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Ensures support for floating numbers without decimal point (eg. <c>123</c>),
        /// scientific notation (eg. <c>1.23e+4</c>), <c>+Inf</c>, <c>-Inf</c> and <c>NaN</c>.
        /// </summary>
        private static string WithFloatConversionAsFluxNotation(string value) => $"float(v: \"{value}\")";


        public static Expression ToFluxAstNode(this float value) => value switch
        {
            float.PositiveInfinity => WithFloatConversionAsFluxAstNode("+Inf"),
            float.NegativeInfinity => WithFloatConversionAsFluxAstNode("-Inf"),
            float.NaN => WithFloatConversionAsFluxAstNode("NaN"),
            _ => new FloatLiteral(nameof(FloatLiteral), value)
        };
        public static Expression ToFluxAstNode(this double value) => value switch
        {
            double.PositiveInfinity => WithFloatConversionAsFluxAstNode("+Inf"),
            double.NegativeInfinity => WithFloatConversionAsFluxAstNode("-Inf"),
            double.NaN => WithFloatConversionAsFluxAstNode("NaN"),
            _ => new FloatLiteral(nameof(FloatLiteral), value)
        };
        public static FloatLiteral ToFluxAstNode(this decimal value) => new(nameof(FloatLiteral), value);

        /// <summary>
        /// Returns an AST representation of a float conversion (noted <c>float(v: "{value}")</c> in Flux).
        /// </summary>
        private static CallExpression WithFloatConversionAsFluxAstNode(string value) => new(nameof(CallExpression),
            callee: new Identifier(nameof(Identifier), "float"),
            arguments:
            [
                new ObjectExpression(nameof(ObjectExpression),
                [
                    new Property(nameof(Property),
                        key: new Identifier(nameof(Identifier), "v"),
                        value: value.ToFluxAstNode()
                    )
                ])
            ]);
    }
}

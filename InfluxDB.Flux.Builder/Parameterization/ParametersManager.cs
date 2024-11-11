using InfluxDB.Client.Api.Domain;
using InfluxDB.Flux.Builder.FluxTypes;
using InfluxDB.Flux.Builder.FluxTypes.Converters;
using InfluxDB.Flux.Builder.Options;
using NodaTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duration = NodaTime.Duration;

namespace InfluxDB.Flux.Builder.Parameterization
{
    public class ParametersManager
    {
        private const string Prefix = "params";

        private readonly Dictionary<FluxIdentifier, object> _parameters = new();
        private readonly FluxBuilderOptions _options;

        internal ParametersManager(FluxBuilderOptions options) => _options = options;

        public string Parameterize(string paramName, object? value)
        {
            if (paramName == null)
                throw new ArgumentNullException(nameof(paramName), "A parameter name is required to parameterize a value.");

            if (value == null)
                throw new ArgumentNullException(nameof(value), $"Cannot parameterize a null value for the \"{paramName}\" parameter.");

            if (value is FluxTimeable timeable)
                _options.ImportPackages(timeable.DependentPackageImports);

            if (!ShouldParameterize(value, _options.ParameterizedTypes))
                return value.ToFluxNotation();

            paramName = $"{paramName}_{_parameters.Count}";
            _parameters.Add(paramName, value);

            return $"{Prefix}.{paramName}";
        }

        private static bool ShouldParameterize(object value, ParameterizedTypes p) => value switch
        {
            bool => p.IsSet(ParameterizedTypes.Boolean),
            sbyte or short or int or long => p.IsSet(ParameterizedTypes.Integer),
            byte or ushort or uint or ulong => p.IsSet(ParameterizedTypes.UInteger),
            float or double or decimal => p.IsSet(ParameterizedTypes.Float),
#if NET6_0_OR_GREATER
            DateOnly or
#endif
            FluxTime or DateTime or DateTimeOffset or Instant or ZonedDateTime or
                OffsetDateTime or OffsetDate or LocalDateTime or LocalDate => p.IsSet(ParameterizedTypes.Time),
            FluxDuration or TimeSpan or Duration or Period => p.IsSet(ParameterizedTypes.Duration),
            FluxIdentifier => p.IsSet(ParameterizedTypes.Identifier),
            string => p.IsSet(ParameterizedTypes.String),
            IEnumerable => p.IsSet(ParameterizedTypes.Array),
            IFluxType t => p.IsSet(ParameterizedTypes.Object) && t.CanConvertToFluxAstNode,
            _ => true
        };

        internal string Parameterize(FormattableString formattableString, string paramName)
        {
            if (formattableString.ArgumentCount < 1)
                return formattableString.Format;

            var paramNames = formattableString.GetArguments()
                .Select(arg => Parameterize(paramName, arg))
                .ToArray();

            return string.Format(formattableString.Format, paramNames);
        }


        internal string? GetParametersAsFluxNotation()
        {
            if (_parameters.Count < 1)
                return null;

            var stringBuilder = new StringBuilder("option ").Append(Prefix).AppendLine(" = {");
            foreach (var p in _parameters)
            {
                stringBuilder.Append("  ").Append(p.Key.ToFluxNotation()).Append(": ").Append(p.Value.ToFluxNotation()).AppendLine(",");
            }
            stringBuilder.AppendLine("}").AppendLine();

            return stringBuilder.ToString();
        }

        internal Statement? GetParametersAsFluxAst()
        {
            if (_parameters.Count < 1)
                return null;

            var paramsProperties = _parameters
                .Select(p => new Property(nameof(Property),
                    key: p.Key.ToFluxAstNode(),
                    value: p.Value.ToFluxAstNode()
                ))
                .ToList();

            return new OptionStatement(nameof(OptionStatement),
                assignment: new VariableAssignment(nameof(VariableAssignment),
                    id: new Identifier(nameof(Identifier), Prefix),
                    init: new ObjectExpression(nameof(ObjectExpression), paramsProperties)
                )
            );
        }
    }
}

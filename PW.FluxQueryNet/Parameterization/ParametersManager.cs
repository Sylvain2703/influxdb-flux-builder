﻿using InfluxDB.Client.Api.Domain;
using NodaTime;
using PW.FluxQueryNet.FluxTypes;
using PW.FluxQueryNet.FluxTypes.Converters;
using PW.FluxQueryNet.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Duration = NodaTime.Duration;

namespace PW.FluxQueryNet.Parameterization
{
    public class ParametersManager
    {
        private const string Prefix = "params";

        private readonly Dictionary<string, object> _parameters = new();
        private readonly FluxBuilderOptions _options;

        internal ParametersManager(FluxBuilderOptions options) => _options = options;

        public string Parameterize(string paramName, object? value) // TODO: check that "paramName" only contains letters, digits and "_"
        {
            if (value == null)
                throw new ArgumentNullException(paramName, "Cannot parameterize a null value.");

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
                 OffsetDateTime or OffsetDate or LocalDateTime or LocalDate => p.IsSet(ParameterizedTypes.DateTime),
            FluxDuration or TimeSpan or Duration or Period => p.IsSet(ParameterizedTypes.Duration),
            string => p.IsSet(ParameterizedTypes.String),
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
                stringBuilder.Append("  ").Append(p.Key).Append(": ").Append(p.Value.ToFluxNotation()).AppendLine(",");
            }
            stringBuilder.AppendLine("}").AppendLine();

            return stringBuilder.ToString();
        }

        internal Statement? GetParametersAsFluxAst()
        {
            if (_parameters.Count < 1)
                return null;

            var paramsProperties = _parameters
                .Select(kvp => new Property(nameof(Property),
                    key: new Identifier(nameof(Identifier), kvp.Key),
                    value: kvp.Value.ToFluxAstNode()
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

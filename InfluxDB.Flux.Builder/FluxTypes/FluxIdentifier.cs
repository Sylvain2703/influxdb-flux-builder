using InfluxDB.Client.Api.Domain;
using System;

namespace InfluxDB.Flux.Builder.FluxTypes
{
    /// <summary>
    /// <para>Represents an identifier that names entities within Flux code.</para>
    /// <para>An identifier is a sequence of one or more letters, digits and underscores, and must start with a letter or an underscore.</para>
    /// </summary>
    /// <remarks>
    /// This can be obtained by instantiating it or with the implicit conversion of a <see cref="string"/>.
    /// </remarks>
    /// <seealso href="https://docs.influxdata.com/flux/latest/spec/lexical-elements/#identifiers">Identifier - InfluxDB documentation</seealso>
    public sealed class FluxIdentifier : IFluxType, IEquatable<FluxIdentifier>
    {
        private readonly string _value;

        public FluxIdentifier(string value)
        {
            ValidateIdentifier(value);
            _value = value;
        }

        public static implicit operator FluxIdentifier(string value) => new(value);

        public override string ToString() => ToFluxNotation();

        public string ToFluxNotation() => _value;

        Expression IFluxType.ToFluxAstNode() => ToFluxAstNode();

        public Identifier ToFluxAstNode() => new(nameof(Identifier), _value);

        public bool CanConvertToFluxAstNode => true;

        private static void ValidateIdentifier(string identifier)
        {
            // According to the Flux identifier specification (https://docs.influxdata.com/flux/latest/spec/lexical-elements/#identifiers
            // and https://docs.influxdata.com/flux/latest/spec/representation/#characters),
            // valid letters and digits for a Flux identifier are the following unicode categories:
            // - Lu = UppercaseLetter
            // - Ll = LowercaseLetter
            // - Lt = TitlecaseLetter
            // - Lm = ModifierLetter
            // - Lo = OtherLetter
            // - Nd = DecimalDigitNumber
            // These are exactly the categories that char.IsLetterOrDigit() validates.

            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentException("A Flux identifier cannot be empty or null.", nameof(identifier));

            if (char.IsDigit(identifier[0]))
                throw new ArgumentException($"The Flux identifier \"{identifier}\" cannot start with a digit.", nameof(identifier));

            foreach (char c in identifier)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                    throw new ArgumentException($"The Flux identifier \"{identifier}\" cannot contain the character \"{c}\": only letters, digits and underscores are valid.", nameof(identifier));
            }
        }


        public bool Equals(FluxIdentifier? other) => other != null && _value == other._value;

        public override bool Equals(object? obj) => Equals(obj as FluxIdentifier);

        public override int GetHashCode() => HashCode.Combine(_value);
    }
}

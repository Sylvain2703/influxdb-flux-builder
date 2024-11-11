using InfluxDB.Client.Api.Domain;
using InfluxDB.Flux.Builder.FluxTypes.Converters;

namespace InfluxDB.Flux.Builder.FluxTypes
{
    /// <summary>
    /// Represents a location record in Flux with a zone and an offset.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/internal/location/">Location - InfluxDB documentation</seealso>
    public sealed class FluxLocation : IFluxType
    {
        private readonly string _zone;
        private readonly FluxDuration _offset;

        public FluxLocation(string zone, FluxDuration offset)
        {
            _zone = zone;
            _offset = offset;
        }

        public override string ToString() => ToFluxNotation();

        public string ToFluxNotation() => $"{{ zone: {_zone.ToFluxNotation()}, offset: {_offset.ToFluxNotation()} }}";

        Expression IFluxType.ToFluxAstNode() => ToFluxAstNode();

        public ObjectExpression ToFluxAstNode() => new(nameof(ObjectExpression),
            [
                new Property(nameof(Property),
                    key: new Identifier(nameof(Identifier), "zone"),
                    value: _zone.ToFluxAstNode()
                ),
                new Property(nameof(Property),
                    key: new Identifier(nameof(Identifier), "offset"),
                    value: _offset.ToFluxAstNode()
                )
            ]);

        public bool CanConvertToFluxAstNode => true;
    }
}

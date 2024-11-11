using InfluxDB.Client.Api.Domain;

namespace InfluxDB.Flux.Builder.FluxTypes
{
    public interface IFluxType
    {
        string ToFluxNotation();

        Expression ToFluxAstNode();

        bool CanConvertToFluxAstNode { get; }
    }
}

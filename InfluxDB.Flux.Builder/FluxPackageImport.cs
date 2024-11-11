using InfluxDB.Client.Api.Domain;
using InfluxDB.Flux.Builder.FluxTypes;
using InfluxDB.Flux.Builder.FluxTypes.Converters;
using System;

namespace InfluxDB.Flux.Builder
{
    /// <summary>
    /// Represents a package to import, with an optional alias.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/spec/packages/#import-declaration">Package import declaration - InfluxDB documentation</seealso>
    public sealed class FluxPackageImport : IEquatable<FluxPackageImport>
    {
        public string Package { get; }
        public FluxIdentifier? Alias { get; }

        public FluxPackageImport(string package, FluxIdentifier? alias = null)
        {
            Package = package;
            Alias = alias;
        }

        public static implicit operator FluxPackageImport(string value) => new(value);

        public override string ToString() => ToFluxNotation();

        public string ToFluxNotation() => Alias == null
            ? $"import {Package.ToFluxNotation()}"
            : $"import {Alias.ToFluxNotation()} {Package.ToFluxNotation()}";

        public ImportDeclaration ToFluxAstNode() => new(nameof(ImportDeclaration),
            _as: Alias?.ToFluxAstNode(),
            path: Package.ToFluxAstNode());


        public bool Equals(FluxPackageImport? other) => other != null && Package == other.Package && Equals(Alias, other.Alias);

        public override bool Equals(object? obj) => Equals(obj as FluxPackageImport);

        public override int GetHashCode() => HashCode.Combine(Package, Alias);
    }
}

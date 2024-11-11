using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Flux.Builder.Extensions;
using InfluxDB.Flux.Builder.Options;
using InfluxDB.Flux.Builder.Parameterization;
using System;
using System.Text;

namespace InfluxDB.Flux.Builder
{
    public partial class FluxQueryBuilder : IFluxBuilder, IFluxConfigurable, IFluxSource, IFluxStream
    {
        private readonly StringBuilder _stringBuilder = new();
        private readonly FluxBuilderOptions _options;
        private readonly ParametersManager _parameters;

        private FluxQueryBuilder(FluxBuilderOptions? options)
        {
            _options = options ?? new FluxBuilderOptions();
            _parameters = new ParametersManager(_options);
        }

        /// <summary>
        /// Creates a <see cref="FluxQueryBuilder"/> to generate a Flux query.
        /// </summary>
        public static IFluxSource Create(FluxBuilderOptions? options = null) => new FluxQueryBuilder(options);

        /// <inheritdoc/>
        public IFluxStream PipeCustomFlux(FormattableString rawFlux)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append(_parameters.Parameterize(rawFlux, "pipeCustomFlux"));
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream PipeCustomFluxUnsafe(Func<ParametersManager, string> rawFluxBuilder)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append(rawFluxBuilder(_parameters));
            return this;
        }


        /// <inheritdoc/>
        public IFluxStream Configure(Action<FluxBuilderOptions> configureAction)
        {
            configureAction(_options);
            return this;
        }

        /// <inheritdoc/>
        public Query ToQuery(Dialect? dialect = null)
        {
            var parametersStatement = _parameters.GetParametersAsFluxAst();

            // Workaround: import packages in Flux notation as the AST representation in the "imports" property seems to be ignored.
            // See https://github.com/influxdata/influxdb/issues/24734.
            return new Query(
                query: _options.GetImportsAsFluxNotation() + _stringBuilder.ToString(),
                _extern: new(nameof(File),
                    imports: [], //_options.GetImportsAsFluxAst(),
                    body: parametersStatement == null ? [] : [parametersStatement]
                ),
                type: Query.TypeEnum.Flux,
                dialect: dialect ?? QueryApi.Dialect,
                now: _options.Now);
        }

        /// <inheritdoc/>
        public string ToDebugQueryString()
        {
            return _options.GetImportsAsFluxNotation()
                + _options.GetNowAsFluxNotation()
                + _parameters.GetParametersAsFluxNotation()
                + _stringBuilder.ToString();
        }
    }
}

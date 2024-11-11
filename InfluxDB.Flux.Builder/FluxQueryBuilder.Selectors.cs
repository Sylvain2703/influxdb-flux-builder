using InfluxDB.Flux.Builder.Extensions;

namespace InfluxDB.Flux.Builder
{
    public partial class FluxQueryBuilder
    {
        /// <inheritdoc/>
        public IFluxStream First(string? column = null) => AggregateCore("first", column);

        /// <inheritdoc/>
        public IFluxStream Last(string? column = null) => AggregateCore("last", column);

        /// <inheritdoc/>
        public IFluxStream Unique(string? column = null) => AggregateCore("unique", column);

        /// <inheritdoc/>
        public IFluxStream Distinct(string? column = null) => AggregateCore("distinct", column);

        /// <inheritdoc/>
        public IFluxStream Min(string? column = null) => AggregateCore("min", column);

        /// <inheritdoc/>
        public IFluxStream Max(string? column = null) => AggregateCore("max", column);

        /// <inheritdoc/>
        public IFluxStream Top(int n, params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("top(n: ").Append(_parameters.Parameterize("top_n", n));

            if (columns != null && columns.Length > 0)
                _stringBuilder.Append(", columns: ").Append(_parameters.Parameterize("top_columns", columns));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Bottom(int n, params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("bottom(n: ").Append(_parameters.Parameterize("bottom_n", n));

            if (columns != null && columns.Length > 0)
                _stringBuilder.Append(", columns: ").Append(_parameters.Parameterize("bottom_columns", columns));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Sort(bool desc, params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("sort(desc: ").Append(_parameters.Parameterize("sort_desc", desc));

            if (columns != null && columns.Length > 0)
                _stringBuilder.Append(", columns: ").Append(_parameters.Parameterize("sort_columns", columns));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Limit(int n, int? offset = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("limit(n: ").Append(_parameters.Parameterize("limit_n", n));

            if (offset.HasValue)
                _stringBuilder.Append(", offset: ").Append(_parameters.Parameterize("limit_offset", offset.Value));

            _stringBuilder.Append(')');
            return this;
        }


        /// <inheritdoc/>
        public IFluxStream Fill(object value, string? column = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("fill(value: ").Append(_parameters.Parameterize("fill_value", value));

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: ").Append(_parameters.Parameterize("fill_column", column));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream FillPrevious(string? column = null)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("fill(usePrevious: true");

            if (!string.IsNullOrWhiteSpace(column))
                _stringBuilder.Append(", column: ").Append(_parameters.Parameterize("fill_column", column));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Group(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("group(");

            if (columns != null && columns.Length > 0)
                _stringBuilder.Append("columns: ").Append(_parameters.Parameterize("group_columns", columns));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream GroupExcept(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("group(mode: \"except\"");

            if (columns != null && columns.Length > 0)
                _stringBuilder.Append(", columns: ").Append(_parameters.Parameterize("group_columns", columns));

            _stringBuilder.Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Keep(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("keep(columns: ").Append(_parameters.Parameterize("keep_columns", columns)).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Drop(params string[] columns)
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("drop(columns: ").Append(_parameters.Parameterize("drop_columns", columns)).Append(')');
            return this;
        }

        /// <inheritdoc/>
        public IFluxStream Pivot(string[] rowKey, string[] columnKey, string valueColumn = "_value")
        {
            _stringBuilder.AppendLine();
            _stringBuilder.AppendPipe().Append("pivot(valueColumn: ").Append(_parameters.Parameterize("pivot_valueColumn", valueColumn))
                .Append(", rowKey: ").Append(_parameters.Parameterize("pivot_rowKey", rowKey))
                .Append(", columnKey: ").Append(_parameters.Parameterize("pivot_columnKey", columnKey))
                .Append(')');
            return this;
        }
    }
}

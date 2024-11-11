namespace InfluxDB.Flux.Builder
{
    public partial interface IFluxStream
    {
        /// <summary>
        /// Returns the first record with a non-<see langword="null"/> value from each input table.
        /// </summary>
        /// <remarks>This function drops empty tables.</remarks>
        /// <param name="column">Column to operate on. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/first/">first() function - InfluxDB documentation</seealso>
        IFluxStream First(string? column = null);

        /// <summary>
        /// <para>Returns the last record with a non-<see langword="null"/> value from each input table.</para>
        /// <para>If the value is <see langword="null"/> in the last record, it returns the previous record with a non-<see langword="null"/> value.</para>
        /// </summary>
        /// <remarks>This function drops empty tables.</remarks>
        /// <param name="column">Column to operate on. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/last/">last() function - InfluxDB documentation</seealso>
        IFluxStream Last(string? column = null);

        /// <summary>
        /// <para>Returns all records containing unique values in a specified <paramref name="column"/>.</para>
        /// <para>Group keys, columns, and values are not modified.</para>
        /// </summary>
        /// <remarks>This function drops empty tables.</remarks>
        /// <param name="column">Column to search for unique values. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/unique/">unique() function - InfluxDB documentation</seealso>
        IFluxStream Unique(string? column = null);

        /// <summary>
        /// <para>Returns all unique values in a specified <paramref name="column"/>.</para>
        /// <para>
        /// The <c>_value</c> of each output record is set to a distinct value in the specified <paramref name="column"/>.<br/>
        /// <see langword="null"/> is considered its own distinct value if present.
        /// </para>
        /// </summary>
        /// <param name="column">Column to return unique values from. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/distinct/">distinct() function - InfluxDB documentation</seealso>
        IFluxStream Distinct(string? column = null);

        /// <summary>
        /// Returns the row with the minimum value in a specified <paramref name="column"/> from each input table.
        /// </summary>
        /// <remarks>This function drops empty tables.</remarks>
        /// <param name="column">Column to return minimum values from. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/min/">min() function - InfluxDB documentation</seealso>
        IFluxStream Min(string? column = null);

        /// <summary>
        /// Returns the row with the maximum value in a specified <paramref name="column"/> from each input table.
        /// </summary>
        /// <remarks>This function drops empty tables.</remarks>
        /// <param name="column">Column to return maximum values from. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/max/">max() function - InfluxDB documentation</seealso>
        IFluxStream Max(string? column = null);

        /// <summary>
        /// Sorts each input table by specified <paramref name="columns"/> and keeps the top <paramref name="n"/> records in each table.
        /// </summary>
        /// <remarks>This function drops empty tables.</remarks>
        /// <param name="n">Number of rows to return from each input table.</param>
        /// <param name="columns">Columns to sort by. Sort precedence is determined by list order. Default is <c>["_value"]</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/top/">top() function - InfluxDB documentation</seealso>
        IFluxStream Top(int n, params string[] columns);

        /// <summary>
        /// Sorts each input table by specified <paramref name="columns"/> and keeps the bottom <paramref name="n"/> records in each table.
        /// </summary>
        /// <remarks>This function drops empty tables.</remarks>
        /// <param name="n">Number of rows to return from each input table.</param>
        /// <param name="columns">Columns to sort by. Sort precedence is determined by list order. Default is <c>["_value"]</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/bottom/">bottom() function - InfluxDB documentation</seealso>
        IFluxStream Bottom(int n, params string[] columns);

        /// <summary>
        /// Orders rows in each input table based on values in specified <paramref name="columns"/>.
        /// </summary>
        /// <param name="desc">Sort results in descending order.</param>
        /// <param name="columns">Columns to sort by. Sort precedence is determined by list order. Default is <c>["_value"]</c>.</param>
        /// <returns></returns>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/sort/">sort() function - InfluxDB documentation</seealso>
        IFluxStream Sort(bool desc, params string[] columns);

        /// <summary>
        /// Returns the first <paramref name="n"/> rows after the specified <paramref name="offset"/> from each input table.
        /// </summary>
        /// <remarks>If an input table has less than <c>offset + n</c> rows, it returns all rows after the <paramref name="offset"/>.</remarks>
        /// <param name="n">Maximum number of rows to return.</param>
        /// <param name="offset">Number of rows to skip per table before limiting to <paramref name="n"/>. Default is <c>0</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/limit/">limit() function - InfluxDB documentation</seealso>
        IFluxStream Limit(int n, int? offset = null);


        /// <summary>
        /// <para>Replaces all <see langword="null"/> values in input tables with a non-<see langword="null"/> value.</para>
        /// <para>Output tables are the same as the input tables with all <see langword="null"/> values replaced in the specified <paramref name="column"/>.</para>
        /// </summary>
        /// <param name="column">Column to replace <see langword="null"/> values in. Default is <c>_value</c>.</param>
        /// <param name="value">Constant value to replace <see langword="null"/> values with. Value type must match the type of the specified <paramref name="column"/>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/fill/">fill() function - InfluxDB documentation</seealso>
        IFluxStream Fill(object value, string? column = null);

        /// <summary>
        /// <para>Replaces all <see langword="null"/> values in input tables with the previous non-<see langword="null"/> value.</para>
        /// <para>Output tables are the same as the input tables with all <see langword="null"/> values replaced in the specified <paramref name="column"/>.</para>
        /// </summary>
        /// <param name="column">Column to replace <see langword="null"/> values in. Default is <c>_value</c>.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/fill/">fill() function - InfluxDB documentation</seealso>
        IFluxStream FillPrevious(string? column = null);

        /// <summary>
        /// Regroups input data by modifying group key of input tables.
        /// </summary>
        /// <remarks>Group does not guarantee sort order. To ensure data is sorted correctly, use <see cref="Sort"/> after.</remarks>
        /// <param name="columns">List of columns to use in the grouping operation. When no column is defined, it ungroups all data merges it into a single output table.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/group/">group() function - InfluxDB documentation</seealso>
        IFluxStream Group(params string[] columns);

        /// <summary>
        /// Regroups input data by modifying group key of input tables with all columns <b>except</b> those defined in the <paramref name="columns"/> parameter.
        /// </summary>
        /// <remarks>Group does not guarantee sort order. To ensure data is sorted correctly, use <see cref="Sort"/> after.</remarks>
        /// <param name="columns">List of columns to exclude from the grouping operation.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/group/">group() function - InfluxDB documentation</seealso>
        IFluxStream GroupExcept(params string[] columns);

        /// <summary>
        /// <para>Returns a stream of tables containing only the specified <paramref name="columns"/>.</para>
        /// <para>Columns in the group key that are not specified in the <paramref name="columns"/> parameter are removed from the group key and dropped from output tables.</para>
        /// </summary>
        /// <remarks>This function is the inverse of <see cref="Drop"/>.</remarks>
        /// <param name="columns">Columns to keep in output tables.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/keep/">keep() function - InfluxDB documentation</seealso>
        IFluxStream Keep(params string[] columns);

        /// <summary>
        /// <para>Removes specified <paramref name="columns"/> from a table.</para>
        /// <para>Columns in the group key that are specified in the <paramref name="columns"/> parameter are removed from the group key and dropped from output tables.</para>
        /// <para>If a specified column is not present in a table, the function returns an error.</para>
        /// </summary>
        /// <remarks>This function is the inverse of <see cref="Keep"/>.</remarks>
        /// <param name="columns">Columns to remove from input tables.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/drop/">drop() function - InfluxDB documentation</seealso>
        IFluxStream Drop(params string[] columns);

        /// <summary>
        /// Collects unique values stored vertically (column-wise) and aligns them horizontally (row-wise) into logical sets.
        /// </summary>
        /// <param name="columnKey">Columns to use to uniquely identify an output row.</param>
        /// <param name="rowKey">Columns to use to identify new output columns.</param>
        /// <param name="valueColumn">Column to use to populate the value of pivoted <paramref name="columnKey"/> columns.</param>
        /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/universe/pivot/">pivot() function - InfluxDB documentation</seealso>
        IFluxStream Pivot(string[] rowKey, string[] columnKey, string valueColumn = "_value");
    }
}

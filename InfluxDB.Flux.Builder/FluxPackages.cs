namespace InfluxDB.Flux.Builder
{
    /// <summary>
    /// List of Flux packages.
    /// </summary>
    /// <seealso href="https://docs.influxdata.com/flux/latest/stdlib/">Flux standard library - InfluxDB documentation</seealso>
    public static class FluxPackages
    {
        // Based on https://github.com/influxdata/flux/blob/v0.194.5/stdlib/packages.go

        public const string Array = "array";
        public const string Bitwise = "bitwise";
        public const string Csv = "csv";
        public const string Date = "date";
        public const string Date_Boundaries = "date/boundaries";
        public const string Dict = "dict";
        public const string Experimental = "experimental";
        public const string Experimental_Aggregate = "experimental/aggregate";
        public const string Experimental_Array = "experimental/array";
        public const string Experimental_Bigtable = "experimental/bigtable";
        public const string Experimental_Bitwise = "experimental/bitwise";
        public const string Experimental_Csv = "experimental/csv";
        public const string Experimental_Date_Boundaries = "experimental/date/boundaries";
        public const string Experimental_Dynamic = "experimental/dynamic";
        public const string Experimental_Geo = "experimental/geo";
        public const string Experimental_Http = "experimental/http";
        public const string Experimental_Http_Requests = "experimental/http/requests";
        public const string Experimental_InfluxDB = "experimental/influxdb";
        public const string Experimental_IOx = "experimental/iox";
        public const string Experimental_Json = "experimental/json";
        public const string Experimental_Mqtt = "experimental/mqtt";
        public const string Experimental_Oee = "experimental/oee";
        public const string Experimental_Polyline = "experimental/polyline";
        public const string Experimental_Prometheus = "experimental/prometheus";
        public const string Experimental_Query = "experimental/query";
        public const string Experimental_Record = "experimental/record";
        public const string Experimental_Table = "experimental/table";
        public const string Experimental_Usage = "experimental/usage";
        public const string Generate = "generate";
        public const string Http = "http";
        public const string Http_Requests = "http/requests";
        public const string InfluxData_InfluxDB = "influxdata/influxdb";
        public const string InfluxData_InfluxDB_Monitor = "influxdata/influxdb/monitor";
        public const string InfluxData_InfluxDB_Sample = "influxdata/influxdb/sample";
        public const string InfluxData_InfluxDB_Schema = "influxdata/influxdb/schema";
        public const string InfluxData_InfluxDB_Secrets = "influxdata/influxdb/secrets";
        public const string InfluxData_InfluxDB_Tasks = "influxdata/influxdb/tasks";
        public const string InfluxData_InfluxDB_V1 = "influxdata/influxdb/v1";
        public const string Internal_Boolean = "internal/boolean";
        public const string Internal_Debug = "internal/debug";
        public const string Internal_Gen = "internal/gen";
        public const string Internal_InfluxQL = "internal/influxql";
        public const string Internal_Location = "internal/location";
        public const string Internal_PromQL = "internal/promql";
        public const string Internal_Testing = "internal/testing";
        public const string Internal_TestUtil = "internal/testutil";
        public const string Interpolate = "interpolate";
        public const string Join = "join";
        public const string Json = "json";
        public const string Kafka = "kafka";
        public const string Math = "math";
        public const string Pagerduty = "pagerduty";
        public const string Planner = "planner";
        public const string Profiler = "profiler";
        public const string Pushbullet = "pushbullet";
        public const string Regexp = "regexp";
        public const string Runtime = "runtime";
        public const string SampleData = "sampledata";
        public const string Slack = "slack";
        public const string Socket = "socket";
        public const string Sql = "sql";
        public const string Strings = "strings";
        public const string System = "system";
        public const string Testing = "testing";
        public const string Testing_Expect = "testing/expect";
        public const string Timezone = "timezone";
        public const string Types = "types";
    }
}

# InfluxDB.Flux.Builder

[![License: Apache-2.0](https://img.shields.io/badge/License-Apache--2.0-blue.svg)](/LICENSE)
![PRs: welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)

**A .NET library to easily build Flux queries for InfluxDB.**

Forked from [PW.FluxQueryNet](https://github.com/paul-wurth/PW.FluxQueryNet), itself forked from [FluxQuery.Net by Malik Rizwan Bashir](https://github.com/MalikRizwanBashir/FluxQuery.Net) and significantly improved.


## Example

```csharp
var builder = FluxQueryBuilder.Create(new FluxBuilderOptions(ParameterizedTypes.All ^ ParameterizedTypes.RecordKey))
    .From("bucketName")
    .Range(new DateTime(2024, 11, 01, 14, 30, 00, DateTimeKind.Utc), TimeSpan.FromDays(2.5))
    .Filter(f => f.And(
        f.MatchMeasurement("weather"),
        f.Or(f.MatchTag("location", "London"), f["location"].Equal("Paris")),
        f.MatchField("temperature"),
        f.Value.Greater(min)
    ))
    .AggregateWindow("mean", TimeSpan.FromSeconds(5), createEmpty: false)
    .Limit(50);

Debug.WriteLine(builder.ToDebugQueryString());

using var client = new InfluxDBClient("http://localhost:8086", token);
var queryApi = client.GetQueryApi();
var tables = await queryApi.QueryAsync(builder.ToQuery(), org);
```

The code above generates the following Flux query (using `ToDebugQueryString()`) and
creates a [`Query` object](https://influxdata.github.io/influxdb-client-csharp/api/InfluxDB.Client.Api.Domain.Query.html) (using `ToQuery()`)
which is then sent with the [InfluxDB Client](https://github.com/influxdata/influxdb-client-csharp):
```flux
option params = {
  from_bucket_0: "bucketName",
  range_start_1: 2024-11-01T14:30:00Z,
  range_stop_2: 2d12h,
  filter_measurement_3: "weather",
  filter_value_4: "London",
  filter_value_5: "Paris",
  filter_field_6: "temperature",
  filter_value_7: 10,
  aggregateWindow_fn_8: mean,
  aggregateWindow_every_9: 5s,
  aggregateWindow_createEmpty_10: false,
  limit_n_11: 50,
}

from(bucket: params.from_bucket_0)
|> range(start: params.range_start_1, stop: params.range_stop_2)
|> filter(fn: (r) => (r._measurement == params.filter_measurement_3 and (r["location"] == params.filter_value_4 or r["location"] == params.filter_value_5) and r._field == params.filter_field_6 and r._value > params.filter_value_7))
|> aggregateWindow(fn: params.aggregateWindow_fn_8, every: params.aggregateWindow_every_9, createEmpty: params.aggregateWindow_createEmpty_10)
|> limit(n: params.limit_n_11)
```


## License

Copyright © Sylvain Bruyère 2024\
Copyright © Paul Wurth S.A. 2022-2024\
Copyright © Malik Rizwan Bashir

This repository is licensed under [Apache License 2.0 (Apache-2.0)](/LICENSE).
# InfluxDB.Flux.Builder

[![License: Apache-2.0](https://img.shields.io/badge/License-Apache--2.0-blue.svg)](/LICENSE)
![PRs: welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)

**A .NET library to easily build Flux queries for InfluxDB.**

Forked from [PW.FluxQueryNet](https://github.com/paul-wurth/PW.FluxQueryNet), itself forked from [FluxQuery.Net by Malik Rizwan Bashir](https://github.com/MalikRizwanBashir/FluxQuery.Net) and significantly improved.


## Example

```csharp
var fluxQuery = FluxQueryBuilder.Create()
    .From("bucketName")
    .Filter(f => f.Measurement("measurementName")
        .Tag("tagKey1", "tagValue1")
        .Tag("tagKey2", "tagValue2")
        .Fields("field1", "field2")
        .Where($"r._value > {min}")
    )
    .Range(new DateTime(2023, 01, 02, 03, 04, 05, DateTimeKind.Utc), TimeSpan.FromDays(2.5))
    .AggregateWindow("mean", TimeSpan.FromSeconds(5), createEmpty: false)
    .Limit(50);

Debug.WriteLine(fluxQuery.ToDebugQueryString());

using var client = new InfluxDBClient("http://localhost:8086", token);
var queryApi = client.GetQueryApi();
var tables = await queryApi.QueryAsync(fluxQuery.ToQuery(), org);
```

The code above generates the following Flux query (using `ToDebugQueryString()`) and
creates a [`Query` object](https://influxdata.github.io/influxdb-client-csharp/api/InfluxDB.Client.Api.Domain.Query.html) (using `ToQuery()`)
which is then sent with the [InfluxDB Client](https://github.com/influxdata/influxdb-client-csharp):
```flux
import  "experimental/record"

option params = {
  from_bucket_0: "bucketName",
  filter_measurement_1: "measurementName",
  filter_tagKey_2: "tagKey1",
  filter_tagValue_3: "tagValue1",
  filter_tagKey_4: "tagKey2",
  filter_tagValue_5: "tagValue2",
  filter_field_6: "field1",
  filter_field_7: "field2",
  filter_where_8: 0,
  range_start_9: 2023-01-02T03:04:05Z,
  range_stop_10: 2d12h,
  aggregateWindow_every_11: 5s,
  aggregateWindow_createEmpty_12: false,
  limit_n_13: 50,
}

from(bucket: params.from_bucket_0)
|> filter(fn: (r) => r._measurement == params.filter_measurement_1 and record.get(r: r, key: params.filter_tagKey_2, default: "") == params.filter_tagValue_3 and record.get(r: r, key: params.filter_tagKey_4, default: "") == params.filter_tagValue_5 and (r._field == params.filter_field_6 or r._field == params.filter_field_7) and r._value > params.filter_where_8)
|> range(start: params.range_start_9, stop: params.range_stop_10)
|> aggregateWindow(fn: mean, every: params.aggregateWindow_every_11, createEmpty: params.aggregateWindow_createEmpty_12)
|> limit(n: params.limit_n_13)
```


## License

Copyright © Sylvain Bruyère 2024\
Copyright © Paul Wurth S.A. 2022-2024\
Copyright © Malik Rizwan Bashir

This repository is licensed under [Apache License 2.0 (Apache-2.0)](/LICENSE).
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal sealed class JsonInt32ArrayConverter : JsonConverter<Int32Array>
{
    public override Int32Array Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        =>
        new(
            value: JsonSerializer.Deserialize<FlatArray<int>>(ref reader, options));

    public override void Write(Utf8JsonWriter writer, Int32Array value, JsonSerializerOptions options)
        =>
        JsonSerializer.Serialize(writer, value.Value, options);
}
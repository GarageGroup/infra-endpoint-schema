using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GarageGroup.Infra;

internal sealed class JsonStringArrayConverter : JsonConverter<StringArray>
{
    public override StringArray Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        =>
        new(
            value: JsonSerializer.Deserialize<FlatArray<string>>(ref reader, options));

    public override void Write(Utf8JsonWriter writer, StringArray value, JsonSerializerOptions options)
        =>
        JsonSerializer.Serialize(writer, value.Value, options);
}
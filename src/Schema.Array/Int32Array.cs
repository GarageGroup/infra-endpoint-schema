using System;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace GarageGroup.Infra;

[JsonConverter(typeof(JsonInt32ArrayConverter))]
public readonly record struct Int32Array : IOpenApiSchemaProvider, IEndpointTypeParser<Int32Array>
{
    public static OpenApiSchema GetSchema(bool nullable, IOpenApiAny? example = null, string? description = null)
        =>
        new()
        {
            Type = "array",
            Nullable = nullable,
            Items = new()
            {
                Type = "integer",
                Format = "int32",
                Nullable = false,
                Example = example,
                Description = description
            },
            Description = description
        };

    public static Result<Int32Array, Failure<Unit>> Parse(string? source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return Result.Success<Int32Array>(default);
        }

        var items = source.Split(',');
        var builder = FlatArray<int>.Builder.OfLength(items.Length);

        for (var i = 0; i < items.Length; i++)
        {
            var item = items[i];

            if (int.TryParse(item, out var result))
            {
                builder[i] = result;
                continue;
            }

            return Failure.Create($"Array item '{item}' is not a valid Int32 value");
        }

        return new Int32Array(
            value: builder.MoveToFlatArray());
    }

    public Int32Array(FlatArray<int> value)
        =>
        Value = value;

    public FlatArray<int> Value { get; }

    public FlatArray<int> ToFlatArray()
        =>
        Value;

    public static Int32Array From(FlatArray<int> value)
        =>
        new(value);

    public static implicit operator FlatArray<int>(Int32Array source)
        =>
        source.Value;

    public static implicit operator FlatArray<int>(Int32Array? source)
        =>
        source is null ? default : source.Value;

    public static implicit operator Int32Array(FlatArray<int> value)
        =>
        new(value);
}
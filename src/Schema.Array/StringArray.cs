using System;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace GarageGroup.Infra;

[JsonConverter(typeof(JsonStringArrayConverter))]
public readonly record struct StringArray : IOpenApiSchemaProvider, IEndpointTypeParser<StringArray>
{
    public static OpenApiSchema GetSchema(bool nullable, IOpenApiAny? example = null, string? description = null)
        =>
        new()
        {
            Type = "array",
            Nullable = nullable,
            Items = new()
            {
                Type = "string",
                Nullable = false,
                Example = example
            },
            Description = description
        };

    public static Result<StringArray, Failure<Unit>> Parse(string? source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return Result.Success<StringArray>(default);
        }

        return new StringArray(
            value: source.Split(',').Where(IsNotEmpty).ToFlatArray());

        static bool IsNotEmpty(string value)
            =>
            string.IsNullOrEmpty(value) is false;
    }

    public StringArray(in FlatArray<string> value)
        =>
        Value = value;

    public FlatArray<string> Value { get; }

    public FlatArray<string> ToFlatArray()
        =>
        Value;

    public static StringArray From(FlatArray<string> value)
        =>
        new(value);

    public static implicit operator FlatArray<string>(StringArray source)
        =>
        source.Value;

    public static implicit operator FlatArray<string>(StringArray? source)
        =>
        source is null ? default : source.Value;

    public static implicit operator StringArray(FlatArray<string> value)
        =>
        new(value);
}
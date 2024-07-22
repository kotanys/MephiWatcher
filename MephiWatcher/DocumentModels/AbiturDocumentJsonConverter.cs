using System.Text.Json;
using System.Text.Json.Serialization;

namespace MephiWatcher;

internal class AbiturDocumentJsonConverter : JsonConverter<AbiturDocument>
{
    public override AbiturDocument? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return AbiturDocumentFactory.Create(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, AbiturDocument value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
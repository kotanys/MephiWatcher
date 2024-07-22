using System.Text.Json.Serialization;

namespace MephiWatcher;

[JsonConverter(typeof(AbiturDocumentJsonConverter))]
public abstract class AbiturDocument;

using System.Text.Json.Serialization;

namespace ParseJsonBench.Supports;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(List<User>))]
internal partial class UserContext : JsonSerializerContext
{
}

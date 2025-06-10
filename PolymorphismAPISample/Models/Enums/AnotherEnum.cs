using System.Text.Json.Serialization;

namespace PolymorphismAPISample.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<AnotherEnum>))]
public enum AnotherEnum
{
	AnotherThing,
	AnotherThing2
}
using PolymorphismAPISample.Models.Enums;
using PolymorphismSample.Models;
using PolymorphismSample.Models.Converters;
using System.Text.Json.Serialization;

namespace PolymorphismAPISample.Models;

public class Person
{
	[JsonConverter(typeof(PetConverter))]
	public List<Pet>? Pets { get; set; } = new List<Pet>();

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public AnotherEnum? AnotherEnum { get; set; }
}
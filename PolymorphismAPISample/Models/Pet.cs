using PolymorphismAPISample.Models.Enums;
using System.Text.Json.Serialization;

namespace PolymorphismSample.Models;

public class Pet
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public FavoriteFoodType FavoriteFoodType { get; set; }
}
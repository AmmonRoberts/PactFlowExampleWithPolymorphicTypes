using System.Text.Json.Serialization;

namespace PolymorphismAPISample.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<FavoriteFoodType>))]
public enum FavoriteFoodType
{
	WetFood,
	DryFood
}
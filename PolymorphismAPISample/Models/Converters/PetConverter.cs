using PolymorphismSample.Models.Attributes;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PolymorphismSample.Models.Converters;

public class PetConverter : JsonConverter<List<Pet>>
{
	private readonly JsonNamingPolicy _namingPolicy = null;

	public override List<Pet> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var templates = new List<Pet>();

		using (var doc = JsonDocument.ParseValue(ref reader))
		{
			foreach (JsonElement element in doc.RootElement.EnumerateArray())
			{
				foreach (JsonProperty property in element.EnumerateObject())
				{
					var templateType = PetTypeAttribute.GetTypeByTemplateName(property.Name);

					if (templateType != null)
					{
						var template = (Pet)JsonSerializer.Deserialize(property.Value.GetRawText(), templateType, options)!;
						templates.Add(template);
					}
				}
			}
		}
		return templates;
	}

	public override void Write(Utf8JsonWriter writer, List<Pet> value, JsonSerializerOptions options)
	{
		writer.WriteStartArray();
		foreach (var template in value)
		{
			writer.WriteStartObject();
			var templateName = PetTypeAttribute.GetNameByTemplateType(template.GetType());

			if (!string.IsNullOrEmpty(templateName))
			{
				writer.WritePropertyName(templateName);
				var pascalCaseOptions = new JsonSerializerOptions(options)
				{
					PropertyNamingPolicy = _namingPolicy
				};

				JsonSerializer.Serialize(writer, template, template.GetType(), pascalCaseOptions);
			}
			writer.WriteEndObject();
		}
		writer.WriteEndArray();
	}
}

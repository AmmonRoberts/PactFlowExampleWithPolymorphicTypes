using Microsoft.OpenApi.Models;
using PolymorphismAPISample.Models.Enums;
using PolymorphismSample.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PolymorphismSample;

public class SampleSchemaFilter : ISchemaFilter
{
	public void Apply(OpenApiSchema schema, SchemaFilterContext context)
	{
		if (context.Type == typeof(List<Pet>))
		{
			schema.Type = "array";
			schema.Items = new OpenApiSchema
			{
				Type = "object",
				OneOf = new List<OpenApiSchema>
				{
					new OpenApiSchema
					{
						Type = "object",
						Properties = new Dictionary<string, OpenApiSchema>
						{
							["Dog"] = new OpenApiSchema
							{
								Type = "object",
								Properties = new Dictionary<string, OpenApiSchema>
								{
									["FavoriteFoodType"] = context.SchemaGenerator.GenerateSchema(typeof(FavoriteFoodType), context.SchemaRepository),
								}
							}
						}
					},
					new OpenApiSchema
					{
						Type = "object",
						Properties = new Dictionary<string, OpenApiSchema>
						{
							["Cat"] = new OpenApiSchema
							{
								Type = "object",
								Properties = new Dictionary<string, OpenApiSchema>
								{
									["FavoriteFoodType"] = context.SchemaGenerator.GenerateSchema(typeof(FavoriteFoodType), context.SchemaRepository),
								}
							}
						}
					}
				}
			};
		}
	}
}
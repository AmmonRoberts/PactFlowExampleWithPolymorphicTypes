using System.Reflection;

namespace PolymorphismSample.Models.Attributes;

public class PetTypeAttribute : Attribute
{
	public string Name { get; set; }

	public PetTypeAttribute(string name)
	{
		Name = name;
	}

	public static Type GetTypeByTemplateName(string name)
	{
		// Attribute can be used in other assemblies as well.
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach (var type in assembly.GetTypes())
			{
				PetTypeAttribute[] PetTypeAttributes =
					(PetTypeAttribute[])type.GetCustomAttributes<PetTypeAttribute>();

				if (PetTypeAttributes.Length > 0
					&& PetTypeAttributes[0].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
				{
					return type;
				}
			}
		}

		return null!;
	}

	public static string GetNameByTemplateType(Type type)
	{
		foreach (var assemblyType in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()))
		{
			if (assemblyType == type)
			{
				PetTypeAttribute[] PetTypeAttributes =
					(PetTypeAttribute[])assemblyType.GetCustomAttributes<PetTypeAttribute>();

				if (PetTypeAttributes.Length > 0)
				{
					return PetTypeAttributes[0].Name;
				}
			}
		}

		return string.Empty;
	}
}
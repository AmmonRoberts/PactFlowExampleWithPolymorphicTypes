namespace PolymorphismSample;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddSwaggerGen(options =>
		{
			options.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);

			options.SchemaFilter<SampleSchemaFilter>();
		});

		builder.Services.AddEndpointsApiExplorer();

		builder.Services.AddControllers();

		var app = builder.Build();

		app.UseSwagger();
		app.UseSwaggerUI();

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}
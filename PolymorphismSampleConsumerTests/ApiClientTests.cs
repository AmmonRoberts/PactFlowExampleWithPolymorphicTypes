using NFluent;
using PactNet;
using PolymorphismAPISample.Clients;
using PolymorphismAPISample.Models;
using PolymorphismAPISample.Models.Enums;
using PolymorphismSample.Models;
using Refit;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PolymorphismSampleConsumerTests;

[TestFixture]
public class Tests
{
	private IPactBuilderV4 _pactBuilder;
	private SystemTextJsonContentSerializer _jsonContentSerializer;
	private JsonSerializerOptions _jsonSerializerOptions;

	[SetUp]
	public void Setup()
	{
		_jsonSerializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			Converters =
			{
				new JsonStringEnumConverter(null)
			}
		};

		_jsonContentSerializer = new SystemTextJsonContentSerializer(_jsonSerializerOptions);

		var pact = Pact.V4("PolymorphismSampleConsumer", "PolymorphismAPISample", new PactConfig
		{
			PactDir = $"{Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName}{Path.DirectorySeparatorChar}pacts",
			DefaultJsonSettings = _jsonSerializerOptions
		});

		// Initialize Rust backend.
		_pactBuilder = pact.WithHttpInteractions();
	}

	[Test]
	public async Task PostPerson()
	{
		var request = new AddPersonRequest
		{
			Person = new Person
			{
				Pets =
				[
					new Dog
					{
						FavoriteFoodType = FavoriteFoodType.DryFood
					},
					new Cat
					{
						FavoriteFoodType = FavoriteFoodType.WetFood
					}
				],
				AnotherEnum = AnotherEnum.AnotherThing
			}
		};

		var response = new Person
		{
			Pets =
			[
				new Dog
				{
					FavoriteFoodType = FavoriteFoodType.DryFood
				},
				new Cat
				{
					FavoriteFoodType = FavoriteFoodType.WetFood
				}
			],
			AnotherEnum = AnotherEnum.AnotherThing
		};

		_pactBuilder
			.UponReceiving("Some POST request")
				.WithRequest(HttpMethod.Post, "/people")
				.WithBody(JsonSerializer.Serialize(request, _jsonSerializerOptions), "application/json; charset=utf-8")
			.WillRespond()
				.WithStatus(HttpStatusCode.OK)
				.WithHeader("Content-Type", "application/json; charset=utf-8")
				.WithBody(JsonSerializer.Serialize(response, _jsonSerializerOptions), "application/json; charset=utf-8");

		await _pactBuilder.VerifyAsync(async ctx =>
		{
			var client = RestService.For<IPersonClient>(ctx.MockServerUri.AbsoluteUri, new RefitSettings
			{
				ContentSerializer = _jsonContentSerializer,
			});

			Check.ThatCode(async () =>
			{
				var result = await client.CreatePersonAsync(request);

				Check.That(result).IsNotNull();
			}).DoesNotThrow();
		});
	}

	[Test]
	public async Task PostPersonButDifferent()
	{
		var request = new AddPersonRequest
		{
			Person = new Person
			{
				Pets =
				[
					new Dog
					{
						FavoriteFoodType = FavoriteFoodType.DryFood
					},
					new Cat
					{
						FavoriteFoodType = FavoriteFoodType.WetFood
					}
				],
				AnotherEnum = AnotherEnum.AnotherThing
			}
		};

		var response = new Person
		{
			Pets =
			[
				new Dog
				{
					FavoriteFoodType = FavoriteFoodType.DryFood
				},
				new Cat
				{
					FavoriteFoodType = FavoriteFoodType.WetFood
				}
			],
			AnotherEnum = AnotherEnum.AnotherThing
		};

		var serializedRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

		_pactBuilder
			.UponReceiving("Some POST request, but different")
				.WithRequest(HttpMethod.Post, "/people")
				.WithBody(JsonSerializer.Serialize(request, _jsonSerializerOptions), "application/json; charset=utf-8")
			.WillRespond()
				.WithStatus(HttpStatusCode.OK)
				.WithHeader("Content-Type", "application/json; charset=utf-8")
				.WithBody(JsonSerializer.Serialize(response, _jsonSerializerOptions), "application/json; charset=utf-8");

		await _pactBuilder.VerifyAsync(async ctx =>
		{
			var client = RestService.For<IPersonClient>(ctx.MockServerUri.AbsoluteUri, new RefitSettings
			{
				ContentSerializer = _jsonContentSerializer,
			});

			Check.ThatCode(async () =>
			{
				var result = await client.CreatePersonAsync(request);

				Check.That(result).IsNotNull();
			}).DoesNotThrow();
		});
	}
}
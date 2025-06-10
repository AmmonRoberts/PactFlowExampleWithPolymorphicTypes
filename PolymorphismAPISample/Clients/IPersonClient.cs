using PolymorphismAPISample.Models;
using PolymorphismSample.Models;
using Refit;

namespace PolymorphismAPISample.Clients;

public interface IPersonClient
{
	[Post("/people")]
	Task<Person> CreatePersonAsync(AddPersonRequest request);
}
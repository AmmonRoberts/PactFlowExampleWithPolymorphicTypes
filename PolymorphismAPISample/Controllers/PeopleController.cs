using Microsoft.AspNetCore.Mvc;
using PolymorphismAPISample.Models;
using PolymorphismSample.Models;

namespace PolymorphismSample.Controllers;

[ApiController]
[Route("people")]
public class PeopleController : ControllerBase
{
	private readonly ILogger<PeopleController> _logger;

	public PeopleController(ILogger<PeopleController> logger)
	{
		_logger = logger;
	}

	[HttpPost]
	[ProducesResponseType(typeof(Person), 200)]
	public async Task<IActionResult> CreatePerson([FromBody] AddPersonRequest request)
	{
		return Ok(await Task.FromResult(request.Person));
	}
}
using IntegrationTestDemo.Api.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTestDemo.Api.Controllers
{
    [Route("persons")]
    public class PersonsController : ControllerBase
    {
        [HttpGet("{personId}")]
        [Authorize]
        public IActionResult GetPerson(PersonId personId) => new OkObjectResult(personId);
    }
}

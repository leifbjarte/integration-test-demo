using IntegrationTestDemo.Api.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.Controllers
{
    [Route("persons")]
    public class PersonsController : ControllerBase
    {
        [HttpGet("{personId}")]
        public async Task<IActionResult> GetPerson(PersonId personId)
        {
            return new OkObjectResult(personId);
        }
    }
}

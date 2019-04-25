using IntegrationTestDemo.Api.Authorization;
using IntegrationTestDemo.Api.ErrorHandling;
using IntegrationTestDemo.Api.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.Controllers
{
    [Route("persons")]
    public class PersonsController : ControllerBase
    {
        #region Fields & Constructor

        private readonly IAuthorizationService authorizationService;

        public PersonsController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        #endregion

        [HttpGet("{personId}")]
        public IActionResult GetPerson(PersonId personId)
        {
            throw new SomeCustomError();
        }

        //[Authorize]
        [HttpPost("{personId}/documents/{documentId}")]
        public async Task<IActionResult> UploadFile(PersonId personId, Guid documentId, IFormFile file)
        {
            #region Authorization

            //var result = await authorizationService.AuthorizeAsync(User, null, new ElevatedUserRequirement());
            //if (!result.Succeeded)
            //    return new ForbidResult();

            #endregion

            var testHeader = Request.Headers.ContainsKey("x-test") ? Request.Headers["x-test"].FirstOrDefault() : null;

            using (var fileStream = file.OpenReadStream())
            {
                //Upload to blob, write to disk, send to someone - or just ignore
            }

            return new OkObjectResult(personId);
        }
    }
}

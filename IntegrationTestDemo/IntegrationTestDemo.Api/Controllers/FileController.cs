using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace IntegrationTestDemo.Api.Controllers
{
    [Route("files")]
    public class FileController : ControllerBase
    {
        [HttpPost("{documentId}")]
        public async Task<IActionResult> UploadFile(Guid documentId, IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            {
                //Upload to blob, write to disk, send to someone - or just ignore
            }

            return new OkResult();
        }
    }
}

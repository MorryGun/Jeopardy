using Jeopardy_Backend.Constants;
using Jeopardy_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Jeopardy_Backend.Controllers
{
    [Route(ApiConstants.FileRout)]
    [ApiController]
    public class S3Controller : ControllerBase
    {
        private readonly S3Service S3Service;

        public S3Controller(S3Service S3Service)
        {
            this.S3Service = S3Service;
        }

        [Route("get/{fileName}")]
        [HttpGet]
        public async Task<IActionResult> GetFile(string fileName)
        {
            try
            {
                var result = await this.S3Service.GetFile(fileName);
                return File(result, "text/html");
            }
            catch
            {
                return Ok("NoFile");
            }
        }
    }
}

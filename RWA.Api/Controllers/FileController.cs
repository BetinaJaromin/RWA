using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using RWA.Api.Models;

namespace RWA.Api.Controllers
{
    [Route("file")]
    [Authorize]
    public class FileController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] {"fileName"})] //cache'owanie odpowiedzi na 1200 sekund
        public ActionResult GetFile([FromQuery] string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrivateFile", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(filePath, out var contentType);

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType, fileName);
        }

        [HttpPost]
        public ActionResult Upload([FromForm] UploadRequest request)
        {
            if (request.File != null && request.File.Length > 0)
            {
                var safeName = Path.GetFileName(request.File.FileName); //usuwa złośliwe ścieżki
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrivateFile", safeName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    request.File.CopyTo(stream);
                }
                return Ok();
            }
            return BadRequest();
        }
    }

    
}

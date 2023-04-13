using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Land.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<FileController> _logger;

        public FileController(ILogger<FileController> logger) 
        {
            _logger = logger;
        }
        // ADD VALID POLICY
        //[HttpPost]
        //public ActionResult Post([FromForm] A file)
        //{
        //    //try
        //    //{
        //    //    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.FileName);

        //    //    using (var stream = new FileStream(path, FileMode.Create))
        //    //    {
        //    //        file.CopyTo(stream);
        //    //    }

        //    //    return StatusCode(StatusCodes.Status201Created);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    //}
        //    return StatusCode(StatusCodes.Status201Created);
        //}

        [HttpGet]
        public IActionResult Get()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "file-upload-article-cover.png");
            var image = System.IO.File.ReadAllBytes(path);
            return File(image, "image/jpeg");
        }
    }
}
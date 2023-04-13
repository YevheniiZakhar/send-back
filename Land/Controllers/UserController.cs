using Land.Models.Help;
using Microsoft.AspNetCore.Mvc;

namespace Land.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<AdController> _logger;
        private readonly LandDbContext _context;

        public UserController(ILogger<AdController> logger, LandDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("up")]
        [HttpPost]
        public ActionResult Up([FromForm] User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created);
        }

        [Route("in")]
        [HttpGet]
        public ActionResult In([FromForm] User user)
        {
            IQueryable<User> query = null;

            //switch(user.LoginMethod)
            //{
            //    case 0:
            //        query = _context.User.Where(u => u.Mail == user.Mail && u.Password == user.Password);
            //        break;
            //    case 1:
            //        query = _context.User.Where(u => u.Phone == user.Phone && u.Password == user.Password);
            //        break;
            //    case 2:
            //        query = _context.User.Where(u => u.Credential == user.Credential);
            //        break;
            //    default:
            //        break;
            //}

            if (query == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var logUser = query.FirstOrDefault();
            
            if (logUser != null)
                return StatusCode(StatusCodes.Status200OK);

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
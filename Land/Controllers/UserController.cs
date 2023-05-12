//using Land.Services;
using Microsoft.AspNetCore.Mvc;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Land.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        //private readonly ILogger<AdController> _logger;
        private readonly LandDbContext _context;
        //private readonly IAuthService _authService;

        public UserController(LandDbContext context)//, IAuthService authService)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            try
            {
                var result = await _context.User.FirstOrDefaultAsync(i => i.Email == user.Email && i.Password == user.Password);
                if (result == null)
                {
                    return BadRequest("User not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                //return ex.Message;
                return BadRequest(ex.Message);
            }
        }

        // oauth 2.0 google login api 
        


        [HttpGet("google")]
        public async Task<IActionResult> Google(string token)
        {
            try
            {
                var payload = await ValidateAsync(token, new ValidationSettings());

                var result = await _context.User.FirstOrDefaultAsync(i => i.Email == payload.Email);
                if (result == null)
                {
                    var record = new User() { Email = payload.Email, Name = payload.Name };
                    await _context.User.AddAsync(record);
                    await _context.SaveChangesAsync();

                    return Ok(record);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                //return ex.Message;
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                var result = await _context.User.FirstOrDefaultAsync(i => i.Email == user.Email);
                if (result == null)
                {
                    await _context.User.AddAsync(user);
                    await _context.SaveChangesAsync();

                    return Ok(user);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                //return ex.Message;
                return BadRequest(ex.Message);
            }
        }

        
    }
}
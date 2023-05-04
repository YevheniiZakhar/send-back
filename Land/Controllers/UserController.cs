using Google.Apis.Auth;
using Land.Helpers;
using Land.Models;
using Land.Models.Help;
using Land.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using static Google.Apis.Auth.JsonWebSignature;

namespace Land.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        //private readonly ILogger<AdController> _logger;
        private readonly LandDbContext _context;
        private readonly IAuthService _authService;

        public UserController(LandDbContext context, IAuthService authService)
        {
            //_logger = logger;
            _context = context;
            _authService = authService;
        }

        //[HttpGet("google")]
        //[AllowAnonymous]
        //public string Auhenticate()
        //{
        //    try
        //    {
        //        //SimpleLogger.Log("userView = " + userView.tokenId);
        //        var payload = GoogleJsonWebSignature.ValidateAsync("eyJhbGciOiJSUzI1NiIsImtpZCI6Ijc3NzBiMDg1YmY2NDliNzI2YjM1NzQ3NjQwMzBlMWJkZTlhMTBhZTYiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJuYmYiOjE2ODMwNjI4NjAsImF1ZCI6IjkwMTI0Mjg5NTI4OC1mY3NyYzQzb3JkbmNyMGhrbHY0bzZhbzZ1dXM4MGszZi5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjEwNzI2MDc5MTc2MTgyODc5MDU4NiIsImVtYWlsIjoieWV2aGVuaWl6YWtoYXJlbmtvQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJhenAiOiI5MDEyNDI4OTUyODgtZmNzcmM0M29yZG5jcjBoa2x2NG82YW82dXVzODBrM2YuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJuYW1lIjoi0J_QuNGC0LHRg9C70Ywg0JXQstCwIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hL0FHTm15eGFhZ2htNDd0Z2hTWWxEZ0gwaDUwRzJUc0pSVzItUktkWTZjOHlsZlE9czk2LWMiLCJnaXZlbl9uYW1lIjoi0J_QuNGC0LHRg9C70YwiLCJmYW1pbHlfbmFtZSI6ItCV0LLQsCIsImlhdCI6MTY4MzA2MzE2MCwiZXhwIjoxNjgzMDY2NzYwLCJqdGkiOiIwOTMxZmEyYTMzODQ1MTY4YTc2ZDI1MWI4NDMzOTU1OTdlNGY1MmY0In0.PS_RcPnhY4Z14Sua-JpY0WBcWUupY7cXAgx-vQjc4IR8u0Y0rG2rMVLVV1SOV6id4bESuuC7WU6rijztsorpqHVZVZXk3O1EroTVptOJEKbuQ66IXzPEFScm-jhxl41kI2XD8Fc_4vS2KqxracLHjaA_rRHVLtoqgHAubR555UzqPRDsWZVzc1xi_4PV-jlbplCdFeyMVgEhBDNnTTqzKh1z5-fwNMplKKbmaPcc76D7_NGj8hqDTBxzFUEXYkJ_aA0zlUZe0fielubsPsYdcxB6regF7hd8_hdrWxAtRG6IHUWl_s5GtSAvdPdX9R14f2KBInOCAL3ejNXtUgwxsw", new GoogleJsonWebSignature.ValidationSettings()).Result;

        //        var token = _authService.Authenticate(payload.Email);
               
        //        return token;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Helpers.SimpleLogger.Log(ex);
        //        BadRequest(ex.Message);
        //    }
        //    return null;
        //}

        //[HttpPost("google")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Auhenticate([FromBody] UserView userView)
        //{
        //    try
        //    {
        //        //SimpleLogger.Log("userView = " + userView.tokenId);
        //        var payload = GoogleJsonWebSignature.ValidateAsync(userView.tokenId, new GoogleJsonWebSignature.ValidationSettings()).Result;
        //        var user = await _authService.Authenticate(payload);
        //        //SimpleLogger.Log(payload.ExpirationTimeSeconds.ToString());

        //        var claims = new[]
        //        {
        //            new Claim(JwtRegisteredClaimNames.Sub, Security.Encrypt("JwtEmailEncryption",user.email)),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        };

        //        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("This is a sample secret key - please don't use in production environment."));
        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //        var token = new JwtSecurityToken(String.Empty,
        //          String.Empty,
        //          claims,
        //          expires: DateTime.Now.AddSeconds(55 * 60),
        //          signingCredentials: creds);
        //        return Ok(new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(token)
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        //Helpers.SimpleLogger.Log(ex);
        //        BadRequest(ex.Message);
        //    }
        //    return BadRequest();
        //}

        [AllowAnonymous]
        [HttpGet("google")]
        public async Task<User> Google(string token)
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

                    return record;
                }

                return result;


                //return _authService.Authenticate(payload.Email); //TODO do we really need that?

            }
            catch (Exception ex)
            {
                //return ex.Message;
                return null;
            }
        }

        //[HttpPost("googlein")]
        //[Authorize]
        //public string GoogleSignIn(string token)
        //{
        //    var payload = ValidateAsync(token, new ValidationSettings()).Result;
        //    return _authService.Authenticate(payload.Email);
        //}

        //[HttpPost("in")]
        //[Authorize]
        //public async Task<string> SignIn(User user)
        //{
        //    if (string.IsNullOrEmpty(user.Email))
        //    {
        //        return "Fill mail";
        //    }

        //    var result = await _context.User.FirstOrDefaultAsync(i => i.Mail == user.Mail);
        //    if (result == null)
        //    {
        //        return "can't find user";
        //    }
        //    var token = _authService.Authenticate(result.Mail);

        //    return token;
        //}

        //[HttpGet("jwt")]
        //[AllowAnonymous]
        //public string Post()
        //{
        //    //var handler = new JwtSecurityTokenHandler();
        //    //var jwtSecurityToken = handler.ReadJwtToken("eyJhbGciOiJSUzI1NiIsImtpZCI6ImM5YWZkYTM2ODJlYmYwOWViMzA1NWMxYzRiZDM5Yjc1MWZiZjgxOTUiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJuYmYiOjE2ODMwNTU0MzgsImF1ZCI6IjkwMTI0Mjg5NTI4OC1mY3NyYzQzb3JkbmNyMGhrbHY0bzZhbzZ1dXM4MGszZi5hcHBzLmdvb2dsZXVzZXJjb250ZW50LmNvbSIsInN1YiI6IjEwNzI2MDc5MTc2MTgyODc5MDU4NiIsImVtYWlsIjoieWV2aGVuaWl6YWtoYXJlbmtvQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjp0cnVlLCJhenAiOiI5MDEyNDI4OTUyODgtZmNzcmM0M29yZG5jcjBoa2x2NG82YW82dXVzODBrM2YuYXBwcy5nb29nbGV1c2VyY29udGVudC5jb20iLCJuYW1lIjoi0J_QuNGC0LHRg9C70Ywg0JXQstCwIiwicGljdHVyZSI6Imh0dHBzOi8vbGgzLmdvb2dsZXVzZXJjb250ZW50LmNvbS9hL0FHTm15eGFhZ2htNDd0Z2hTWWxEZ0gwaDUwRzJUc0pSVzItUktkWTZjOHlsZlE9czk2LWMiLCJnaXZlbl9uYW1lIjoi0J_QuNGC0LHRg9C70YwiLCJmYW1pbHlfbmFtZSI6ItCV0LLQsCIsImlhdCI6MTY4MzA1NTczOCwiZXhwIjoxNjgzMDU5MzM4LCJqdGkiOiJlOGM0ZTU4OTQwMmRmNzFkYWVkMmUzYjNmOGFkMzAyY2Y0NTIyMjY1In0.DbR-KRv4mM-P8YKyAtj_2oMe7sfsHOsWcuhF2iytnz8jZExUgxoAuG2EajuzCJzEfaeJdIMB4A6K538KyBqLRFoPTQTPVgdGD0r1Wq3FnSLeCHw7F00l80PArZ1RQfr7uGxjZ8msdAYPs8zdjTu3cmHoTP6hzpBC2qoiSB-9-aC-t6eg9AiJKX-ZMuGuGb0DkTWzHqQ8ATUCc7VkDKEMcnjVHXVSp7vMs4pLvxwlFAIuynDT-KaFj18517Btwj0jpuGokftTGsCtD_Xu-UrKgDd6fvaY7GcRIyufWK8O_awyF__E0YOQDwpcDJf74mCxLmxy_biY2JGSxc8OdE0btQ");
        //   // var issuer = "senduasendua";
        //   // var audience = "senduasendua";
        //    var key = Encoding.ASCII.GetBytes
        //    ("senduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasendua");
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //        //new Claim("Id", Guid.NewGuid().ToString()),
        //       // new Claim(JwtRegisteredClaimNames.Sub, "senduasendua"),
        //        new Claim(JwtRegisteredClaimNames.Email, "senduasendua"),
        //        new Claim(JwtRegisteredClaimNames.Jti,
        //        Guid.NewGuid().ToString())
        //     }),
        //        Expires = DateTime.UtcNow.AddMinutes(5),
        //      //  Issuer = issuer,
        //     //   Audience = audience,
        //        SigningCredentials = new SigningCredentials
        //        (new SymmetricSecurityKey(key),
        //        SecurityAlgorithms.HmacSha512Signature)
        //    };
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var jwtToken = tokenHandler.WriteToken(token);
        //    var stringToken = tokenHandler.WriteToken(token);
        //    return stringToken;
        //}

        //[Route("up")]
        //[HttpPost]
        //public ActionResult Up([FromForm] User user)
        //{
        //    _context.User.Add(user);
        //    _context.SaveChanges();

        //    return StatusCode(StatusCodes.Status201Created);
        //}

        //[Route("in")]
        //[HttpGet]
        //public ActionResult In([FromForm] User user)
        //{
        //    IQueryable<User> query = null;

        //    //switch(user.LoginMethod)
        //    //{
        //    //    case 0:
        //    //        query = _context.User.Where(u => u.Mail == user.Mail && u.Password == user.Password);
        //    //        break;
        //    //    case 1:
        //    //        query = _context.User.Where(u => u.Phone == user.Phone && u.Password == user.Password);
        //    //        break;
        //    //    case 2:
        //    //        query = _context.User.Where(u => u.Credential == user.Credential);
        //    //        break;
        //    //    default:
        //    //        break;
        //    //}

        //    if (query == null)
        //        return StatusCode(StatusCodes.Status500InternalServerError);

        //    var logUser = query.FirstOrDefault();

        //    if (logUser != null)
        //        return StatusCode(StatusCodes.Status200OK);

        //    return StatusCode(StatusCodes.Status500InternalServerError);
        //}
    }
}
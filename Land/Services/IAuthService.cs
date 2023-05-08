namespace Land.Services
{
    //public interface IAuthService
    //{
    //    string Authenticate(string email);
    //}
    //public class AuthService : IAuthService
    //{
    //    public string Authenticate(string email)
    //    {
    //        var key = Encoding.ASCII.GetBytes
    //        ("senduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasenduasendua");
    //        var tokenDescriptor = new SecurityTokenDescriptor
    //        {
    //            Subject = new ClaimsIdentity(new[]
    //            {
    //            new Claim("Id", Guid.NewGuid().ToString()),
    //            new Claim(JwtRegisteredClaimNames.Email, email),
    //            new Claim(JwtRegisteredClaimNames.Jti,
    //            Guid.NewGuid().ToString())
    //         }),
    //            Expires = DateTime.UtcNow.AddMinutes(5),
    //            //Issuer = issuer,
    //            //Audience = audience,
    //            SigningCredentials = new SigningCredentials
    //            (new SymmetricSecurityKey(key),
    //            SecurityAlgorithms.HmacSha512Signature)
    //        };
    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var token = tokenHandler.CreateToken(tokenDescriptor);
    //        var stringToken = tokenHandler.WriteToken(token);
    //        return stringToken;
    //    }
    //}
}

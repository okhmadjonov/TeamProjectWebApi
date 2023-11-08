using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeamProject.Entity;
using TeamProject.Entity.Enums;

namespace TeamProject.AuthToken
{
    public static class CreateTokenFromUser
    {
        private static readonly IHttpContextAccessor? httpContextAccessor;
        private static readonly IConfiguration _configuration;
     
        public static string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, (ERole.ADMIN).ToString()),
            new Claim(ClaimTypes.Role, (ERole.USER).ToString()),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aqpalxndhcbfur5dosps6sf54f654rt5we4r1vd5fg435v13d1f54s35vx5ssd5f45df2c1dll"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds
              //  issuer: "http://localhost:5019/"
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }



    }
}

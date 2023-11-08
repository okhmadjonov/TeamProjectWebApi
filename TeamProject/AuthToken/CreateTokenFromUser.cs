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

        public static string

CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, (ERole.ADMIN).ToString()),
                new Claim(ClaimTypes.Role, (ERole.USER).ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aqpalxndhcbfur5dosps6sf54f654rt5we4r1vd5fg435v13d1f54s35vx5ssd5f45df2c1dll"));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = signingCredentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);


            var jwtToken = tokenHandler.WriteToken(securityToken);

            return jwtToken;
        }
    }
}
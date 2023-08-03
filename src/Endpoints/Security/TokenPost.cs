using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using StoreAppStudy.Endpoints.Security;
using System.Security.Claims;
using System.Text;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace StoreAppStudy.Endpoints.Employees;

public class TokenPost {

    public static string Template => "/token";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static IResult Action(LoginRequest loginRequest, IConfiguration configuration, UserManager<IdentityUser> userManager) {

        var user = userManager.FindByEmailAsync(loginRequest.email).Result;

        if(user == null) {
            return Results.NotFound();
        }

        if (!userManager.CheckPasswordAsync(user, loginRequest.password).Result) {
            return Results.Unauthorized();
        }

        var claims = userManager.GetClaimsAsync(user).Result;

        var subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Email, loginRequest.email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
        });

        subject.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(configuration["JwtTokenSettings:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor {

            Subject = subject,

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),

            Audience = configuration["JwtTokenSettings:Audience"],
            Issuer = configuration["JwtTokenSettings:Issuer"],
            Expires = DateTime.UtcNow.AddHours(8)

        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Results.Ok(new { token = tokenHandler.WriteToken(token) });

    }
}

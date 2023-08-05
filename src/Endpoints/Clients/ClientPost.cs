using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using StoreAppStudy.Endpoints.Employees;
using System.Security.Claims;

namespace StoreAppStudy.Endpoints.Clients;

public class ClientPost {
    public static string Template => "/clients";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ClientRequest clientRequest, UserManager<IdentityUser> userManager) {

        var user = new IdentityUser {
            UserName = clientRequest.Email,
            Email = clientRequest.Email
        };
        var result = await userManager.CreateAsync(user, clientRequest.Password);

        if (!result.Succeeded) {
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());
        }

        var userClaims = new List<Claim> {
            new Claim("CPF", clientRequest.CPF),
            new Claim("Name", clientRequest.Name)
        };

        var claimResult = await userManager.AddClaimsAsync(user, userClaims);

        if (!claimResult.Succeeded) {
            return Results.BadRequest(claimResult.Errors.First());
        }


        return Results.Created($"/clients/{user.Id}", user.UserName);
    }
}

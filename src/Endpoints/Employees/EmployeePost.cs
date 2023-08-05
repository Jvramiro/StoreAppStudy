using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace StoreAppStudy.Endpoints.Employees;

public class EmployeePost {
    public static string Template => "/employee";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(EmployeeRequest employeeRequest, UserManager<IdentityUser> userManager) {

        var user = new IdentityUser {
            UserName = employeeRequest.name,
            Email = employeeRequest.email
        };
        var result = await userManager.CreateAsync(user, employeeRequest.password);

        if(!result.Succeeded) {
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());
        }

        var userClaims = new List<Claim> {
            new Claim("EmployeeCode", employeeRequest.EmployeeCode),
            new Claim("Name", employeeRequest.name)
        };

        var claimResult = await userManager.AddClaimsAsync(user, userClaims);

        if (!claimResult.Succeeded) {
            return Results.BadRequest(claimResult.Errors.First());
        }


        return Results.Created($"/employee/{user.Id}", user.UserName);
    }
}

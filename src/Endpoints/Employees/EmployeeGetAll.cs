using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace StoreAppStudy.Endpoints.Employees;

public class EmployeeGetAll {
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(UserManager<IdentityUser> userManager, int page, int rows) {

        if(rows > 100) {
            return Results.BadRequest("The number of rows cannot exceed 100.");
        }

        var users = await userManager.Users.Skip((page-1)*rows).Take(rows).ToListAsync();

        var response = new List<EmployeeResponse>();

        foreach (var user in users) {

            var claims = await userManager.GetClaimsAsync(user);
            var claimName = claims.FirstOrDefault(c => c.Type == "Name");
            var userName = claimName != null ? claimName.Value : string.Empty;

            response.Add(new EmployeeResponse(Name: userName, Email: user.Email));

        }

        return Results.Ok(response);

    }
}

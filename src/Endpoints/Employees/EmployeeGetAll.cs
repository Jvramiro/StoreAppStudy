using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace StoreAppStudy.Endpoints.Employees;

public class EmployeeGetAll {
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(UserManager<IdentityUser> userManager, int page, int rows) {

        if(rows > 100) {
            return Results.BadRequest("The number of rows cannot exceed 100.");
        }

        var users = userManager.Users.Skip((page-1)*rows).Take(rows).ToList();

        var response = new List<EmployeeResponse>();

        foreach (var user in users) {

            var claims = userManager.GetClaimsAsync(user).Result;
            var claimName = claims.FirstOrDefault(c => c.Type == "Name");
            var userName = claimName != null ? claimName.Value : string.Empty;

            response.Add(new EmployeeResponse(Name: userName, Email: user.Email));

        }

        return Results.Ok(response);

    }
}

﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace StoreAppStudy.Endpoints.Employees;

public class EmployeeGetAll {
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(UserManager<IdentityUser> userManager) {

        var users = userManager.Users.ToList();

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

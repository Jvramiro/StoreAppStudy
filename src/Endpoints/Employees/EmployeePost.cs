﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace StoreAppStudy.Endpoints.Employees;

public class EmployeePost {
    public static string Template => "/employee";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(EmployeeRequest employeeRequest, UserManager<IdentityUser> userManager) {

        var user = new IdentityUser {
            UserName = employeeRequest.name,
            Email = employeeRequest.email
        };
        var result = userManager.CreateAsync(user, employeeRequest.password).Result;

        if(!result.Succeeded) {
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());
        }

        var userClaims = new List<Claim> {
            new Claim("EmployeeCode", employeeRequest.EmployeeCode),
            new Claim("Name", employeeRequest.name)
        };

        var claimResult = userManager.AddClaimsAsync(user, userClaims).Result;

        if (!claimResult.Succeeded) {
            return Results.BadRequest(claimResult.Errors.First());
        }


        return Results.Created($"/employee/{user.Id}", user.Id);
    }
}

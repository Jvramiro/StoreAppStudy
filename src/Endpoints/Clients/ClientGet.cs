using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAppStudy.Data;
using StoreAppStudy.Endpoints.Products;
using System.Security.Claims;

namespace StoreAppStudy.Endpoints.Clients;

public class ClientGet {
    public static string Template => "/clients/{id:Guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromRoute] Guid id, HttpContext httpContext, ApplicationDbContext context) {

        var user = httpContext.User;
        var response = new {
            Id = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            Name = user.Claims.First(c => c.Type == "Name").Value,
            CPF = user.Claims.First(c => c.Type == "CPF").Value

        };

        return Results.Ok(response);

    }
}

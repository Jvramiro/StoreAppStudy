using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAppStudy.Data;
using StoreAppStudy.Domain.Products;
using System.Security.Claims;

namespace StoreAppStudy.Endpoints.Categories;

public class CategoryPost {
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(CategoryRequest categoryRequest, HttpContext httpContext, ApplicationDbContext context) {

        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var category = new Category(categoryRequest.name, userId, userId);

        if (!category.IsValid) {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.id}", category);
    }
}

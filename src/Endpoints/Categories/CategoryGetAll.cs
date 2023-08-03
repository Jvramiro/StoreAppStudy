using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAppStudy.Data;
using StoreAppStudy.Domain.Products;

namespace StoreAppStudy.Endpoints.Categories;

public class CategoryGetAll {
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action(ApplicationDbContext context, int page, int rows) {

        if (rows > 100) {
            return Results.BadRequest("The number of rows cannot exceed 100.");
        }

        var categories = context.Categories.Skip((page - 1) * rows).Take(rows).ToList();

        if (categories == null) {
            return Results.NotFound("No category found");
        }

        var response = categories.Select(c => new CategoryResponse {
            id = c.id,
            name = c.name
        }); ;

        return Results.Ok(response);
        
    }
}

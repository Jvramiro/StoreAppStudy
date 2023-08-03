using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAppStudy.Data;
using StoreAppStudy.Domain.Products;

namespace StoreAppStudy.Endpoints.Categories;

public class CategoryGet {
    public static string Template => "/categories/{id:Guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromRoute] Guid id, ApplicationDbContext context) {

        var category = await context.Categories.Where(p => p.id == id).FirstOrDefaultAsync();

        if (category == null) {
            return Results.NotFound("Requested Id not found");
        }

        var response = new CategoryRequest(category.name);

        return Results.Ok(response);

    }
}

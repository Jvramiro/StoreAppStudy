using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StoreAppStudy.Data;
using StoreAppStudy.Domain.Products;
using StoreAppStudy.Endpoints.Categories;

namespace StoreAppStudy.Endpoints.Products;

public class ProductGetAll {
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ApplicationDbContext context, int page, int rows) {

        if (rows > 100) {
            return Results.BadRequest("The number of rows cannot exceed 100.");
        }

        var products = await context.products.AsNoTracking().Skip((page - 1) * rows).Take(rows).ToListAsync();

        if (products == null) {
            return Results.NotFound("No product found");
        }
        var response = products.Select(p => new ProductResponse(p.name, p.categoryId, p.description, p.hasStock, p.price));
        return Results.Ok(response);

    }
}

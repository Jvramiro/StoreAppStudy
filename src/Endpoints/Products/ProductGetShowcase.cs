using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StoreAppStudy.Data;

namespace StoreAppStudy.Endpoints.Products;

public class ProductGetShowcase {
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ApplicationDbContext context, int page, int rows, string? orderBy) {

        if (rows > 100) {
            return Results.BadRequest("The number of rows cannot exceed 100.");
        }

        var queryBase = context.products.AsNoTracking().OrderBy(p => p.name)
            .Where(p => p.hasStock);

        switch (orderBy) {
            case "name":
                queryBase = queryBase.OrderBy(p => p.name);
            break;
            case "price":
                queryBase = queryBase.OrderBy(p => p.price);
            break;
        }

        var queryFilter = queryBase.Skip((page - 1) * rows).Take(rows);

        var products = await queryFilter.ToListAsync();

            //.ToListAsync();

        if (products == null) {
            return Results.NotFound("No product found");
        }
        var response = products.Select(p => new ProductResponse(p.name, p.categoryId, p.description, p.hasStock, p.price));
        return Results.Ok(response);

    }
}

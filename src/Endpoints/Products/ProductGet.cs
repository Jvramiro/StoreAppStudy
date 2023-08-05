using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAppStudy.Data;
using StoreAppStudy.Endpoints.Categories;

namespace StoreAppStudy.Endpoints.Products;

public class ProductGet {
    public static string Template => "/products/{id:Guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromRoute] Guid id, ApplicationDbContext context) {

        var product = await context.products.Where(p => p.id == id).FirstOrDefaultAsync();

        if (product == null) {
            return Results.NotFound("Requested Id not found");
        }

        var response = new ProductResponse(product.name, product.categoryId, product.description, product.hasStock, product.price);

        return Results.Ok(response);

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StoreAppStudy.Data;
using StoreAppStudy.Domain.Products;
using System.Security.Claims;

namespace StoreAppStudy.Endpoints.Products;

public class ProductPost {
    public static string Template => "/products";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(ProductRequest productRequest, HttpContext httpContext, ApplicationDbContext context) {

        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var category = await context.Categories.FirstOrDefaultAsync(c => c.id == productRequest.CategoryId);

        if(category == null) {
            return Results.NotFound("Category not found");
        }

        var product = new Product(productRequest.Name, productRequest.CategoryId,
            productRequest.Description, productRequest.HasStock, userId, userId, productRequest.Price);

        product.createdOn = DateTime.UtcNow;

        if(!product.IsValid) {
            return Results.ValidationProblem(product.Notifications.ConvertToProblemDetails());
        }

        await context.products.AddAsync(product);
        await context.SaveChangesAsync();

        return Results.Created($"/products/{product.id}", product.name);

    }
}

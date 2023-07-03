using Microsoft.AspNetCore.Mvc;
using StoreAppStudy.Data;
using StoreAppStudy.Domain.Products;

namespace StoreAppStudy.Endpoints.Categories;

public class CategoryGetAll {
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(ApplicationDbContext context) {

        var categories = context.Categories;
        
        if(categories == null) {
            return Results.NotFound("No category found");
        }

        var response = categories.Select(c => new CategoryResponse {
            id = c.id,
            name = c.name
        }); ;

        return Results.Ok(response);
        
    }
}

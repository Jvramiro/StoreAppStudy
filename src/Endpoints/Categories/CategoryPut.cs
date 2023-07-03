using Microsoft.AspNetCore.Mvc;
using StoreAppStudy.Data;
using StoreAppStudy.Domain.Products;

namespace StoreAppStudy.Endpoints.Categories;

public class CategoryPut {
    public static string Template => "/categories/{id:Guid}";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action([FromRoute] Guid id, CategoryRequest categoryRequest, ApplicationDbContext context) {

        var category = context.Categories.Where(p => p.id == id).FirstOrDefault();

        if (category == null) {
            return Results.NotFound("Requested Id not found");
        }

        if (!category.IsValid) {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        category.name = categoryRequest.name;

        var response = new CategoryRequest() {
            name = category.name
        };

        context.SaveChanges();

        return Results.Ok(response);
    }
}

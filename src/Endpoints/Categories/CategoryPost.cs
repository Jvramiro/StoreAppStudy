using Microsoft.AspNetCore.Mvc;
using StoreAppStudy.Data;
using StoreAppStudy.Domain.Products;

namespace StoreAppStudy.Endpoints.Categories;

public class CategoryPost {
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    public static IResult Action(CategoryRequest categoryRequest, ApplicationDbContext context) {

        var category = new Category(categoryRequest.name);

        if (!category.IsValid) {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        context.Categories.Add(category);
        context.SaveChanges();

        return Results.Created($"/categories/{category.id}", category);
    }
}

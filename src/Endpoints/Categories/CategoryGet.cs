using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAppStudy.Data;
using StoreAppStudy.Domain.Products;

namespace StoreAppStudy.Endpoints.Categories;

public class CategoryGet {
    public static string Template => "/categories/{id:Guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action([FromRoute] Guid id, ApplicationDbContext context) {

        var category = context.Categories.Where(p => p.id == id).FirstOrDefault();

        if (category == null) {
            return Results.NotFound("Requested Id not found");
        }

        var response = new CategoryRequest() {
            name = category.name
        };

        return Results.Ok(response);

    }
}

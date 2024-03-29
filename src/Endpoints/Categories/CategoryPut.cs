﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAppStudy.Data;
using StoreAppStudy.Domain.Products;
using System.Security.Claims;

namespace StoreAppStudy.Endpoints.Categories;

public class CategoryPut {
    public static string Template => "/categories/{id:Guid}";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handler => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action([FromRoute] Guid id, CategoryRequest categoryRequest, HttpContext httpContext, ApplicationDbContext context) {

        var userId = httpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var category = await context.Categories.Where(p => p.id == id).FirstOrDefaultAsync();

        if (category == null) {
            return Results.NotFound("Requested Id not found");
        }

        if (!category.IsValid) {
            return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
        }

        category.name = categoryRequest.name;
        category.editedBy = userId;

        var response = new CategoryRequest(category.name);

        context.SaveChanges();

        return Results.Ok(response);
    }
}

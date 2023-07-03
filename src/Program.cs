using Microsoft.AspNetCore.Identity;
using StoreAppStudy.Data;
using StoreAppStudy.Endpoints.Categories;
using StoreAppStudy.Endpoints.Employees;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionStrings:Local"]);
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapMethods(CategoryGet.Template, CategoryGet.Methods, CategoryGet.Handler);
app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handler);
app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handler);
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handler);
app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handler);
app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handler);

app.Run();
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using StoreAppStudy.Data;
using StoreAppStudy.Endpoints.Categories;
using StoreAppStudy.Endpoints.Clients;
using StoreAppStudy.Endpoints.Employees;
using StoreAppStudy.Endpoints.Products;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["ConnectionStrings:Local"]);
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options => {
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
    options.AddPolicy("EmployeePolicy", p => {
        p.RequireAuthenticatedUser()
            .RequireClaim("EmployeeCode");
    });
});
builder.Services.AddAuthentication(options => {

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options => {

    options.TokenValidationParameters = new TokenValidationParameters() {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtTokenSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtTokenSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtTokenSettings:Key"]))
    };

});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapMethods(CategoryGet.Template, CategoryGet.Methods, CategoryGet.Handler);
app.MapMethods(CategoryGetAll.Template, CategoryGetAll.Methods, CategoryGetAll.Handler);
app.MapMethods(CategoryPost.Template, CategoryPost.Methods, CategoryPost.Handler);
app.MapMethods(CategoryPut.Template, CategoryPut.Methods, CategoryPut.Handler);
app.MapMethods(EmployeePost.Template, EmployeePost.Methods, EmployeePost.Handler);
app.MapMethods(EmployeeGetAll.Template, EmployeeGetAll.Methods, EmployeeGetAll.Handler);
app.MapMethods(TokenPost.Template, TokenPost.Methods, TokenPost.Handler);
app.MapMethods(ProductGet.Template, ProductGet.Methods, ProductGet.Handler);
app.MapMethods(ProductGetAll.Template, ProductGetAll.Methods, ProductGetAll.Handler);
app.MapMethods(ProductGetShowcase.Template, ProductGetShowcase.Methods, ProductGetShowcase.Handler);
app.MapMethods(ProductPost.Template, ProductPost.Methods, ProductPost.Handler);
app.MapMethods(ClientGet.Template, ClientGet.Methods, ClientGet.Handler);
app.MapMethods(ClientPost.Template, ClientPost.Methods, ClientPost.Handler);

app.UseExceptionHandler("/error");
app.Map("/error", (HttpContext httpContext) => {
    var error = httpContext.Features?.Get<IExceptionHandlerFeature>()?.Error;
    if(error != null) {
        if(error is SqlException) {
            return Results.Problem(title: "Database out", statusCode: 500);
        }
    }
    return Results.Problem(title: "An error occured", statusCode: 500);
});

app.Run();
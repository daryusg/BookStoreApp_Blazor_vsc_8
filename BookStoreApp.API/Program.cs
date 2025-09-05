using BookStoreApp.API.Configurations;
using BookStoreApp.API.Data;
using BookStoreApp.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//--------------------------------------------------------------------------
//cip...14
var connString = builder.Configuration.GetConnectionString("BookStoreDbConnection");
builder.Services.AddDbContext<BookStoreDbContext>(options => options.UseSqlServer(connString));
//--------------------------------------------------------------------------
builder.Services.AddAutoMapper(typeof(MapperConfig)); //cip...19 the dependency injection package allows me to add automapper here

//builder.Services.AddIdentityCore<IdentityUser>()
builder.Services.AddIdentityCore<ApiUser>() //cip...28
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BookStoreDbContext>(); //cip...27

builder.Services.AddScoped<IAuthorsRepository, AuthorsRepository>(); //cip...64
builder.Services.AddScoped<IBooksRepository, BooksRepository>(); //cip...64

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//26/07/25 chatgpt: builder.Services.AddOpenApi();
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console()
      .ReadFrom.Configuration(ctx.Configuration)
); //cip...10 ctx=context, lc=logging configuration

//cip...11. esssentially, this is the ootb (default) policy.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
}); //cip...31

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    //26/07/25 chatgpt: You're using .AddOpenApi() and .MapOpenApi(), which means your project is using the Carter.OpenApi or similar minimal API extension, not the default Swashbuckle Swagger tooling that comes with the standard webapi template.
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); //cip...55. used to store images locally.

app.UseCors("AllowAll"); //cip...11

app.UseAuthentication(); //cip...31

app.UseAuthorization();

app.MapControllers();

app.Run();

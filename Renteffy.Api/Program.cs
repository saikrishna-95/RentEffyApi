using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Renteffy.Application.Implementation.Authetication;
using Renteffy.Application.Implementation.Owner;
using Renteffy.Application.Implementation.PasswordRestChange;
using Renteffy.Application.Implementation.Registration;
using Renteffy.Application.Implementation.User;
using Renteffy.Application.Interfaces.Authentication;
using Renteffy.Application.Interfaces.Owner;
using Renteffy.Application.Interfaces.PasswordRestChange;
using Renteffy.Application.Interfaces.Registration;
using Renteffy.Application.Interfaces.User;
using Renteffy.Domain.Services.Implementation.Authentication;
using Renteffy.Domain.Services.Implementation.Owner;
using Renteffy.Domain.Services.Implementation.PasswordRestChange;
using Renteffy.Domain.Services.Implementation.Registration;
using Renteffy.Domain.Services.Implementation.User;
using Renteffy.Domain.Services.Interfaces.Authentication;
using Renteffy.Domain.Services.Interfaces.Owner;
using Renteffy.Domain.Services.Interfaces.PasswordRestChange;
using Renteffy.Domain.Services.Interfaces.Registration;
using Renteffy.Domain.Services.Interfaces.User;
using Renteffy.Domain.Services.PersistanceInterfaces;
using Renteffy.Domain.Services.PersistanceInterfaces.Authentication;
using Renteffy.Domain.Services.PersistanceInterfaces.Owner;
using Renteffy.Domain.Services.PersistanceInterfaces.PasswordRestChange;
using Renteffy.Domain.Services.PersistanceInterfaces.User;
using Renteffy.Infrastructure.Security;
using Renteffy.Persistence.Implementation.Authentication;
using Renteffy.Persistence.Implementation.Owner;
using Renteffy.Persistence.Implementation.PasswordRestChange;
using Renteffy.Persistence.Implementation.Registration;
using Renteffy.Persistence.Implementation.User;
using Renteffy.Persistence.RegistrationDbContext;
using Renteffy.Shared.Database.DbConnection;
using Renteffy.Shared.Security;
using System.IO;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.Configure<FileStorageOptions>(builder.Configuration.GetSection("FileStorage"));
//var fileOptions = builder.Configuration.GetSection("FileStorage").Get<FileStorageOptions>();
//var folderName = builder.Configuration["FileStorage:FolderName"];
////var storagePath = Path.Combine(builder.Environment.ContentRootPath,folderName);
////var storagePath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", folderName));
//string storagePath;
//if (builder.Environment.IsDevelopment())
//{
//    storagePath = Path.Combine("C:\\", fileOptions.FolderName);
//}
//else
//{
//    storagePath = Path.Combine(Directory.GetParent(builder.Environment.ContentRootPath)!.FullName,fileOptions.FolderName);
//}
//try
//{
//    if (!Directory.Exists(storagePath))
//    {
//        Directory.CreateDirectory(storagePath);
//    }
//}
//catch (UnauthorizedAccessException)
//{
//    throw;
//}
//builder.Services.AddSingleton(new PhysicalFileProvider(storagePath));// Add services to the container.
builder.Services.AddSingleton(provider =>
{
    var config = builder.Configuration.GetSection("Cloudinary");
    var account = new Account(
        config["CloudName"],
        config["ApiKey"],
        config["ApiSecret"]
    );

    return new Cloudinary(account);
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("RenteffyCorsPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200", "http://localhost:3000", "https://www.renteffy.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwt = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwt["SecretKey"]!);

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddAuthentication("Bearer")
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IJwtKeyGenerator, JwtKeyGenerator>();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<IUserAuthApplication, UserAuthApplication>();
builder.Services.AddScoped<IUserRegistrationAapplication, UserRegistrationApplication>();
builder.Services.AddScoped<IPasswordRestOrChange, PasswordRestOrChange>();
builder.Services.AddScoped<IAddPostApplication, AddPostApplication>();
builder.Services.AddScoped<IGetOwnerPostsApplication, GetOwnerPostsApplication>();
builder.Services.AddScoped<IGetPostsByOwnerApplication, GetPostsByOwnerApplication>();

builder.Services.AddScoped<IUserAuthDomain, UserAuthDomain>();
builder.Services.AddScoped<IUserRegistrationDomain, UserRegistrationDomain>();
builder.Services.AddScoped<IPasswordRestOrChangeDomain, PasswordRestOrChangeDomain>();
builder.Services.AddScoped<IAddPostDomain, AddPostDomain>();
builder.Services.AddScoped<IGetOwnerPostsDomain, GetOwnerPostsDomain>();
builder.Services.AddScoped<IGetPostsByOwnerDomain, GetPostsByOwnerDomain>();

builder.Services.AddScoped<IUserReadPersistance, UserReadPersistance>();
builder.Services.AddScoped<IUserRegistrationPersistence, UserRegistrationPersistence>();
builder.Services.AddScoped<IPasswordRestOrChangePersistance,PasswordRestOrChangePersistance>();
builder.Services.AddScoped<IAddPostPersistence, AddPostPersistance>();
builder.Services.AddScoped<IGetOwnerPostsPersistence, GetOwnerPostsPersistence>();
builder.Services.AddScoped<IGetPostsByOwnerPersistance, GetPostsByOwnerPersistance>();

var app = builder.Build();
//app.UseForwardedHeaders(new ForwardedHeadersOptions
//{
//    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
//});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
//app.UsePathBase("");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1/swagger.json", "Renteffy API v1"); /*/ swagger /*/
    options.RoutePrefix = "swagger";
});
app.UseHttpsRedirection();
app.UseHostFiltering();
app.UseRouting();
//app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(storagePath),
//    RequestPath = fileOptions.PublicBaseUrl
//});
app.UseCors("RenteffyCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
//app.Urls.Add($"http://0.0.0.0:{port}");
app.Run();

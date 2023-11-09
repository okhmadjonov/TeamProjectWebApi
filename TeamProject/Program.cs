using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TeamProject.Data;
using TeamProject.Entity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new

string[] { }
        }
    });
});

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionStrings"));
    options.EnableSensitiveDataLogging();
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();


using (var onescope = app.Services.CreateScope())
{
    // Directly access the IHost instance from the app object
    var host = app; 
    await Seed.SeedUsersAndRolesAsync(host);
}

/*
if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await Seed.SeedUsersAndRolesAsync(app);
}
*/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policyBuilder =>
{
    policyBuilder
        .SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using

var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();




app.Run();
using CarInventory;
using CarInventory.CarInventory.Api;
using CarInventory.CarInventory.Bll;
using CarInventory.CarInventory.Dal;
using CarInventory.CarInventory.Dal.BaseObjects;
using CarInventory.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ��������� Identity 
builder.Services.AddIdentity<User, IdentityRole>() // ������� ApplicationUser  
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ��������� �������������� JWT 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

// ��������� NLog 
builder.Logging.ClearProviders();
builder.Host.UseNLog();

builder.Services.AddControllers();

// ��������� Swagger/OpenAPI 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "������� ����� � ��������� Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[]{ }
        }
    });
});


var app = builder.Build();

// ��������� middleware 
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ��������� Swagger � ������ ���������� 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ������������� �����
await InitializeRoles(app);

app.UseMiddleware<ErrorHandlingMiddleware>();

// ������ ����������
await app.RunAsync();


static async Task InitializeRoles(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await SeedRoles(roleManager);
        }
        catch (Exception ex)
        {
            // ����������� ������
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding roles.");
        }
    }
}

static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "Manager", "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole { Name = roleName });
        }
    }
}
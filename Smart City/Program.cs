using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Smart_City.Managers;
using Smart_City.Mapping;
using Smart_City.Models;
using Smart_City.Repositories;

var builder = WebApplication.CreateBuilder(args);

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// DbContext
builder.Services.AddDbContext<SmartCityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =============== Repositories ===============
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IComplaintRepositry, ComplaintsRepositry>();
builder.Services.AddScoped<ISuggestionsRepositories, SuggestionsRepositories>();
builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<INotificationsRepository, NotificationRepository>();
builder.Services.AddScoped<IUtilityIssueRepository, UtilityIssueRepository>();

// =============== Managers ===============
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IComplaintManager, ComplaintManager>();
builder.Services.AddScoped<ISuggestionManager, SuggestionManager>();
builder.Services.AddScoped<INotificationManager, NotificationManager>();
builder.Services.AddScoped<IUtilityIssueManager, UtilityIssueManager>();

// =============== JWT ===============
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    jwtKey = "dev_key_change_me_please_make_it_long_32_chars_min";
}

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),
            ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ===== configure Swagger to support Bearer auth (Authorize button) =====
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Smart City API",
        Version = "v1",
        Description = "API documentation"
    });

    // Define the Bearer scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Make sure swagger UI sends the token
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});
// =====================================================================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // must be before UseAuthorization
app.UseAuthorization();

app.MapControllers();
app.Run();

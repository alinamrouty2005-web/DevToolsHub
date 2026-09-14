using DevToolsHub.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using DevToolsHub.DataAccess.Repositories;
using DevToolsHub.Core.Interfaces;
using DevToolsHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DevToolsHub.API.Middleware;
using Microsoft.AspNetCore.Mvc;






var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// ======================================================
// Controllers
// ======================================================

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
 {
     options.InvalidModelStateResponseFactory = context =>
     {
         var errors = context.ModelState.Where(x => x.Value != null).ToDictionary(x => x.Key,x => x.Value!.Errors.Select(e => e.ErrorMessage).ToList());

         return new BadRequestObjectResult(
             new
             {
                 success = false,
                 message = "Validation failed.",
                 errors = errors
             });
     };
 });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// ======================================================
// OpenAPI / Swagger
// ======================================================

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

// ======================================================
// Database
// ======================================================

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DevToolsHub")));
// ======================================================
// Repositories
// ======================================================

builder.Services.AddScoped<ProjectRepository>();

builder.Services.AddScoped<ToolRepository>();

builder.Services.AddScoped<ToolHistoryRepository>();

builder.Services.AddScoped<FavoriteRepository>();

builder.Services.AddScoped<CollectionRepository>();

builder.Services.AddScoped<CollectionItemRepository>();

builder.Services.AddScoped<ApiRequestRepository>();

builder.Services.AddScoped<PlanRepository>();

builder.Services.AddScoped<SubscriptionRepository>();

builder.Services.AddScoped<NotificationRepository>();

builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<RoleRepository>();

builder.Services.AddScoped<UserRoleRepository>();
// ======================================================
// Services
// ======================================================

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IRoleService, RoleService>();

builder.Services.AddScoped<IUserRoleService, UserRoleService>();

builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<IToolService, ToolService>();

builder.Services.AddScoped<IToolHistoryService, ToolHistoryService>();

builder.Services.AddScoped<IFavoriteService, FavoriteService>();

builder.Services.AddScoped<ICollectionService, CollectionService>();

builder.Services.AddScoped<ICollectionItemService, CollectionItemService>();

builder.Services.AddScoped<IApiRequestService, ApiRequestService>();

builder.Services.AddScoped<IPlanService, PlanService>();

builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

builder.Services.AddScoped<INotificationService, NotificationService>();
// ======================================================
// Authentication - JWT
// ======================================================

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// ======================================================
// Authorization
// ======================================================

builder.Services.AddAuthorization();

// Health Checks

builder.Services.AddHealthChecks();


var app = builder.Build();

// Configure the HTTP request pipeline.
// ======================================================
// HTTP Request Pipeline
// ======================================================
// Swagger
// ======================================================
  
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();

    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

// ==========================================
// Middleware
// ==========================================

app.UseMiddleware<GlobalExceptionMiddleware>();

// Authentication must come before Authorization

app.UseAuthentication();

app.UseAuthorization();

// Controllers

app.MapControllers();

// ==========================================
// Health Check
// ==========================================

app.MapHealthChecks("/health");

app.Run();

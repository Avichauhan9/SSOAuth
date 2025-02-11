using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using SSO_Backend.Context;
using SSO_Backend.Middlewares;
using SSO_Backend.Services;
using SSO_Backend.Models.AppSettings;
using SSO_Backend.Utilities;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDBContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDB")));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
builder.Services.AddTransient<IPermissionService, PermissionService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo { Title = "SSO Auth Management", Version = "v1" });
    s.EnableAnnotations();
    // add JWT Authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    s.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, Array.Empty<string>()}
    });
});

builder.Services.Configure<AzureADConfig>(builder.Configuration.GetSection("AzureAd"));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(nameof(SmtpSettings)));

builder.Services.AddScoped<UserInfo, CurrentUser>();

builder.Services.AddTransient<IGraphService, GraphService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddTransient<IManageUserService, ManageUserService>();
builder.Services.AddTransient<IManageRoleService, ManageRoleService>();
builder.Services.AddTransient<IManagePermissionService, ManagePermissionService>();


builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpGlobalExceptionFilter>();
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpGlobalExceptionFilter>();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDBContext>();
        context.Database.Migrate();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
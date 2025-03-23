using Aperta_web_app.Configurations;
using Aperta_web_app.Contracts;
using Aperta_web_app.Data;
using Aperta_web_app.Data.Configurations;
using Aperta_web_app.Repository;
using Aperta_web_app.Services;
using Aperta_web_app.Services.Implementations;
using Aperta_web_app.Services.interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("GeneralAdmin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

builder.Services.AddHttpClient<MailgunEmailService>();

builder.Services.AddSingleton<IEmailService>(provider =>
            new MailgunEmailService(
            builder.Configuration["Mailgun:ApiKey"],
               builder.Configuration["Mailgun:Domain"],
                provider.GetRequiredService<HttpClient>()
           ));

builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<IAuthService, authService>();
builder.Services.AddScoped<IGroupService, GroupService>();


var apiKey = builder.Configuration["Mailgun:ApiKey"];
var domain = builder.Configuration["Mailgun:Domain"];
Console.WriteLine($"Extracted API Key: {apiKey}");
Console.WriteLine($"Extracted Domain: {domain}");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // Your frontend URL
              .AllowAnyHeader()
              .AllowAnyOrigin()
              .AllowAnyMethod();
    });
});




builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IClubsRepository, ClubsRepository>();
builder.Services.AddScoped<IGroupsRepository, GroupsRepository>();
builder.Services.AddScoped<IClubService, ClubService>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<User>>();

// Add controllers, endpoints, etc.
builder.Services.AddControllers();

//Stripe.StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

var app = builder.Build();



//var emailService = app.Services.GetRequiredService<IEmailService>();
//await (emailService as MailgunEmailService).SendEmailAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using System.Text;
using DNIT.WebAPI.Controllers.NewsAdmin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using DNIT.Core.Models;
using DNIT.Core;
using DNIT.Dao;
using DNIT.Core.Service;
using DNIT.Dao.Repository;


var builder = WebApplication.CreateBuilder(args);

// Đăng ký cấu hình MongoDBSettings từ appsettings.json
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings")
);

builder.Services.AddSingleton<IMongoClient>(sp =>
{
  var mongoSettings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
  return new MongoClient(mongoSettings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
  var mongoSettings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
  var client = sp.GetRequiredService<IMongoClient>();
  return client.GetDatabase(mongoSettings.DatabaseName);
});

builder.Services.AddScoped<IPasswordHasher<AuthModel>, PasswordHasher<AuthModel>>();


// Đăng ký AccountService
builder.Services.AddScoped<AuthService>();

// Thêm dịch vụ xác thực JWT
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
      };
    });
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<UserRepository>();

builder.Services.AddAuthorization();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Sử dụng Authentication và Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

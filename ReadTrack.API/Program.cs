using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReadTrack.API;
using ReadTrack.API.Data;
using ReadTrack.API.Extensions;
using ReadTrack.API.Services;
using ReadTrack.API.Models;
using ReadTrack.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(builder => 
    {
        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("sqldb");
builder.Services.AddDbContext<ReadTrackContext>(options => options.UseSqlServer(connectionString));

var jwtSettings = new JwtSettings();
var section = builder.Configuration.GetSection("JwtSettings");
section.Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddJwt(builder.Configuration, jwtSettings);

builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<ISessionService, SessionService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ReadTrack API");
    });
}

app.UseCors();

//app.UseHttpsRedirection();

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()
        .Error;

    var response = new { error = exception.Message };

    await context.Response.WriteAsJsonAsync(response);
}));

app.UseAuthorization();

app.CreateOrUpdateDatabaseAsync();

app.MapControllers();

app.Run();

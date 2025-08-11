using DotNetEnv;
using Desktop.Resume_Builder_API.resume_builder_api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load environment variables from .env file
Env.Load();

// Add configuration for environment variables
builder.Configuration.AddEnvironmentVariables();

// Register services
builder.Services.AddSingleton<OpenAIService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration["SQL_Connection_String"]));
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configure static files to use Resources folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Resources")),
    RequestPath = "/resources"
});

app.MapGet("/", async context =>
{ 
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("Resources/index.html");
});

app.MapControllers();

app.Run();

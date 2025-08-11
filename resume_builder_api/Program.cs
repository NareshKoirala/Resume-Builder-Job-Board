using Desktop.Resume_Builder_API.resume_builder_api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// **Remove Env.Load();**

builder.Configuration.AddEnvironmentVariables();  // This is fine

// Register services
builder.Services.AddSingleton<OpenAIService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration["SQL_Connection_String"]));  // Use env var

// ... rest of your code unchanged ...
builder.Services.AddControllers();

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Clear();
app.Urls.Add($"http://*:{port}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

// Optional: skip HTTPS redirection in production on Cloud Run
// app.UseHttpsRedirection();

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

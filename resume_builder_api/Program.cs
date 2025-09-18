using resume_builder_api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services
builder.Services.AddSingleton<JobFetching>();
builder.Services.AddSingleton<OpenAIService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ... rest of your code unchanged ...
builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}

// Optional: skip HTTPS redirection in production on Cloud Run
 app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Resources")),
    RequestPath = "/resources"
});

app.MapGet("/", async context =>
{
    var filePath = Path.Combine(app.Environment.ContentRootPath, "Resources", "index.html");
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync(filePath);
});


app.MapControllers();

app.Run();

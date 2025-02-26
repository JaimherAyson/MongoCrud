using MongoCrud.Server.Data;
using MongoCrud.Server.Models;
using MongoCrud.Server.Repositories;
using MongoCrud.Server.Services;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = "_myAllowSpecificOrigins";

// Configure CORS (restrict to Blazor Server URL)
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedOrigins, policy =>
    {
        policy.WithOrigins("https://localhost:7079", "http://localhost:5103") // Include all possible client URLs
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Required for secure Blazor Server communication
    });
});

// 🔹 Ensure cookies are **always secure** (before app.Build())
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always; // Force HTTPS-only cookies
});

// Load MongoDB settings from configuration
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register dependencies for DI
builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// Add controllers
builder.Services.AddControllers();

// Enable Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use middleware
app.UseCors(allowedOrigins);
app.UseHttpsRedirection();
app.UseCookiePolicy(); // Apply secure cookie policies
app.UseRouting(); // Required before authentication
app.UseAuthorization();

app.MapControllers();
app.Run();

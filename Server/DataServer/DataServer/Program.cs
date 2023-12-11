using DataServer.Hubs;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:5174") // Specify the client's origin
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // This is crucial
    });
});

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANT: Cors must be configured before UseRouting and endpoint-related middleware
app.UseCors("CorsPolicy");

// Uncomment if you use HTTPS redirection in production
// app.UseHttpsRedirection();

// Routing
app.UseRouting();

// Uncomment if you use authentication/authorization
// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<BlogHub>("/postHub");
    // ... other endpoints
});

app.Run();

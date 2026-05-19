using Models.Models;

var builder = WebApplication.CreateBuilder(args);

// Remove AddApiVersioning entirely (not needed for just displaying version)
// Remove AddSwaggerGen (use built-in OpenAPI only)

builder.Services.AddDbContext<ProductStoreContext>();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Version = "0.1.1";
        document.Info.Title = "backend_netcore_06";
        return Task.CompletedTask;
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API Product Management v0.1.1");
        options.RoutePrefix = string.Empty;
    });
}

app.Run();
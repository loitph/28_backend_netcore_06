using _28_backend_netcore_06.Models.DBQuanLyNhanVien;
using Models.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductStoreContext>();
builder.Services.AddDbContext<DBQuanLyNhanVienContext>();

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

// DI AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile(typeof(MappingProfile));
});

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
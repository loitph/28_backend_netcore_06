using _28_backend_netcore_06.Models.DBQuanLyNhanVien;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
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

// DI CORS cấp quyền GET cho domain có port 5000 và 5001
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowGETData", builder =>
    {
        builder.WithOrigins("http://127.0.0.1:51434", "http://127.0.0.1:51434")
            .WithMethods("GET")
            .AllowAnyHeader();
    });

    // định nghĩa phương post gọi api POST
    // /api/Product/AddProductLinq
    options.AddPolicy("AllowPOSTData", builder =>
    {
        builder.WithOrigins("http://127.0.0.1:51434", "http://127.0.0.1:51434")
            .WithMethods("POST")
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowGETData");
app.UseCors("AllowPOSTData");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API Product Management v0.1.1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapControllers();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var request = context.Request;
        var ipAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = request.Headers["User-Agent"].ToString();

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception != null)
        {
            var exceptionType = exception.GetType().Name;
            var exceptionMessage = exception.Message;
            var stackTrace = exception.StackTrace;

            // Stack Trace: {stackTrace}\n
            await context.Response.WriteAsync($"Error: {exceptionType}\nMessage: {exceptionMessage}\nIP Address: {ipAddress}\nUser Agent: {userAgent}");
        }
    });
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "FileServer")),
    RequestPath = "/uploads"
});


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "FileServer")),
    RequestPath = "/uploads"
});
app.Run();
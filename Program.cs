using _28_backend_netcore_06.Models.DBQuanLyNhanVien;
using _28_backend_netcore_06.Models.DBUser;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Models.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using middleware.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProductStoreContext>();
builder.Services.AddDbContext<DBQuanLyNhanVienContext>();


builder.Services.AddOpenApi(options =>
{
    // Tài liệu OpenAPI native (/openapi/v1.json) chính là tài liệu mà Swagger UI đọc.
    // Vì vậy security scheme phải được khai báo ở ĐÂY, không phải ở AddSwaggerGen.
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Version = "0.1.1";
        document.Info.Title = "backend_netcore_06";

        // Khai báo scheme Bearer -> tạo nút "Authorize" + ô nhập token trong Swagger UI
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Nhập token JWT (KHÔNG cần thêm tiền tố 'Bearer ')"
        };

        return Task.CompletedTask;
    });

    // Chỉ gắn yêu cầu bảo mật (icon ổ khóa) cho endpoint có [Authorize] và không có [AllowAnonymous]
    options.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;
        var requiresAuth = metadata.OfType<AuthorizeAttribute>().Any()
            && !metadata.OfType<AllowAnonymousAttribute>().Any();

        if (requiresAuth)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = new List<string>()
            });
        }

        return Task.CompletedTask;
    });
});


// DI DBUser
string? connectionStringUser = builder.Configuration.GetConnectionString("DBUser");
builder.Services.AddDbContext<UserDBContext>(options =>
{
    options.UseSqlServer(connectionStringUser);
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



//DI authentication - authorization = jwt
var key = builder.Configuration["Jwt:Key"]              // Khóa bí mật để ký token
    ?? throw new InvalidOperationException("Thiếu cấu hình 'Jwt:Key' trong appsettings.json");
var issuer = builder.Configuration["Jwt:Issuer"];     // Issuer (bên phát hành token)
var audience = builder.Configuration["Jwt:Audience"]; // Audience (người nhận token)
// 2. Cấu hình Authentication sử dụng JWT Bearer
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuerSigningKey = true, // Xác thực key bí mật của token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true,// Xác thực Issuer 
        ValidIssuer = issuer, // Phải khớp với Issuer trong token
        ValidateAudience = true,    // Xác thực Audience
        ValidAudience = audience, // Phải khớp với Audience trong token
        ValidateLifetime = true, // Xác thực thời gian hết hạn của token
        ClockSkew = TimeSpan.Zero, // Bỏ qua độ trễ thời gian giữa server và client (ngăn lỗi thời gian)
        RoleClaimType = ClaimTypes.Role, // Ánh xạ claim role
        NameClaimType = "UserName",
    };
});

// DI Jwt Service
builder.Services.AddScoped<JwtAuthService>();

//DI custom middleware CountIpRequestMiddleware

builder.Services.AddTransient<CountIpRequestMiddleware>();

var app = builder.Build();

app.UseCors("AllowGETData");
app.UseCors("AllowPOSTData");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API Product Management v0.1.1");
        // options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
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

app.UseAuthentication(); //Xác thực (đăng nhập)
app.UseAuthorization(); //Phân quyen
app.UseMiddleware<CountIpRequestMiddleware>();
app.Run();
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
using middleware.Middleware;

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


// DI DBUser
string? connectionStringUser = builder.Configuration.GetConnectionString("DBUser");
builder.Services.AddDbContext<UserDBContext>(options =>
{
    options.UseSqlServer(connectionStringUser);
});

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "API documentation for .NET 10"
    });
    // Khai báo scheme Bearer -> tạo nút "Authorize" + ô nhập token trong Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token JWT vào ô dưới đây"
    });

    // Áp scheme cho toàn bộ endpoint -> hiện icon ổ khóa và tự gắn header Authorization khi gọi API
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});

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
var key = builder.Configuration["Jwt:Key"];           // Khóa bí mật để ký token
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
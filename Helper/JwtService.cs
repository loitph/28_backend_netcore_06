using _28_backend_netcore_06.Models.DBUser;
using Microsoft.EntityFrameworkCore;
using static System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
public class JwtAuthService
{
    private readonly string? _key;
    private readonly string? _issuer;
    private readonly string? _audience;
    private readonly UserDBContext _context;
    public JwtAuthService(IConfiguration Configuration, UserDBContext db)
    {
        // Phải đọc đúng key như khi validate ở Program.cs (Jwt:Key), nếu không token ký ra sẽ không hợp lệ
        _key = Configuration["Jwt:Key"];
        _issuer = Configuration["Jwt:Issuer"];
        _audience = Configuration["Jwt:Audience"];
        _context = db;
    }
    
    public string GenerateToken(UserLoginDTO userLogin)
    {
        if (string.IsNullOrEmpty(_key))
        {
            throw new InvalidOperationException("Thiếu cấu hình 'Jwt:Key' trong appsettings.json");
        }

        // Khóa bí mật để ký token (dùng UTF8 để khớp với cấu hình validate ở Program.cs)
        var key = Encoding.UTF8.GetBytes(_key);
        User? userModel = _context.Users.SingleOrDefault(item => item.Username == userLogin.UserNameOrEmailOrPhone || item.Email == userLogin.UserNameOrEmailOrPhone || item.Phone == userLogin.UserNameOrEmailOrPhone);

        if (userModel == null)
        {
            throw new InvalidOperationException("Không tìm thấy người dùng để tạo token");
        }

        // Tạo danh sách các claims cho token
        var claims = new List<Claim>
        {
            new Claim("UserName", userModel.Username),               // Claim mặc định cho username
            // new Claim(ClaimTypes.Role, userLogin.Role),                   // Claim mặc định cho Role
            new Claim(JwtRegisteredClaimNames.Sub, userModel.Email),   // Subject của token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID của token
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), // Thời gian tạo token
            new Claim("Email", userModel.Email) // Thời gian tạo token
        };
        //Đưa role vào token - phải Include(IdRoleNavigation) vì lazy loading không bật, nếu không sẽ null
        List<UserRole> lstUserRole = _context.UserRoles
            .Include(item => item.IdRoleNavigation)
            .Where(item => item.IdUser == userModel.Id)
            .ToList();
        foreach(UserRole item in lstUserRole)
        {
            claims.Add(new Claim(ClaimTypes.Role, item.IdRoleNavigation.Rolename));
        }



        // Tạo khóa bí mật để ký token
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );
        // Thiết lập thông tin cho token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(2), // Token hết hạn sau 1 giờ
            SigningCredentials = credentials,
            Issuer = _issuer,                 // Thêm Issuer vào token
            Audience = _audience,              // Thêm Audience vào token
        };
        // Tạo token bằng JwtSecurityTokenHandler
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Trả về chuỗi token đã mã hóa
        return tokenHandler.WriteToken(token);
    }

    public string DecodePayloadToken(string token)
    {
        try
        {
            // Kiểm tra token có null hoặc rỗng không
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token không được để trống", nameof(token));
            }

            // Tạo handler và đọc token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Lấy username từ claims (thường nằm trong claim "sub" hoặc "name")
            var usernameClaim = jwtToken.Claims.FirstOrDefault(x =>x.Type == "UserName"); // Common in some identity providers

            if (usernameClaim == null)
            {
                throw new InvalidOperationException("Không tìm thấy username trong payload");
            }

            return usernameClaim.Value;
        }
        catch (Exception ex)
        {
            // Xử lý lỗi (có thể log lỗi ở đây)
            throw new InvalidOperationException($"Lỗi khi decode token: {ex.Message}", ex);
        }
    }

}
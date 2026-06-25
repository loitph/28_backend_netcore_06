using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _28_backend_netcore_06.Models.DBUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_netcore_06.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    // public List<UserDTO> lstUser = new List<UserDTO>
    // {
    //   new UserDTO { Id = 1, Email = "A", Name = "A" },
    //   new UserDTO { Id = 2, Email = "B", Name = "B" },
    //   new UserDTO { Id = 3, Email = "C", Name = "C" },
    //   new UserDTO { Id = 4, Email = "D", Name = "D" },
    //   new UserDTO { Id = 5, Email = "E", Name = "E" }
    // };
    

    // [HttpGet("GetAllUser")]
    // public List<UserDTO> GetAllUser()
    // {
    //   return lstUser;
    // }

    // [HttpGet("GetUserById/{id}")]
    // public UserDTO? GetUserById([FromRoute] int id)
    // {
    //   return lstUser.FirstOrDefault(p => p.Id == id);
    // }


    // [HttpGet("SearchUser")]
    // public List<UserDTO> SearchUser([FromQuery]string userName)
    // {
    //   var result = lstUser.Where(p => p.Name.Contains(userName)).ToList();
    //   return result;
    // }

    // [HttpPost("AddUser")]
    // public List<UserDTO> AddUser([FromBody]UserDTO user)
    // {
    //   lstUser.Add(user);

    //   return lstUser;
    // }

    // [HttpDelete("DeleteUser/{userId}")]
    // public List<UserDTO> DeleteUser([FromQuery]string userId)
    // {
    //   lstUser.Remove(lstUser.FirstOrDefault(p => p.Id == int.Parse(userId)));
    //   return lstUser;
    // }

    // [HttpPut("UpdateUser")]
    // public List<UserDTO> UpdateUser([FromBody]UserDTO user)
    // {
    //   var item = lstUser.FirstOrDefault(p => p.Id == user.Id);
    //   if (item != null)
    //   {
    //     item.Name = user.Name;
    //     item.Email = user.Email;
    //   }

    //   return lstUser;
    // }

    // [HttpPatch("PatchUser")]
    // public List<UserDTO> PatchUser([FromBody]UserDTO user)
    // {
    //   var item = lstUser.FirstOrDefault(p => p.Id == user.Id);
    //   if (item != null)
    //   {
    //     item.Email = user.Email;
    //   }

    //   return lstUser;
    // }

    private readonly UserDBContext _context;
    private readonly JwtAuthService _jwtService;

    public UserController(UserDBContext context, JwtAuthService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]UserRegisterDTO model)
    {
        var user = _context.Users.SingleOrDefault(item => item.Username == model.Username || item.Email == model.Email);
        if (user != null)
        {

            var res = new
            {
                message = "Tài khoản hoặc email đã được đăng ký !",
                satatus = 400
            };
            return StatusCode(400, res);
        }
        //Mặc định có 1 role -> User
        //Lưu ý: 1 nghiệp vụ chỉ savechange 1 lần
        //UserInsert:
        User newUser = new User();
        newUser.Email = model.Email;
        newUser.Phone = model.Phone;
        newUser.Deleted = false;
        newUser.Username = model.Username;
        newUser.HashPassword = HelperFunction.HashPassword(model.Password);

        UserRole userRole = new UserRole();
        userRole.IdUser = newUser.Id;
        userRole.IdRole = CRole.User;

        // Lưu ý: 1 nghiệp vụ chỉ savechange 1 lần
        // Lưu liên bảng UserRoles trước, nếu có failed cũng sẽ tự roll back
        newUser.UserRoles.Add(userRole);
        
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return Ok("User registered successfully!");
    }


    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody]UserLoginDTO user)
    {
      // check login -> create token
      User userLogin = _context.Users.SingleOrDefault(item => item.Username == user.UserNameOrEmailOrPhone || item.Email == user.UserNameOrEmailOrPhone || item.Phone == user.UserNameOrEmailOrPhone);
      if (userLogin == null)
      {
        return BadRequest("User not found");
      }

      // check pass with hash
      if (!HelperFunction.VerifyPassword(user.Password, userLogin.HashPassword))
      {
        return BadRequest("Password incorrect");
      }

      // if correct
      
      var token = _jwtService.GenerateToken(user);
      return Ok(token);
    }


    [HttpGet("GetUserInfo")]
    [Authorize]
    public async Task<IActionResult> GetUserInfo()
    {
      // Decode token through HttpContext
      bool valid = HttpContext.Request.Headers.TryGetValue("Authorization", out var token);
      if (valid)
      {
          string tokenValue = token.ToString().Replace("Bearer ", "");
          string username = _jwtService.DecodePayloadToken(tokenValue);
          User user = _context.Users.SingleOrDefault(item => item.Username == username);
          return Ok(user);
      }
      
      return Ok();
    }
  }
}
//api-controller-async
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using _28_backend_netcore_06.Models.DBUser;

namespace backend_netcore_dotnet06.Controllers
{
  [Route("api/[controller]")]
  [ApiController]

  public class DemoRedisController : ControllerBase
  {
    private readonly RedisService _redisService;
    private readonly UserDBContext _userContext;

    public DemoRedisController(RedisService redisService, UserDBContext userContext)
    {
      redisService.indexDB = 0;
      _redisService = redisService;
      _userContext = userContext;
    }

    /// <summary>
    /// Lấy danh sách tất cả người dùng từ Redis cache hoặc từ cơ sở dữ liệu nếu không có trong cache.
    /// </summary>
    /// <returns>
    ///  Danh sách người dùng dưới dạng JSON.
    /// </returns>
    /// <response code="200">Trả về danh sách người dùng.</response>
    /// <response code="500">Lỗi server.</response>
    [HttpGet("GetAllUserRedis")]
    public async Task<ActionResult> GetAllUserRedis()
    {
      //Kiểm tra key "all_users" có tồn tại trong Redis hay không
      var cachedUsers = await _redisService.GetStringAsync("all_users");

      if (!string.IsNullOrEmpty(cachedUsers))
      {
          var users = JsonSerializer.Deserialize<List<User>>(cachedUsers);
          return Ok(users);
      }

      var usersFromDb = await _userContext.Users.ToListAsync();
      await _redisService.SetStringAsync("all_users", JsonSerializer.Serialize(usersFromDb));
      return Ok(usersFromDb);
    }
  }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace backend_netcore_06.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : Controller
  {
    public List<UserDTO> lstUser = new List<UserDTO>
    {
      new UserDTO { Id = 1, Email = "A", Name = "A" },
      new UserDTO { Id = 2, Email = "B", Name = "B" },
      new UserDTO { Id = 3, Email = "C", Name = "C" },
      new UserDTO { Id = 4, Email = "D", Name = "D" },
      new UserDTO { Id = 5, Email = "E", Name = "E" }
    };
    

    [HttpGet("GetAllUser")]
    public List<UserDTO> GetAllUser()
    {
      return lstUser;
    }

    [HttpGet("GetUserById/{id}")]
    public UserDTO? GetUserById([FromRoute] int id)
    {
      return lstUser.FirstOrDefault(p => p.Id == id);
    }


    [HttpGet("SearchUser")]
    public List<UserDTO> SearchUser([FromQuery]string userName)
    {
      var result = lstUser.Where(p => p.Name.Contains(userName)).ToList();
      return result;
    }

    [HttpPost("AddUser")]
    public List<UserDTO> AddUser([FromBody]UserDTO user)
    {
      lstUser.Add(user);

      return lstUser;
    }

    [HttpDelete("DeleteUser/{userId}")]
    public List<UserDTO> DeleteUser([FromQuery]string userId)
    {
      lstUser.Remove(lstUser.FirstOrDefault(p => p.Id == int.Parse(userId)));
      return lstUser;
    }

    [HttpPut("UpdateUser")]
    public List<UserDTO> UpdateUser([FromBody]UserDTO user)
    {
      var item = lstUser.FirstOrDefault(p => p.Id == user.Id);
      if (item != null)
      {
        item.Name = user.Name;
        item.Email = user.Email;
      }

      return lstUser;
    }

    [HttpPatch("PatchUser")]
    public List<UserDTO> PatchUser([FromBody]UserDTO user)
    {
      var item = lstUser.FirstOrDefault(p => p.Id == user.Id);
      if (item != null)
      {
        item.Email = user.Email;
      }

      return lstUser;
    }
  }
}
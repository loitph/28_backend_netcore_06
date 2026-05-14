// RestFul API id, email, name, password
// get getall, getbyid, searchbyname
// post: adduser
// delete: deleteuser
// put: updateuser
// patch: updatepassword

using System.ComponentModel.DataAnnotations;

public class UserDTO
{

  public int Id { get; set; } =0;

  [Required(ErrorMessage = "Name required")]
  [MaxLength(50, ErrorMessage = "Name must be less than 50 characters")]
  public string? Name { get; set; }

  [Required(ErrorMessage = "Email required")]
  [EmailAddress(ErrorMessage = "Invalid email address")]
  public string? Email { get; set; }

  [Required(ErrorMessage = "Password required")]
  [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
  public string? Password { get; set; }
}
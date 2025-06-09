using System.ComponentModel.DataAnnotations;

namespace Tutorial11.DTO;

public class UserDataDTO
{
    [Required]
    public required string Login { get; set; }

    [Required] 
    public required string Password { get; set; }
}
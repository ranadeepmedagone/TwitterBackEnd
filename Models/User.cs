using Social.DTOs;

namespace Social.Models;

public record User
{
    public int Id { get; set; }
    public string FullName { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }



    public UserDTO asDto => new UserDTO
    {
        FullName = FullName,
        Email = Email,
        Password = Password,
    };
}
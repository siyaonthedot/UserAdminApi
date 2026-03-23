using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserDtos;

public class RequestUserDto
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
}

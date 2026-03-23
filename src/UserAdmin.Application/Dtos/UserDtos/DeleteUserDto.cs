using System;
using System.Security.Cryptography.X509Certificates;

namespace Application.Dtos.UserDtos;

public class DeleteUserDto
{
    public Guid UserId { get; set; }
    public Guid DeletedBy { get; set; }

    public string Email { get; set; }


}

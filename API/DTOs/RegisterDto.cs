using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; } = "";  //Else we get NRT warning
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
    [Required]
    [MinLength(4)]
    public string Password { get; set; } = "";
    
}

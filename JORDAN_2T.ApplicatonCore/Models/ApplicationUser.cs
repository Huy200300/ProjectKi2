using Microsoft.AspNetCore.Identity;

namespace JORDAN_2T.ApplicationCore.Models;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string? FullName { get; set; }
    [PersonalData]
    public DateTime? DOB { get; set; }
}
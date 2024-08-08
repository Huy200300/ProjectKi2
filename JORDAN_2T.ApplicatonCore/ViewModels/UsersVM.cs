using Microsoft.AspNetCore.Mvc.Rendering;
using JORDAN_2T.ApplicationCore.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.ComponentModel.DataAnnotations;

namespace JORDAN_2T.ApplicationCore.ViewModels;

public class UserRolesViewModel
{
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public bool IsSelected { get; set; }
}
public class EditUserViewModel
{
    public EditUserViewModel()
    {
        Claims = new List<string>();
        Roles = new List<string>();
    }

    public string Id { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string Phone { get; set; }

    public List<string> Claims { get; set; }

    public IList<string> Roles { get; set; }
}
public class UserVM
{
    public IEnumerable<ApplicationUser> users{get;set;}
    public string Search{get;set;}
}
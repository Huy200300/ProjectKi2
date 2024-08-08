#nullable disable

using JORDAN_2T.ApplicationCore.Models;
using JORDAN_2T.ApplicationCore.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JORDAN_2T.Infrastructure.Data;

    public class DbInitializer
    {
       
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MvcMovieContext _context;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            MvcMovieContext context)
        {
           
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public void Initialize()
        {
            // Migrations if they are not applied
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {
            }

            // _context.Categories.AddRange(
            //     new Category{
            //         Id=0,
            //         Name="New Category"
            //     }
            // );
            // _context.SubCategories.AddRange(
            //     new SubCategory{
            //         Id=0,
            //         CategoryId=1,
            //         Name="New Sub Category"
            //     }
            // );
            // Create roles if they are not created
            if (!_roleManager.RoleExistsAsync(WebsiteRole.Admin).GetAwaiter().GetResult())
            {

                
                _roleManager.CreateAsync(new IdentityRole(WebsiteRole.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRole.Managers)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRole.Customer)).GetAwaiter().GetResult();

                // If roles are not created, then we will create admin user as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@ltvaz.com",
                    Email = "admin@ltvaz.com",
                    FullName = "Admin",
                    DOB = new DateTime(2000, 01, 01),
                    PhoneNumber = "111222333",
                }, "Admin123@#").GetAwaiter().GetResult();

                ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@ltvaz.com");
 
                _userManager.AddToRoleAsync(user, WebsiteRole.Admin).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(user, WebsiteRole.Managers).GetAwaiter().GetResult();

            }
                 
           
                
            return;
        }
    }


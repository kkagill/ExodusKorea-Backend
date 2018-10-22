using ExodusKorea.Model.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExodusKorea.Data
{
    public class DbInitializer
    {
        private ExodusKoreaContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ExodusKoreaContext context,
                             UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeData()
        {
            ////////// Create Two Roles (Admin, User) and one admin account && one user account assigned with proper roles //////////

            var findAdminRole = await _roleManager.FindByNameAsync("Admin");
            var findUserRole = await _roleManager.FindByNameAsync("User");
            var adminRole = new IdentityRole("Admin");
            var userRole = new IdentityRole("User");

            //If admin role does not exists, create it
            if (findAdminRole == null)
            {
                await _roleManager.CreateAsync(adminRole);
            }
            //If user role does not exists, create it
            if (findUserRole == null)
            {
                await _roleManager.CreateAsync(userRole);
            }

            var findAdminAccount = await _userManager.FindByNameAsync("admin@gmail.com");

            //If there is no user account "admin@adps.com", create it       
            if (findAdminAccount == null)
            {
                var admin = new ApplicationUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",                 
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(admin, "P@$$w0rd");
                var account = await _userManager.FindByEmailAsync(admin.Email);
                account.EmailConfirmed = true;

                try
                {
                    if (result.Succeeded)
                    {
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            var adminAccount = await _userManager.FindByNameAsync("admin@gmail.com");
            //If Admin account is not in an admin role, add it to the role.
            if (!await _userManager.IsInRoleAsync(adminAccount, adminRole.Name))
            {
                await _userManager.AddToRoleAsync(adminAccount, adminRole.Name);
            }

            var findUserAccount = await _userManager.FindByNameAsync("user@gmail.com");
            //If there is no user account "test@gmail.com, create it       
            if (findUserAccount == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "user@gmail.com",
                    Email = "user@gmail.com",
                    NickName = "ㅋㅋㅋ",
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, "P@$$w0rd");
                var account = await _userManager.FindByEmailAsync(user.Email);
                account.EmailConfirmed = true;

                try
                {
                    if (result.Succeeded)
                    {
                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            var userAccount = await _userManager.FindByNameAsync("user@gmail.com");
            //If User account is not in an User role, add it to the role.
            if (!await _userManager.IsInRoleAsync(userAccount, userRole.Name))
            {
                await _userManager.AddToRoleAsync(userAccount, userRole.Name);
            }       

            /////////////////////////////////////////////////////////////////////////////////////////////////////

            // Look for any students.
            //if (_context.Students.Any())
            //{
            //    return;   // DB has been seeded
            //}

            //var students = new Student[]
            //{
            //    new Student { FirstMidName = "Carson",   LastName = "Alexander",
            //        EnrollmentDate = DateTime.Parse("2010-09-01") },
            //    new Student { FirstMidName = "Meredith", LastName = "Alonso",
            //        EnrollmentDate = DateTime.Parse("2012-09-01") },
            //    new Student { FirstMidName = "Arturo",   LastName = "Anand",
            //        EnrollmentDate = DateTime.Parse("2013-09-01") },
            //    new Student { FirstMidName = "Gytis",    LastName = "Barzdukas",
            //        EnrollmentDate = DateTime.Parse("2012-09-01") },
            //    new Student { FirstMidName = "Yan",      LastName = "Li",
            //        EnrollmentDate = DateTime.Parse("2012-09-01") },
            //    new Student { FirstMidName = "Peggy",    LastName = "Justice",
            //        EnrollmentDate = DateTime.Parse("2011-09-01") },
            //    new Student { FirstMidName = "Laura",    LastName = "Norman",
            //        EnrollmentDate = DateTime.Parse("2013-09-01") },
            //    new Student { FirstMidName = "Nino",     LastName = "Olivetto",
            //        EnrollmentDate = DateTime.Parse("2005-09-01") }
            //};

            //foreach (Student s in students)
            //{
            //    _context.Students.Add(s);
            //}         

            await _context.SaveChangesAsync();
        }
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProdCoreTPC.Identity
{
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated(); // создание базы и таблиц если её не существует
        }
    }
}

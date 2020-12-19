using Donator.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Donator.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<NPO> NonProfitOrgs { get; set; }
        public DbSet<NPOType> NPOTypes { get; set; }
        public DbSet<OrgUser> OrgUsers { get; set; }
        public DbSet<RequestForVolunteer> RequestForVolunteers { get; set; }
    }
}

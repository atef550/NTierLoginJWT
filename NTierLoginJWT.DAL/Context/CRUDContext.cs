using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace NTierLoginJWT.DAL.Context
{
    public class CRUDContext : IdentityDbContext
    {
        public CRUDContext(DbContextOptions<CRUDContext> options):base(options)
        {

        }
        public DbSet<User> Users{get;set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeamProject.Entity;

namespace TeamProject.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options, IServiceProvider services) : base(options)
        {
            this.Services = services;
        }

        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<Product> Products { get; set; }

        public IServiceProvider Services { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration<IdentityRole>(new RoleConfiguration(Services));
        }
    }
}

using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreAppStudy.Domain.Products;

namespace StoreAppStudy.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser> {

    public DbSet<Product> products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<Notification>();

        modelBuilder.Entity<Product>()
            .Property(p => p.name).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Product>()
            .Property(p => p.description).HasMaxLength(255).IsRequired(false);

        modelBuilder.Entity<Category>()
            .Property(p => p.name).IsRequired();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder) {
        configurationBuilder.Properties<string>().HaveMaxLength(100);
    }

}

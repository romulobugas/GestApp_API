using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GestApp_API.Models;

public class DbContext : IdentityDbContext<ApplicationUser> // Substitua ApplicationUser pela sua classe de usuário, se necessário
{
    public DbContext(DbContextOptions<DbContext> options) : base(options) { }

    public DbSet<UserLogin> UserLogins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações específicas do ASP.NET Core Identity (se necessário)
        // ...

        // Suas configurações personalizadas
        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasIndex(e => e.Login).IsUnique(); // Garante que a coluna Login seja única
            entity.Property(e => e.Login).HasColumnType("varchar(25)"); // Define o tipo da coluna Login
        });

        // Aplicar outras configurações de entidade de forma modular
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}

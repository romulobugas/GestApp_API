using Microsoft.EntityFrameworkCore;
using GestApp_API.Models;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbContext(DbContextOptions<DbContext> options) : base(options) { }

    // Adicionando DbSet para UserLogin
    public DbSet<UserLogin> UserLogins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configurações específicas de entidade        
        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasIndex(e => e.Login).IsUnique(); // Isso garantirá que a coluna Login seja única
            entity.Property(e => e.Login).HasColumnType("varchar(25)");
        });

        // Ou, para aplicar todas as configurações de uma vez, use:
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
    }
}

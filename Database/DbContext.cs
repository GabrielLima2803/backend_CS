using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Item> Itens { get; set; }
    public DbSet<Carrinho> Carrinhos { get; set; }
    public DbSet<ItemCarrinho> ItensCarrinho { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=itens.db")
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ItemCarrinho>()
            .HasKey(ic => new { ic.ItemId, ic.CarrinhoId });

        modelBuilder.Entity<ItemCarrinho>()
            .HasOne(ic => ic.Item)
            .WithMany()
            .HasForeignKey(ic => ic.ItemId)
            .OnDelete(DeleteBehavior.Restrict); // Impede a exclusão em cascata

        modelBuilder.Entity<ItemCarrinho>()
            .HasOne(ic => ic.Carrinho)
            .WithMany()
            .HasForeignKey(ic => ic.CarrinhoId)
            .OnDelete(DeleteBehavior.Restrict); // Impede a exclusão em cascata
    }
}

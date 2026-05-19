using Microsoft.EntityFrameworkCore;

namespace Models.Models {
  public class ProductStoreContext : DbContext {
    public ProductStoreContext() { }
    public ProductStoreContext(DbContextOptions<ProductStoreContext> options) : base (options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Server=localhost,1433;Database=dotnetcore_06_db;User Id=sa;Password=Admin@1234;TrustServerCertificate=True;");
    }

    // Define Tables as Class in Codebase
    public DbSet<Product> Products { get; set; }
  }
}
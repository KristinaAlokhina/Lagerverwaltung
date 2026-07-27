using Microsoft.EntityFrameworkCore;

namespace Lagerverwaltung
{
    public class AppDbContext : DbContext
    {
        // Produkttabelle in der Datenbank
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Wir geben an, dass wir SQLite verwenden und die Datenbankdatei lager.db heißen wird
            optionsBuilder.UseSqlite("Data Source=lager.db");
        }
    }
}

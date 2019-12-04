using AdvertisementSuperSlayer.DbModels;
using Microsoft.EntityFrameworkCore;

namespace AdvertisementSuperSlayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        private string _dbPath;

        public static ApplicationDbContext Create(string databasePath)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext(databasePath);
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
            return dbContext;
        }

        public ApplicationDbContext(string dbPath)
        {
            this._dbPath = dbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=" + _dbPath);
        }

        public DbSet<FirstTime> FirstTime { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
    }
}

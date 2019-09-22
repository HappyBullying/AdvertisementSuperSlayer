using Microsoft.EntityFrameworkCore;

namespace AdvertisementSuperSlayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        private string _dbPath;

        public ApplicationDbContext(string dbPath)
        {
            this._dbPath = dbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=" + _dbPath);
        }
    }
}

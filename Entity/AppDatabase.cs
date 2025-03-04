using AWSS3Zip.Entity.Contracts;
using AWSS3Zip.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace AWSS3Zip.Entity
{
    public class AppDatabase : DbContext, IAppDatabase
    {
        public DbSet<IISLogEvent> IISLogEvents;

        public string ConnectionString { get; set; }

        public AppDatabase(DbContextOptions<AppDatabase> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

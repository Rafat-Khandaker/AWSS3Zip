using AWSS3Zip.Entity.Contracts;
using AWSS3Zip.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace AWSS3Zip.Entity
{
    public class AppDatabase : DbContext, IAppDatabase
    {
        public DbSet<IISLogEvent> IISLogEvents { get; set; }

        public string ConnectionString { get; set; }

        public AppDatabase(DbContextOptions<AppDatabase> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public AppDatabase DetachEntities() {
            var trackedEntities = ChangeTracker.Entries()
                                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Unchanged)
                                    .ToList();

            foreach (var entry in trackedEntities)
            {
                entry.State = EntityState.Detached;
            }

            ChangeTracker.Clear();

            return this;
        }

        public void Attach_And_Save_Entities(List<IISLogEvent> newEntities) { 

            foreach(var entity in newEntities)
            {
               var existingEntity = ChangeTracker.Entries<IISLogEvent>()
                                        .FirstOrDefault(e => e.Entity.Id == entity.Id);

                if(existingEntity != null)
                    Entry(existingEntity.Entity).CurrentValues.SetValues(entity);
                else IISLogEvents.Attach(entity);

                try { 
                    SaveChanges(); 
                } 
                catch (Exception ex) {
                    continue;
                }
            }

        }
    }
}

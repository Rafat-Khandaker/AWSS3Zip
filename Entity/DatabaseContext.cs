using AWSS3Zip.Entity.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AWSS3Zip.Entity
{
    public class DatabaseContext : IDatabaseContext<AppDatabase>, IDisposable
    {
        public AppDatabase Database { get; set; }
        public string ConnectionString { get; set; }

        string DefaultConnection = "Data Source=localdb.db";
        private bool _disposed = false;


        public DatabaseContext AddConnection(string connectionString) {
            ConnectionString = connectionString;
            return this;
        }
        public AppDatabase Build() => Build(ConnectionString);

        public AppDatabase Detach() => Database.DetachEntities();


        private AppDatabase Build(string connection = null)
        {
            if (Database == null)
            {
                var optionsBuilder = new DbContextOptionsBuilder<AppDatabase>();
                if (!connection.IsNullOrEmpty())
                    optionsBuilder.EnableSensitiveDataLogging().UseSqlServer(connection);
                else optionsBuilder.EnableSensitiveDataLogging().UseSqlite(DefaultConnection);
                Database = new AppDatabase(optionsBuilder.Options);
            }

            return Database;
        }                

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
             if (_disposed)
                    return;

             if (disposing)
             {
                Database?.Dispose();
             }

             _disposed = true;
        }

    }
}

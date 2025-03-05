using AWSS3Zip.Entity.Contracts;

namespace AWSS3Zip.Entity
{
    public class DatabaseContext : IDatabaseContext<DatabaseFactory, AppDatabase>
    {
        IDatabaseFactory DatabaseFactory;
        public AppDatabase AppDatabase { get; set; }

        public string ConnectionString { get; set; }

        public DatabaseContext(IDatabaseFactory _DatabaseFactory) {
            DatabaseFactory = _DatabaseFactory;
        }

        public DatabaseContext AddConnection(string connectionString) {
            ConnectionString = connectionString;
            return this;
        }
        public AppDatabase Build()
        {
            AppDatabase = DatabaseFactory.Build(ConnectionString);
            return AppDatabase;
        }
    }
}

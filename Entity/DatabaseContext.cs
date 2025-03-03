using AWSS3Zip.Entity.Contracts;

namespace AWSS3Zip.Entity
{
    public class DatabaseContext : IDatabaseContext<DatabaseFactory, AppDatabase>
    {
        IDatabaseFactory DatabaseFactory;
        public DatabaseContext(IDatabaseFactory _DatabaseFactory) {
            DatabaseFactory = _DatabaseFactory;
        }
        public AppDatabase Build()
        {
            return DatabaseFactory.Build();
        }
    }
}

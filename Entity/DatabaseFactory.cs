using AWSS3Zip.Entity.Contracts;

namespace AWSS3Zip.Entity
{
    public class DatabaseFactory : IDatabaseFactory
    {
        IAppDatabase AppDatabase;
        public DatabaseFactory(IAppDatabase _AppDatabase) {
            AppDatabase = _AppDatabase;
        }

        public AppDatabase Build()
        {
            return null;
        }
    }
}

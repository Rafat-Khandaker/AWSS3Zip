using AWSS3Zip.Entity.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AWSS3Zip.Entity
{
    public class AppDatabase : IAppDatabase
    {
        public DbSet<object> timestamp;

        private readonly string _connectionString;
    }
}

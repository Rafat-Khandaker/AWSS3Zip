using AWSS3Zip.Entity.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AWSS3Zip.Entity
{
    public class DatabaseFactory : IDatabaseFactory
    {
        AppDatabase AppDatabase { get; set; }
    
        public AppDatabase Build(string connection)
        {
            if (AppDatabase == null) {
                var optionsBuilder = new DbContextOptionsBuilder<AppDatabase>();
                optionsBuilder.UseSqlServer(connection);

                AppDatabase = new AppDatabase(optionsBuilder.Options);
            }
           
            return AppDatabase;
        }
    }
}

using AWSS3Zip.Entity.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;

namespace AWSS3Zip.Entity
{
    public class DatabaseFactory : IDatabaseFactory
    {
        AppDatabase AppDatabase { get; set; }
        string DefaultConnection = "Data Source=localdb.db";

        public AppDatabase Build(string connection = null)
        {
            if (AppDatabase == null) {
                var optionsBuilder = new DbContextOptionsBuilder<AppDatabase>();
                if (!connection.IsNullOrEmpty())
                    optionsBuilder.EnableSensitiveDataLogging().UseSqlServer(connection);
                else optionsBuilder.EnableSensitiveDataLogging().UseSqlite(DefaultConnection);
                    AppDatabase = new AppDatabase(optionsBuilder.Options);
            }
           
            return AppDatabase;
        }

    }
}

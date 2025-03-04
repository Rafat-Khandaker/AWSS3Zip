using AWSS3Zip.Commands.Contracts;
using AWSS3Zip.Entity;
using AWSS3Zip.Entity.Contracts;
using AWSS3Zip.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace AWSS3Zip.Commands
{
    public class DatabaseJob : IProcessJob
    {
        public IDatabaseContext<DatabaseFactory, AppDatabase> DatabaseContext { get; set; }
        public string[] Parameters { get; set; }
        List<IISLogEvent> EntityLogEvents { get; set; }

        public void Execute()
        {
            var iPath = Array.IndexOf(Parameters, "-db");

            if (iPath == -1) Array.IndexOf(Parameters, "--database");
            if (iPath != -1 && (Parameters[iPath + 1].Contains("-") || Parameters[iPath + 1].Contains("--")))
            {
                Console.WriteLine("Command not formatted Correctly, contains '-' or '--' followed by command variable");
                return;
            }

            Console.WriteLine("Creating Database Schema for IIS Logs from S3 Zip Files");
            DatabaseContext.AddConnection(Parameters[iPath + 1]).Build().Database.Migrate();
        }


        public DatabaseJob(IDatabaseContext<DatabaseFactory, AppDatabase> _dbContext) {
            DatabaseContext = _dbContext;  
        }

        public DatabaseJob BuildParameters(string[] parameters, List<IISLogEvent> entityLogEvents) {
            Parameters = parameters;
            EntityLogEvents = entityLogEvents;
            return this;
        }
    }
}

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
            if (iPath != -1 &&  iPath < Parameters.Length && (Parameters[iPath + 1].Contains("-") || Parameters[iPath + 1].Contains("--")))
            {
                Console.WriteLine("Command not formatted Correctly, contains '-' or '--' followed by command variable");
                try { DatabaseContext.AddConnection(Parameters[iPath + 1]).Build().Database.Migrate(); }
                catch(Exception e){
                    Console.WriteLine("I am sorry! This flow of program was not tested appropriately. May have to change migration style to a sql creation script for foreign database.");
                }
                return;
            }
            
            if (iPath != -1) {
                DatabaseContext.Build().Database.EnsureCreated();

                var existingLogIds = DatabaseContext.AppDatabase.IISLogEvents.Select(s => s.Id).ToList();
                var newLogsToSave = EntityLogEvents.Where(w => !existingLogIds.Contains(w.Id)).ToList();

                DatabaseContext.AppDatabase.IISLogEvents.AddRange(newLogsToSave);
                DatabaseContext.AppDatabase.SaveChanges();
                DatabaseContext.AppDatabase.Dispose();

                Console.WriteLine("Changes Saved to SQLite DB! \nYou can use Query Syntax -SQL to query data\nYou can take the local.db file and upload into SQLite db browser or MS Access");
            }

            Console.WriteLine("Creating Database Schema for IIS Logs from S3 Zip Files");
         

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

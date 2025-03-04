using AWSS3Zip.Commands.Contracts;
using AWSS3Zip.Entity.Contracts;
using AWSS3Zip.Entity;

namespace AWSS3Zip.Commands
{
    public class JobFactory : IProcessFactory<IProcessJob>
    {
        public List<IProcessJob> Jobs { get; set; }
        public IDatabaseContext<DatabaseFactory, AppDatabase> DatabaseContext { get; set; }
        private ExtractJob ExtractJob { get; set; }
        private DatabaseJob DatabaseJob { get; set; }
        private WriteFileJob WriteFileJob { get; set; }


        public JobFactory(
            IDatabaseContext<DatabaseFactory, AppDatabase> _databaseContext, 
            ExtractJob _extractJob, 
            DatabaseJob _databaseJob, 
            WriteFileJob _writeFileJob
            ) {
            DatabaseContext = _databaseContext;
            ExtractJob = _extractJob;
            WriteFileJob = _writeFileJob;
        }

        IProcessFactory<IProcessJob> IProcessFactory<IProcessJob>.Build(string[] parameters)
        {
            Jobs = new List<IProcessJob>();
            if (parameters.Contains("-e") || parameters.Contains("--extract"))
                Jobs.Add(ExtractJob.BuildParameters(parameters));
            if (parameters.Contains("-db") || parameters.Contains("--database"))
                Jobs.Add(DatabaseJob.BuildParameters(parameters, ((ExtractJob)Jobs[0]).EntityLogEvents));
            else if (parameters.Contains("-w") || parameters.Contains("--write"))
                Jobs.Add(WriteFileJob.BuildParameters(parameters, ((ExtractJob)Jobs[0]).EntityLogEvents));

            else Console.WriteLine("Command parameter missing. Check Options!");
            return this;
        }

        public void Execute() => Jobs.ForEach(j => j.Execute());

    }
}

using AWSS3Zip.Commands.Contracts;


namespace AWSS3Zip.Commands
{
    public class JobFactory : IProcessFactory<IProcessJob>
    {
        public List<IProcessJob> Jobs { get; set; }
        private ExtractJob ExtractJob { get; set; }


        public JobFactory( ExtractJob _extractJob ) {
            ExtractJob = _extractJob;
        }

        IProcessFactory<IProcessJob> IProcessFactory<IProcessJob>.Build(string[] parameters)
        {
            Jobs = new List<IProcessJob>();
            if (parameters.Contains("-e") || parameters.Contains("--extract")) 
                Jobs.Add(ExtractJob.BuildParameters(parameters));


            else Console.WriteLine("Command parameter missing. Check Options!");
            return this;
        }

        public void Execute() => Jobs.ForEach(j => j.Execute());

    }
}

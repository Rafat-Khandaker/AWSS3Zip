using AWSS3Zip.Commands.Contracts;

namespace AWSS3Zip.Commands
{
    public class JobFactory : IProcessFactory<IProcessJob>
    {
        public IProcessJob Job { get; set; }
        IProcessFactory<IProcessJob> IProcessFactory<IProcessJob>.Build(string[] parameters)
        {
            if (parameters.Contains("-e") || parameters.Contains("--extract"))
                Job = new ExtractJob(parameters);

            else Console.WriteLine("Command parameter missing. Check Options!");
            return this;
        }

        public void Execute() => Job.Execute();

    }
}

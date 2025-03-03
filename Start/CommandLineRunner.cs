using AWSS3Zip.Commands;
using AWSS3Zip.Commands.Contracts;


namespace AWSS3Zip.Start
{
    public class CommandLineRunner
    {
        IProcessFactory<IProcessJob> JobFactory;

        public CommandLineRunner(IProcessFactory<IProcessJob> _JobFactory) {
            JobFactory = _JobFactory;
        }
        public void Run(string[] args) {
            if (args.Length == 0)
                Console.WriteLine(File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}Text\\Welcome.txt"));

            else if (JobFactory.Build(args) != null)
                JobFactory.Execute();
        }
    }
}

using AWSS3Zip.Commands.Contracts;
using AWSS3Zip.Entity.Models;

namespace AWSS3Zip.Commands
{
    public class WriteFileJob : IProcessJob
    {
        public string[] Parameters { get; set; }
        List<IISLogEvent> EntityLogEvents { get; set; }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public WriteFileJob BuildParameters(string[] parameters, List<IISLogEvent> entityLogEvents) {
            Parameters = parameters;
            EntityLogEvents = entityLogEvents;
            return this;
        }
    }
}

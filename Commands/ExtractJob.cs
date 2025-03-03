using AWSS3Zip.Commands.Contracts;
using AWSS3Zip.Models;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace AWSS3Zip.Commands
{
    public class ExtractJob : IProcessJob
    {
        string[] Parameters { get; set; }

        DirectoryNode DirectoryNode { get; set; }

        public ExtractJob(string[] parameters) {
            Parameters = parameters;
            DirectoryNode = new DirectoryNode();
        }

        public void Execute()
        {
            var iPath = Array.IndexOf(Parameters, "-e");

            if (iPath == -1) Array.IndexOf(Parameters, "--extract");
            if (iPath != -1 && (Parameters[iPath + 1].Contains("-") || Parameters[iPath + 1].Contains("--"))) {
                Console.WriteLine("Command not formatted Correctly, contains '-' or '--' followed by command variable");
                return;
            }

            var iOutput = Array.IndexOf(Parameters, "-o");
            if (iOutput == -1) Array.IndexOf(Parameters, "--output");
            if (iOutput != -1 && (Parameters[iOutput + 1].Contains("-") || Parameters[iOutput + 1].Contains("--"))){
                Console.WriteLine("Command not formatted Correctly, contains '-' or '--' followed by command variable");
                return;
            }

            if (iPath != -1)
                ExtractZipFiles(iPath, iOutput);

            else Console.WriteLine("no execution command found!");
        }

        private void ExtractZipFiles(int iPath, int iOutput = -1 ) {
            try
            {
                var directory = (iOutput != -1) ? Parameters[iOutput + 1] : $"{AppDomain.CurrentDomain.BaseDirectory}output";
                var command = $"{AppDomain.CurrentDomain.BaseDirectory}7-Zip\\7z.exe";
                var arguments = $@"x {Parameters[iPath + 1]} -o{directory}";
                Console.WriteLine("Please Wait!\n This Could Take a While! ....");
             //   InvokeProcess(command,arguments);
                Console.WriteLine($"\n Files Extracted: {directory}\n building directory structure...");
                DirectoryNode.BuildDirectoryStructure(directory); 

            }
            catch (Exception e)
            {
                Console.WriteLine($"zip path file not found error!\nDetails:\n\t{e.Message}");
            }        
        }

        private void InvokeProcess(string command, string arguments) {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = command,  
                Arguments = arguments,  
                RedirectStandardOutput = true,  
                UseShellExecute = false, 
                CreateNoWindow = true 
            };

            using (Process process = Process.Start(processStartInfo))
            {
                using (var reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                }
            }
        }


    }
}

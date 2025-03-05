using AWSS3Zip.Commands.Contracts;
using AWSS3Zip.Entity;
using AWSS3Zip.Entity.Contracts;
using AWSS3Zip.Entity.Models;
using AWSS3Zip.Models;
using AWSS3Zip.Service;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.Json;


namespace AWSS3Zip.Commands
{
    public class ExtractJob : IProcessJob
    {
        public IDatabaseContext<DatabaseFactory, AppDatabase> DatabaseContext { get; set; }

        public string[] Parameters { get; set; }
        public string OriginalDirectory { get; set; }
        public List<IISLogEvent> EntityLogEvents { get; set; }

        bool _isDatabaseTask = false;

        public ExtractJob(IDatabaseContext<DatabaseFactory, AppDatabase> _dbContext)
        {
            DatabaseContext = _dbContext;
        }
        public ExtractJob BuildParameters(string[] parameters)
        {
            Parameters = parameters;
            EntityLogEvents = new List<IISLogEvent>();
            return this;
        }

        public void Execute()
        {
            var iPath = Array.IndexOf(Parameters, "-e");
            _isDatabaseTask = Array.IndexOf(Parameters, "-db") != -1 || Array.IndexOf(Parameters, "--database") != -1;


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
                OriginalDirectory = (iOutput != -1) ? Parameters[iOutput + 1] : $"{AppDomain.CurrentDomain.BaseDirectory}output";
                var command = $"{AppDomain.CurrentDomain.BaseDirectory}7-Zip\\7z.exe";
                var arguments = $@"x {Parameters[iPath + 1]} -o{OriginalDirectory}";
                Console.WriteLine("Please Wait!\n This Could Take a While! ....");
              //  Processor.InvokeProcess(command,arguments);
                Console.WriteLine($"\n Files Extracted: {OriginalDirectory}\n Creating Database and building directory structure...");
                
                if(_isDatabaseTask)
                    DatabaseContext.Build().Database.EnsureCreated();

                BuildDirectoryStructure(OriginalDirectory);
                DatabaseContext.AppDatabase.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine($"zip path file not found error!\nDetails:\n\t{e.Message}");
            }        
        }

        private DirectoryNode BuildDirectoryStructure(string directory, DirectoryNode node = null, bool first = true)
        {
            if (node == null) node = new DirectoryNode();

            if (Directory.Exists(directory))
            {
                var directoryFolders = Directory.GetDirectories(directory);

                if (directoryFolders.Length > 0)
                    foreach (var folder in directoryFolders)
                    {
                        if (first)
                        {
                            node.Name = folder.Split("\\").Last().ToString();
                            node.Path = $"{folder}";
                            node.Type = FileType.Folder;

                            first = false;
                            node.Inside = new DirectoryNode() { Parent = node };

                        }
                        else
                        {
                            node.Next = new DirectoryNode(folder.Split("\\").Last().ToString(), $"{folder}") { Previous = node, Parent = node.Parent };
                            node = node.Next;
                            node.Inside = new DirectoryNode() { Parent = node };

                        }
                    }
                else
                    foreach (var file in Directory.GetFiles(directory))
                    {
                        if (first)
                        {
                            node.Name = file.Split("\\").Last().ToString(); ;
                            node.Path = $"{file}";
                            node.Type = node.Name.Contains('~')? FileType.Text : FileType.Zip;

                            first = false;
                        }
                        else
                        {
                            var name = file.Split("\\").Last().ToString();
                            node.Next = new DirectoryNode(name, $"{file}") { 
                                Previous = node, Parent = node.Parent, Type = name.Contains('~') ? FileType.Text : FileType.Zip 
                            };

                            node = node.Next;
                            node.Inside = new DirectoryNode() { Parent = node };
                        }
                    }

                first = true;

                return Unzip_File_Execute_SQL_Task_And_Recurse_Directory(directory, node.Name, first, node);

            }

            return node.Previous != null ?
                    BuildDirectoryStructure(node.Previous.Path, node.Previous, first) :
                        node.Parent != null ?
                            BuildDirectoryStructure(node.Parent.Path, node.Parent, first) :
                            node;
        }

        private DirectoryNode Unzip_File_Execute_SQL_Task_And_Recurse_Directory(string directory, string name, bool first, DirectoryNode node)
        {
            var previousDirectory = directory;
            directory = $"{directory}\\{name}";

            if (Directory.Exists(directory))
                return BuildDirectoryStructure(directory, node.Inside, first);
            else
            {
                if (node.Type.Equals(FileType.Zip) ) {
                    Console.WriteLine("Unzipping contents of inner zip files...May take a while.. ");
                    var command = $"{AppDomain.CurrentDomain.BaseDirectory}7-Zip\\7z.exe";
                    var arguments = $@"x {directory} -o{previousDirectory}";
                    Processor.InvokeProcess(command, arguments);

                 //   Console.WriteLine("Deleting previous zip file.. ");
                 //   File.Delete(directory);
                    node.Name += (node.Name.Contains('~'))? "" : "~";
                    node.Path += (node.Path.Contains('~')) ? "" : "~"; 
                    node.Type = FileType.Text;
                }

                if (node.Type.Equals(FileType.Text)) {
                    var json = File.ReadAllText(node.Path);

                    json = json.Insert(0, "[") + "]";
                    json = json.Replace("}{", "},{");

                    var logEventList = JsonSerializer.Deserialize<List<IISLog>>(json);

                    logEventList.ForEach(x => {
                        EntityLogEvents.AddRange(
                            x.logEvents.Select(s => new IISLogEvent()
                            {
                                Id = s.id,
                                MessageType = x.messageType,
                                Owner = x.owner,
                                LogGroup = x.logGroup,
                                LogStream = x.logStream,
                                SubscriptionFilters = JsonSerializer.Serialize(x.subscriptionFilters),
                                DateTime = DateTimeOffset.FromUnixTimeMilliseconds(s.timestamp).DateTime,
                                RequestMessage = s.message
                            }));
                    });

                    if (_isDatabaseTask)
                    {
                        DatabaseContext.AppDatabase.IISLogEvents.AddRange(EntityLogEvents);

                        DatabaseContext.AppDatabase.SaveChanges();

                        EntityLogEvents = new List<IISLogEvent>();
                        File.Delete(node.Path);

                        Console.WriteLine("Changes Saved to SQLite DB! \nYou can use Query Syntax -SQL to query data\nYou can take the local.db file and upload into SQLite db browser or MS Access");
                    }
                }

                node.Inside = null;
                var parts = previousDirectory.Split("\\");
                directory = string.Join("\\", parts, 0, parts.Length - 1);
                return (node.Previous != null) ?
                        Unzip_File_Execute_SQL_Task_And_Recurse_Directory(previousDirectory, node.Previous.Name, first, node.Previous) :
                           (!directory.Equals(OriginalDirectory) && node.Parent != null) ?
                                Unzip_File_Execute_SQL_Task_And_Recurse_Directory(directory, node.Parent.Previous.Name, first, node.Parent.Previous) :
                                    node;
            }
        }

        




    }
}

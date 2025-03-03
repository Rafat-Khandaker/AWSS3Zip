


namespace AWSS3Zip.Models
{
    public class DirectoryNode
    {
        public DirectoryNode Parent { get; set; }
        public DirectoryNode Inside { get; set; }
        public DirectoryNode Previous { get; set; }
        public DirectoryNode Next { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public FileType Type { get; set; }

        public DirectoryNode(string name = null, string path = null) {
            Name = name;
            Path = path;
        }

        public DirectoryNode BuildDirectoryStructure(string directory, DirectoryNode node = null, bool first = true)
        {
            if (Directory.Exists(directory))
            {
                var directoryFolders = Directory.GetDirectories(directory);

                if(directoryFolders.Length > 0)
                    foreach (var folder in directoryFolders) 
                    {
                        if (first)
                        {
                            Name = folder.Split("\\").Last().ToString() ;
                            Path = $"{directory}{folder}";
                            Type = FileType.Folder;
                        }
                        else
                        {
                            Next = new DirectoryNode(folder, $"{directory}{folder}") { Previous = this, Parent = Parent };
                        }
                    }
                else
                    foreach (var file in Directory.GetFiles(directory))
                    {
                        if (first)
                        {
                            Name = file;
                            Path = $"{directory}{file}";
                            Type = (Array.Exists([".zip", ".gzip", ".7z"], x => file.Contains(x))) ? FileType.Zip : FileType.Text;
                        }
                        else
                        {
                            Next = new DirectoryNode(file, $"{directory}{file}") { Previous = this, Parent = Parent };
                        }
                    }

                directory = $"{directory}\\{Name}";

                return BuildDirectoryStructure(directory, Inside, first);

            }

            return Previous != null ?
                    BuildDirectoryStructure(Previous.Path, Previous, first) :
                        Parent != null ?
                            BuildDirectoryStructure(Parent.Path, Parent, first) :
                            node;

        }

    }
}

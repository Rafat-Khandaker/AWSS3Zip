


using System.Diagnostics;


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

        public DirectoryNode(string name = null, string path = null)
        {
            Name = name;
            Path = path;
        }

        
    }
}

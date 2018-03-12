using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.Models
{
    public class DriveFile
    {
        public string FileID { get; set; }
        public string Name { get; set; }
        public long? Size { get; set; }
        public string Version { get; set; }
    }
}

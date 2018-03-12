using System.Collections.Generic;
using GoogleDrive.Models;

namespace MvcVanced.Models
{
    public class DriveModel
    {
        public List<DriveFile> NonRoot { get; set; }
        public List<DriveFile> NonRoot_Beta { get; set; }
        public List<DriveFile> Root { get; set; }
        public List<DriveFile> Root_Beta { get; set; }
        public List<DriveFile> Magisk { get; set; }
        public List<DriveFile> Magisk_Beta { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDrive.Models
{
    public class Change
    {
        public string FileID { get; set; }
        public CHANGE_TYPE Type { get; set; }
        public string Version { get; set; }
        public string Version_Before { get; set; }
    }

    public enum CHANGE_TYPE
    {
        ADD,
        DELETE,
        CHANGE
    }
}

﻿using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MvcVanced.Models
{
    public class APK
    {
        public int ID { get; set; }
        [Display(Name = "Theme/Title")]
        public string Title { get; set; }
        public string Version { get; set; }
        public string Architecture { get; set; }
        public string MinimumAPI { get; set; }
        public string DPI { get; set; }
        public long Size { get; set; }
        public APKTYPE Type { get; set; }
        public int Downloads { get; set; }
        public string FileID { get; set; }
        public bool Public { get; set; }
    }

    public class APKDBContext : DbContext
    {
        public APKDBContext() : base("name=DefaultConnection") {
        }

        public DbSet<APK> APKs { get; set; }
    }
}
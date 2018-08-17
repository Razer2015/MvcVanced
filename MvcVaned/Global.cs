using GoogleDrive;
using MvcVanced.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcVanced
{
    public static class Global
    {
        public static Client GoogleClient;

        public static IEnumerable<APK> APKCache;
        public static IEnumerable<APK> MicroGCache;
    }
}
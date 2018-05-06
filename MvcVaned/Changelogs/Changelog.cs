using MvcVanced.Changelogs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MvcVanced.Changelogs
{
    public class Changelog
    {
        public static string VersionPath { get; set; }
        private static List<Change> Version { get; set; }
        public static bool VersionNeedsUpdate { get; set; }

        public static string BuildPath { get; set; }
        private static List<Change> Build { get; set; }
        public static bool BuildNeedsUpdate { get; set; }

        public static string ThemePath { get; set; }
        private static List<Change> Theme { get; set; }
        public static bool ThemeNeedsUpdate { get; set; }

        public static List<Change> GetChangelogs(CHANGELOG_TYPE changelog_type) {
            try {
                switch (changelog_type) {
                    case CHANGELOG_TYPE.VERSION:
                        if (Version == null || VersionNeedsUpdate) {
                            Version = JsonConvert.DeserializeObject<List<Change>>(File.ReadAllText(Path.Combine(VersionPath, "changes.json")));
                            Version.Reverse();
                            VersionNeedsUpdate = false;
                        }
                        return (Version);
                    case CHANGELOG_TYPE.BUILD:
                        if (Build == null || BuildNeedsUpdate) {
                            Build = JsonConvert.DeserializeObject<List<Change>>(File.ReadAllText(Path.Combine(BuildPath, "changes.json")));
                            Build.Reverse();
                            BuildNeedsUpdate = false;
                        }
                        return (Build);
                    case CHANGELOG_TYPE.THEME:
                        if (Theme == null || ThemeNeedsUpdate) {
                            Theme = JsonConvert.DeserializeObject<List<Change>>(File.ReadAllText(Path.Combine(ThemePath, "changes.json")));
                            Theme.Reverse();
                            ThemeNeedsUpdate = false;
                        }
                        return (Theme);
                }
                return (null);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return (null);
            }
        }

        public static void GenerateExampleData() {
            Version = new List<Change> {
                new Change() { Title = "13.14.01", Changes = new string[] { "Build 1", "Build 2" } },
                new Change() { Title = "13.14.02", Changes = new string[] { "Build 3" } }
            };
            Directory.CreateDirectory(VersionPath);
            File.WriteAllText(Path.Combine(VersionPath, "changes.json"), JsonConvert.SerializeObject(Version));
        }
    }
}
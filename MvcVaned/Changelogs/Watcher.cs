using MvcVanced;
using System;
using System.IO;
using System.Security.Permissions;

public class Watcher
{
    CHANGELOG_TYPE ChangeLog_Type;

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public void Run(string path, CHANGELOG_TYPE changelog_type) {
        ChangeLog_Type = changelog_type;

        // Create a new FileSystemWatcher and set its properties.
        FileSystemWatcher watcher = new FileSystemWatcher {
            Path = path,
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName,
            // Only watch json files.
            Filter = "*.json"
        };

        // Add event handlers.
        watcher.Changed += new FileSystemEventHandler(OnChanged);
        watcher.Created += new FileSystemEventHandler(OnChanged);
        watcher.Deleted += new FileSystemEventHandler(OnChanged);
        watcher.Renamed += new RenamedEventHandler(OnRenamed);

        // Begin watching.
        watcher.EnableRaisingEvents = true;
    }

    // Define the event handlers.
    private void OnChanged(object source, FileSystemEventArgs e) {
        // Specify what is done when a file is changed, created, or deleted.
        //Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        UpdateNeeded(ChangeLog_Type);
    }

    private void OnRenamed(object source, RenamedEventArgs e) {
        // Specify what is done when a file is renamed.
        //Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
    }

    private void UpdateNeeded(CHANGELOG_TYPE changelog_type) {
        switch (changelog_type) {
            case CHANGELOG_TYPE.VERSION:
                MvcVanced.Changelogs.Changelog.VersionNeedsUpdate = true;
                break;
            case CHANGELOG_TYPE.BUILD:
                MvcVanced.Changelogs.Changelog.BuildNeedsUpdate = true;
                break;
            case CHANGELOG_TYPE.THEME:
                MvcVanced.Changelogs.Changelog.ThemeNeedsUpdate = true;
                break;
        }
    }
}
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using GoogleDrive.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleDrive
{
    public class Client
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        string[] Scopes = { DriveService.Scope.Drive };
        string ApplicationName = "Drive API .NET Vanced";
        Dictionary<string, string> Folders = new Dictionary<string, string>
        {
            { "NONROOT", "1utwigC-F1bUP6ngq8TN14fhm_gL56Vqd" },
            { "NONROOT_BETA", "1lCjeuF_iSV1DK-z8L9BuOSw07EpjPQo7" },
            { "ROOT", "1x6QBsfv4pAa8tlRAAhnuR8OFioPMGaCo" },
            { "ROOT_BETA", "1LmFXx4ZpDvDmNmCvQrxqjOfZVwTUvXv3" },
            { "MAGISK", "1yvhwIlb3Kg-nC-A7GvrZJe48QaYH1-XO" },
            { "MAGISK_BETA", "1DSjxl-2J6xYLPzSezdz5u_gq89mc7isD" },
        };

        public Dictionary<string, List<DriveFile>> FetchedFiles;

        string BasePath;
        DriveService Service;

        public Client(string app_data) {
            BasePath = app_data;
            Connect();
        }

        public void Connect() {
            UserCredential credential;

            using (var stream =
                new FileStream(Path.Combine(BasePath, "client_secret.json"), FileMode.Open, FileAccess.Read)) {
                string credPath = BasePath;
                credPath = Path.Combine(credPath, ".credentials/drive-dotnet-vanced.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Debug.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            Service = new DriveService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }

        #region Changes
        private string GetStartPageToken(bool fresh = false) {
            if (!fresh && System.IO.File.Exists(Path.Combine(BasePath, "start_page_token.json"))) {
                return (System.IO.File.ReadAllText(Path.Combine(BasePath, "start_page_token.json")));
            }
            else {
                var response = Service.Changes.GetStartPageToken().Execute();
                Debug.WriteLine("Start token: " + response.StartPageTokenValue);
                SetStartPageToken(response.StartPageTokenValue);
                return (response.StartPageTokenValue);
            }
        }

        private void SetStartPageToken(string token) {
            System.IO.File.WriteAllText(Path.Combine(BasePath, "start_page_token.json"), token);
        }

        public Dictionary<string, List<Models.Change>> GetChanges(Dictionary<string, List<DriveFile>> before, 
                                                                        Dictionary<string, List<DriveFile>> after) {

            var changes = new Dictionary<string, List<Models.Change>> {
                { "NONROOT", new List<Models.Change>() },
                { "NONROOT_BETA", new List<Models.Change>() },
                { "ROOT", new List<Models.Change>() },
                { "ROOT_BETA", new List<Models.Change>() },
                { "MAGISK", new List<Models.Change>() },
                { "MAGISK_BETA", new List<Models.Change>() },
            };

            foreach (var kvp in before) {
                if (!after.ContainsKey(kvp.Key))
                    continue;

                var newKVP = after[kvp.Key];
                foreach (var item in kvp.Value) {
                    if (!newKVP.Any(x => x.FileID.Equals(item.FileID))) { // Deleted
                        changes[kvp.Key].Add(new Models.Change {
                            FileID = item.FileID,
                            Type = CHANGE_TYPE.DELETE,
                        });
                    }
                    else if (!newKVP.Any(x => x.FileID.Equals(item.FileID) && x.Version.Equals(item.Version))) { // Version changed
                        var newFile = newKVP.First(x => x.FileID.Equals(item.FileID));
                        changes[kvp.Key].Add(new Models.Change {
                            FileID = item.FileID,
                            Type = CHANGE_TYPE.CHANGE,
                            Version = newFile.Version,
                            Version_Before = item.Version
                        });
                    }
                }

                var Ids = new HashSet<string>(newKVP.Select(x => x.FileID));
                var addedItems = kvp.Value.Where(p => !Ids.Contains(p.FileID)).ToArray();
                if (addedItems != null && addedItems.Length > 0) { // Added
                    foreach (var addedItem in addedItems) {
                        changes[kvp.Key].Add(new Models.Change {
                            FileID = addedItem.FileID,
                            Type = CHANGE_TYPE.ADD
                        });
                    }
                }
            }

            return (changes);
        }

        public List<string> GetChanges() {
            // Begin with our last saved start token for this user or the
            // current token from GetStartPageToken()
            List<string> deletedIDs = new List<string>();
            string pageToken = GetStartPageToken();
            while (pageToken != null) {
                var request = Service.Changes.List(pageToken);
                request.Spaces = "drive";
                var changes = request.Execute();
                foreach (var change in changes.Changes) {
                    // Process change
                    // If file is deleted
                    if (change.Removed ?? false) {
                        deletedIDs.Add(change.FileId);
                    }

                    // Something changed in the tracked folders
                    if (Folders.Any(x => x.Value.Equals(change.FileId))) {
                        var beforeFetch = new Dictionary<string, List<DriveFile>>(FetchedFiles);
                        var fetchedFiles = RefreshFolder(change.FileId);
                        foreach (var kvp in beforeFetch) {
                            if (!fetchedFiles.ContainsKey(kvp.Key)) {
                                continue;
                            }

                            var newKVP = fetchedFiles[kvp.Key];
                            foreach (var item in kvp.Value) {
                                if (!newKVP.Any(x => x.FileID.Equals(item.FileID) && x.Version.Equals(item.Version))) {
                                    deletedIDs.Add(item.FileID);
                                }
                            }
                        }
                    }

                    // If file is not deleted
                    if (change.File.MimeType.Equals("application/vnd.android.package-archive") || change.File.MimeType.Equals("application/x-zip-compressed")) {
                        FetchFile(change.FileId);
                    }
                }
                if (changes.NewStartPageToken != null) {
                    // Last page, save this token for the next polling interval
                    SetStartPageToken(changes.NewStartPageToken);
                }
                pageToken = changes.NextPageToken;
            }

            return (deletedIDs);
        }
        #endregion

        #region Get Files
        public Dictionary<string, List<DriveFile>> FetchAllFiles() {
            if (Service == null) {
                throw new Exception("Error: DriveService is null. Have you forgot to connect?");
            }

            var output = new Dictionary<string, List<DriveFile>>();
            foreach (var category in Folders) {
                // Define parameters of request.
                FilesResource.ListRequest folderRequest = Service.Files.List();
                folderRequest.Fields = "nextPageToken, files(id, name)";
                folderRequest.Q = $"mimeType='application/vnd.google-apps.folder' and trashed=false and '{category.Value}' in parents";

                // List folders
                List<DriveFile> driveFiles = new List<DriveFile>();
                var folders = folderRequest.Execute().Files;
                if (folders != null && folders.Count > 0) {
                    foreach (var folder in folders) {
                        // Define parameters of request.
                        var fileRequest = Service.Files.List();
                        fileRequest.Fields = "nextPageToken, files(id, name, size, shared, parents, mimeType, md5Checksum)";
                        fileRequest.Q = $"mimeType!='application/vnd.google-apps.folder' and trashed=false and '{folder.Id}' in parents";

                        // List files
                        var files = fileRequest.Execute().Files;
                        if (files != null && files.Count > 0) {
                            foreach (var file in files) {
                                driveFiles.Add(new DriveFile {
                                    FileID = file.Id,
                                    Name = file.Name,
                                    Size = file.Size,
                                    Version = folder.Name,
                                    MD5Checksum = file.Md5Checksum
                                });
                            }
                        }
                    }
                }

                output.Add(category.Key, driveFiles);
            }

            // Refresh the changes token
            GetStartPageToken(fresh: true);

            return (output);
        }

        public Dictionary<string, DriveFile> FetchFile(string fileID) {
            if (Service == null) {
                throw new Exception("Error: DriveService is null. Have you forgot to connect?");
            }

            // Define parameters of request.
            var fileRequest = Service.Files.List();
            fileRequest.Fields = "nextPageToken, files(id, name, size, shared, parents, mimeType)";
            fileRequest.Q = $"mimeType!='application/vnd.google-apps.folder' and trashed=false and '{fileID}' in parents";

            // List files
            var files = fileRequest.Execute().Files;
            files = files.Where(x => x.Id.Equals(fileID)).ToList();
            if (files != null && files.Count > 0) {
                // Define parameters of request.
                var folderRequest = Service.Files.List();
                folderRequest.Fields = "nextPageToken, files(id, name, parents)";
                folderRequest.Q = $"mimeType='application/vnd.google-apps.folder' and trashed=false and '{fileID}' in parents";
                var folders = fileRequest.Execute().Files;

                if (folders != null && folders.Count > 0) {
                    var file = files[0];
                    var folder = folders[0];

                    if (Folders.Any(x => x.Value.Equals(folder.Parents[0]))) {
                        // Add the change to files list
                        FetchedFiles[Folders.FirstOrDefault(x => x.Value.Equals(folder.Parents[0])).Key].Add(new DriveFile {
                            FileID = file.Id,
                            Name = file.Name,
                            Size = file.Size,
                            Version = folder.Name
                        });

                        return (new Dictionary<string, DriveFile>() {
                            {
                                Folders.FirstOrDefault(x => x.Value.Equals(folder.Parents[0])).Key,
                                new DriveFile {
                                    FileID = file.Id,
                                    Name = file.Name,
                                    Size = file.Size,
                                    Version = folder.Name
                                }
                            }
                        });
                    }
                }
            }

            return (null);
        }

        public Dictionary<string, List<DriveFile>> RefreshFolder(string folderId) {
            if (Service == null) {
                throw new Exception("Error: DriveService is null. Have you forgot to connect?");
            }

            // Define parameters of request.
            FilesResource.ListRequest folderRequest = Service.Files.List();
            folderRequest.Fields = "nextPageToken, files(id, name)";
            folderRequest.Q = $"mimeType='application/vnd.google-apps.folder' and trashed=false and '{folderId}' in parents";

            // List folders
            List<DriveFile> driveFiles = new List<DriveFile>();
            var folders = folderRequest.Execute().Files;
            if (folders != null && folders.Count > 0) {
                foreach (var folder in folders) {
                    // Define parameters of request.
                    var fileRequest = Service.Files.List();
                    fileRequest.Fields = "nextPageToken, files(id, name, size, shared, parents, mimeType)";
                    fileRequest.Q = $"mimeType!='application/vnd.google-apps.folder' and trashed=false and '{folder.Id}' in parents";

                    // List files
                    var files = fileRequest.Execute().Files;
                    if (files != null && files.Count > 0) {
                        foreach (var file in files) {
                            driveFiles.Add(new DriveFile {
                                FileID = file.Id,
                                Name = file.Name,
                                Size = file.Size,
                                Version = folder.Name
                            });
                        }
                    }
                }
            }

            var key = Folders.First(x => x.Value.Equals(folderId)).Key ?? "";
            if (FetchedFiles.ContainsKey(key)) {
                FetchedFiles[key] = driveFiles;
            }

            return (FetchedFiles);
        }

        /// <summary>
        /// Retrieve a list of File resources.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <returns>List of File resources.</returns>
        public static List<Google.Apis.Drive.v3.Data.File> RetrieveAllFiles(DriveService service) {
            List<Google.Apis.Drive.v3.Data.File> result = new List<Google.Apis.Drive.v3.Data.File>();
            FilesResource.ListRequest request = service.Files.List();

            do {
                try {
                    FileList files = request.Execute();

                    result.AddRange(files.Files);
                    request.PageToken = files.NextPageToken;
                }
                catch (Exception e) {
                    Console.WriteLine("An error occurred: " + e.Message);
                    request.PageToken = null;
                }
            } while (!String.IsNullOrEmpty(request.PageToken));
            return result;
        }
        #endregion
    }
}

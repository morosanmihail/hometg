using Microsoft.Data.Sqlite;
using System.Security.Cryptography;

namespace HomeTG.API.Utils
{
    public static class DBFiles
    {
        public async static Task<bool> DownloadPrintingsDBIfNotExists(string DBURL, string Filename)
        {
            bool download = false;

            string flag = Environment.GetEnvironmentVariable("SKIP_PRINTINGS_DB_DOWNLOAD");
            if (string.Equals(flag, "TRUE", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("SKIP_PRINTINGS_DB_DOWNLOAD set to TRUE. Skipping DB download.");
                return false;
            }

            string remoteHash = "";

            string? LocalFolder = new FileInfo(Filename).DirectoryName;
            if (LocalFolder != null) Directory.CreateDirectory(LocalFolder);

            await SaveUrlContent(DBURL + ".sha256", Filename + ".sha256");
            remoteHash = File.ReadAllText(Filename + ".sha256");

            string localHash = "";
            if (!File.Exists(Filename))
            {
                download = true;
            }
            else
            {
                localHash = GetSHA256HashFromFile(Filename);
                if (remoteHash != localHash)
                {
                    download = true;
                }
            }

            if (download)
            {
                Console.WriteLine("File is NOT up to date. Downloading to " + Filename + "...");
                await SaveUrlContent(DBURL, Filename);
                Console.WriteLine("Downloaded.");
            }

            return download;
        }

        static string GetSHA256HashFromFile(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                using (SHA256 sha = SHA256.Create())
                {
                    byte[] hash = sha.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }

        static async Task SaveUrlContent(string url, string Filename)
        {
            using (var client = new HttpClient()) {
                client.Timeout = Timeout.InfiniteTimeSpan;
                using (var result = await client.GetAsync(url))
                {
                    var content = result.IsSuccessStatusCode ? await result.Content.ReadAsByteArrayAsync() : null;

                    if (content != null)
                    {
                        await File.WriteAllBytesAsync(Filename, content);
                    }
                }
            }
        }
    }
}

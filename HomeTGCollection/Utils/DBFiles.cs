using Microsoft.Data.Sqlite;
using System;
using System.Net;
using System.Security.Cryptography;

namespace HomeTGCollection.Utils
{
    public static class DBFiles
    {
        public static void CreateDBIfNotExists(string Filename, string sql)
        {
            if (!System.IO.File.Exists(Filename))
            {
                using (var sqlite = new SqliteConnection(@"Data Source=" + Filename))
                {
                    sqlite.Open();
                    SqliteCommand command = new SqliteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }
            }
        }

        public async static Task DownloadPrintingsDBIfNotExists(string DBURL, string LocalFolder, string Filename)
        {
            bool download = false;

            string remoteHash = "";

            await SaveUrlContent(DBURL + ".sha256", LocalFolder, Filename + ".sha256");
            remoteHash = File.ReadAllText(LocalFolder + Filename + ".sha256");

            string localHash = "";
            if (!File.Exists(LocalFolder + Filename))
            {
                download = true;
            }
            else
            {
                localHash = GetSHA256HashFromFile(LocalFolder + Filename);
                if (remoteHash != localHash)
                {
                    download = true;
                }
            }

            if (download)
            {
                Console.WriteLine("File is NOT up to date. Downloading...");
                await SaveUrlContent(DBURL, LocalFolder, Filename);
                Console.WriteLine("Downloaded.");
            }
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

        static async Task SaveUrlContent(string url, string LocalFolder, string Filename)
        {
            using (var client = new HttpClient())
            using (var result = await client.GetAsync(url))
            {
                var content = result.IsSuccessStatusCode ? await result.Content.ReadAsByteArrayAsync() : null;

                if (content != null)
                {
                    if (!Directory.Exists(LocalFolder)) Directory.CreateDirectory(LocalFolder);
                    await File.WriteAllBytesAsync(LocalFolder + Filename, content);
                }
            }
        }
    }
}

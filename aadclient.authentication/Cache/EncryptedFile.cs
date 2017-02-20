using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace aadclient.authentication.Cache
{
    public static class EncryptedFile
    {
        public static Lazy<string> CachePath = new Lazy<string>(() =>
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".aad");
        });

        public static string GetEncryptedFilePath(string fileName)
        {
            Directory.CreateDirectory(EncryptedFile.CachePath.Value);
            return Path.Combine(EncryptedFile.CachePath.Value, fileName);
        }

        public static T ReadEncryptedFile<T>(string fileName)
        {
            var filePath = EncryptedFile.GetEncryptedFilePath(fileName);
            var bytes = File.ReadAllBytes(filePath);
            bytes = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
            var json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static void WriteEncryptedFile<T>(string fileName, T content)
        {
            var filePath = EncryptedFile.GetEncryptedFilePath(fileName);
            var json = JsonConvert.SerializeObject(content);
            var bytes = Encoding.UTF8.GetBytes(json);
            bytes = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(filePath, bytes);
        }

        public static void DeleteEncryptedFile(string fileName)
        {
            var filePath = EncryptedFile.GetEncryptedFilePath(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helper
{
    public static class DocumentSettings
    {
        private const string defaultProfilePic = "defaultPP.png";

        public static string DefaultProfilePic
        {
            get { return defaultProfilePic; }
        }

        public static bool IsDefaultPictureOrNull(string profilePic)
        {
            return profilePic == DefaultProfilePic || string.IsNullOrEmpty(profilePic);
        }

        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1- Get The Folder Path (Location)
            //string FolderPath = @"D:\FullStack Deploma (Route)\Back-end (Asp.net)\05- ASP.Net MVC\Session 03\Demo MVC Solution\Demo.PL\wwwroot\files\images\";
            //string FolderPath = Directory.GetCurrentDirectory() + @"\wwwroot\files\" + folderName;
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

            string FileName;
            if (file is not null)
            {
                // 2- Get File Name Must be Unique.
                FileName = $"{Guid.NewGuid()}{file.FileName}";
                // 3- Get File Path: FolderPath + File NAme
                string FilePath = Path.Combine(FolderPath, FileName);

                // 4- Save File As Stream. => Data per Time
                using var fileStream = new FileStream(FilePath, FileMode.Create); // saved As File Stream

                file.CopyTo(fileStream);
                return FileName;
            }
            FileName = DefaultProfilePic;

            return FileName;
        }



        public static void DeleteFile(string fileName, string folderName)
        {
            if (IsDefaultPictureOrNull(fileName))
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }
    }
}

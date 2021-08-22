using Microsoft.AspNetCore.Hosting;
using QuanLyKho.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.Common
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _productContentFolder;
        private const string IMAGE_CONTENT_FOLDER_NAME = SystemConstants.ImageFolder;

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _productContentFolder = Path.Combine(webHostEnvironment.WebRootPath, IMAGE_CONTENT_FOLDER_NAME);
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{IMAGE_CONTENT_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_productContentFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            string[] arrListStr = fileName.Split('/');
            var file = arrListStr[2];

            var filePath = Path.Combine(_productContentFolder, file);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
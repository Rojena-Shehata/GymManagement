using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[]  _allowedExtesions=[".png",".jpeg",".jpg"] ;

        private readonly int _maxSize = 5 * 1024 * 1024;
        private readonly IWebHostEnvironment _webHost;

        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public string? Upload(string foldername, IFormFile file)
        {
            try
            {
                if(string.IsNullOrEmpty(foldername)||file is null || file.Length==0)
                    return null;

                if(file.Length>_maxSize) 
                    return null;

                var extension = Path.GetExtension(file.FileName);

                if(!_allowedExtesions.Contains(extension))
                    return null;

                var folderPath=Path.Combine(_webHost.WebRootPath, "images",foldername);

                if(!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName=$"{Guid .NewGuid()}{extension}";

                var filePath=Path.Combine(folderPath,fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

                return fileName;



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to upload file to folder {foldername} : {ex.Message}");
                return null;
            }
                       
        }


        public bool Delete(string fileName, string foldername)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(foldername))
                    return false;

                var fullPath = Path.Combine(_webHost.WebRootPath, "images", foldername, fileName);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete with Name {fileName} :{ex.Message} ");
                return false; ;
            }
        }
    }
}

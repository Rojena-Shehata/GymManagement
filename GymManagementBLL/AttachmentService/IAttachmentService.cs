using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace GymManagementBLL.AttachmentService
{
    public interface IAttachmentService
    {
        string? Upload(string foldername, IFormFile file);
        bool Delete(string fileName, string foldername);
    }
}

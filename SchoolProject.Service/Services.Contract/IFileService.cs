using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Services.Contract
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile file,string folder);
        //Task<string> DeleteImageAsync(string imagename,string folder);
    }
}

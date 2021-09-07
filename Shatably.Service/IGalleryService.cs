using Shatably.Data.Models;
using Shatably.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shatably.Service
{
    public interface IGalleryService
    {
        Task<Response<string>> AddGallery(Gallery gallery);
        Task<Response<string>> DeleteGallery(int galleryId, int companyId);
        Task<Response<string>> DeleteAllGallery(int companyId);
    }
}

using Shatably.BLL;
using Shatably.Data.Models;
using Shatably.Service.Model;
using System;
using System.Threading.Tasks;

namespace Shatably.Service
{
    public class GalleryService : IGalleryService
    {
        private readonly IGalleryReposatory _galleryReposatory;

        public GalleryService(IGalleryReposatory galleryReposatory)
        {
            _galleryReposatory = galleryReposatory;
        }

        public async Task<Response<string>> AddGallery(Gallery gallery)
        {
            try
            {
                await _galleryReposatory.AddGallery(Gallery: gallery);
                return new Response<string>(true, "Added successfully.");
            }
            catch
            {
                return new Response<string>(false, "Failed to add.");
            }
        }

        public async Task<Response<string>> DeleteAllGallery(int companyId)
        {
            try
            {
                await _galleryReposatory.DeleteAllGallery(CompanyId: companyId);
                return new Response<string>(true, "Deleted successfully.");
            }
            catch
            {
                return new Response<string>(false, "Failed to delete.");
            }
        }

        public async Task<Response<string>> DeleteGallery(int galleryId, int companyId)
        {
            try
            {
                await _galleryReposatory.DeleteGallery(GalleryId: galleryId, CompanyId: companyId);
                return new Response<string>(true, "Deleted successfully.");
            }
            catch
            {
                return new Response<string>(false, "Failed to delete.");
            }
        }
    }
}
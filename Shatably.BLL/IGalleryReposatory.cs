using Shatably.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shatably.BLL
{
    public interface IGalleryReposatory
    {
        //Task<Gallery> GetGallery(int GalleryId, int CompanyId);

        Task<Gallery> GetGallerybyID(int GalleryId);

        Task<List<Gallery>> GetAllGalleries(int CompanyId);

        Task AddGallery(Gallery Gallery);

        //Task<Gallery> UpdateGallery(Gallery Gallery);

        Task DeleteGallery(int GalleryId, int CompanyId);
        Task DeleteAllGallery(int CompanyId);
        int GetNewGalleryID(int CompanyID);
    }
}
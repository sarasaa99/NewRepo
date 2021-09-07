using Microsoft.EntityFrameworkCore;
using Shatably.Data.Context;
using Shatably.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shatably.BLL
{
    public class GalleryRepository : IGalleryReposatory
    {
        private readonly ShatblyDbContext _dbContext;

        public GalleryRepository(ShatblyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddGallery(Gallery Gallery)
        {
            Gallery.GalleryID = GetNewGalleryID(Gallery.CompanyId);
            await _dbContext.AddAsync(Gallery);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAllGallery(int CompanyId)
        {
            var deleteList =  _dbContext.Galleries.Where(photo => photo.CompanyId == CompanyId).ToList();
            _dbContext.Galleries.RemoveRange(deleteList);
            var res = await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteGallery(int GalleryId, int CompanyId)
        {
            var photo = await _dbContext.Galleries.Where(g => g.CompanyId == CompanyId && g.GalleryID == GalleryId).FirstOrDefaultAsync();
            if (photo != null)
            {
                _dbContext.Remove(photo);
                _dbContext.SaveChanges();
            }
        }

        public async Task<List<Gallery>> GetAllGalleries(int CompanyId)
        {
            var companyGallery = await _dbContext.Galleries.Where(gallery => gallery.CompanyId == CompanyId).ToListAsync();
            return companyGallery;
        }


        public Task<Gallery> GetGallerybyID(int GalleryId)
        {
            return _dbContext.Galleries.Where(g => g.GalleryID == GalleryId).FirstOrDefaultAsync();
        }

        public int GetNewGalleryID(int CompanyID)
        {
            int galleryID = _dbContext.Galleries.Where(g => g.CompanyId == CompanyID).Select(g => g.GalleryID).OrderByDescending(g => g).FirstOrDefault();
            return (galleryID + 1);
        }

       
    }
}
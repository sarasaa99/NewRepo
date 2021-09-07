using Microsoft.EntityFrameworkCore;
using Shatably.Data.Context;
using Shatably.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shatably.BLL
{
    public class ReviewsRepository : IReviewsReposatory
    {
        private readonly ShatblyDbContext _dbContext;

        public ReviewsRepository(ShatblyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddReviews(Reviews Reviews)
        {
            bool existReview = _dbContext.Reviewss.Any(rev => rev.ReviewsId == Reviews.ReviewsId);
            if (!existReview)
            {
                await _dbContext.Reviewss.AddAsync(Reviews);
                await _dbContext.SaveChangesAsync();
            }
            //Optional
            //else
            //{
            //    await UpdateReviews(Reviews);
            //}
        }

        public void DeleteReviews(int ReviewsId)
        {
            Reviews existingReview = _dbContext.Reviewss.Where(rev => rev.ReviewsId == ReviewsId).FirstOrDefault();
            if (existingReview != null)
            {
                _dbContext.Remove(existingReview);
                _dbContext.SaveChanges();
            }
        }

        public async Task<List<Reviews>> GetAllReviews(int CompanyId)
        {
            var reviews = await _dbContext.Reviewss.Where(cid => cid.CompanyId == CompanyId).ToListAsync();
            return reviews;
        }

        // not used
        public Task<Reviews> GetAllReviewsByRate()
        {
            throw new NotImplementedException();
        }

        public async Task<Reviews> GetReviewsById(int ReviewsId)
        {
            Reviews getReview = await _dbContext.Reviewss.Where(rev => rev.ReviewsId == ReviewsId).FirstOrDefaultAsync();
            return getReview;
        }

        // not used
        public Task<Reviews> GetReviewsByUserId(string ReviewsUserId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateReviews(Reviews Reviews)
        {
            Reviews oldReview = await _dbContext.Reviewss.Where(rev => rev.ReviewsId == Reviews.ReviewsId).FirstOrDefaultAsync();
            if (oldReview != null)
            {
                oldReview.Massege = Reviews.Massege;
                oldReview.Rate = Reviews.Rate;
                _dbContext.Reviewss.Attach(oldReview);
                _dbContext.Entry(oldReview).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            
        }
    }
}
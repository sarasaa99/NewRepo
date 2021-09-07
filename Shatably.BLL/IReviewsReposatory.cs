using Shatably.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shatably.BLL
{
    public interface IReviewsReposatory
    {
        Task<List<Reviews>> GetAllReviews(int CompanyId);

        Task<Reviews> GetAllReviewsByRate();

        Task<Reviews> GetReviewsById(int ReviewsId);

        Task<Reviews> GetReviewsByUserId(string ReviewsUserId);

        Task AddReviews(Reviews Reviews);

        Task UpdateReviews(Reviews Reviews);

        void DeleteReviews(int ReviewsId);
    }
}
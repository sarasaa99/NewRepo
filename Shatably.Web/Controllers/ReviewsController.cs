using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shatably.BLL;
using Shatably.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shatably.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewsReposatory _reviewsReposatory;

        public ReviewsController(IReviewsReposatory companyReposatory)
        {
            _reviewsReposatory = companyReposatory;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reviews>>> getCompanyReviews(int companyID)
        {
            try
            {
                var companyReviews = await _reviewsReposatory.GetAllReviews(companyID);
                return Ok(companyReviews);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get reviews.");
            }
        }

        [AllowAnonymous]
        [HttpGet("getCompanybyID")]
        public async Task<ActionResult<Reviews>> getReview(int reviewID)
        {
            try
            {
                var companyReview = await _reviewsReposatory.GetReviewsById(reviewID);
                return Ok(companyReview);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get review.");
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("AddReview")]
        public async Task<ActionResult> addReview(Reviews review)
        {
            try
            {
                await _reviewsReposatory.AddReviews(review);
                return StatusCode(StatusCodes.Status201Created, "Failed to add review.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add review.");
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("Update")]
        public async Task<ActionResult> updateReview(Reviews review)
        {
            try
            {
                await _reviewsReposatory.UpdateReviews(review);
                return Ok("Review updated.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update review.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public  ActionResult removeReview(int reviewID)
        {
            try
            {
                _reviewsReposatory.DeleteReviews(reviewID);
                return Ok("Review deleted.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete review.");
            }
        }
    }
}
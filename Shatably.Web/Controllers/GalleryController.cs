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
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryReposatory _galleryRepository;

        public GalleryController(IGalleryReposatory galleryRepository)
        {
            _galleryRepository = galleryRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gallery>>> getGallery(int companyID)
        {
            try
            {
                var companyGallery = await _galleryRepository.GetAllGalleries(companyID);
                return Ok(companyGallery);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
            }
        }

        [Authorize(Roles = "Admin, Company")]
        [HttpPost]
        public async Task<ActionResult> addPhoto(Gallery gallery)
        {
            try
            {
                await _galleryRepository.AddGallery(gallery);
                return StatusCode(StatusCodes.Status201Created, "Photo added successfully.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Could not add photo.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult deletePhoto(int galleryID, int companyID)
        {
            try
            {
                _galleryRepository.DeleteGallery(galleryID, companyID);
                return StatusCode(StatusCodes.Status200OK, "Photo removed successfully.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to remove photo.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAllPhotos(int companyID)
        {
            try
            {
                await _galleryRepository.DeleteAllGallery(companyID);
                return StatusCode(StatusCodes.Status200OK, "Photos removed successfully.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to remove photos.");
            }
        }
    }
}
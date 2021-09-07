using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shatably.Data.Models;
using Shatably.Service;
using Shatably.Service.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shatably.Web.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [Authorize(Roles = "Admin, Company")]
        [HttpPost("newCompany")]
        public async Task<ActionResult> addCompany(Company newCompany)
        {
            Response<string> response = await _companyService.AddCompany(newCompany);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("removeCompany")]
        public async Task<ActionResult> removeCompany(int companyID)
        {
            Response<string> response = await _companyService.DeleteCompany(companyID);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("fetchCompanies")]
        public async Task<ActionResult<List<Company>>> getCompanies()
        {
            Response<List<Company>> response = await _companyService.GetCompanies();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("fetchCompany")]
        public async Task<ActionResult<Response<CompanyViewModel>>> getCompanyInfo(string userId)
        {
            Response<CompanyViewModel> response = await _companyService.GetCompany(userId);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }
        }

        [Authorize(Roles = "Admin, Company")]
        [HttpPost("updateCompany")]
        public async Task<ActionResult> updateCompany(Company company)
        {
            Response<string> response = await _companyService.UpdateCompany(company);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Message);
            }
        }
    }
}
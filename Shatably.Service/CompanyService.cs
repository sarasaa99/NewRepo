using Shatably.BLL;
using Shatably.Data.Models;
using Shatably.Service.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shatably.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyReposatory _companyReposatory;
        private readonly IGalleryReposatory _galleryReposatory;
        private readonly IReviewsReposatory _reviewsReposatory;

        public CompanyService(ICompanyReposatory companyReposatory, IGalleryReposatory galleryReposatory, IReviewsReposatory reviewsReposatory)
        {
            _companyReposatory = companyReposatory;
            _galleryReposatory = galleryReposatory;
            _reviewsReposatory = reviewsReposatory;
        }

        public async Task<Response<CompanyViewModel>> GetCompany(string userId)
        {
            CompanyViewModel companyView = new CompanyViewModel();

            companyView.company = await _companyReposatory.GetCompanyByUser(UserId: userId);
            companyView.galleries = await _galleryReposatory.GetAllGalleries(CompanyId: companyView.company.CompanyId);
            companyView.reviews = await _reviewsReposatory.GetAllReviews(CompanyId: companyView.company.CompanyId);

            return new Response<CompanyViewModel>(companyView);
        }

        public async Task<Response<List<Company>>> GetCompanies()
        {
            List<Company> companies = new List<Company>();
            companies = await _companyReposatory.GetAllCompanies();
            return new Response<List<Company>>(companies);
        }

        public async Task<Response<string>> AddCompany(Company company)
        {
            try
            {
                await _companyReposatory.AddCompany(company);
                return new Response<string>(true, "Created successfully.");
            }
            catch
            {
                return new Response<string>(false, "Failed to create.");
            }
        }

        public async Task<Response<string>> UpdateCompany(Company company)
        {
            try
            {
                await _companyReposatory.UpdateCompany(company);
                return new Response<string>(true, "Update successfully.");
            }
            catch 
            {

                return new Response<string>(false, "Failed to update.");
            }
           
        }

        public async Task<Response<string>> DeleteCompany(int companyId)
        {
            try
            {
                await _companyReposatory.DeleteCompany(companyId);
                return new Response<string>(true, "Deleted successfully.");

            }
            catch 
            {
                return new Response<string>(true, "Failed to delete.");
            }

        }

        public async Task<Response<Company>> GetCompanyByName(string companyName)
        {
            try
            {
                Company company = await _companyReposatory.GetCompanyByName(CompanyName: companyName.Trim());
                return new Response<Company>(company);
            }
            catch (System.Exception)
            {
                return new Response<Company>(false, "Could not find company.");
            }
            
            
        }

        public async Task<Response<Company>> GetCompanyById(int companyId)
        {
            try
            {
                Company company = await _companyReposatory.GetCompanyById(CompanyId: companyId);
                return new Response<Company>(company);
            }
            catch (System.Exception)
            {

                return new Response<Company>(false,"Could not find company.");
            }
        }
    }
}
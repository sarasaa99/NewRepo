using Shatably.Data.Models;
using Shatably.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shatably.Service
{
    public interface ICompanyService
    {
        Task<Response<CompanyViewModel>> GetCompany(string uyserId);
        Task<Response<List<Company>>> GetCompanies();
        Task<Response<string>> AddCompany(Company company);
        Task<Response<string>> UpdateCompany(Company company);
        Task<Response<string>> DeleteCompany(int companyId);
        Task<Response<Company>> GetCompanyByName(string companyName);
        Task<Response<Company>> GetCompanyById(int companyId);
        
    }
}
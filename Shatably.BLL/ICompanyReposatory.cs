using Shatably.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shatably.BLL
{
    public interface ICompanyReposatory
    {
        Task<List<Company>> GetAllCompanies();
        Task<Company> GetCompany(int CompanyId, string UserId);
        Task<Company> GetCompanyByUser(string UserId);
        Task<Company> GetCompanyById(int CompanyId);
        Task AddCompany(Company Company);
        Task UpdateCompany(Company Company);
        Task DeleteCompany(int CompanyId);
        Task<Company> GetCompanyByName(string CompanyName);
    }
}

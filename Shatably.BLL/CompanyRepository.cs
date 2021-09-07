using Microsoft.EntityFrameworkCore;
using Shatably.Data.Context;
using Shatably.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shatably.BLL
{
    public class CompanyRepository : ICompanyReposatory
    {
        private readonly ShatblyDbContext _dbContext;

        public CompanyRepository(ShatblyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddCompany(Company Company)
        {
            await _dbContext.Companies.AddAsync(Company);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCompany(int CompanyId)
        {
            var company = GetCompanyById(CompanyId);
            if (company != null)
            {
                _dbContext.Remove(company);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<Company>> GetAllCompanies()
        {
            var companies = await _dbContext.Companies.ToListAsync();
            return companies;
        }

        public async Task<Company> GetCompany(int CompanyId, string UserId)
        {
            var company = await _dbContext.Companies.Where(c => c.UserId == UserId && c.CompanyId == CompanyId).FirstOrDefaultAsync();
            return company;
        }

        public async Task<Company> GetCompanyById(int CompanyId)
        {
            var company = await _dbContext.Companies.Where(c => c.CompanyId == CompanyId).FirstOrDefaultAsync();
            return company;
        }

        public async Task<Company> GetCompanyByName(string CompanyName)
        {
            var company = await _dbContext.Companies.Where(c => c.CompanyName.Trim() == CompanyName.Trim()).FirstOrDefaultAsync();
            return company;
        }

        public async Task<Company> GetCompanyByUser(string UserId)
        {
            var company = await _dbContext.Companies.Where(c => c.UserId == UserId).FirstOrDefaultAsync();
            return company;
        }

        public async Task UpdateCompany(Company Company)
        {
            var oldcompany = GetAllCompanies().Result.Where(c => c.CompanyId == Company.CompanyId).FirstOrDefault();
            if (oldcompany != null)
            {
                oldcompany.CompanyName = Company.CompanyName;
                oldcompany.Address = Company.Address;
                oldcompany.City = Company.City;
                oldcompany.Country = Company.Country;
                oldcompany.Email = Company.Email;
                oldcompany.Phone1 = Company.Phone1;
                oldcompany.Phone2 = Company.Phone2;
                oldcompany.Hotline = Company.Hotline;
                oldcompany.Fax = Company.Fax;

                _dbContext.Companies.Update(oldcompany);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;
using Shatably.Data.Context;
using Shatably.Data.Models;
using System;
using System.Linq;

namespace Shatably.Data.Validators
{
    public class CompanyValidator : AbstractValidator<Company>
    {
        private readonly ShatblyDbContext _context;

        [Obsolete]
        public CompanyValidator(ShatblyDbContext dbContext)
        {
            _context = dbContext;

            RuleFor(company => company.CompanyName).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("Company name is required.").Must(name => _context.Companies.Any(c => c.CompanyName.Trim() == name.Trim()) == false).WithMessage("Company name exists.");
            RuleFor(company => company.Address).NotEmpty().WithMessage("Address is required.");
            RuleFor(company => company.City).NotEmpty().WithMessage("City is required.");
            RuleFor(company => company.Country).NotEmpty().WithMessage("Country is required.");
            RuleFor(company => company.Phone1).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().WithMessage("Phone number is required.").Must(number => _context.Companies.Any(c => c.Phone1.Trim() == number.Trim()) == false).WithMessage("Phone number exists.");

            When(company => string.IsNullOrWhiteSpace(company.Phone2) == false, () =>
            {
                RuleFor(company => company.Phone2).Must(number => _context.Companies.Any(c => c.Phone2.Trim() == number.Trim()) == false).WithMessage("Phone number exists.");
            });
            When(company => string.IsNullOrWhiteSpace(company.Mobile) == false, () =>
            {
                RuleFor(company => company.Mobile).Must(number => _context.Companies.Any(c => c.Mobile.Trim() == number.Trim()) == false).WithMessage("Mobile number exists.");
            });
            When(company => string.IsNullOrWhiteSpace(company.Fax) == false, () =>
            {
                RuleFor(company => company.Fax).Must(number => _context.Companies.Any(c => c.Fax.Trim() == number.Trim()) == false).WithMessage("Fax number exists.");
            });
            When(company => string.IsNullOrWhiteSpace(company.Hotline) == false, () =>
            {
                RuleFor(company => company.Hotline).Must(number => _context.Companies.Any(c => c.Hotline.Trim() == number.Trim()) == false).WithMessage("Hotline number exists.");
            });
        }
    }
}
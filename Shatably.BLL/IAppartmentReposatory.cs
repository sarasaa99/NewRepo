using Shatably.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shatably.BLL
{
    public interface IAppartmentReposatory
    {
        Task<Appartment> GetAllAppartment(string UserID);
        Task<Appartment> GetAppartmentById(int AppartmentId, string UserID);
        Task<Appartment> AddAppartment(Appartment Appartment, string UserId);
        Task<Appartment> UpdateAppartment(Appartment Appartment);
        Task<Appartment> DeleteAppartment(int AppartmentId, string UserId);
    }
}

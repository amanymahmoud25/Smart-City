using Microsoft.EntityFrameworkCore;
using Smart_City.Models;

namespace Smart_City.Repositories
{
    public interface IBillRepository
    {
         List<Bill> GetAll();
         Bill GetById(int id);
         List<Bill> GetByCitizenId(int citizenId);
         bool Add(Bill bill);
         bool Update(Bill bill);
         bool Delete(int id);
         List<Bill> GetUnpaid();
         List<Bill> GetPaid();
         List<Bill> GetByType(string type);
         bool MarkAsPaid(int id);
       
    }
}



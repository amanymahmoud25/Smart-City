using Smart_City.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Smart_City.Repositories
{
    public class BillRepository :IBillRepository
    {
        private readonly SmartCityContext _context;

        public BillRepository(SmartCityContext context)
        {
            _context = context;
        }
        //  Get all bills
        public List<Bill> GetAll()
        {
            return _context.Bills.Include(b => b.Citizen).ToList();
        }
        //  Get bill by ID
        public Bill GetById(int id)
        {
            if (id <= 0)
                return null;

            return _context.Bills.Include(b => b.Citizen).FirstOrDefault(b => b.Id == id);
        }
        //  Get bills by citizen ID
        public List<Bill> GetByCitizenId(int citizenId)
        {
            if (citizenId <= 0)
                return new List<Bill>();

            return _context.Bills
                .Include(b => b.Citizen)
                .Where(b => b.CitizenId == citizenId)
                .ToList();
        }
        //  Add new bill
        public bool Add(Bill bill)
        {
            if (bill == null)
                return false;

            if (bill.CitizenId <= 0 || bill.Amount <= 0)
                return false;

            _context.Bills.Add(bill);
            _context.SaveChanges();
            return true;
        }

        //  Update existing bill
        public bool Update(Bill bill)
        {
            if (bill == null || bill.Id <= 0)
                return false;

            var existingBill = _context.Bills.FirstOrDefault(b => b.Id == bill.Id);
            if (existingBill == null)
                return false;

            existingBill.Type = bill.Type;
            existingBill.Amount = bill.Amount;
            existingBill.IssueDate = bill.IssueDate;
            existingBill.IsPaid = bill.IsPaid;

            _context.SaveChanges();
            return true;
        }
         // Delete bill
        public bool Delete(int id)
        {
            if (id <= 0)
                return false;

            var bill = _context.Bills.FirstOrDefault(b => b.Id == id);
            if (bill == null)
                return false;

            _context.Bills.Remove(bill);
            _context.SaveChanges();
            return true;
        }
        //  Get unpaid bills
        public List<Bill> GetUnpaid()
        {
            return _context.Bills
                .Include(b => b.Citizen)
                .Where(b => b.IsPaid == false)
                .ToList();
        }
        //  Get paid bills
        public List<Bill> GetPaid()
        {
            return _context.Bills
                .Include(b => b.Citizen)
                .Where(b => b.IsPaid == true)
                .ToList();
        }
        //  Get bills by type (e.g. Water / Electricity)
        public List<Bill> GetByType(string type)
        {
            if (string.IsNullOrEmpty(type))
                return new List<Bill>();

            return _context.Bills
                .Include(b => b.Citizen)
                .Where(b => b.Type.ToLower() == type.ToLower())
                .ToList();
        }
        //  Mark bill as paid
        public bool MarkAsPaid(int id)
        {
            var bill = _context.Bills.FirstOrDefault(b => b.Id == id);
            if (bill == null)
                return false;

            bill.IsPaid = true;
            _context.SaveChanges();
            return true;
        }
    }
}

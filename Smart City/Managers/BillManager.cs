using AutoMapper;
using Smart_City.Dtos;
using Smart_City.Models;
using Smart_City.Repositories;

namespace Smart_City.Managers
{
    public class BillManager : IBillManager
    {
        private readonly BillRepository _repo;
        private readonly IMapper _mapper;
        private readonly NotificationRepository _notificationRepo;

        public BillManager(
            BillRepository repo,
            IMapper mapper,
            NotificationRepository notificationRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _notificationRepo = notificationRepo;
        }

        //  Get all bills
        public List<BillDto> GetAll()
        {
            var list = _repo.GetAll();
            return _mapper.Map<List<BillDto>>(list);
        }

        // Get bill by ID
        public BillDto GetById(int id)
        {
            var bill = _repo.GetById(id);
            return _mapper.Map<BillDto>(bill);
        }

        //  Get bills for a specific citizen
        public List<BillDto> GetByCitizenId(int citizenId)
        {
            var list = _repo.GetByCitizenId(citizenId);
            return _mapper.Map<List<BillDto>>(list);
        }

        //  Create new bill
        public BillDto Create(BillCreateDto dto)
        {
            var bill = _mapper.Map<Bill>(dto);
            bill.IssueDate = DateTime.Now;
            bill.IsPaid = false;

            var saved = _repo.Add(bill);

            if (saved)
            {
                //  Send notification
                _notificationRepo.Add(new Notification
                {
                    CitizenId = bill.CitizenId,
                    Message = $"A new {bill.Type} bill has been issued."
                });

                return _mapper.Map<BillDto>(bill);
            }

            return null;
        }

        //  Update bill (without changing CitizenId)
        public BillDto Update(int id, BillUpdateDto dto)
        {
            var bill = _repo.GetById(id);
            if (bill == null)
                return null;

            bill.Type = dto.Type;
            bill.Amount = dto.Amount ?? bill.Amount;
            bill.IssueDate = dto.IssueDate ?? bill.IssueDate;
            bill.IsPaid = dto.IsPaid ?? bill.IsPaid;


            var updated = _repo.Update(bill);
            if (!updated)
                return null;

            return _mapper.Map<BillDto>(bill);
        }

        //  Delete bill
        public bool Delete(int id)
        {
            return _repo.Delete(id);
        }

        //  Mark bill as paid
        public bool MarkAsPaid(int id)
        {
            var ok = _repo.MarkAsPaid(id);
            return ok;
        }

        //  Get all paid bills
        public List<BillDto> GetPaid()
        {
            var list = _repo.GetPaid();
            return _mapper.Map<List<BillDto>>(list);
        }

        //  Get all unpaid bills
        public List<BillDto> GetUnpaid()
        {
            var list = _repo.GetUnpaid();
            return _mapper.Map<List<BillDto>>(list);
        }
    }
}

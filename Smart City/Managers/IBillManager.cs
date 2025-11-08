using Smart_City.Dtos;

namespace Smart_City.Managers
{
    public interface IBillManager
    {
        List<BillDto> GetAll();
        BillDto GetById(int id);
        List<BillDto> GetByCitizenId(int citizenId);
        BillDto Create(BillCreateDto dto);
        BillDto Update(int id, BillUpdateDto dto);
        bool Delete(int id);
        bool MarkAsPaid(int id);
        List<BillDto> GetPaid();
        List<BillDto> GetUnpaid();
    }
}

using System.ComponentModel.DataAnnotations;
using Smart_City.Models;

namespace Smart_City.Dtos
{
    public class ComplaintStatusUpdateDto
    {
        [Required]
        [EnumDataType(typeof(ComplaintStatus))]
        public ComplaintStatus Status { get; set; }
    }
}

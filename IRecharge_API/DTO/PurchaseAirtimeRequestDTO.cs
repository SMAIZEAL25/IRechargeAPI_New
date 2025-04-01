using System.ComponentModel.DataAnnotations;

namespace IRecharge_API.DTO
{
    public class PurchaseAirtimeRequestDTO
    {
        [Required(ErrorMessage = "Phone number is required")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }

        public required string NetworkType { get; set; }
    }
}

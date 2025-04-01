using System.ComponentModel.DataAnnotations;

namespace IRecharge_API.DTO
{
    public class PurchaseAirtimeRequestDTO
    {
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^0[7-9][0-9]{9}$", ErrorMessage = "Invalid Nigerian phone number")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(50, 10000, ErrorMessage = "Amount must be between ₦50 and ₦10,000")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Network type is required")]
        [RegularExpression("^(MTN|AIRTEL|GLO|9MOBILE)$",
        ErrorMessage = "Network must be MTN, AIRTEL, GLO, or 9MOBILE")]
        public required string NetworkType { get; set; }
    }
}

namespace IRecharge_API.Entities
{
    public class User
    {
        public Guid UserId { get; set; }

        public required string UserName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public decimal WalletBalance { get; set; }

        public required string PhoneNumber { get; set; }

        public DateTime DateCreated { get; set; }
    }
}

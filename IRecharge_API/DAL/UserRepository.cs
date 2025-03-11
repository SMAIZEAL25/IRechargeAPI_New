using IRecharge_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace IRecharge_API.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly IRechargeDbContext _context;
        public UserRepository(IRechargeDbContext context)
        {
            _context = context;
        }
        public User? GetByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName);
        }

        public User? GetUserById(Guid id)
        {
            return _context.Users.SingleOrDefault(u => u.UserId == id);
        }

        public User? GetUserByPhoneNumber(string phoneNumber)
        {
            return _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
        }

        public void UpdateUser(User user)
        {
            var updateUserRecord = _context.Users.Update(user);
        }
    }
}

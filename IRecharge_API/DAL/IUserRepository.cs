﻿using IRecharge_API.Entities;

namespace IRecharge_API.DAL
{
    public interface IUserRepository
    {
        User GetUserByPhoneNumber(string phoneNumber);

        void UpdateUser(User user);

        User GetUserById(Guid id);

        User GetByUserName(string userName);

    }
}

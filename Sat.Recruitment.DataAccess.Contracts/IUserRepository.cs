using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sat.Recruitment.DTO.Users;
using Sat.Recruitment.DTO.Responses;
using Sat.Recruitment.DataAccess.Contracts;
namespace Sat.Recruitment.DataAccess.Contracts
{
    public interface IUserRepository
    {
        Task<bool> CreateUser(User userDTO);
        Task<IEnumerable<User>> GetAllUsers();

    }
}

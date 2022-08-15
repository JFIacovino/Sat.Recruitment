using System;
using Sat.Recruitment.DTO.Users;
using Sat.Recruitment.DTO.Responses;
using System.Threading.Tasks;

namespace Sat.Recruitment.Services.Contracts
{
    public interface IUserService
    {
        Task<UserResponse> CreateUser(User userDTO);


    }
}

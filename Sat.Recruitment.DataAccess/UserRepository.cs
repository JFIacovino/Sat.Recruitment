using System;
using Sat.Recruitment.DTO.Responses;
using Sat.Recruitment.DTO.Users;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sat.Recruitment.DataAccess.Common;
using Sat.Recruitment.DataAccess.Contracts;
using Microsoft.Extensions.Configuration;

namespace Sat.Recruitment.DataAccess
{
    public class UserRepository:RepositoryBase ,IUserRepository
    {

        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration):base( configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateUser(User userDTO)
        {

            return await InsertNewUser(userDTO);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {

            return await GetUsers(); 
        }


    }
}

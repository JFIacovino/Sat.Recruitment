using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Sat.Recruitment.DTO.Responses;
using Sat.Recruitment.DTO.Users;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Sat.Recruitment.Services.Contracts;

namespace Sat.Recruitment.Api.Controllers
{

    [Route("Sat/")]
    public  class UsersController : ControllerBase
    {
        private readonly IUserService _userService;


        public UsersController(IUserService userService)
        {
            _userService = userService;
        }




        [HttpPost]
        [Route("CreateUser")]
        public async Task<UserResponse> CreateUser([FromBody] User userDTO)
        {
            return await _userService.CreateUser(userDTO);

        }

        
       
    }

}

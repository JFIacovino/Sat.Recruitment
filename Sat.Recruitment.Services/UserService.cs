using System;
using Sat.Recruitment.Services.Contracts;
using Sat.Recruitment.DTO.Users;
using Sat.Recruitment.DTO.Responses;
using System.Threading.Tasks;
using System.IO;
using Sat.Recruitment.DataAccess.Contracts;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Sat.Recruitment.Services
{
    public class UserService: IUserService
    {
        #region PrivateFields
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public User JsonObject { get; private set; }

        #endregion

        #region Constructor
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        #endregion

        #region Validations

        private UserResponse ValidateNull(User userDTO)
        {
            List<string> errorList = new List<string>();
          
                foreach (PropertyInfo pi in userDTO.GetType().GetProperties())
                {
                    string value = (pi.GetValue(userDTO) == null) ? (string)pi.GetValue(userDTO) : pi.GetValue(userDTO).ToString();

                    if (String.IsNullOrEmpty(value))
                    {
                        errorList.Add(pi.Name);
                    }
                }
            if (errorList.Count>0)
            { return CreateResponse(false,errorList); }
            else
            { return CreateResponse(true); }
        }
        private UserResponse ValidateUserType(User userDTO)
        {
            if (userDTO.UserType != "Normal" && userDTO.UserType != "SuperUser" && userDTO.UserType != "Premium")
            { return CreateResponse(false, "Tipo de Usuario Inválido"); }
            else { return CreateResponse(true); }


        }
        #endregion

        #region Responses
        private UserResponse CreateResponse(bool state)
        {
            return new UserResponse()
            {
                IsSuccess = state,
                Errors = ""
            };

        }
        private UserResponse CreateResponse(bool state,string error)
        {
            return new UserResponse()
            {
                IsSuccess = state,
                Errors = error
            };

        }
        private UserResponse CreateResponse(bool state, List<string> errors)
        {
            return new UserResponse()
            {
                IsSuccess = state,
                Errors = "Los siguientes campos no deben ser Vacío y/o NULL:"+String.Join(",",errors.ToArray())
            };

        }
        #endregion

        #region Functions
        private decimal DoCalc(string percentage,decimal money)
        {

            var perc = Convert.ToDecimal(Convert.ToDouble(percentage));
            return money * perc;
        }
        private User PerformCalc(User user)
        {
           

            switch (user.UserType)
            {
                case "Normal":
                    if (user.Money > 100)
                    { user.Money +=  DoCalc(_configuration.GetSection("NormalUser>100").Value, user.Money); }
                    else
                    {if (user.Money > 10)
                        { user.Money += DoCalc(_configuration.GetSection("10<NormalUser>100").Value, user.Money); }
                    }
                    break;
                case "SuperUser":
                    if (user.Money > 100)
                    { user.Money +=  DoCalc(_configuration.GetSection("SuperUser>100").Value, user.Money); }
                    break;
                case "Premium":
                    if (user.Money > 100)
                    { user.Money +=user.Money*2; }
                    break;
            }
            return user; 
        }

        private string NormalizeMail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                var pos =addr.Address.IndexOf("+", StringComparison.Ordinal);
                if (pos < 0)
                { addr.Address.Replace(".", ""); }
                else { addr.Address.Replace(".", "").Remove(pos); }
                             
                return addr.Address;
            }
            catch
            {
                return email;
            }

        }

        private bool FindDuplicated(User user, List<User> Users)
        {
            foreach (User us in Users)
            {        
                         if (us.Email == user.Email || us.Phone == user.Phone)
                                    { return true; }
                        else {
                                if (us.Name == user.Name || us.Address == user.Address)
                                { return true; }
                             }
            }
            return false;
        }
        #endregion

        public async Task<UserResponse> CreateUser(User userDTO)
        {
            var resultValidation = ValidateNull(userDTO);
            if (resultValidation.IsSuccess == false)
            { return resultValidation; }

            var validationUser = ValidateUserType(userDTO);
            if (validationUser.IsSuccess == false)
            { return validationUser; }

            var firstUserChanged = PerformCalc(userDTO);
            firstUserChanged.Email = NormalizeMail(firstUserChanged.Email);
            var allUsers = await _userRepository.GetAllUsers();
            var isDuplicated = FindDuplicated(firstUserChanged,allUsers.ToList());

            if (!isDuplicated)
            {   if (await _userRepository.CreateUser(userDTO))
                { return CreateResponse(true, "User Created"); }
                else { return CreateResponse(false, "User Creation Failed"); }
            }
            else
            { return CreateResponse(false, "The user is Duplicated"); }

        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sat.Recruitment.DTO.Users;
using Microsoft.Extensions.Configuration;

namespace Sat.Recruitment.DataAccess.Common
{
    public abstract class RepositoryBase
    {
        private readonly IConfiguration _configuration;

        public RepositoryBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        protected async Task<bool> InsertNewUser(User userDTO)
        {

            var path = Directory.GetCurrentDirectory() + _configuration.GetSection("FilePath").Value;

            try {
                using (StreamWriter writer =  File.AppendText(path))
                {
                    await writer.WriteLineAsync(userDTO.Name+","+userDTO.Email+","+userDTO.Phone+","+userDTO.Address+","+userDTO.UserType+","+userDTO.Money.ToString().Replace(",","."));
                    writer.Flush();
                    writer.Close();

                    
                }
                return true;
            }
            catch 
            { return false; }
        }

        protected async Task<IEnumerable<User>> GetUsers()
        {
            var path = Directory.GetCurrentDirectory() + _configuration.GetSection("FilePath").Value;

            string FileToRead = path;
            IEnumerable<string> line = await  File.ReadAllLinesAsync(FileToRead);
            var Users = new List<User>();
                foreach (string us in line)
                   {
                            string[] userString = us.Split(',');
                            var user = new User()
                            {
                                Name = userString[0].ToString(),
                                Email = userString[1].ToString(),
                                Phone = userString[2].ToString(),
                                Address = userString[3].ToString(),
                                UserType = userString[4].ToString(),
                                Money =Decimal.Parse( userString[5].ToString())
                            };
                            Users.Add(user);
                     }
            return Users;
        }
    }

 
}

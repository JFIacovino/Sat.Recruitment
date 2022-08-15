using Sat.Recruitment.DTO.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sat.Recruitment.Services.Test.Fake
{
    class FakeUserDTO
    {
        public static IEnumerable<User> GetSome()
        {
                    IEnumerable<User> Users = new List<User>()
                {
                new User(){Name="Facundo",Email="facundo@gmail.com",Address="Pacifico123",Phone="1188776655",UserType="Normal",Money=123 },
                 new User(){Name="Juan",Email="juan@gmail.com",Address="Pacifico456",Phone="1155667788",UserType="Premium",Money=123 }

                };
            return Users;
        
        }
        public static User GetOneUnique()
        {
            User Us = new User
                {
                Name = "Pedro", Email = "pedro@gmail.com", Address = "Pedro123", Phone = "1122334455", UserType = "Normal", Money = 123 
                

                };
            return Us;

        }
        public static User GetOneRepeat()
        {
            User Us = new User
            {
                Name = "Facundo",
                Email = "facundo@gmail.com",
                Address = "Pacifico123",
                Phone = "1188776655",
                UserType = "Normal",
                Money = 123


            };
            return Us;

        }

    }
}

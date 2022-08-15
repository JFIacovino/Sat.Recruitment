using Microsoft.Extensions.DependencyInjection;
using Sat.Recruitment.DataAccess;
using Sat.Recruitment.DataAccess.Contracts;
using Sat.Recruitment.Services;
using Sat.Recruitment.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.IoC
{
    public static class IocRegistrer
    {

        public static IServiceCollection AddRegistration(this IServiceCollection services)
        {
            #region Services
            services.AddTransient<IUserService, UserService>();
            #endregion

            #region Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            #endregion
            return services;
        }
    }
}
